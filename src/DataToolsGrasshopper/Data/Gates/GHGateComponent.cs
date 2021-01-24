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
    public abstract class GHGateComponent<T> : GHDataComponent where T : IGH_Goo
    {
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
        public GHGateComponent(string name, string nickname, string description) 
            : base (name, nickname, description) 
        {
            PreviousData = new GH_Structure<T>();
        }

        /// <summary>
        /// Main solution, shared by all Gates.
        /// </summary>
        /// <param name="access"></param>
        protected override void SolveInstance(IGH_DataAccess access)
        {
            // This stops the component from assigning nulls to the output if there is no other data
            access.DisableGapLogic();

            // Don't exit if no data, we want to check against the empty structure
            access.GetDataTree(0, out GH_Structure<T> currentData);

            // Optional comparison precision for some components
            // (some components don't need epsilon)
            double precision = 0.0001;
            try
            {
                access.GetData(1, ref precision);
            }
            catch { }

            // This avoids components updating twice on definition startup
            if (Active)
            {
                access.SetDataTree(0, currentData);
            }

            // If component was flagged for update, then stop scheduling new solutions.
            if (UpdateOutput)
            {
                // DataTrees work by reference. Must create a deep copy to avoid PreviousData pointing at the new incoming data. 
                PreviousData = new GH_Structure<T>(currentData, false);
                UpdateOutput = false;
                return;
            }

            // If data structure and content is different
            if (!Compare<T>.EqualDataTreeStructure(PreviousData, currentData) ||
                !Compare<T>.EqualDataTreeContent(PreviousData, currentData, this, precision))
            {
                // DataTrees work by reference. Must create a deep copy to avoid PreviousData pointing at the new incoming data. 
                PreviousData = new GH_Structure<T>(currentData, false);
                UpdateOutput = true;
                Active = true;

                // Schedule a new solution
                var doc = OnPingDocument();
                doc?.ScheduleSolution(5, PreCallBack);
            }
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
