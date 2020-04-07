using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace DataToolsGrasshopper.Gates
{
    public class PlaneGate : GH_Component
    {
        private bool UpdateOutput { get; set; }
        private List<Plane> PreviousData { get; set; }

        public PlaneGate()
          : base("Plane Gate", "Plane Gate",
              "Will let data trough only if it changed since the previous solution. WORKS ON LISTS OF PLANES TOO.",
              "Data Tools", "Gates")
        {
            PreviousData = new List<Plane>();
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.icons_plane_gate;
        public override Guid ComponentGuid => new Guid("61988426-af3e-4eaa-a1b6-a4eaa01e369d");

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddPlaneParameter("plane", "pl", "", GH_ParamAccess.list);
            pManager.AddNumberParameter("precision", "e", "Decimal precision for equality comparison", GH_ParamAccess.item, 0.0001);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddPlaneParameter("plane", "pl", "", GH_ParamAccess.list);
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

            List<Plane> currentData = new List<Plane>();
            double epsilon = 0.0001;

            if (!access.GetDataList(0, currentData)) return;
            access.GetData(1, ref epsilon);

            access.SetDataList(0, currentData);

            if (UpdateOutput)
            {
                UpdateOutput = false;
                PreviousData = currentData;
                return;
            }

            if (!Utils.AreSimilar.PlaneList(PreviousData, currentData, epsilon))
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
