using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROJEKT_CHAT1_SERVER.Cryptography
{
    class Crypto
    {
        public string Base64Encode(string strBeforeBase64)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(strBeforeBase64);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public string Base64Decode(string encodedDataByBase)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(encodedDataByBase);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}
