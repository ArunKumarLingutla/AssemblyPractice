using NXOpen;
using NXOpen.Assemblies;
using NXOpen.CAE;
using NXOpen.UF;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyPractice
{
    public class AssemblyUtilities
    {
        public static void CreateNewAssemPart(string fileNameWithoutExt, string outputDir)
        {
            NXOpen.Session theSession = NXOpen.Session.GetSession();
            NXOpen.FileNew fileNew1 = theSession.Parts.FileNew();

            fileNew1.TemplateFileName = "assembly-mm-template.prt";
            fileNew1.ApplicationName = "AssemblyTemplate";
            fileNew1.Units = NXOpen.Part.Units.Millimeters;
            fileNew1.UsesMasterModel = "No";
            fileNew1.MakeDisplayedPart = true;

            fileNew1.TemplateType = NXOpen.FileNewTemplateType.Item;

            //fileNew1.TemplatePresentationName = "Assembly";
            fileNew1.NewFileName = Path.Combine(outputDir, fileNameWithoutExt + ".prt");
            //fileNew1.DisplayPartOption = NXOpen.DisplayPartOption.AllowAdditional;

            NXOpen.NXObject nXObject1;
            nXObject1 = fileNew1.Commit();

            fileNew1.Destroy();
        }
        public static void CreateNewPartUsingNewDisplayMethod(string fileNameWithoutExt, string outputDir)
        {
            NXOpen.Session theSession = NXOpen.Session.GetSession();
            Part part = theSession.Parts.NewDisplay(Path.Combine(outputDir, fileNameWithoutExt + ".prt"), NXOpen.Part.Units.Millimeters);
            theSession.Parts.SetWork(part); theSession.Parts.SetDisplay(part, false, false, out _); // Optionally, you can set the template file if needed // part.TemplateFileName = "assembly-mm-template.prt"; // part.UsesMasterModel = false; // Set to true if you want to use a master model // part.MakeDisplayedPart = true; // Set to true if you want to display the part immediately
        }
        public static void AddComponentToWorkPart(string partFilePath)
        {
            Part workPart=Session.GetSession().Parts.Work; 
            Part displayPart=Session.GetSession().Parts.Display; 

            string refSetName = "MODEL";
            string compName= Path.GetFileNameWithoutExtension(partFilePath);
             
            ComponentAssembly componentAssembly = workPart.ComponentAssembly;
            PartLoadStatus partLoadStatus = null;
            Point3d point3D = new Point3d(0, 0, 0);
            Matrix3x3 matrix3X3 = new Matrix3x3();
            matrix3X3.Xx = 1; matrix3X3.Xy = 0; matrix3X3.Xz = 0;
            matrix3X3.Yx = 0; matrix3X3.Yy = 1; matrix3X3.Yz = 0;
            matrix3X3.Zx = 0; matrix3X3.Zy = 0; matrix3X3.Zz = 1;

            componentAssembly.AddComponent(partFilePath,refSetName,compName,point3D,matrix3X3,1,out partLoadStatus);
        }
    }
}
