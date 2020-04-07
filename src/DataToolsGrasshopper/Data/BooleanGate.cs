using System;
using System.Collections.Generic;

using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

namespace DataToolsGrasshopper.Data
{
    public class BooleanGate : GHDataComponent
    {
        private bool UpdateOutput { get; set; }
        private DataTree<bool> PreviousData { get; set; }

        public BooleanGate()
          : base("Boolean Gate", 
              "Boolean Gate",
              "Will let data trough only if it changed since the previous solution.")
        {
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.icons_boolean_gate;
        public override Guid ComponentGuid => new Guid("28bf9d69-0fae-4511-ac67-7ade22697ffa");

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddBooleanParameter("bool", "b", "", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddBooleanParameter("bool", "b", "", GH_ParamAccess.item);
        }

        protected override void ExpireDownStreamObjects()
        {
            if (UpdateOutput)
            {
                Params.Output[0].ExpireSolution(false);
            }
        }

        protected override void SolveInstance(IGH_DataAccess access)
        {
            access.DisableGapLogic();

            bool currentData = false;

            if (!access.GetData(0, ref currentData)) return;
            access.SetData(0, currentData);
            
            if (UpdateOutput)
            {
                UpdateOutput = false;
                PreviousData = currentData;
                return;
            }

            if (PreviousData != currentData)
            {
                UpdateOutput = true;
                PreviousData = currentData;

                var doc = OnPingDocument();
                doc?.ScheduleSolution(5, Callback);
            }
        }

        private void Callback(GH_Document doc)
        {
            if (UpdateOutput) ExpireSolution(false);
        }


    }
}
