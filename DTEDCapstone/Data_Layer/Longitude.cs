using System;

namespace DTEDCapstone.Data_Layer
{
    /* The class abstracts the longitude data that will be
     * read from the file. The data members will consist
     * of the components of the longitude such as 'minutes'.
     * A heading data member will also be included to show 
     * if it is 'East' or 'West' of the prime meridian. 
     *
     * NOTE: Might need to throw errors for bad values
     */
    public class Longitude : GeographicCoordinate
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
                if (value >= MIN_LONGITUDE && value <= MAX_LONGITUDE)
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
                if ((value >= MIN_MINUTES && value <= MAX_MINUTES) && degrees != MAX_LONGITUDE)
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
                if ((value >= MIN_SECONDS && value <= MAX_SECONDS) && degrees != MAX_LONGITUDE)
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
                if ('E'.Equals(value) || 'W'.Equals(value))
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
        public Longitude(string deg, string min, string sec, string heading)
        {
            Degrees = int.Parse(deg);
            Minutes = int.Parse(min);
            Seconds = int.Parse(sec);
            Heading = char.Parse(heading);
        }

        /* Constructor creates longitude from decimal 
        * degree representation */
        public Longitude(double decimalDeg)
        {
            if(decimalDeg < 0.0) // Negative is west
            {
                Heading = 'W';
            }
            else // Positive is east
            {
                Heading = 'E';
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
