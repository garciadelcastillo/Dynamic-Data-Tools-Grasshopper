//using System;
//using System.Collections.Generic;

//using Grasshopper.Kernel;
//using Rhino.Geometry;

//namespace DataToolsGrasshopper.Switches
//{
//    public class SwitchOff : GH_Component
//    {
//        private bool UpdateOutput { get; set; }
//        private bool PreviousData { get; set; }

//        private bool PreviousReading { get; set; }


//        private int delay; 

//        public SwitchOff()
//          : base("SwitchOff ", "SwitchOff",
//              "Whenever this component is triggered true, it will output true and then switch to false after x milliseconds",
//              "Data Tools", "Switches")
//        {
//        }

//        protected override System.Drawing.Bitmap Icon => null;
//        public override Guid ComponentGuid => new Guid("b128abc1-06f6-4aab-b4d5-bca97398b5eb");

//        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
//        {
//            pManager.AddBooleanParameter("trigger", "T", "", GH_ParamAccess.item);
//            pManager.AddIntegerParameter("delay", "D", "", GH_ParamAccess.item, 100);
//        }

//        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
//        {
//            pManager.AddBooleanParameter("on", "T", "", GH_ParamAccess.item);
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

//            bool currentData = false;
//            int del = 0;

//            if (!access.GetData(0, ref currentData)) return;
//            access.GetData(1, ref del);

//            access.SetData(0, currentData);
            
//            delay = del;


//            if (PreviousData == false && currentData == true)
//            {
               
//            }

//        }

//    }
//}