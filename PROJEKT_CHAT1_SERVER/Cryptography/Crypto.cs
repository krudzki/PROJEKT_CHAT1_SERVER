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
            var data = Encoding.ASCII.GetBytes(strBeforeBase64);
            var base64 = Convert.ToBase64String(data);
            return base64;
        }

        public string Base64Decode(string encodedDataByBase)
        {
            var decoded = Convert.FromBase64String(encodedDataByBase);
            var result = Encoding.ASCII.GetString(decoded);
            return result;
        }
    }
}
