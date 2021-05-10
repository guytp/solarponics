using System;
using System.Diagnostics.CodeAnalysis;

namespace Solarponics.WebApi.Exceptions
{
    [ExcludeFromCodeCoverage]
    internal sealed class MissingSslCertificateException : Exception
    {
        public MissingSslCertificateException() : base("The SSL certificate could not be found")
        {
        }
    }
}