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
    public class ToggleGate : GHDataComponent
    {
        /// <summary>
        /// The data from previous solution.
        /// </summary>
        protected GH_Structure<IGH_Goo> PreviousData { get; set; }

        /// <summary>
        /// Flag this component to get an update after a scheduled solution.
        /// </summary>
        protected bool UpdateOutput { get; set; }

        /// <summary>
        /// A switch flag to avoid updating twice upon definition startup.
        /// </summary>
        protected bool Active = false;

        public ToggleGate() : base(
            "Toggle Gate",
            "Toggle Gate",
            "Let's data through only if active.")
        {
            PreviousData = new GH_Structure<IGH_Goo>();
        }

        public override GH_Exposure Exposure => GH_Exposure.primary;
        protected override System.Drawing.Bitmap Icon => null;
        public override Guid ComponentGuid => new Guid("349b06cf-83fc-478c-a02b-579cd2fcc88e");

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Data", "D", "Data to gate.", GH_ParamAccess.tree);
            pManager[0].Optional = true;
            pManager.AddBooleanParameter("On", "O", "Should data flow through?", GH_ParamAccess.item, true);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Data", "D", "Output data.", GH_ParamAccess.tree);
        }

        protected override void SolveInstance(IGH_DataAccess access)
        {
            // This stops the component from assigning nulls to the output if there is no other data
            access.DisableGapLogic();

            // Maybe Data input could be optional...?
            // Don't exit if no data, we want to check against the empty structure
            access.GetDataTree(0, out GH_Structure<IGH_Goo> currentData);

            bool flowing = true;
            access.GetData(1, ref flowing);

            // This avoids components updating twice on definition startup
            if (Active)
            {
                access.SetDataTree(0, currentData);
            }

            // If component was flagged for update, then stop scheduling new solutions.
            if (UpdateOutput)
            {
                // DataTrees work by reference. In this case, we can use a pointer
                // instead of needing a deep copy.
                PreviousData = currentData;
                UpdateOutput = false;
                return;
            }

            // If component is flagged as active.
            if (flowing)
            {
                // DataTrees work by reference. In this case, we can use a pointer
                // instead of needing a deep copy.
                PreviousData = currentData;
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
