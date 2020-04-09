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
        private List<GH_Structure<IGH_Goo>> data;
        private bool recordToggle;
        private bool justReset;
        private int dataLimit;

        public DataRecorder() : base(
            "Data Recorder", 
            "Data Recorder", "Stores changing data persistently, with options as parameters.")
        {
            data = new List<GH_Structure<IGH_Goo>>();
            recordToggle = true;
            justReset = false;
            dataLimit = 0;
        }

        public override GH_Exposure Exposure => GH_Exposure.secondary;
        protected override System.Drawing.Bitmap Icon => Properties.Resources.icons_data_recorder;
        public override Guid ComponentGuid => new Guid("8825ead4-abc6-4b0b-8c72-97f8d7086c25");

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Data", "D", "Data to record.", GH_ParamAccess.tree);
            pManager.AddBooleanParameter("Record", "R", "Record data?", GH_ParamAccess.item, true);
            pManager.AddBooleanParameter("Reset", "C", "Clear stored data?", GH_ParamAccess.item, false);
            pManager.AddIntegerParameter("Max", "M", "Max number of elements to store? Use 0 for no limit.",
                GH_ParamAccess.item, 0);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Data", "D", "Recorded data", GH_ParamAccess.tree);
        }

        protected override void SolveInstance(IGH_DataAccess access)
        {
            // Maybe Data input could be optional...?
            // Don't exit if no data, we want to check against the empty structure
            access.GetDataTree(0, out GH_Structure<IGH_Goo> datum);

            bool isRecording = true;
            access.GetData(1, ref isRecording);

            bool shouldReset = false;
            access.GetData(2, ref shouldReset);

            int lim = 0;
            access.GetData(3, ref lim);

            dataLimit = lim;

            if (shouldReset)
            {
                data = new List<GH_Structure<IGH_Goo>>();
                justReset = true;
                return;
            }
            // If turning off reset, do not record right away (like the vanilla component),
            // but wait for a new data tick.
            else if (justReset)
            {
                justReset = false;
                return;
            }

            if (isRecording)
            {
                if (!recordToggle)
                {
                    // If just turned on recording, don't record this datum (like original component).
                    recordToggle = true;
                }
                else
                {
                    // Add a deep clone to avoid pointer problems.
                    data.Add(new GH_Structure<IGH_Goo>(datum, false));
                }
            }
            else
            {
                recordToggle = false;
            }

            if (dataLimit != 0)
            {
                while (data.Count > dataLimit)
                {
                    data.RemoveAt(0);
                }
            }

            // This is not very optimal, but oh well, at least its O(n)...?
            var merge = new GH_Structure<IGH_Goo>();
            foreach (var tree in data)
            {
                merge.MergeStructure(tree);
            }

            access.SetDataTree(0, merge);
        }

    }
}
