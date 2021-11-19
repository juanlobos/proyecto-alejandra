using System;
using System.Collections.Generic;
using System.Text;

namespace Helpers
{
    public class FileProcesorConfiguration
    {
        public string InitVector { get; set; }
        public byte[] PassPhrase { get; set; }
        public int Keysize { get; set; }
        public char SeparadorCsv { get; set; }
    }
}
