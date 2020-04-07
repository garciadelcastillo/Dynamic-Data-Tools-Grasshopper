using System;
using System.Collections.Generic;

using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

using DataToolsGrasshopper.Utils;

namespace DataToolsGrasshopper.Data
{
    public class BooleanGate : GHDataComponent
    {
        
        private GH_Structure<GH_Boolean> PreviousData { get; set; }

        public BooleanGate()
          : base("Boolean Gate", 
              "Boolean Gate",
              "Will let boolean data trough only if it changed since the previous solution. Works at the full Data Tree level.")
        {
            PreviousData = new GH_Structure<GH_Boolean>();
        }

        public override GH_Exposure Exposure => GH_Exposure.primary;
        protected override System.Drawing.Bitmap Icon => Properties.Resources.icons_boolean_gate;
        public override Guid ComponentGuid => new Guid("28bf9d69-0fae-4511-ac67-7ade22697ffa");

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddBooleanParameter("Bool", "B", "Input Boolean Data Tree.", GH_ParamAccess.tree);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddBooleanParameter("Bool", "B", "This output will update if Input changed.", GH_ParamAccess.tree);
        }

        protected override void SolveInstance(IGH_DataAccess access)
        {
            // This stops the component from assigning nulls 
            // if we don't assign anything to an output.
            access.DisableGapLogic();

            if (!access.GetDataTree(0, out GH_Structure<GH_Boolean> currentData)) return;
            
            // This avoids components updating twice on definition startup
            if (Active)
            {
                access.SetDataTree(0, currentData);
            }

            // If component was flagged for update, 
            if (UpdateOutput)
            {
                UpdateOutput = false;
                PreviousData = new GH_Structure<GH_Boolean>(currentData, false);
                return;
            }

            if (!Compare<GH_Boolean>.EqualDataTrees(PreviousData, currentData, 0) ||
                !Compare<GH_Boolean>.EqualBoolData(PreviousData, currentData))
            {
                Active = true;
                UpdateOutput = true;
                PreviousData = new GH_Structure<GH_Boolean>(currentData, false);

                var doc = OnPingDocument();
                doc?.ScheduleSolution(5, PreCallBack);  // the precallback will be called before next solution is scheduled
            }
        }

    }
}
