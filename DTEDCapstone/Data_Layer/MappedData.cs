namespace DTEDCapstone.Data_Layer
{
    /* Class contains data from DTED file
    * that maps elevation values to the respective
    * latitude/longitude posts. This object will then
    * passed to a converter that takes the coordinates and 
    * changes them to cartesian coordinate system.
    *
    */
    public class MappedData
    {
        /* The latitude value for the post*/
        private Latitude latitude;

        /* Property */
        public Latitude Latitude
        {
            get
            {
                return latitude;
            }
        }

        /* The longitude value for the post */
        private Longitude longitude;

        /* Property */
        public Longitude Longitude
        {
            get
            {
                return longitude;
            }
        }

        /* The elevation vale for the post */
        private int elevation;

        /* Property */
        public int Elevation
        {
            get
            {
                return elevation;
            }
        }

        /* Construct object that joins latitude, longitude, and elevation 
        * as one piece of data */
        public MappedData(Latitude lat, Longitude lon, int elev)
        {
            latitude = lat;
            longitude = lon;
            elevation = elev;
        }
    }
}
