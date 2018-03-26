using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExploScripts.Data
{
    public class ExploScript
    {
        public bool Active { get; set; }
        public string Name { get; set; }
        public string Caption { get; set; }

        public ExploScript()
        {

        }

        public ExploScript(bool active, string name, string caption)
        {
            Active = active;
            Name = name;
            Caption = caption;
        }
    }
}
