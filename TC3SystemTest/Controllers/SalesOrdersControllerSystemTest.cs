// SalesOrdersControllerSystemTest.cs
// Copyright © 2018-2021 NextStep IT Training. All rights reserved.
//

using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using TC3.Dtos;
using TC3.Models;
using Xunit;

namespace TC3SystemTest.Controllers {

    // These are behavorial tests to make sure the correct data is recorded in through the
    // data access layer (Entity Framework) and data results are passed correctly. The application
    // is started with a mock database (look at the appsettings.SystemIntegration.json file).

    public class SalesOrdersControllerSystemTest : IClassFixture<WebApplicationFactory<TC3.Startup>> {

        private static string appConfigFile = "appsettings.SystemIntegration.json";

        private readonly WebApplicationFactory<TC3.Startup> factory;
        private HttpClient httpClient;
        private TC3Context dbContext;

        public SalesOrdersControllerSystemTest(WebApplicationFactory<TC3.Startup> factory) {

            // The WebApplicationFactory allows true E2E testing spinning up the application in memory
            // and allowing HttpClient calls agains the HTTP interface. Without any adjustments this
            // runs the application as configured in the project folder (appsettings, Startup class, etc.)

            string currentDir = Directory.GetCurrentDirectory();
            string configPath = Path.Combine(currentDir, appConfigFile);

            this.factory = factory.WithWebHostBuilder(builder => {

                // Part 1: override the appsettings in the configuration. A system test is an E2E
                // "plumbing" test. We don't want to spin up the application with the live database,
                // so a test-appsettings file can be injected to override those settings. In this
                // example an external database is used, but it could be replaced by an in-memory
                // database as the intergration test example used. 

                builder.ConfigureAppConfiguration((context, conf) => {

                    conf.AddJsonFile(configPath);
                });

                // There are two options in system testing for the database. In general a system
                // test will be an E2# integration test: check individual operations to make sure
                // everything works from beginning to end. In order to effectively test
                // operations database changes need to be rolled back, but that becomes a problem
                // when .NET core injects a new DbContext into each request (scoped) and we don't
                // know what it is! Some net articles advocate channging the Startup class to use
                // a singleton DbContext instead, but that fails on two counts: you don't want to
                // do a "plumbing" test with a different Startup, tests could "pass" because of
                // differences in the versions. And, tests running in parallel may share the same
                // context, leading to false negatives!
                //
                // Each test runs in it's own instance of this test suite class, and each test has
                // its own web application instance in memory, so the solution is to instaniate a
                // DbContext in each test class instance and inject it into the configuration for
                // that specific web application instance :)

                IConfigurationRoot configurationRoot = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile(appConfigFile, optional: true)
                    .Build();

                var options = new DbContextOptionsBuilder<TC3Context>()
                        .UseSqlite(configurationRoot.GetConnectionString("DefaultConnection"))
                        .EnableSensitiveDataLogging()
                        .Options;

                dbContext = new TC3Context(options);

                builder.ConfigureServices(services => {

                    // Inject our DbContext for the configuration for this application instance.
                    // This is the same place that you would inject third-party mocks, such as
                    // credit-card authorization.

                    services.AddSingleton<TC3Context>(dbContext);
                });
            });
        }

        private void GivenHttpClient() {

            httpClient = factory.CreateClient();
        }

        [Fact]
        public async Task GetSalesOrderByIdSuccess() {

            GivenHttpClient();

            // A read operation shouldn't need a transaction because there are no changes,

            var response = await httpClient.GetAsync("/api/salesorders/1");

            response.EnsureSuccessStatusCode();
            Assert.StartsWith("application/json", response.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task CreateSalesOrderSuccess() {

            GivenHttpClient();

            SalesOrderDto salesOrderDto = new SalesOrderDto { OrderDate = DateTime.Now, CustomerId = 1, Total = 0 };

            // A write operation definately needs to be rolled back!

            IDbContextTransaction transaction = dbContext.Database.BeginTransaction();
            HttpResponseMessage response = await httpClient.PostAsJsonAsync<SalesOrderDto>("/api/salesorders", salesOrderDto);

            transaction.Rollback();

            response.EnsureSuccessStatusCode();
            Assert.StartsWith("application/json", response.Content.Headers.ContentType.ToString());
        }
    }
}