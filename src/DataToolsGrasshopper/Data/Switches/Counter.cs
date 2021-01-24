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

namespace DataToolsGrasshopper.Data.Switches
{
    public class Counter : GHDataComponent
    {
        private int _count;
        private bool _prevReset;

        public Counter() : base(
           "Counter",
           "Counter", "Every time this component is ticked, it will update by 1 unit.")
        {
            _count = 0;
            _prevReset = false;
        }

        public override GH_Exposure Exposure => GH_Exposure.tertiary;
        protected override System.Drawing.Bitmap Icon => null;
        public override Guid ComponentGuid => new Guid("8825ead4-abc6-4b0b-8c72-97f8d7086c25");

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("D", "DataTicker", "Input data. The nature of this data is irrelevant, it is only used as a ticker to update this component.", GH_ParamAccess.tree);
            pManager[0].Optional = true;

            pManager.AddBooleanParameter("On", "O", "On?", GH_ParamAccess.item, true);
            pManager.AddBooleanParameter("Reset", "R", "Reset count?", GH_ParamAccess.item, false);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddIntegerParameter("Count", "C", "Counter data.", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess access)
        {
            //// Is this even needed?
            //access.GetDataTree(0, out GH_Structure<IGH_Goo> datum);



        }

    }
}
