using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KVVspeichelner
{
    class fahrtInfo
    {
        public fahrtInfo()
        {
           this.Umstieginfo = new List<fahrtUmstieg>();
        }
        public string Abfahrt{ get; set; }
        public string Ankunft{ get; set; }
        public string Reisedauer{ get; set; }
        public string Umstiegnum{ get; set; }
        public string Wabennum{ get; set; }
        public string Preis{ get; set; }
        public List<fahrtUmstieg> Umstieginfo { get; set; }
    }
}
