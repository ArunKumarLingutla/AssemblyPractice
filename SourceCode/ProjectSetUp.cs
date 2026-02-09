using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyPractice
{
    public class ProjectSetUp
    {
        public static void InitializeTool()
        {
            string BaseDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName;
            ProjectVariables.InputDirectory = Path.Combine(BaseDirectory, "Input");
            ProjectVariables.OutputDirectory = Path.Combine(BaseDirectory, "Output");
            
            if (!Directory.Exists(ProjectVariables.OutputDirectory))
            {
                Directory.CreateDirectory(ProjectVariables.OutputDirectory);
            }
            else
            {
                FileManager.DeleteFilesInDirectory(ProjectVariables.OutputDirectory);
            }
            NXLogger.Init(System.IO.Path.Combine(ProjectVariables.OutputDirectory, "NXLog"), true, true);
            NXLogger.Instance.Log("Tool initialized with input directory: " + ProjectVariables.InputDirectory, LogLevel.Info);
            NXLogger.Instance.Log("Output directory set to: " + ProjectVariables.OutputDirectory, LogLevel.Info);
        }
    }
}
