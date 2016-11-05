using System.Runtime.InteropServices;
using System;

/*
 * This class handles the connection to the native GEOTRANS
 * library to convert from geodetic to cartesian
 * coordinates
 */
namespace DTEDCapstone.Translator
{
    /*Struct to hold the x,y,z*/
    public struct CartesianPoint
    {
        public double X, Y, Z;
    }

    class Translate
    {
        /*Import the needed method from the native .dll*/
        /* Using regular code to perform standard geospatial transform
        [DllImport("Translator.dll", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern string ConvertToCartesian(double lat, double lon, double height, double* x, double* y, double* z);
        */

        /*Construct wrapper method around unsafe method call*/
        public static CartesianPoint Convert(double lat, double lon, double height)
        {
            //Point to return
            CartesianPoint returnVal;

            height += 6371000; // Height is in meters, add sea-level radius.
            double frac = Math.Cos(lat);
            returnVal.X = Math.Cos(lon) * frac * height;
            returnVal.Y = Math.Sin(lon) * frac * height;
            returnVal.Z = Math.Sin(lat) * height;

            /*
            //Declare unsafe to make call, use method from native code
            unsafe
            {
                double x = 0;
                double y = 0;
                double z = 0;

                lat *= .0175;
                lon *= .0175;

                ConvertToCartesian(lat, lon, height, &x, &y, &z);

                returnVal.X = x;
                returnVal.Y = y;
                returnVal.Z = z;
            }
            */

            //Return final point
            return returnVal;
        }
    }
}
