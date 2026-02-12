using NXOpen;
using NXOpen.UF;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyPractice
{
    public class AssemblyPractice
    {
        //class members
        private static NXOpen.Session theSession = null;
        private static NXOpen.UF.UFSession theUFSession = null;
        private static NXOpen.UI theUI = null;
        public static UI_Part_Selection theUI_Part_Selection = null;
        public static ProjectVariables projVariablesObj = null;

        public static void Main(string[] args)
        {
            try
            {
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                //AssemblyUtilities.CreateNewAssemPart("AssemblyPractice", desktopPath);
                AssemblyUtilities.CreateNewPartUsingNewDisplayMethod("AssemblyPractice", desktopPath);
                theSession = Session.GetSession();
                theUFSession = NXOpen.UF.UFSession.GetUFSession();
                theUI = NXOpen.UI.GetUI();
                NXOpen.Part workPart = theSession.Parts.Work;
                NXOpen.Part displayPart = theSession.Parts.Display;
                ProjectSetUp.InitializeTool();

                //Write your code here
                if (!theSession.IsBatch && projVariablesObj==null)
                {
                    projVariablesObj = new ProjectVariables();

                    theUI_Part_Selection = new UI_Part_Selection(projVariablesObj);
                    // The following method shows the dialog immediately
                    theUI_Part_Selection.Show(); 
                }
                UI.GetUI().NXMessageBox.Show("DLX Path", NXMessageBox.DialogType.Information, projVariablesObj.FolderWithParts);
                Directory.GetFiles(projVariablesObj.FolderWithParts);
                foreach (var item in Directory.GetFiles(projVariablesObj.FolderWithParts))
                {
                    if (item.EndsWith(".prt"))
                    {
                        AssemblyUtilities.AddComponentToWorkPart(item);
                        NXLogger.Instance.Log("Added component: " + item);
                    }
                }
            }
            catch (Exception ex)
            {
                NXLogger.Instance.LogException(ex);
                NXLogger.Instance.Dispose();
                throw;
            }
            finally
            {
                if (theUI_Part_Selection != null)
                    theUI_Part_Selection.Dispose();
                theUI_Part_Selection = null;
            }
        }

        public static int GetUnloadOption(string arg)
        {
            //return System.Convert.ToInt32(Session.LibraryUnloadOption.Explicitly);
            return System.Convert.ToInt32(Session.LibraryUnloadOption.Immediately);
            // return System.Convert.ToInt32(Session.LibraryUnloadOption.AtTermination);
        }

        //------------------------------------------------------------------------------
        // Following method cleanup any housekeeping chores that may be needed.
        // This method is automatically called by NX.
        //------------------------------------------------------------------------------
        public static void UnloadLibrary(string arg)
        {
            try
            {
                //---- Enter your code here -----
            }
            catch (Exception ex)
            {
                //---- Enter your exception handling code here -----
                theUI.NXMessageBox.Show("Main", NXMessageBox.DialogType.Error, ex.ToString());
            }
        }
    }
}

/*
 1. creating a point (utilities)
 2. 
 
 */
