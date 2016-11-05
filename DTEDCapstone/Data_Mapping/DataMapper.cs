using DTEDCapstone.Data_Layer;

namespace DTEDCapstone.Data_Mapping
{
    /*
    * This class will take in 'DTED_Data' and use the 
    * corresponding information to parse and generate 
    * data that maps the elevation data to the 
    * latitude and longitude posts.
    */
    public class DataMapper
    {
        /* Data member represents file data that will be parsed and 
        * used to generate data */
        private DTED_Data fileData;

        /* Constructs 'DataMapper' with DTED file data */
        public DataMapper(DTED_Data data)
        {
            fileData = data;
        }

        /* 
        * Function uses 'fileData' data member to parse and generate
        * a 'MappedData' objects and place them in a 2D array
        */
        public MappedData[,] map()
        {
            // First, the latitude and longitude intervals must be listed!
            int numberLat;
            double latitudeInterDecimal;
            int numberLon;
            double longitudeInterDecimal;

            // Retrieve data from the header of DTED to create latitude/longitude grid
            numberLat = fileData.Header.NumberLatitudeLines;
            latitudeInterDecimal = fileData.Header.LatitudeInterval / 3600 * .0175; // Get interval of latitude in decimal value
            numberLon = fileData.Header.NumberLongitudeLines;
            longitudeInterDecimal = fileData.Header.LongitudeInterval / 3600 * .0175; // Get interval of longitude in decimal value

            // Get decimal degree of the origin point for cell
            double latDecimal = fileData.Header.LatitudeOrigin.getDecimalDegree();
            double lonDecimal = fileData.Header.LongitudeOrigin.getDecimalDegree();

            double[] latValues = new double[numberLat];
            double[] lonValues = new double[numberLon];

            MappedData[,] mappedData = new MappedData[numberLat, numberLon];

            // Generate latitude values for grid based on interval from file
            for(int i = 0; i < numberLat; ++i)
            {
                latValues[i] = latDecimal;
                latDecimal += latitudeInterDecimal;
            }

            // Generate longitude values for grid based on interval from file
            for(int i = 0; i < numberLon; ++i)
            {
                lonValues[i] = lonDecimal;
                lonDecimal += longitudeInterDecimal;
            }

            int[][] elevData = fileData.ElevationGrid; // Get elevation data to map onto latitude and longitude posts
            int elev; // Hold particular elevation from the data
            Latitude lat; // Latitude at post
            Longitude lon; // Longitude at post

            // Generate latitude and longitude grid data mapped to elevation

            // Must go along longitude lines as outer loop from left to right
            for (int i = 0; i < numberLon; ++i)
            {
                // Starting with lowest latitude and work to the top from bottom to top
                for(int j = 0; j < numberLat; ++j)
                {
                    elev = elevData[i][j];
                    lat = new Latitude(latValues[j]);
                    lon = new Longitude(lonValues[i]);
                    mappedData[j, i] = new MappedData(lat, lon, elev);
                }
            }
            
            return mappedData;
        }
    }
}
