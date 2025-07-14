using System;
using System.Drawing;
using Grasshopper.Kernel;

namespace TukuangGH
{
    public class TukuangGHInfo : GH_AssemblyInfo
    {
        public override string Name
        {
            get
            {
                return "TukuangGH";
            }
        }
        public override Bitmap Icon
        {
            get
            {
                return Properties.Resources.TukuangIcon;
            }
        }
        public override string Description
        {
            get
            {
                return "Grasshopper components for generating paper frames from DWG files";
            }
        }
        public override Guid Id
        {
            get
            {
                return new Guid("87654321-4321-8765-CBA9-987654321ABC");
            }
        }

        public override string AuthorName
        {
            get
            {
                return "Tukuang Developer";
            }
        }
        public override string AuthorContact
        {
            get
            {
                return "contact@tukuang.com";
            }
        }
    }
}