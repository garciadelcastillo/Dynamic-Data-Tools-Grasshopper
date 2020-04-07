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
    public class BooleanGate : GHDataComponent<GH_Boolean>
    {
        public BooleanGate()
          : base("Boolean Gate", 
              "Boolean Gate",
              "Will let Boolean data trough only if it changed since the previous solution. Works at the full Data Tree level.")
        { }

        public override GH_Exposure Exposure => GH_Exposure.primary;
        protected override System.Drawing.Bitmap Icon => Properties.Resources.icons_boolean_gate;
        public override Guid ComponentGuid => new Guid("28bf9d69-0fae-4511-ac67-7ade22697ffa");

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddBooleanParameter("Bool", "B", "Input Boolean data.", GH_ParamAccess.tree);
            pManager[0].Optional = true;  // avoid "failed to collect data" 
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddBooleanParameter("Bool", "B", "Output will update only if input changed.", GH_ParamAccess.tree);
        }

        //protected override void SolveInstance(IGH_DataAccess access)
        //{
        //    // This stops the component from assigning nulls 
        //    // if we don't assign anything to an output.
        //    access.DisableGapLogic();

        //    if (!access.GetDataTree(0, out GH_Structure<GH_Boolean> currentData)) return;
            
        //    // This avoids components updating twice on definition startup
        //    if (Active)
        //    {
        //        access.SetDataTree(0, currentData);
        //    }

        //    // If component was flagged for update, then stop scheduling new solutions.
        //    if (UpdateOutput)
        //    {
        //        // DataTrees work by reference. Must create a deep copy to avoid PreviousData pointing at the new incoming data. 
        //        PreviousData = new GH_Structure<GH_Boolean>(currentData, false);
        //        UpdateOutput = false;
        //        return;
        //    }

        //    // If data structure and content is different
        //    if (!Compare<GH_Boolean>.EqualDataTrees(PreviousData, currentData, 0) ||
        //        !Compare<GH_Boolean>.EqualDataContent(PreviousData, currentData, this))
        //    {
        //        // DataTrees work by reference. Must create a deep copy to avoid PreviousData pointing at the new incoming data. 
        //        PreviousData = new GH_Structure<GH_Boolean>(currentData, false);
        //        UpdateOutput = true;
        //        Active = true;

        //        // Schedule a new solution
        //        var doc = OnPingDocument();
        //        doc?.ScheduleSolution(5, PreCallBack);
        //    }
        //}
    }
}
