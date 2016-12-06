using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KVVspeichelner
{
    class fahrtUmstieg
    {
        public string UmstiegType { get; set; }
        public string Ab { get; set; }
        public string An { get; set; }
        public string AbStation { get; set; }
        public string AnStation { get; set; }
        public fahrtUmstieg(string type, string ab, string an, string abstation, string anstation)
        {
            UmstiegType = type;
            Ab = ab;
            An = an;
            AbStation = abstation;
            AnStation = anstation;
        }
    }
}
