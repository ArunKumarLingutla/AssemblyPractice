using NXOpen;
using NXOpen.Assemblies;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        public static void CreateAssembly()
        {
            var session = NXOpen.Session.GetSession();
            NXOpen.Part.Units mm = NXOpen.Part.Units.Millimeters;
            NXOpen.Part doorAssy = session.Parts.NewDisplay(@"C:\Temp\doorAssy.prt", mm);
            session.Parts.SetWork(doorAssy);
            NXOpen.Assemblies.ComponentAssembly compAssy = doorAssy.ComponentAssembly;
            PartLoadStatus status = null;
            NXOpen.Point3d origin = new NXOpen.Point3d(0, 0, 0);
            int layers = -1;
            // Create an identity matrix to use for orientation 
            NXOpen.Matrix3x3 matrix = new NXOpen.Matrix3x3();
            matrix.Xx = 1; matrix.Xy = 0; matrix.Xz = 0;
            matrix.Yx = 0; matrix.Yy = 1; matrix.Yz = 0;
            matrix.Zx = 0; matrix.Zy = 0; matrix.Zz = 1;
            // Add the two parts to the assembly 
            var refSetName = "MODEL";
            var partFilePath = @"C:\Temp\door.prt";
            string compName = "doorComp";
            compAssy.AddComponent(partFilePath, refSetName, compName, origin, matrix, layers, out status);
            partFilePath = @"C:\Temp\grip.prt";
            compName = "gripComp";
            compAssy.AddComponent(partFilePath, refSetName, compName, origin, matrix, layers, out status);
        }
        public static void CreateNewAssemblyPartUsingNewDisplayMethod(string fileNameWithoutExt, string outputDir)
        {
            NXOpen.Session theSession = NXOpen.Session.GetSession();
            Part part = theSession.Parts.NewDisplay(Path.Combine(outputDir, fileNameWithoutExt + ".prt"), NXOpen.Part.Units.Millimeters);
            theSession.Parts.SetWork(part); theSession.Parts.SetDisplay(part, false, false, out _); // Optionally, you can set the template file if needed // part.TemplateFileName = "assembly-mm-template.prt"; // part.UsesMasterModel = false; // Set to true if you want to use a master model // part.MakeDisplayedPart = true; // Set to true if you want to display the part immediately
            NXOpen.Assemblies.ComponentAssembly compAssy = part.ComponentAssembly;
        }
        public static void AddComponentToWorkPart(string partFilePath)
        {
            Part workPart = Session.GetSession().Parts.Work;
            Part displayPart = Session.GetSession().Parts.Display;

            string refSetName = "MODEL";
            string compName = Path.GetFileNameWithoutExtension(partFilePath);

            ComponentAssembly componentAssembly = workPart.ComponentAssembly;
            PartLoadStatus partLoadStatus = null;
            Point3d point3D = new Point3d(0, 0, 0);
            Matrix3x3 matrix3X3 = new Matrix3x3();
            matrix3X3.Xx = 1; matrix3X3.Xy = 0; matrix3X3.Xz = 0;
            matrix3X3.Yx = 0; matrix3X3.Yy = 1; matrix3X3.Yz = 0;
            matrix3X3.Zx = 0; matrix3X3.Zy = 0; matrix3X3.Zz = 1;

            componentAssembly.AddComponent(partFilePath, refSetName, compName, point3D, matrix3X3, 1, out partLoadStatus);
        }

        /// <summary>
        /// Gets all the components in the assembly along with sub assemblies in all levels.
        /// </summary>
        /// <param name="subAssemblies"></param>
        /// <returns></returns>
        public static List<NXOpen.Assemblies.Component> GetAllComponents(out List<NXOpen.Assemblies.Component> subAssemblies)
        {
            List<NXOpen.Assemblies.Component> components;
            Part workPart = Session.GetSession().Parts.Work;
            Part displayPart = Session.GetSession().Parts.Display;

            //Get all parts open in the session
            //If it is an assembly, it gets all the components along with assembly, if ypu have 2 parts in an assembly it will give 3
            //PartCollection partList=theSession.Parts;
            //lw.WriteLine(Convert.ToString(partList.ToArray().Length));

            //Getting all the sub assemblies and components in the assembly
            subAssemblies = new List<Component>();
            components = new List<Component>();

            try
            {
                //If the display part is not an assembly than root component will be null
                if (displayPart.ComponentAssembly.RootComponent is null)
                {
                    NXLogger.Instance.Log("It is not an assembly");
                }
                else
                {
                    NXOpen.Assemblies.Component rootComponent = displayPart.ComponentAssembly.RootComponent;

                    //Going layer by level by level and getting all the child components
                    //storing current level components into "currentLevelComponents" list and traversing them to get their child components
                    //storing the child components in allChildComponents and making those child components into current level components and continuing the process
                    List<Component> currentLevelComponents = rootComponent.GetChildren().ToList();
                    List<Component> allChildComponents = rootComponent.GetChildren().ToList();
                    while (true)
                    {
                        //Getting child components of current level and adding it to allChildComponents, if there are no child components loop terminates
                        List<Component> childComponentsOfCurrentLevelComponents = currentLevelComponents.SelectMany(x => x.GetChildren()).ToList();
                        if (childComponentsOfCurrentLevelComponents.Count == 0) break;
                        allChildComponents.AddRange(childComponentsOfCurrentLevelComponents);
                        currentLevelComponents = childComponentsOfCurrentLevelComponents;
                    }

                    //saparating assemblies and individual components from allChildComponents
                    subAssemblies = allChildComponents.Where(x => x.GetChildren().Length != 0).ToList();
                    components = allChildComponents.Where(x => x.GetChildren().Length == 0).ToList();

                }
                
            }
            catch (Exception ex)
            {
                NXOpen.UI.GetUI().NXMessageBox.Show("Error", NXMessageBox.DialogType.Error, Convert.ToString(ex.Message));
            }
            return components;
        }
        public static List<Face> GetFaceInComponent(Component component, string faceType, string inwardOrOutward = "all")
        {
            List<Face> reqFaces = new List<Face>();
            int reqNormalDir = 0;
            if (inwardOrOutward.ToLower() == "inward")
            {
                reqNormalDir = -1;
            }
            if (inwardOrOutward.ToLower() == "outward")
            {
                reqNormalDir = 1;
            }
            NXLogger.Instance.Log("Component: " + component.DisplayName);
            Part p = (Part)component.Prototype;
            foreach (Body body in p.Bodies)
            {
                NXLogger.Instance.Log("Body Type: " + body.GetType().ToString());

                foreach (var face in body.GetFaces())
                {
                    if (face.SolidFaceType.ToString().ToLower() == faceType.ToLower())
                    {
                        NXLogger.Instance.Log("Fcae Type: " + face.SolidFaceType.ToString());
                        FacePractice.AskFaceData(face, out int type, out double[] point, out double[] direction, out double[] box, out double radius, out double radDataForCone, out int normalDir);
                        if (normalDir == reqNormalDir)
                        {
                            reqFaces.Add((Face)component.FindOccurrence(face));
                        }

                    }
                }
            }
            return reqFaces;
        }
        public static void CreateConcentricConstraint(NXOpen.Assemblies.Component comp1, Edge edge1, NXOpen.Assemblies.Component comp2, Edge edge2)
        {
            Part workPart = Session.GetSession().Parts.Work;

            NXOpen.Positioning.ComponentPositioner componentPositioner1 = workPart.ComponentAssembly.Positioner;

            componentPositioner1.ClearNetwork();

            componentPositioner1.BeginAssemblyConstraints();

            NXOpen.Positioning.Network network1 = componentPositioner1.EstablishNetwork();

            NXOpen.Positioning.ComponentNetwork componentNetwork1 = (NXOpen.Positioning.ComponentNetwork)network1;
            componentNetwork1.MoveObjectsState = true;

            NXOpen.Positioning.Constraint constraint1 = componentPositioner1.CreateConstraint(true);

            NXOpen.Positioning.ComponentConstraint componentConstraint1 = (NXOpen.Positioning.ComponentConstraint)constraint1;
            componentConstraint1.ConstraintType = NXOpen.Positioning.Constraint.Type.Concentric;

            NXOpen.Positioning.ConstraintReference constraintReference1 = componentConstraint1.CreateConstraintReference(comp1, edge1, false, false, false);

            //NXOpen.Point3d helpPoint1 = new NXOpen.Point3d(99.733099149140102, -9.480644901513033, 58.783556687660315);
            //constraintReference1.HelpPoint = helpPoint1;

            NXOpen.Positioning.ConstraintReference constraintReference2 = componentConstraint1.CreateConstraintReference(comp2, edge2, false, false, false);

            //NXOpen.Point3d helpPoint2 = new NXOpen.Point3d(103.02382283243409, -5.9297703173327561, -17.014132902728935);
            //constraintReference2.HelpPoint = helpPoint2;

            componentNetwork1.Solve();

            componentPositioner1.ClearNetwork();
            componentPositioner1.DeleteNonPersistentConstraints();
            componentPositioner1.EndAssemblyConstraints();
        }
    }
}
