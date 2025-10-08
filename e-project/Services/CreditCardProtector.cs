using Microsoft.AspNetCore.DataProtection;
using System;

namespace e_project.Services
{
    public class CreditCardProtector
    {
        private readonly IDataProtector _protector;

        public CreditCardProtector(IDataProtectionProvider provider)
        {
            _protector = provider.CreateProtector("CreditCardProtector");
        }

        public string Encrypt(string plainText)
        {
            return string.IsNullOrEmpty(plainText) ? null : _protector.Protect(plainText);
        }

        public string Decrypt(string protectedText)
        {
            return string.IsNullOrEmpty(protectedText) ? null : _protector.Unprotect(protectedText);
        }

    }
}
