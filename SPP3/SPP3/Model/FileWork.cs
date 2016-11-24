using System;
using System.Collections.Generic;
using SPP3.ViewModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPP3.Model
{
    public class Filework
    {
        public List<string> fullpaths = new List<string> { };
        public List<bool> savedpaths = new List<bool> { };

        public void SavingXMLFile(string fullname, IEnumerable<Threads> list)
        {
            XMLTreeDAsm dasm = new XMLTreeDAsm(fullname);
            dasm.LoadThreads(list);
        }

    }
}
