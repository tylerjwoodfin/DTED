using DTEDCapstone.Data_Layer;

namespace DTEDCaptsone.Data_Scaling
{
    /* 
    * Class is responsible for scaling the raw data read from the '*dt1' file 
    * into a usable chunk of data to plot on the GNUPLOT. The class will allow
    * arbitrary amounts of scaling, but the more the data is scaled down the quicker the 
    * plot will work, however, this comes at a cost of resolution.
    */
    public class DataScaler
    {
        /* Data member represents the entire data set contained in the 
        * DTED file that needs to be scaled, in order to plot it in
        * GNUPLOT 
        */
        private MappedData[,] rawData;

        /* Data member to track row dimensions of raw data */
        private int rawDataRowDimensions;

            /* Data member to track column dimensions of raw data */
        private int rawDataColumnDimensions;
        
        /* Constructor initializes object */
        public DataScaler(MappedData[,] data)
        {
            rawData = data;
            rawDataRowDimensions = data.GetLength(0);
            rawDataColumnDimensions = data.GetLength(1);
        }
        
        /*
        * Function will scale 'rawTerrainData' into specified dimensions 
        * through the use of averaging the data for sub matrix chunks.
        */
        public MappedData[,] scale(double scaleFactor)
        {
            // Find neccessary dimensions and scale both dimensions evenly!
            int rowDimensions = (int) (rawDataRowDimensions * scaleFactor);
            int columnDimensions = (int) (rawDataColumnDimensions * scaleFactor);

            // Don't allow 'scaling-up'
            if (rowDimensions > rawDataRowDimensions || columnDimensions > rawDataColumnDimensions)
            {
                return rawData; // Simply return normal non-scaled data
            }

            MappedData[,] scaledData = new MappedData[rowDimensions, columnDimensions]; // Matrix will contain scaled down data

            // Need to generate sub-matrices in order to scale down, so we need to figure out size
            int subMatrixRowSize = (rawDataRowDimensions / rowDimensions) + 1;
            int subMatrixColumnSize = (rawDataColumnDimensions / columnDimensions) + 1;
            MappedData[,] subMatrix = new MappedData[subMatrixRowSize, subMatrixColumnSize];

            // Declare increment amount for both row and column. This will be based on the modulus of the 'rawData' dimensions divided by 'scaledDimensions'
            int incrementRow;
            int incrementColumn;

            // If the row dimensions did not divide evenly then we have overlap, which changes increment amount
            if(rawDataRowDimensions % rowDimensions > 0)
            {
                incrementRow = subMatrixRowSize - 1;
            }
            else
            {
                incrementRow = subMatrixRowSize;
            }

            // If the column dimensions did not divide evenly then we have overlap, which changes increment amount
            if (rawDataColumnDimensions % columnDimensions > 0)
            {
                incrementColumn = subMatrixColumnSize - 1;
            }
            else
            {
                incrementColumn = subMatrixColumnSize;
            }

            // Start generating sub-matrices and creating weighted data to put into scaled matrix
            for (int j = 0, col = 0; j < (rawDataColumnDimensions - (subMatrixColumnSize - 1)); j += incrementColumn, ++col)
            {
                for(int i = 0, row = (rowDimensions - 1); i < (rawDataRowDimensions - (subMatrixRowSize - 1)); i += incrementRow, --row)
                {
                    subMatrix = generateSubMatrix(i, j, 
                        subMatrixRowSize, subMatrixColumnSize); // Generate and assign generated sub-matrix
                    scaledData[row, col] = new MappedData(subMatrix[0, 0].Latitude, subMatrix[0, 0].Longitude, calculateAverage(subMatrix));
                }
            }

            return scaledData;
        }
        
        /* 
        * Function will generate sub-matrix from 'rawTerrainData' data member to be used to 
        * generate an average for the elevation. It will parse the 'rawData' starting at
        * 'startRow' and 'startColumn' and ITERATES UP to 'rowDimensions' and 'columnDimensions'
        */
        private MappedData[,] generateSubMatrix(int startRow, int startColumn, int rowDimensions, int columnDimensions)
        {
            // Allocate sub-matrix
            MappedData[,] subMatrix = new MappedData[rowDimensions,columnDimensions];

            // Start generating sub-matrix
            for(int i = 0; i < rowDimensions; ++i)
            {
                for(int j = 0; j < columnDimensions; ++j)
                {
                    subMatrix[i, j] = rawData[(startRow + i), (startColumn + j)];
                }
            }

            return subMatrix;
        }
        
        /*
        * Function will generate average values found in the sub-matrix
        * and it will return the scaled data as a 'MappedData' type
        */
        private int calculateAverage(MappedData[,] submatrix)
        {
            int rowDimensions = submatrix.GetLength(0);
            int columnDimensions = submatrix.GetLength(1);

            // Delcare variable to hold summation of elevation
            int totalElev = 0;

            for (int i = 0; i < rowDimensions; ++i)
            {
                for(int j = 0; j < columnDimensions; ++j)
                {
                    totalElev += submatrix[i, j].Elevation;
                }
            }

            return (totalElev / (rowDimensions * columnDimensions)); // Calculate average for submatrix
        } 
    }
}
