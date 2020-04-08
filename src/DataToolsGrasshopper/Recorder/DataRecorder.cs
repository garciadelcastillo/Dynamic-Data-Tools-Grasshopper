//using System;
//using System.Collections.Generic;

//using Grasshopper.Kernel;
//using Rhino.Geometry;

//namespace DataToolsGrasshopper.Recorder
//{
//    public class DataRecorder : GH_Component
//    {
//        private List<object> data;
//        private bool recordToggle;
//        private bool justReset;
//        private int limit;

//        public DataRecorder() : base(
//            "DataRecorder",
//            "DataRecorder",
//            "Stores data changing over time, with options as parameters.",
//            "Data Tools",
//            "Recorders")
//        {
//            data = new List<object>();
//            recordToggle = false;
//            justReset = false;
//            limit = 0;
//        }

//        protected override System.Drawing.Bitmap Icon => Properties.Resources.icons_data_recorder;
//        public override Guid ComponentGuid => new Guid("66e704c2-2f85-401a-a107-71267bbdcce8");

//        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
//        {
//            pManager.AddGenericParameter("data", "D", "Data to record.", GH_ParamAccess.item);
//            pManager.AddBooleanParameter("record", "R", "Record data?", GH_ParamAccess.item, true);
//            pManager.AddBooleanParameter("reset", "C", "Clear stored data?", GH_ParamAccess.item, false);
//            pManager.AddIntegerParameter("max", "M", "Max number of elements to store? Use 0 for no limit",
//                GH_ParamAccess.item, 0);
//        }

//        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
//        {
//            pManager.AddGenericParameter("data", "D", "Recorded data", GH_ParamAccess.list);
//        }

//        protected override void SolveInstance(IGH_DataAccess DA)
//        {
//            object datum = null;
//            bool record = true;
//            bool reset = false;
//            int lim = 0;

//            if (!DA.GetData(0, ref datum)) return;
//            if (!DA.GetData(1, ref record)) return;
//            if (!DA.GetData(2, ref reset)) return;
//            if (!DA.GetData(3, ref lim)) return;

//            limit = lim;

//            if (reset)
//            {
//                data = new List<object>();
//                justReset = true;
//                return;
//            }
//            // If turning off reset, do not record right away (like original component), 
//            // but wait for a new data tick.
//            else if (justReset)
//            {
//                justReset = false;
//                return;
//            }

//            if (record)
//            {
//                if (!recordToggle)
//                {
//                    // If just turned on recording, don't record this datum (like original component).
//                    recordToggle = true;
//                }
//                else
//                {
//                    // Store null items too, can be cleaned with another component
//                    data.Add(datum);
//                }
//            }
//            else
//            {
//                recordToggle = false;
//            }

//            if (limit != 0)
//            {
//                while (data.Count > limit)
//                {
//                    data.RemoveAt(0);
//                }
//            }

//            DA.SetDataList(0, data);
//        }

//    }
//}