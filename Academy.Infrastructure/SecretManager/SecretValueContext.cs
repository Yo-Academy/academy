﻿using Amazon.SecretsManager.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Academy.Infrastructure.SecretManager
{
    /// <summary>
    /// A secret context when making a request to get a secret value.
    /// </summary>
    public class SecretValueContext
    {
        public SecretValueContext(SecretListEntry secret)
        {
            _ = secret ?? throw new ArgumentNullException(nameof(secret));

            Name = secret.Name;
            VersionsToStages = secret.SecretVersionsToStages;
        }

        /// <summary>
        /// The secret name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// A list of all the secret's currently assigned SecretVersionStage staging levels and SecretVersionId attached to each one.
        /// </summary>
        public Dictionary<string, List<string>> VersionsToStages { get; private set; }
    }
}
