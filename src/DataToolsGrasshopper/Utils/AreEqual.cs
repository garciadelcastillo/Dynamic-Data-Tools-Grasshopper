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

namespace DataToolsGrasshopper.Utils
{
    internal static class AreEqual
    {
        internal static bool IntLists(List<int> A, List<int> B)
        {
            if (A.Count != B.Count)
            {
                return false;
            }

            for (int i = 0; i < A.Count; i++)
            {
                if (A[i] != B[i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}
