using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Academy.Infrastructure.SecretManager
{
    public class MissingSecretValueException : Exception
    {
        public MissingSecretValueException(string errorMessage, string secretName, string secretArn, Exception exception) : base(errorMessage, exception)
        {
            SecretName = secretName;
            SecretArn = secretArn;
        }

        public string SecretArn { get; }

        public string SecretName { get; }
    }
}
