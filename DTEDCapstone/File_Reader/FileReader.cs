using System;
using System.IO;
using System.Text;

using DTEDCapstone.Data_Layer;

namespace DTEDCapstone.File_Reader
{
    /* 
    * Class is responsible for reading a '*.dt1' file type and parsing
    * the data into usable parts. The parts will be used to construct 
    * the latitude, longitude, and elevation matrix that will be 
    * converted later.
    */
    public class FileReader
    {
        /* The path to the file to be read */
        private string fileName;

        /* The buffer that will hold byte data to be converted */
        private byte[] buffer;

        /* 
        * Construct object with path to be read.
        */
        public FileReader(string path)
        {
            fileName = path;
        }

        /*
        * Function will take byte representation of a data record
        * and parse it into elevation data and place the data in the
        * specified column that represents page of a data record.
        * Keep in mind the first elevation value is the sourthen most
        * data point and the last elevation value is the northern most point.
        * This means the array must be filled from the last element to the first 
        * element (reverse order), if the data is to be mapped easily to
        * the precise latitude and longitude point.
        *
        * NOTE: Might want to check out different way besides passing 
        * array by reference
        */
        private int[] parseDataRecord(byte[] data)
        {
            // Number of elevations is related to '*.dt1' file
            int numElev = ((FileConstants.SIZE_DATA_REC - FileConstants.SIZE_CHECKSUM) - 8) / FileConstants.SIZE_ELEVATION; 
            int row = numElev - 1; // Start at last element in array

            byte[] byteElev = new byte[FileConstants.SIZE_ELEVATION]; // Initialize small array to size of elevation data piece
            int[] elevData = new int[numElev]; // Create array to store elevations of one data record

            // Iterate through each elevation record and assign value to 'elev' paramater at specified row and column
            for (int i = FileConstants.LOC_START_ELEVATION;
            i < (FileConstants.SIZE_DATA_REC - FileConstants.SIZE_CHECKSUM); 
                i+= FileConstants.SIZE_ELEVATION, --row)
            {
                /*
                * POTENIAL BUG: Because of the vague and contradictory information in 
                * file docuementation. Converting any negative elevation values may provide 
                * incorrect results! This will need to be further explored during test phase 
                * and 'ironed out' by maintenance.
                */

                // Fill in byte data and use it to parse to 'Int16'
                byteElev[0] = data[i];
                byteElev[1] = data[i+1];
                elevData[row] = BitConverter.ToInt16(byteElev, 0); // Convert byte code to 'Int16' and place in respective element
            }

            return elevData;
        }

