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
    public class PlaneGate : GHGateComponent<GH_Plane>
    {
        public PlaneGate()
          : base("Plane Gate",
              "Plane Gate",
              "Will let Plane data trough only if it changed since the previous solution. Works at the full Data Tree level.")
        { }

        public override GH_Exposure Exposure => GH_Exposure.primary;
        protected override System.Drawing.Bitmap Icon => Properties.Resources.icons_plane_gate;
        public override Guid ComponentGuid => new Guid("2793424d-4318-43c9-93d1-418a0dac73af");

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddPlaneParameter("Pl", "P", "Input Plane data.", GH_ParamAccess.tree);
            pManager[0].Optional = true;  // avoid "failed to collect data" 
            pManager.AddNumberParameter("Epsilon", "E", "Precision threshold for coordinate comparison", GH_ParamAccess.item, 0.0001);
            pManager[1].Optional = true;
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddPlaneParameter("Pl", "P", "Output will update only if input changed.", GH_ParamAccess.tree);
        }
    }
}
