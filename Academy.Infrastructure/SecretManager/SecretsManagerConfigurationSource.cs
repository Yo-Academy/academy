﻿using Amazon;
using Amazon.Runtime;
using Amazon.SecretsManager;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Academy.Infrastructure.SecretManager
{
    public class SecretsManagerConfigurationSource : IConfigurationSource
    {
        public SecretsManagerConfigurationSource(AWSCredentials? credentials = null, SecretsManagerConfigurationProviderOptions? options = null)
        {
            Credentials = credentials;
            Options = options ?? new SecretsManagerConfigurationProviderOptions();
        }

        public SecretsManagerConfigurationProviderOptions Options { get; }

        public AWSCredentials? Credentials { get; }

        public RegionEndpoint? Region { get; set; }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            var client = CreateClient();

            return new SecretsManagerConfigurationProvider(client, Options);
        }

        private IAmazonSecretsManager CreateClient()
        {
            if (Options.CreateClient != null)
            {
                return Options.CreateClient();
            }

            var clientConfig = new AmazonSecretsManagerConfig
            {
                RegionEndpoint = Region
            };

            Options.ConfigureSecretsManagerConfig(clientConfig);

            return Credentials switch
            {
                null => new AmazonSecretsManagerClient(clientConfig),
                _ => new AmazonSecretsManagerClient(Credentials, clientConfig)
            };
        }
    }
}