        /* 
        * Function will read and parse the '*.dt1' file and 
        * populate the 'DTED_buffer' parameter passed.
        */
        public void read(out DTED_Data dtedData)
        {
            BinaryReader reader = new BinaryReader(File.Open(fileName, FileMode.Open));

            // Declare variables to store token buffer
            string result;
            string lonInterval;
            string latInterval;
            int numLat;
            int numLon;
            int[][] elevGrid; // Create array of arrays where each element contains a page of each data record

            // Skip bytes to origin buffer
            reader.BaseStream.Seek((FileConstants.SIZE_UHL_RECOGNITION_SENTINEL +
                FileConstants.SIZE_SKIP), SeekOrigin.Current);

            // Read Longitude Origin
            buffer = reader.ReadBytes(FileConstants.SIZE_LONGITUDE_ORIGIN);
            result = Encoding.ASCII.GetString(buffer);

            // Now parse the resulting 'string' into the components of longitude
            Longitude lonOrigin = new
                Longitude(result.Substring(FileConstants.LOC_UHL_DEG, FileConstants.SIZE_UHL_DEG),
                result.Substring(FileConstants.LOC_UHL_MIN, FileConstants.SIZE_UHL_MIN),
                result.Substring(FileConstants.LOC_UHL_SEC, FileConstants.SIZE_UHL_SEC),
                result.Substring(FileConstants.LOC_UHL_DIRECTION, FileConstants.SIZE_UHL_DIRECTION));

            // Read Latitude Origin
            buffer = reader.ReadBytes(FileConstants.SIZE_LATITUDE_ORIGIN);
            result = Encoding.ASCII.GetString(buffer);

            // Now parse the resulting 'string' into the components of latitude
            Latitude latOrigin = new
                Latitude(result.Substring(FileConstants.LOC_UHL_DEG, FileConstants.SIZE_UHL_DEG),
                result.Substring(FileConstants.LOC_UHL_MIN, FileConstants.SIZE_UHL_MIN),
                result.Substring(FileConstants.LOC_UHL_SEC, FileConstants.SIZE_UHL_SEC),
                result.Substring(FileConstants.LOC_UHL_DIRECTION, FileConstants.SIZE_UHL_DIRECTION));

            // Read Longitude Interval
            buffer = reader.ReadBytes(FileConstants.SIZE_LONGITUDE_INTERVAL);
            lonInterval = Encoding.ASCII.GetString(buffer).Insert(FileConstants.LOC_UHL_INTERVAL_DECIMAL,
                "."); // Insert decimal point at 3rd element as specified in docuement

            // Read Latitude Interval
            buffer = reader.ReadBytes(FileConstants.SIZE_LATITUDE_INTERVAL);
            latInterval = Encoding.ASCII.GetString(buffer).Insert(FileConstants.LOC_UHL_INTERVAL_DECIMAL,
                "."); // Insert decimal point at 3rd element as specified in docuement

            // Skip past tokens that are NOT needed
            reader.BaseStream.Seek((FileConstants.SIZE_ABSOLUTE_VERTICAL_ACC +
                FileConstants.SIZE_UNCLASSIFIED_SECURITY +
                FileConstants.SIZE_UNIQUE_REFERENCE), SeekOrigin.Current);

            // Read Number of Latitude
            buffer = reader.ReadBytes(FileConstants.SIZE_NUMBER_LONGITUDE);
            numLon = int.Parse(Encoding.ASCII.GetString(buffer));

            // Read Number of Longitude
            buffer = reader.ReadBytes(FileConstants.SIZE_NUMBER_LATITUDE);
            numLat = int.Parse(Encoding.ASCII.GetString(buffer));

            elevGrid = new int[numLon][]; // Allocate 'elevData' based on size of latitude and longitude amount

            // Skip the rest of the UHL header file
            reader.BaseStream.Seek((FileConstants.SIZE_MULTIPLE_ACC + FileConstants.SIZE_RESERVED),
                SeekOrigin.Current);

            UHL_Header header = new UHL_Header(lonOrigin, latOrigin, lonInterval,
                latInterval, numLat, numLon); // Header is fully read initialize the object

            // Reader is now at start of DSI part of the file
            reader.BaseStream.Seek(FileConstants.SIZE_DSI, SeekOrigin.Current); // Skip DSI

            // Reader is now at start of ACC part of the file
            reader.BaseStream.Seek(FileConstants.SIZE_ACC, SeekOrigin.Current); // Skip ACC

            // Reader is now at the start of the 'buffer Record(s)'
            Array.Resize(ref buffer, FileConstants.SIZE_DATA_REC); // Must resize buffer to hold at least one data record

            int numBytesRead;
            int recordNum = 0;

            numBytesRead = reader.Read(buffer, 0, FileConstants.SIZE_DATA_REC);

            while (numBytesRead == FileConstants.SIZE_DATA_REC) // Check if number of bytes read was successful
            {
                elevGrid[recordNum] = parseDataRecord(buffer);
                numBytesRead = reader.Read(buffer, 0, FileConstants.SIZE_DATA_REC);
                ++recordNum;
            }

            dtedData = new DTED_Data(header, elevGrid); // Assign paramter to newly constructed data

            reader.Close(); // Close file stream
        }
    }
}
