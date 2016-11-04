namespace DTEDCapstone.Data_Layer
{
    /* The abstract class will contain data members shared by
     * 'Latitude' and 'Longitude' objects. This will help with 
     * the readablility of the code.
     *
     * NOTE: Make sure to implement abstract methods that are
     * shared between 'Latitude' and 'Longitude' 
     */
    public abstract class GeographicCoordinate
    {
        /* The degree character */
        private const char DEGREE_CHAR = (char) 176;

        /* The degree symbol for output */
        public static readonly string DEGREE_SYMBOL = DEGREE_CHAR.ToString();

        /* Maximum value of latitude */
        public const int MAX_LATITUDE = 90;

        /* Minimum value of latitude */
        public const int MIN_LATITUDE = -90;

        /* Maximum value of longitude */
        public const int MAX_LONGITUDE = 180;

        /* Maximum value of longitude */
        public const int MIN_LONGITUDE = -180;

        /* Maximum value for minute coordinate */
        public const int MAX_MINUTES = 60;

        /* Minimum value for minute coordinate */
        public const int MIN_MINUTES = 0;

        /* Maximum value for seconds coordinate */
        public const int MAX_SECONDS = 60;

        /* Minimum value for seconds coordinate */
        public const int MIN_SECONDS = 0;

        /* Amount of minutes per degree */
        public const int MINUTES_PER_DEG = 60;

        /* Amount of seconds per degree */
        public const int SECONDS_PER_DEG = 3600;

        /* The degree part of the coordinate */
        protected int degrees;

        /* The minute part of the coordinate */
        protected int minutes;

        /* The seconds part of the coordinate */
        protected int seconds;

        /* The heading for the coordinate */
        protected char heading;

        /* Method convertes geographic coordinate into decimal 
        * value for easier mathematical manipulation */
        public double getDecimalDegree()
        {
            double decimalDeg = degrees + (minutes / 60.0) + (seconds / 3600.0);

            // If heading is either south or west we need to negate value
            if('S'.Equals(heading) || 'W'.Equals(heading)) 
            {
                decimalDeg = -decimalDeg;
            }

            return decimalDeg;
        }
    }
}
