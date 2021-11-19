using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Interfaces.Helper
{
    public interface IFileProcesor
    {
        string DecryptString(string cipherText);
        string EncryptString(string plainText);
        string GetCsv<T>(IEnumerable<T> items);
    }
}