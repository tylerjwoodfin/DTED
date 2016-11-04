namespace DTEDCapstone.File_Reader
{
    /* Class will store constants used by reader such as 
     * byte sizes of each value or parsing the byte data
     * into other tokens such as latitude (degrees, minutes,
     * seconds)
     */
    public static class FileConstants
    {
        /* ************ SIZE OF FILE PARTS ************ */

        /* Size of DSI part of the file in bytes */
        public const int SIZE_DSI = 648;

        /* Size of ACC part of the file in bytes */
        public const int SIZE_ACC = 2700;

        /* Size of each Data Record in the file */
        public const int SIZE_DATA_REC = 2414;

        /* ************ SIZE OF UHL DATA TOKENS ************ */

        /* Size of sentinel for UHL */
        public const int SIZE_UHL_RECOGNITION_SENTINEL = 3;

        /* Size of byte needed to skip to next token */
        public const int SIZE_SKIP = 1;

        /* Size of longitude origin */
        public const int SIZE_LONGITUDE_ORIGIN = 8;

        /* Size of latitude origin */
        public const int SIZE_LATITUDE_ORIGIN = 8;

        /* Size of longitude data interval */
        public const int SIZE_LONGITUDE_INTERVAL = 4;

        /* Size of latitude data interval */
        public const int SIZE_LATITUDE_INTERVAL = 4;

        /* Size of Absolute Vertical Accuracy */
        public const int SIZE_ABSOLUTE_VERTICAL_ACC = 4;

        /* Size of Unclassified Security Code */
        public const int SIZE_UNCLASSIFIED_SECURITY = 3;

        /* Size of Unique Reference */
        public const int SIZE_UNIQUE_REFERENCE = 12;

        /* Size of Number of Longitude Lines */
        public const int SIZE_NUMBER_LONGITUDE = 4;

        /* Size of Number of Latitude Points Per Longitude Line */
        public const int SIZE_NUMBER_LATITUDE = 4;

        /* Size of Multiple Accuracy */
        public const int SIZE_MULTIPLE_ACC = 1;

        /* Size of Reserved space */
        public const int SIZE_RESERVED = 24;

        /* ************ UHL TOKEN ATTRIBUTES - (i.e latitude origin) ************ */

        /* Location of degree attribute in UHL header for latitude/longitude origin token */
        public const int LOC_UHL_DEG = 0;

        /* Size of degree attribute in UHL header for latitude/longitude origin token */
        public const int SIZE_UHL_DEG = 3;

        /* Location of minute attribute in UHL header for latitude/longitude origin token */
        public const int LOC_UHL_MIN = SIZE_UHL_DEG;

        /* Size of minute attribute in UHL header for latitude/longitude origin token */
        public const int SIZE_UHL_MIN = 2;

        /* Location of second attribute in UHL header for latitude/longitude origin token */
        public const int LOC_UHL_SEC = SIZE_UHL_DEG + SIZE_UHL_MIN;

        /* Size of second attribute in UHL header for latitude/longitude origin token */
        public const int SIZE_UHL_SEC = 2;

        /* Location of heading attribute in UHL header for latitude/longitude origin token */
        public const int LOC_UHL_DIRECTION = SIZE_UHL_DEG + SIZE_UHL_MIN + SIZE_UHL_SEC;

        /* Size of the heading attribute in UHL header for latitude/longitude origin token */
        public const int SIZE_UHL_DIRECTION = 1;

        /* Specifies the location of the decimal point for the interval values from the UHL */
        public const int LOC_UHL_INTERVAL_DECIMAL = 3;

        /* ************ SIZE OF DATA RECORD TOKENS ************ */

        /* Size of recognition sentinel for data record */
        public const int SIZE_DATA_REC_RECOGNITION_SENTINEL = 1;

        /* Size of data block count for data record */
        public const int SIZE_BLOCK_COUNT = 3;

        /* Size of longitude count for data record */
        public const int SIZE_LONGITUDE_COUNT = 2;

        /* Size of latitude count for data record */
        public const int SIZE_LATITUDE_COUNT = 2;

        /* Size of elevation for data record */
        public const int SIZE_ELEVATION = 2;

        /* Size of checksum attribute at the end of each data record */
        public const int SIZE_CHECKSUM = 4;

        /* ************ DATA RECORD TOKEN ATTRIBUTES ************ */

        /* Location to beginning of elevation data records */
        public const int LOC_START_ELEVATION = 9;

    }
}
