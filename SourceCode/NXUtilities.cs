using NXOpen;
using NXOpen.CAE;
using NXOpen.Features;
using NXOpen.UF;
using NXOpen.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NXOpenSetUPCSharp
{
    public class NXUtilities
    {
        /// <summary>
        /// Create a new sketch and activate sketch environment to create sketch. after activating we can create what ever sketch we want
        /// </summary>
        /// <param name="normal">input normal direction for plane on which we want to create sketch</param>
        /// <param name="origin">origin point at which plane should be created</param>
        /// <returns>returns a sketch feature</returns>
        public static Feature ActivateSketch(double[] normal, double[] origin)
        {
            NXOpen.Session theSession = NXOpen.Session.GetSession();
            NXOpen.Part workPart = theSession.Parts.Work;
            NXOpen.Part displayPart = theSession.Parts.Display;
            // ----------------------------------------------
            //   Menu: Insert->Sketch...
            // ----------------------------------------------

            NXOpen.Sketch nullNXOpen_Sketch = null;
            NXOpen.SketchInPlaceBuilder sketchInPlaceBuilder1;
            sketchInPlaceBuilder1 = workPart.Sketches.CreateSketchInPlaceBuilder2(nullNXOpen_Sketch);

            NXOpen.Point3d origin1 = new NXOpen.Point3d(origin[0], origin[1], origin[2]);
            NXOpen.Vector3d normal1 = new NXOpen.Vector3d(normal[0], normal[1], normal[2]);
            NXOpen.Plane plane1;
            plane1 = workPart.Planes.CreatePlane(origin1, normal1, NXOpen.SmartObject.UpdateOption.WithinModeling);

            sketchInPlaceBuilder1.PlaneReference = plane1;

            //NXOpen.Unit unit1 = (NXOpen.Unit)workPart.UnitCollection.FindObject("MilliMeter");
            //NXOpen.Expression expression1;
            //expression1 = workPart.Expressions.CreateSystemExpressionWithUnits("0", unit1);

            //NXOpen.Expression expression2;
            //expression2 = workPart.Expressions.CreateSystemExpressionWithUnits("0", unit1);

            //NXOpen.SketchAlongPathBuilder sketchAlongPathBuilder1;
            //sketchAlongPathBuilder1 = workPart.Sketches.CreateSketchAlongPathBuilder(nullNXOpen_Sketch);

            //sketchAlongPathBuilder1.PlaneLocation.Expression.RightHandSide = "0";

            //theSession.Preferences.Sketch.CreateInferredConstraints = true;

            //theSession.Preferences.Sketch.ContinuousAutoDimensioning = true;

            //theSession.Preferences.Sketch.DimensionLabel = NXOpen.Preferences.SketchPreferences.DimensionLabelType.Expression;

            //theSession.Preferences.Sketch.TextSizeFixed = true;

            //theSession.Preferences.Sketch.FixedTextSize = 3.0;

            //theSession.Preferences.Sketch.DisplayParenthesesOnReferenceDimensions = true;

            //theSession.Preferences.Sketch.DisplayReferenceGeometry = false;

            //theSession.Preferences.Sketch.ConstraintSymbolSize = 3.0;

            //theSession.Preferences.Sketch.DisplayObjectColor = false;

            //theSession.Preferences.Sketch.DisplayObjectName = true;

            NXOpen.NXObject nXObject1;
            nXObject1 = sketchInPlaceBuilder1.Commit();

            NXOpen.Sketch sketch1 = (NXOpen.Sketch)nXObject1;
            NXOpen.Features.Feature feature1;
            feature1 = sketch1.Feature;

            //NXOpen.Session.UndoMarkId markId4;
            //markId4 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "update");

            //int nErrs1;
            //nErrs1 = theSession.UpdateManager.DoUpdate(markId4);

            sketch1.Activate(NXOpen.Sketch.ViewReorient.True);

            sketchInPlaceBuilder1.Destroy();

            //sketchAlongPathBuilder1.Destroy();

            plane1.DestroyPlane();

            return feature1;
        }

        /// <summary>
        /// After activating sketch environment and creating sketch, it should be deactivated
        /// </summary>
        public static void DeActiveSketch()
        {
            NXOpen.Session theSession = NXOpen.Session.GetSession();
            NXOpen.Part workPart = theSession.Parts.Work;
            NXOpen.Part displayPart = theSession.Parts.Display;
            NXOpen.Sketch sketch2;
            sketch2 = theSession.ActiveSketch;

            theSession.ActiveSketch.Deactivate(NXOpen.Sketch.ViewReorient.True, NXOpen.Sketch.UpdateLevel.Model);
        }

        /// <summary>
        /// Converts an NXOpen.Tag to its corresponding TaggedObject instance, later convert or cast to specific types if needed like -- Point nxPoint = (Point)obj;.
        /// </summary>
        /// <remarks>This method uses the NXObjectManager to retrieve the object associated with the given
        /// tag.  Ensure that the tag is valid and corresponds to an existing object in the NXOpen
        /// environment.</remarks>
        /// <param name="tag">The tag representing the object to be retrieved.</param>
        /// <returns>The <see cref="TaggedObject"/> associated with the specified tag, or <see langword="null"/> if the tag does
        /// not correspond to a valid object.</returns>
        public static TaggedObject ConvertTagToObject(NXOpen.Tag tag)
        {
            TaggedObject obj = NXOpen.Utilities.NXObjectManager.Get(tag);
            return obj;

            //
        }

        /// <summary>
        /// Creates a Point feature at the specified coordinates (x, y, z) in the current work part using Builder pattern.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public static Point CreatePointAsFeature(double x, double y, double z)
        {
            NXOpen.Features.Feature nullNXOpen_Features_Feature = null;
            NXOpen.Features.PointFeatureBuilder pointFeatureBuilder1 = NXOpen.Session.GetSession().Parts.Work.BaseFeatures.CreatePointFeatureBuilder(nullNXOpen_Features_Feature);


            // Create Point3d (temporary positions)
            Point3d p1 = new Point3d(x,y,z);
            //Add this to builder
            pointFeatureBuilder1.Point = NXOpen.Session.GetSession().Parts.Work.Points.CreatePoint(p1);
            NXOpen.NXObject nXObject1 = pointFeatureBuilder1.Commit(); //should be committed to view the point in part navigator and graphics window
            Point point=pointFeatureBuilder1.Point;
            pointFeatureBuilder1.Destroy();

            return point;
        }

        /// <summary>
        /// Line will be created in part, but it wont be visible in part navigator
        /// </summary>
        /// <param name="startPoint"></param>
        /// <param name="endPoint"></param>
        /// <returns></returns>
        public static Line CreateBasicLine(double[] startPoint, double[] endPoint)
        {
            Session theSession = Session.GetSession();
            Part workPart = theSession.Parts.Work;

            // Define 3D points
            Point3d p0 = new Point3d(startPoint[0], startPoint[1], startPoint[2]);
            Point3d p1 = new Point3d(endPoint[0], endPoint[1], endPoint[2]);

            // Create line between coordinates
            Line line1 = workPart.Curves.CreateLine(p0, p1);

            //// Create actual NX point objects
            //Point pt0 = workPart.Points.CreatePoint(p0);
            //Point pt1 = workPart.Points.CreatePoint(p1);

            //// Create curve using NX Point objects
            //Line line2 = workPart.Curves.CreateLine(pt0, pt1);

            //// Make visible (default is invisible)
            //line2.SetVisibility(SmartObject.VisibilityOption.Visible);

            return line1;
        }

        /// <summary>
        /// Creates an associative line feature between two points in the current work part using Builder pattern.
        /// </summary>
        /// <param name="startPoint"></param>
        /// <param name="endPoint"></param>
        /// <returns></returns>
        public static Line CreateAssociativeLine(double[] startPoint, double[] endPoint)
        {
            Session theSession = Session.GetSession();
            Part workPart = theSession.Parts.Work;

            NXOpen.Features.AssociativeLine nullAssociativeLine = null;
            AssociativeLineBuilder builder = workPart.BaseFeatures.CreateAssociativeLineBuilder(nullAssociativeLine);

            builder.Associative = true;

            // Start point
            Point3d p0 = new Point3d(startPoint[0], startPoint[1], startPoint[2]);
            Point pt0 = workPart.Points.CreatePoint(p0);
            builder.StartPointOptions = AssociativeLineBuilder.StartOption.Point;
            builder.StartPoint.Value = pt0;

            // End point
            Point3d p1 = new Point3d(endPoint[0], endPoint[1], endPoint[2]);
            Point pt1 = workPart.Points.CreatePoint(p1);
            builder.EndPointOptions = AssociativeLineBuilder.EndOption.Point;
            builder.EndPoint.Value = pt1;

            // Commit feature
            NXObject featureObj = builder.Commit();
            builder.Destroy();

            // Get underlying curve if needed
            AssociativeLine assocLine = (AssociativeLine)featureObj;
            Line line = assocLine.GetEntities()[0] as Line;

            return line;
        }
        public static double MeasureDistance(TaggedObject obj1,TaggedObject obj2)
        {
            UFSession ufs=UFSession.GetUFSession();
            double[] guess1 = new double[3] { 0.0, 0.0, 0.0 };
            double[] guess2 = new double[3] { 0.0, 0.0, 0.0 };

            int guess1_given = 0;
            int guess2_given = 0;

            // Output arrays
            double[] pt_on_obj1 = new double[3];
            double[] pt_on_obj2 = new double[3];
            double min_dist;


            // Call UF function
            ufs.Modl.AskMinimumDist(
                obj1.Tag,
                obj2.Tag,
                guess1_given,
                guess1,
                guess2_given,
                guess2,
                out min_dist,
                pt_on_obj1,
                pt_on_obj2
            );
            return min_dist;
        }
        /// <summary>
        /// deletes the specified TaggedObject(s) from the current work part.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static int DeleteObject(TaggedObject[] obj)
        {
            Session theSession = Session.GetSession();
            string msg = "Deleting object.";
            Session.UndoMarkId undoMarkId = theSession.SetUndoMark(Session.MarkVisibility.Invisible, msg);
            int returnCode1 = theSession.UpdateManager.AddObjectsToDeleteList(obj);
            int returnCode2 = theSession.UpdateManager.DoUpdate(undoMarkId);

            return returnCode2;
        }
    }
}
