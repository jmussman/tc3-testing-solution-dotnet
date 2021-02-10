// InMemoryDatabase.cs
// Copyright © 2018-2021 NextStep IT Training. All rights reserved.
//
// Multiple tests should see the same in-memory database and avoid testing issues by using transactions. Unless the test
// loads the .Net application, .NET Core cannot be used to provide the singleton. Use the InMemoryDatabaseFactory to
// retrieve the singleton.
//

using System;
using System.Data.Common;
using System.IO;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;

namespace TC3IntegrationTest.Mocks {

    public class InMemorySqliteDatabase : IInMemoryDatabase, IDisposable {

        private SqliteConnection _sqliteConnection;

        public InMemorySqliteDatabase(IConfigurationRoot configurationRoot) {

            string connectionString = configurationRoot.GetConnectionString("DefaultConnection");
            string sqlSchemaFile = configurationRoot.GetSection("InMemoryDatabase").GetValue<string>("DbSchemaScript");
            string sqlDataFile = configurationRoot.GetSection("InMemoryDatabase").GetValue<string>("DbLoadScript");

            _sqliteConnection = new SqliteConnection(connectionString);
            _sqliteConnection.Open();

            SqliteCommand sqliteCommand = _sqliteConnection.CreateCommand();

            sqliteCommand.CommandText = File.ReadAllText(sqlSchemaFile);
            sqliteCommand.ExecuteNonQuery();

            sqliteCommand.CommandText = File.ReadAllText(sqlDataFile);
            sqliteCommand.ExecuteNonQuery();
        }

        public void Dispose() {

            if (_sqliteConnection != null) {

                _sqliteConnection.Close();
            }
        }

        public DbConnection Connection { get { return _sqliteConnection; }}
    }
}
