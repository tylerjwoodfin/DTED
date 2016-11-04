using System;

namespace DTEDCapstone.Data_Layer
{
    /* The class abstracts the latitude data that will be
     * read from the file. The data members will consist
     * of the components of the latitude such as 'minutes'.
     * A heading data member will also be included to show 
     * if it is 'North' or 'South' of the equator. 
     *
     * NOTE: Might need to throw errors for bad values
     */
    public class Latitude : GeographicCoordinate
    {
        /* Property */
        public int Degrees
        {
            get
            {
                return degrees;
            }
            set
            {
                // Ensure value is within bounds for latitude coordinate
                if (value >= MIN_LATITUDE && value <= MAX_LATITUDE)
                {
                    degrees = value;
                }
            }
        }

        /* Property */
        public int Minutes
        {
            get
            {
                return minutes;
            }
            set
            {
                // Ensure value is within bounds for minutes coordinate
                if ((value >= MIN_MINUTES && value <= MAX_MINUTES) && degrees != MAX_LATITUDE)
                {
                    minutes = value;
                }
            }
        }

        /* Property */
        public int Seconds
        {
            get
            {
                return seconds;
            }
            set
            {
                // Ensure value is within bounds for seconds coordinate
                if ((value >= MIN_SECONDS && value <= MAX_SECONDS) && degrees != MAX_LATITUDE)
                {
                    seconds = value;
                }
            }
        }

        public char Heading
        {
            get
            {
                return heading;
            }
            set
            {
                // Check if heading is valid. NOTE: Do we care about capitalization?
                if('N'.Equals(value) || 'S'.Equals(value))
                {
                    heading = value;
                }
            }
        }

        /* Constructor will initialize and create object
         * that is valid. It will convert 'string' paramaters
         * into the proper values and the class will check if
         * such values are valid.
         */
        public Latitude(string deg, string min, string sec, string heading)
        {
            Degrees = int.Parse(deg);
            Minutes = int.Parse(min);
            Seconds = int.Parse(sec);
            Heading = char.Parse(heading);
        }

        /* Constructor creates latitude from decimal 
        * degree representation */
        public Latitude(double decimalDeg)
        {
            if (decimalDeg < 0.0) // Negative is south
            {
                Heading = 'S';
            }
            else // Positive is north
            {
                Heading = 'N';
            }

            double absDecimalDeg = Math.Abs(decimalDeg); // Take absolute value for conversion
            Degrees = (int) Math.Floor(absDecimalDeg); // Take floor of decimal and convert to 'int'
            Minutes = (int) Math.Floor(MINUTES_PER_DEG * (absDecimalDeg - Degrees)); // Convert to minutes and take floor
            Seconds = (int) Math.Floor((SECONDS_PER_DEG * (absDecimalDeg - Degrees) - MINUTES_PER_DEG * Minutes)); // Convert to seconds
        }

        /* Returns a human readable representation of latitude */
        public string toString()
        {
            return degrees + DEGREE_SYMBOL + " " + minutes + "'" 
                + " " + seconds + "\"" 
                + " " + heading;
        }
    }
}