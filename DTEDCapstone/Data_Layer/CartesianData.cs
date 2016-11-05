using System;
using System.IO;

using DTEDCapstone.Translator;

/*
 * Class handles the converted Cartesian data
 */
namespace DTEDCapstone.Data_Layer
{
    class CartesianData
    {
        /*The raw lat, lon, height data read from the file*/
        private MappedData[,] data;

        /*Converted Data*/
        private CartesianPoint[,] cartData;

        /*Construct object and convert the data*/
        public CartesianData(MappedData[,] data)
        {
            this.data = data;
            Convert();
        }

        /*Convert the raw data using GEOTRANS native api support*/
        public void Convert()
        {
            int mid1 = data.GetLength(0) / 2;
            int mid2 = data.GetLength(1) / 2;
            MappedData pt = data[mid1, mid2];
            double lat = pt.Latitude.getDecimalDegree();
            double lng = pt.Longitude.getDecimalDegree();
            double cx = Math.Cos(lng);
            double cy = Math.Cos(lat);
            double sx = Math.Sin(lng);
            double sy = Math.Sin(lat);
            //Components of the vector pointing up at this point
            double Zx = cx * cy;
            double Zy = sx * cy;
            double Zz = sy;
            //Components of the vector pointing East at this point
            double Xx = -sx;
            double Xy = cx;
            double Xz = 0;
            //Components of the vector pointing North at this point
            double Yx = -cx * sy;
            double Yy = -sx * sy;
            double Yz = cy;
            //Translate each point in the matrix
            cartData = new CartesianPoint[data.GetLength(0), data.GetLength(1)];
            for (int i = 0; i < cartData.GetLength(1); i++)
            {
                for (int j = data.GetLength(0) - 1; j >= 0; j--)
                {
                    CartesianPoint tmp = Translate.Convert(data[j, i].Latitude.getDecimalDegree(), data[j, i].Longitude.getDecimalDegree(), data[j, i].Elevation);
                    cartData[j, i].X = Xx * tmp.X + Xy * tmp.Y + Xz * tmp.Z;
                    cartData[j, i].Y = Yx * tmp.X + Yy * tmp.Y + Yz * tmp.Z;
                    cartData[j, i].Z = Zx * tmp.X + Zy * tmp.Y + Zz * tmp.Z;
                }
            }
        }

        /*Return converted data*/
        public CartesianPoint[,] getCartesianData()
        {
            return cartData;
        }

        /*Export the converted data to a .csv*/
        public void Export(string fileName)
        {
            String myPoint;

            //Creates file if it doesn't exist, "false" will overwrite data in file if it does exist
            using (StreamWriter stream = new StreamWriter(fileName, false))
            {

                for (int i = 0; i < cartData.GetLength(0); i++)
                {
                     for (int j = 0; j < cartData.GetLength(1); j++)
                     {
                         // Grab x,y,z points to be added to CSV file
                         myPoint = cartData[i, j].X.ToString() + ", " + cartData[i, j].Y.ToString() + ", "
                               + cartData[i, j].Z.ToString();

                         stream.WriteLine(myPoint);
                     }
                }
                stream.Close();
            }
        }
    }
}