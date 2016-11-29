using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPP3.ViewModel
{
    public class Props
    {
        public string name;
        public string package;
        public int time;
        public int paramsc;

        public Props(string _name, string _pack, int _time, int _paramsc)
        {
            name = _name;
            package = _pack;
            time = _time;
            paramsc = _paramsc;
        }

    }
}
