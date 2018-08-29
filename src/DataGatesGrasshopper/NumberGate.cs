using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace DataGatesGrasshopper
{
    public class NumberGate : GH_Component
    {
        private bool UpdateOutput { get; set; }
        private double PreviousData { get; set; }

        public NumberGate()
          : base("Number Gate", "Number Gate",
              "Triggers an update only if new different data came through.",
              "Data Gates", "Data")
        {
        }

        protected override System.Drawing.Bitmap Icon => null;
        public override Guid ComponentGuid => new Guid("5a4d12ca-113e-41be-ba56-33a77ea0541a");

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("num", "n", "", GH_ParamAccess.item);
            pManager.AddNumberParameter("precision", "e", "Decimal precision for equality comparison", GH_ParamAccess.item, 0.0001);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddNumberParameter("num", "n", "", GH_ParamAccess.item);
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

            double currentData = 0;
            double epsilon = 0.0001;

            if (!access.GetData(0, ref currentData)) return;
            access.GetData(1, ref epsilon);
            access.SetData(0, currentData);

            if (UpdateOutput)
            {
                UpdateOutput = false;
                PreviousData = currentData;
                return;
            }

            if (Math.Abs(PreviousData - currentData) > epsilon)
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
