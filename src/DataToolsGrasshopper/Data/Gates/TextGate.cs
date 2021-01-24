using System;
using System.Collections.Generic;

using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

using DataToolsGrasshopper.Utils;

namespace DataToolsGrasshopper.Data
{
    public class TextGate : GHGateComponent<GH_String>
    {
        public TextGate()
          : base("Text Gate",
              "Text Gate",
              "Will let Text data trough only if it changed since the previous solution. Works at the full Data Tree level.")
        { }

        public override GH_Exposure Exposure => GH_Exposure.primary;
        protected override System.Drawing.Bitmap Icon => Properties.Resources.icons_text_gate;
        public override Guid ComponentGuid => new Guid("d4d2e05d-6cc5-415d-83b5-8105b4f1cbe2");

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Txt", "T", "Input Text data.", GH_ParamAccess.tree);
            pManager[0].Optional = true;  // avoid "failed to collect data" 
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Txt", "T", "Output will update only if input changed.", GH_ParamAccess.tree);
        }
    }
}
