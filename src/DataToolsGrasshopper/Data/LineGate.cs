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
    public class LineGate : GHGateComponent<GH_Line>
    {
        public LineGate()
          : base("Line Gate",
              "Line Gate",
              "Will let Line data trough only if it changed since the previous solution. Works at the full Data Tree level.")
        { }

        public override GH_Exposure Exposure => GH_Exposure.primary;
        protected override System.Drawing.Bitmap Icon => Properties.Resources.icons_line_gate;
        public override Guid ComponentGuid => new Guid("228a5116-08d5-442b-ab6f-66bb31d6890a");

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddLineParameter("Line", "L", "Input Line data.", GH_ParamAccess.tree);
            pManager[0].Optional = true;  // avoid "failed to collect data" 
            pManager.AddNumberParameter("Epsilon", "E", "Precision threshold for coordinate comparison", GH_ParamAccess.item, 0.0001);
            pManager[1].Optional = true;
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddLineParameter("Line", "L", "Output will update only if input changed.", GH_ParamAccess.tree);
        }
    }
}
