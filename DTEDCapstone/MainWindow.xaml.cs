using System;
using System.Windows;
using System.Windows.Forms;

using DTEDCapstone.Data_Layer;
using DTEDCapstone.File_Reader;
using DTEDCapstone.Data_Mapping;
using DTEDCaptsone.Data_Scaling;
using DTEDCapstone.Plotter;

using DTEDCapstone.Translator;

///Capstone Project for DTED group
///Austin Graham
///Brandon Shaw
///Nick Von Busch
///Spencer Daniel
///Landon Sherwood
///Edward Baldwin
///Jared Hughes
///Braden Roper
///Brian Neldon
namespace DTEDCapstone
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public const double DEFAULT_SCALE_FACTOR = 0.50;


        /*Holds raw data read from the .dt1 file*/
        DTED_Data data;

        /*data for exporting stuff*/
        CartesianData cData;

        /*Construct the new window*/
        public MainWindow()
        {
            InitializeComponent();
        }

        /*When the import button is clicked*/
        private void import_Click(object sender, RoutedEventArgs e)
        {
            // Declare variable to hold scaled data set for faster plotting
            MappedData[,] scaledData;

            //Show the user that we are importing
            TextLabel.Content = "Importing...";

            //Open the file dialog
            OpenFileDialog fopen = new OpenFileDialog();
            fopen.RestoreDirectory = true;
            fopen.Filter = "dt1 files (*.dt1)|*.dt1";

            //When the file is selected
            if(fopen.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    //Read the file
                    FileReader reader = new FileReader(fopen.FileName);
                    reader.read(out data);

                    //Map data to the matrix
                    DataMapper mapper = new DataMapper(data);

                    // Scale the data as desired, by default half the points taken
                    DataScaler scaler = new DataScaler(mapper.map());
                    scaledData = scaler.scale(DEFAULT_SCALE_FACTOR); // Scale about half resolution (or about half the points in the file)
                }
                catch(Exception error)
                {
                    //Show the user any error that has occured
                    System.Windows.MessageBox.Show("File is invalid. Aborting Import.");
                    TextLabel.Content = "Please select Import or Export";
                    return;
                }
            }
            else
            {
                TextLabel.Content = "Please select Import or Export";
                return;
            }

            // Retrieve size of scaled data
            int longiNum = scaledData.GetLength(0);
            int latiNum = scaledData.GetLength(1);

            int size = longiNum * latiNum;
            double[] x = new double[size]; // 'x' is distance eastward
            double[] y = new double[size]; // 'y' is distance northward
            double[] z = new double[size]; // 'z' is elevation

            int counter = 0; // Counter to place elevation in proper order

            cData = new CartesianData(scaledData);

            CartesianPoint[,] pts = cData.getCartesianData();

            // Iterate from left to right on grid
            for(int i = 0; i < longiNum; ++i)
            {
                // Iterate from bottom to top on grid
                for(int j = 0; j < latiNum; ++j)
                {
                    //z[counter] = scaledData[i, j].Elevation;
                    x[counter] = pts[i, j].X;
                    y[counter] = pts[i, j].Y;
                    z[counter] = pts[i, j].Z;
                    counter++;
                }
            }

            //Setup grid size and sampling
            GnuPlot.Set("dgrid3d 45, 45, 2");
            GnuPlot.Set("isosamples 30");

            //set the range for the x,y,z axis and plot (using pm3d to map height to color)
            /*
            GnuPlot.Set("xrange[" + x.Min().ToString() + ":" + x.Max().ToString() + "]",
             "yrange[" + y.Min().ToString() + ":" + y.Max().ToString()  + "]",
             "zrange[" + z.Min().ToString() + ":" + z.Max().ToString() + "]");
            */
            //GnuPlot.SPlot(x, y, z); // Don't think this does what I think it does. :/
            GnuPlot.SPlot(scaledData.GetLength(1), z);

            export.IsEnabled = true;
            TextLabel.Content = "File Loaded";
        }

        /*When the export button is clicked*/
        private void export_Click(object sender, RoutedEventArgs e)
        {
            //Construct file dialog
            TextLabel.Content = "Exporting...";
            SaveFileDialog fopen = new SaveFileDialog();
            String fileName = "";

            try
            {
                //Once the file is selected, export the data
                if (fopen.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    String[] filePath = fopen.FileName.Split('\\');
                    fileName = filePath[filePath.Length - 1] + ".csv";
                    cData.Export(fopen.FileName + ".csv");
                }
            }
            catch(Exception error)
            {
                //Show user an error has occurred if needed
                System.Windows.MessageBox.Show("An error occured in export.");
            }

            //Show user if successful.
            TextLabel.Content = "Export '" + fileName + "' Successful";
        }
    }
}
