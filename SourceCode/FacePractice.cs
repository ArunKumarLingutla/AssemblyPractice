using NXOpen;
using NXOpen.Routing;
using NXOpen.UF;
using static NXOpen.GeometricUtilities.OrientXpressBuilder;
using static NXOpen.UF.UFPath;

namespace AssemblyPractice
{
    public class FacePractice
    {
        public Face MyFace { get; set; }


        //private static readonly ListingWindow lw = Session.GetSession().ListingWindow;

        public static void GetFaceDetails(Face face)
        {
            ListingWindow lw = Session.GetSession().ListingWindow;
            lw.Open();

            lw.WriteLine($"face type: {face.SolidFaceType}");
            foreach (Edge edge in face.GetEdges())
            {

            }

        }
        public FacePractice(Face face)
        {
            MyFace = face;
        }

        /// <summary>
        /// AskFaceData is a method to get the details of the face, such as the type, point, direction, box, radius, radDataForCone and normalDir. It is a static method, which means it can be called without creating an instance of the class. It returns a boolean value indicating whether the operation was successful or not.
        /// </summary>
        /// <param name="face">face for which to get data</param>
        /// <param name="type">face type</param>
        /// <param name="point">face point. Point information is returned according to the of type as follows.
        ///Plane = Position in plane
        ///Cylinder = Position on axis
        ///Cone = Position on axis
        ///Sphere = Center position
        ///Torus = Center position
        ///Revolved = Position on axis</param>
        /// <param name="direction">face direction. Direction information is returned according to the value of type as follows.
        ///Plane = Normal direction
        ///Axis direction = Cylinder, Cone, Torus, Revolved </param>
        /// <param name="box">The coordinates of the opposite corners of a rectangular box with sides parallel to X,Y, and Z axes(Absolute Coordinate System) arereturned.</param>
        /// <param name="radius">face radius</param>
        /// <param name="radDataForCone">radial data for cone</param>
        /// <param name="normalDir"> 1 if outwards, -1 if inwards</param>
        /// <returns>true if successful, false otherwise</returns>
        public static bool AskFaceData(
            Face face,
            out int type,
            out double[] point,
            out double[] direction,
            out double[] box,
            out double radius,
            out double radDataForCone,
            out int normalDir)
        {
            Session Session = Session.GetSession();
            UFSession theUFSession = UFSession.GetUFSession();
            //ListingWindow lw = Session.GetSession().ListingWindow;
            //lw.Open();

            bool status = true;
            type = 0;
            point = new double[3];
            direction = new double[3];
            box = new double[6];
            radius = 0.0;
            radDataForCone = 0.0;
            normalDir = 0;

            try
            {
                theUFSession.Modl.AskFaceData(
                        face.Tag,
                        out type,
                        point,
                        direction,
                        box,
                        out radius,
                        out radDataForCone,
                        out normalDir);
            }
            catch (NXException ex)
            {
                //lw.WriteLine($"Exception at asking face data: {ex.Message}");
                NXLogger.Instance.Log($"Exception at asking face data: {ex.Message}", LogLevel.Error);
                status = false;
            }
            finally {  }
            return status;
        }

        public static bool AskFaceProps(
            Face face,
            double[] param,
            out double[] pointAtParam,
            out double[] u1,
            out double[] v1,
            out double[] u2,
            out double[] v2,
            out double[] unitNormal,
            out double[] radii)
        {
            UFSession theUFSession = UFSession.GetUFSession();
            bool status = true;

            pointAtParam = new double[3];
            u1 = new double[3]; v1 = new double[3];
            u2 = new double[3]; v2 = new double[3]; 
            unitNormal = new double[3];
            radii = new double[2];

            try
            {
                theUFSession.Modl.AskFaceProps(
                        face.Tag,
                        param,
                        pointAtParam,
                        u1, v1,
                        u2, v2,
                        unitNormal,
                        radii);
            }
            catch (NXException ex)
            {
                NXLogger.Instance.Log(ex.Message);
                status= false;
            }
            return status;
        }
        /// <summary>
        /// Gets the minimum and maximum UV coordinates of a face. 
        /// </summary>
        /// <param name="face"></param>
        /// <param name="uvMinMax">uvMinMax[0] = Umin, uvMinMax[1] = Umax, uvMinMax[2] = Vmin, uvMinMax[3] = Vmax</param>
        /// <returns></returns>
        public static bool AskFaceUVMinMax(Face face,out double[] uvMinMax)
        {
            UFSession theUFSession = UFSession.GetUFSession();
            bool status = true;

            uvMinMax= new double[4];
            try
            {
                theUFSession.Modl.AskFaceUvMinmax(face.Tag, uvMinMax);
            }
            catch (NXException ex)
            {
                status=false;
            }
            return status;
        }
    }
}
