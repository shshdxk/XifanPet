using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XifanPet
{
    internal class Setting
    {
        public List<String> Plugins { get; set; }

        private Boolean through = true;
        public Boolean Through { get { return through; } set { this.through = value; } }

    }
}
