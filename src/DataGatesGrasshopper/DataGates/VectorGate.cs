using System;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace DataGatesGrasshopper.DataGates
{
    public class VectorGate : GH_Component
    {
        private bool UpdateOutput { get; set; }
        private Vector3d PreviousData { get; set; }

        public VectorGate()
          : base("Vector Gate", "Vector Gate",
              "Triggers an update only if new different data came through.",
              "Data Gates", "Data")
        {
        }

        protected override System.Drawing.Bitmap Icon => null;
        public override Guid ComponentGuid => new Guid("05ae0220-a59e-4b95-afcf-6034a0d503d4");

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddVectorParameter("vector", "v", "", GH_ParamAccess.item);
            pManager.AddNumberParameter("precision", "e", "Decimal precision for equality comparison", GH_ParamAccess.item, 0.0001);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddVectorParameter("vector", "v", "", GH_ParamAccess.item);
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

            Vector3d currentData = Vector3d.Unset;
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

            if (!Utils.AreSimilar.Vector3d(PreviousData, currentData, epsilon))
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
