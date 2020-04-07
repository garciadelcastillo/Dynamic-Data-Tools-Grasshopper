//using System;
//using System.Collections.Generic;

//using Grasshopper.Kernel;
//using Rhino.Geometry;

//namespace DataToolsGrasshopper.Gates
//{
//    public class IntegerGate : GH_Component
//    {
//        private bool UpdateOutput { get; set; }
//        private List<int> PreviousData { get; set; }

//        public IntegerGate()
//          : base("Integer Gate", "Integer Gate",
//              "Will let data trough only if it changed since the previous solution. WORKS WITH LISTS OF DATA",
//              "Data Tools", "Gates")
//        {
//            PreviousData = new List<int>();
//        }

//        protected override System.Drawing.Bitmap Icon => Properties.Resources.icons_integer_gate;
//        public override Guid ComponentGuid => new Guid("49a4fd75-4667-45d6-93f5-41a97e2b0a72");

//        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
//        {
//            pManager.AddIntegerParameter("int", "i", "", GH_ParamAccess.list);
//        }

//        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
//        {
//            pManager.AddIntegerParameter("int", "i", "", GH_ParamAccess.list);
//        }

//        protected override void ExpireDownStreamObjects()
//        {
//            if (UpdateOutput)
//            {
//                Params.Output[0].ExpireSolution(false);
//            }
//        }

//        protected override void SolveInstance(IGH_DataAccess access)
//        {
//            access.DisableGapLogic();

//            List<int> currentData = new List<int>();

//            if (!access.GetDataList(0, currentData)) return;
//            access.SetDataList(0, currentData);

//            if (UpdateOutput)
//            {
//                UpdateOutput = false;
//                PreviousData = currentData;
//                return;
//            }

//            if (!Utils.AreEqual.IntLists(PreviousData, currentData))
//            {
//                UpdateOutput = true;
//                PreviousData = currentData;

//                var doc = OnPingDocument();
//                doc?.ScheduleSolution(5, Callback);
//            }
//        }

//        private void Callback(GH_Document doc)
//        {
//            if (UpdateOutput) ExpireSolution(false);
//        }

//    }
//}
