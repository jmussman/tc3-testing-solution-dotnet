// ConfigurationLoader.cs
// Copyright © 2018-2021 NextStep IT Training. All rights reserved.
//
// Centrailized singleton to handle confiugration loading for all test suites. Since this
// is the test project, we know to look in the build folder for a name that we have
// selected.
//

using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace TC3IntegrationTest.Configuration {

    public class ConfigurationLoader {

        private static readonly string configurationFileName = "appsettings.Integration.json";
        private static ConfigurationLoader _instance;
        private static Object theLock = new Object();

        private IConfigurationRoot _configurationRoot;

        private ConfigurationLoader() {

           _configurationRoot = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(configurationFileName, optional: true)
                .Build();
        }

        public static ConfigurationLoader Instance { get {

            if (_instance == null) {

                lock(theLock) {

                    if (_instance == null) {

                        _instance = new ConfigurationLoader();
                    }
                }
            }

            return _instance;
        }}

        public IConfigurationRoot ConfigurationRoot { get { return _configurationRoot; }}
    }
}
