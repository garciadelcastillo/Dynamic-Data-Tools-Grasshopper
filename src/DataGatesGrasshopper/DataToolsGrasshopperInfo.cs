using System;
using System.Drawing;
using Grasshopper.Kernel;

namespace DataToolsGrasshopper
{
    public class DataToolsGrasshopperInfo : GH_AssemblyInfo
    {
        public override string Name
        {
            get
            {
                return "Data Tools";
            }
        }
        public override Bitmap Icon
        {
            get
            {
                //Return a 24x24 pixel bitmap to represent this GHA library.
                return null;
            }
        }
        public override string Description
        {
            get
            {
                //Return a short string describing the purpose of this GHA library.
                return "Helper tools for dynamic data management";
            }
        }
        public override Guid Id
        {
            get
            {
                return new Guid("f455c4b5-14a4-4189-8f06-b3f65f8db033");
            }
        }

        public override string AuthorName
        {
            get
            {
                //Return a string identifying you or your company.
                return "Jose Luis Garcia del Castillo y Lopez";
            }
        }
        public override string AuthorContact
        {
            get
            {
                //Return a string representing your preferred contact details.
                return "https://github.com/garciadelcastillo";
            }
        }
    }
}
