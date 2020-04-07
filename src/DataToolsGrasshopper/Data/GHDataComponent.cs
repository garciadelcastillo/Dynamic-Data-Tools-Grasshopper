using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataToolsGrasshopper.Data
{
    public abstract class GHDataComponent : GHDDTComponent
    {
        internal static readonly string CATEGORY_NAME = "Data";

        public GHDataComponent(string name, string nickname, string description) 
            : base (name, nickname, description, CATEGORY_NAME) { }
    }
}
