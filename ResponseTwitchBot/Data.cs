using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace ResponseTwitchBot
{
    [Serializable]
    class Data
    {
        public string UserName { get; set; }
        public string Token { get; set; }
        public Data(string userName, string token)
        {
            this.UserName = userName;
            this.Token = token;
        }
    }
}
