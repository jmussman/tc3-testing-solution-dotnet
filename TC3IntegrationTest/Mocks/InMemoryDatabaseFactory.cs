// InMemoryDatabaseFactory.cs
// Copyright 2018-2021 NextStep IT Training. All rights reserved.
//
// This factory produces the singleton instance of the in-memory database requested by the configuration. Because
// .NET Core is not launching the application, the configuration is passed in when the factory is instantiated.

using System;
using Microsoft.Extensions.Configuration;
using TC3IntegrationTest.Configuration;

namespace TC3IntegrationTest.Mocks {

    public class InMemoryDatabaseFactory {

        private static IInMemoryDatabase _instance;
        private static Object theLock = new Object();

        public InMemoryDatabaseFactory() {

            if (_instance == null) {

                lock(theLock) {

                    if (_instance == null) {

                        IConfigurationRoot configurationRoot = ConfigurationLoader.Instance.ConfigurationRoot;
                        string classname = configurationRoot.GetSection("InMemoryDatabase").GetValue<string>("DbClass");

                        _instance = (IInMemoryDatabase)Activator.CreateInstance(null, classname, false, 0, null, new Object[] { configurationRoot }, null, null).Unwrap();
                    }
                }
            }
        }

        public IInMemoryDatabase Instance { get { return _instance; }}
    }
}
