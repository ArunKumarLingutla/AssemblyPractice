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
    }
}
