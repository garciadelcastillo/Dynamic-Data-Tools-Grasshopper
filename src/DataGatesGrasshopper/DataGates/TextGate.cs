using System;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace DataGatesGrasshopper.DataGates
{
    public class TextGate : GH_Component
    {
        private bool UpdateOutput { get; set; }
        private string PreviousData { get; set; }

        public TextGate()
          : base("Text Gate", "Text Gate",
              "Triggers an update only if new different data came through.",
              "Data Gates", "Data")
        {
        }

        protected override System.Drawing.Bitmap Icon => null;
        public override Guid ComponentGuid => new Guid("3c924fe1-565d-4fcf-8acd-919c6f229d88");

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("text", "t", "", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("text", "t", "", GH_ParamAccess.item);
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

            string currentData = "";

            if (!access.GetData(0, ref currentData)) return;
            access.SetData(0, currentData);

            if (UpdateOutput)
            {
                UpdateOutput = false;
                PreviousData = currentData;
                return;
            }

            if (!string.Equals(PreviousData, currentData))
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
