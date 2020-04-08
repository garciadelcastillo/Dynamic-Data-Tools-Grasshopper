using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

using DataToolsGrasshopper.Utils;

namespace DataToolsGrasshopper.Data
{
    public class DataRecorder : GHDataComponent
    {
        private List<object> data;
        private bool recordToggle;
        private bool justReset;
        private int limit;

        public DataRecorder() : base(
            "Data Recorder", 
            "Data Recorder", "Stores changing data persistently, with options as parameters.")
        {
            data = new List<object>();
            recordToggle = false;
            justReset = false;
            limit = 0;
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.icons_data_recorder;
        public override Guid ComponentGuid => new Guid("8825ead4-abc6-4b0b-8c72-97f8d7086c25");

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Data", "D", "Data to record.", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Record", "R", "Record data?", GH_ParamAccess.item, true);
            pManager.AddBooleanParameter("Reset", "C", "Clear stored data?", GH_ParamAccess.item, false);
            pManager.AddIntegerParameter("Max", "M", "Max number of elements to store? Use 0 for no limit.",
                GH_ParamAccess.item, 0);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("data", "D", "Recorded data", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            object datum = null;
            bool record = true;
            bool reset = false;
            int lim = 0;

            if (!DA.GetData(0, ref datum)) return;
            if (!DA.GetData(1, ref record)) return;
            if (!DA.GetData(2, ref reset)) return;
            if (!DA.GetData(3, ref lim)) return;

            limit = lim;

            if (reset)
            {
                data = new List<object>();
                justReset = true;
                return;
            }
            // If turning off reset, do not record right away (like original component), 
            // but wait for a new data tick.
            else if (justReset)
            {
                justReset = false;
                return;
            }

            if (record)
            {
                if (!recordToggle)
                {
                    // If just turned on recording, don't record this datum (like original component).
                    recordToggle = true;
                }
                else
                {
                    // Store null items too, can be cleaned with another component
                    data.Add(datum);
                }
            }
            else
            {
                recordToggle = false;
            }

            if (limit != 0)
            {
                while (data.Count > limit)
                {
                    data.RemoveAt(0);
                }
            }

            DA.SetDataList(0, data);
        }

    }
}
