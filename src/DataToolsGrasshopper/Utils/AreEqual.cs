using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataToolsGrasshopper.Utils
{
    internal class AreEqual
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
