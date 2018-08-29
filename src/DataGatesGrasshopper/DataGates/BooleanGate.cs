using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace DataGatesGrasshopper.DataGates
{
    public class BooleanGate : GH_Component
    {
        private bool UpdateOutput { get; set; }
        private bool PreviousData { get; set; }

        public BooleanGate()
          : base("Boolean Gate", "Boolean Gate",
              "Triggers an update only if new different data came through.",
              "Data Gates", "Data")
        {
        }

        protected override System.Drawing.Bitmap Icon => null;
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
