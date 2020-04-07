﻿using System;
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
    public abstract class GHDataComponent : GHDDTComponent
    {
        /// <summary>
        /// Category name for components.
        /// </summary>
        internal static readonly string CATEGORY_NAME = "Data";

        /// <summary>
        /// Flag this component to get an update after a scheduled solution.
        /// </summary>
        internal bool UpdateOutput { get; set; }

        /// <summary>
        /// A switch flag to avoid updating twice upon definition startup.
        /// </summary>
        internal bool Active = false;

        /// <summary>
        /// Middleware constructor.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="nickname"></param>
        /// <param name="description"></param>
        public GHDataComponent(string name, string nickname, string description) 
            : base (name, nickname, description, CATEGORY_NAME) { }

        

        protected void PreCallBack(GH_Document doc)
        {
            // This ends up calling `ExpireDownStreamObjects` through managed code
            if (UpdateOutput) ExpireSolution(false);  // flag this solution as expired, but do not recompute 
        }

        protected override void ExpireDownStreamObjects()
        {
            if (UpdateOutput)
            {
                Params.Output[0].ExpireSolution(false);
            }
        }
    }
}