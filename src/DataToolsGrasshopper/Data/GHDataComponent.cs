using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

namespace DataToolsGrasshopper.Data
{
    public abstract class GHDataComponent : GHDDTComponent
    {
        /// <summary>
        /// Category name for components.
        /// </summary>
        protected static readonly string CATEGORY_NAME = "Data";

        /// <summary>
        /// Middleware constructor.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="nickname"></param>
        /// <param name="description"></param>
        public GHDataComponent(string name, string nickname, string description)
            : base(name, nickname, description, CATEGORY_NAME)
        { }
    }
}
