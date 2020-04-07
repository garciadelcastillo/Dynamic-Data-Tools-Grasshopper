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
    public class NumberGate : GHDataComponent<GH_Number>
    {
        public NumberGate()
          : base("Number Gate",
              "Number Gate",
              "Will let Number data trough only if it changed since the previous solution. Works at the full Data Tree level.")
        { }

        public override GH_Exposure Exposure => GH_Exposure.primary;
        protected override System.Drawing.Bitmap Icon => Properties.Resources.icons_number_gate;
        public override Guid ComponentGuid => new Guid("5a4d12ca-113e-41be-ba56-33a77ea0541a");

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("Num", "N", "Input Number data.", GH_ParamAccess.tree);
            pManager[0].Optional = true;  // avoid "failed to collect data" 
            pManager.AddNumberParameter("Epsilon", "E", "Precision threshold for number comparison", GH_ParamAccess.item, 0.0001);
            pManager[1].Optional = true;
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddNumberParameter("Num", "N", "Output will update only if input changed.", GH_ParamAccess.tree);
        }
    }
}
