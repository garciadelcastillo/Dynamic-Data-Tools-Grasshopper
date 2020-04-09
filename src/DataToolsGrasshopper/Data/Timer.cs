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
    public class Timer : GHDataComponent
    {
        private int counter = 0;

        public Timer() : base(
            "Timer",
            "Timer", "Emits a data update signal at regular time intervals. Can be used as a ticker.")
        { }

        public override GH_Exposure Exposure => GH_Exposure.tertiary;
        protected override System.Drawing.Bitmap Icon => null;
        public override Guid ComponentGuid => new Guid("a533d2d3-7e7c-4e93-9080-4fa30c19247a");

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddIntegerParameter("T", "Time", "Update period time in milliseconds.", GH_ParamAccess.item, 1000);
            pManager.AddBooleanParameter("O", "On", "Is this timer on?", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddIntegerParameter("Tick", "T", "Data tick.", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess access)
        {
            int millis = 1000;
            if (!access.GetData(0, ref millis)) return;
            bool active = true;
            if (!access.GetData(1, ref active)) return;

            // Some sanity
            if (millis < 5)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Refresh interval is too small.");
                return;
            }

            // Otherwise, back to regular autoupdate
            if (active)
            {
                // Output some data
                access.SetData(0, counter++);

                // Schedule a new solution
                this.OnPingDocument().ScheduleSolution(millis, doc => {
                    this.ExpireSolution(false);
                });
            }
        }

    }
}
