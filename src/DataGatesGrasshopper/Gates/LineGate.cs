using System;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace DataToolsGrasshopper.Gates
{
    public class LineGate : GH_Component
    {
        private bool UpdateOutput { get; set; }
        private Line PreviousData { get; set; }

        public LineGate()
          : base("Line Gate", "Line Gate",
              "Will let data trough only if it changed since the previous solution.",
              "Data Tools", "Gates")
        {
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.icons_line_gate;
        public override Guid ComponentGuid => new Guid("6744e093-2875-44dc-b4e1-f3dc938872c3");

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddLineParameter("line", "l", "", GH_ParamAccess.item);
            pManager.AddNumberParameter("precision", "e", "Decimal precision for equality comparison", GH_ParamAccess.item, 0.0001);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddLineParameter("line", "l", "", GH_ParamAccess.item);
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

            Line currentData = Line.Unset;
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

            if (!Utils.AreSimilar.Line(PreviousData, currentData, epsilon))
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
