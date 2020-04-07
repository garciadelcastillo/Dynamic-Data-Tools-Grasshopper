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

using DataToolsGrasshopper.Utils;

namespace DataToolsGrasshopper.Data
{
    public abstract class GHDataComponent<T> : GHDDTComponent where T : IGH_Goo
    {
        /// <summary>
        /// Category name for components.
        /// </summary>
        protected static readonly string CATEGORY_NAME = "Data";

        /// <summary>
        /// The data from previous solution.
        /// </summary>
        protected GH_Structure<T> PreviousData { get; set; }

        /// <summary>
        /// Flag this component to get an update after a scheduled solution.
        /// </summary>
        protected bool UpdateOutput { get; set; }

        /// <summary>
        /// A switch flag to avoid updating twice upon definition startup.
        /// </summary>
        protected bool Active = false;

        /// <summary>
        /// Middleware constructor.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="nickname"></param>
        /// <param name="description"></param>
        public GHDataComponent(string name, string nickname, string description) 
            : base (name, nickname, description, CATEGORY_NAME) 
        {
            PreviousData = new GH_Structure<T>();
        }

        
        /// <summary>
        /// "Pre"callback for solution scheduler. Will be executed before next scheduled solution.
        /// </summary>
        /// <param name="doc"></param>
        protected void PreCallBack(GH_Document doc)
        {
            // Flag this solution as expired, but do not recompute 
            if (UpdateOutput) ExpireSolution(false);  
        }

        /// <summary>
        /// Called by managed code after `ExpireSolution`.
        /// </summary>
        protected override void ExpireDownStreamObjects()
        {
            if (UpdateOutput)
            {
                Params.Output[0].ExpireSolution(false);
            }
        }

    }
}
