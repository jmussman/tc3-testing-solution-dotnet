// SalesOrdersControllerSystemTest.cs
// Copyright © 2018-2021 NextStep IT Training. All rights reserved.
//

using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using TC3.Models;
using Xunit;

namespace TC3SystemTest.Controllers {

    // These are behavorial tests to make sure the correct data is recorded in through the
    // data access layer (Entity Framework) and data results are passed correctly. The application
    // is started with a mock database (look at the appsettings.SystemIntegration.json file).

    public class SalesOrdersControllerSystemTest : IClassFixture<WebApplicationFactory<TC3.Startup>> {

        private readonly WebApplicationFactory<TC3.Startup> factory;
        private HttpClient httpClient;

        public SalesOrdersControllerSystemTest(WebApplicationFactory<TC3.Startup> factory) {

            // Inject our overrides into the configuration.

            string currentDir = Directory.GetCurrentDirectory();
            string configPath = Path.Combine(currentDir, "appsettings.SystemIntegration.json");

            this.factory = factory.WithWebHostBuilder(builder => {

                builder.ConfigureAppConfiguration((context, conf) => {

                    conf.AddJsonFile(configPath);
                });
            });
        }

        private void GivenHttpClient() {

            httpClient = factory.CreateClient();
        }

        [Fact]
        public async Task GetSalesOrderByIdSuccess() {

            GivenHttpClient();

            var response = await httpClient.GetAsync("/api/salesorders/1");
            
            response.EnsureSuccessStatusCode();
            Assert.StartsWith("application/json", response.Content.Headers.ContentType.ToString());
        }
    }
}