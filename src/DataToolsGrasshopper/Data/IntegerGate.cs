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
    public class IntegerGate : GHDataComponent<GH_Integer>
    {
        public IntegerGate()
          : base("Integer Gate", 
              "Integer Gate",
              "Will let Integer data trough only if it changed since the previous solution. Works at the full Data Tree level.")
        { }

        public override GH_Exposure Exposure => GH_Exposure.primary;
        protected override System.Drawing.Bitmap Icon => Properties.Resources.icons_integer_gate;
        public override Guid ComponentGuid => new Guid("94db6553-9603-40c2-a307-999f58507ea0");

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddIntegerParameter("Int", "I", "Input Integer data.", GH_ParamAccess.tree);
            pManager[0].Optional = true;  // avoid "failed to collect data" 
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddIntegerParameter("Int", "I", "Output will update only if input changed.", GH_ParamAccess.tree);
        }

        //protected override void SolveInstance(IGH_DataAccess access)
        //{
        //    // This stops the component from assigning nulls 
        //    // if we don't assign anything to an output.
        //    access.DisableGapLogic();

        //    // Don't exit if no data, we want to check against the empty structure
        //    access.GetDataTree(0, out GH_Structure<GH_Integer> currentData);

        //    // This avoids components updating twice on definition startup
        //    if (Active)
        //    {
        //        access.SetDataTree(0, currentData);
        //    }

        //    // If component was flagged for update, then stop scheduling new solutions.
        //    if (UpdateOutput)
        //    {
        //        // DataTrees work by reference. Must create a deep copy to avoid PreviousData pointing at the new incoming data. 
        //        PreviousData = new GH_Structure<GH_Integer>(currentData, false);
        //        UpdateOutput = false;
        //        return;
        //    }

        //    // If data structure and content is different
        //    if (!Compare<GH_Integer>.EqualDataTreeStructure(PreviousData, currentData) ||
        //        !Compare<GH_Integer>.EqualDataTreeContent(PreviousData, currentData, this))
        //    {
        //        // DataTrees work by reference. Must create a deep copy to avoid PreviousData pointing at the new incoming data. 
        //        PreviousData = new GH_Structure<GH_Integer>(currentData, false);
        //        UpdateOutput = true;
        //        Active = true;

        //        // Schedule a new solution
        //        var doc = OnPingDocument();
        //        doc?.ScheduleSolution(5, PreCallBack);
        //    }
        //}
    }
}
