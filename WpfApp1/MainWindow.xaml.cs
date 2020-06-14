using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Security.Cryptography;

namespace WpfApp1
{
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly int _rowLimit = 10;
    UserInformation locationAndTime = new UserInformation();
    public MainWindow()
    {
        InitializeComponent();
        this.DataContext = locationAndTime;
    }

    HttpClient httpClientGeo = new HttpClient
    {
        BaseAddress = new Uri("https://earthquake.usgs.gov/fdsnws/event/1/"),
    };

        private async void Button_Click(object sender, RoutedEventArgs e)
        {

            var EarthquakeQuery = "query?format=geojson&starttime=" + locationAndTime.StartTime +
                "&endtime=" + locationAndTime.EndTime + "&latitude=" + locationAndTime.Laditude +
                "&longitude=" + locationAndTime.Longitude + "&maxradius=" + locationAndTime.SearchRadius;
            var EarthquakeHttpInfo = await httpClientGeo.GetAsync(EarthquakeQuery);

            var EarthquakeJson = await EarthquakeHttpInfo.Content.ReadAsStringAsync();

            var geoJsonInfo = JsonConvert.DeserializeObject<GeoJsonInfo>(EarthquakeJson);


            // Most of this is copied straight from microsoft, I am just tweaking it.

            // Create the parent FlowDocument...
            var flowDoc = new FlowDocument();
            // Create the Table...
            var table1 = new Table();
            // ...and add it to the FlowDocument Blocks collection.
            flowDoc.Blocks.Add(table1);

            // Set some global formatting properties for the table.
            table1.CellSpacing = 10;
            table1.Background = Brushes.White;

            int numberOfColumns = 7;
            for (int x = 0; x < numberOfColumns; x++)
            {
                table1.Columns.Add(new TableColumn());

                // Set alternating background colors for the middle colums.
                if (x % 2 == 0)
                    table1.Columns[x].Background = Brushes.BurlyWood;
                else
                    table1.Columns[x].Background = Brushes.ForestGreen;
            }

            // Create and add an empty TableRowGroup to hold the table's Rows.
            table1.RowGroups.Add(new TableRowGroup());

            // Add the first (title) row.
            table1.RowGroups[0].Rows.Add(new TableRow());

            // Alias the current working row for easy reference.
            TableRow currentRow = table1.RowGroups[0].Rows[0];

            // Global formatting for the title row.
            currentRow.Background = Brushes.Aquamarine;
            currentRow.FontSize = 40;
            currentRow.FontWeight = FontWeights.Bold;

            // Add the header row with content,
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Number of EarthQuakes Found " + geoJsonInfo.features.Length))));
            // and set the row to span all 6 columns.
            currentRow.Cells[0].ColumnSpan = 6;

            // Add the second (header) row.
            table1.RowGroups[0].Rows.Add(new TableRow());
            currentRow = table1.RowGroups[0].Rows[1];

            // Global formatting for the header row.
            currentRow.FontSize = 18;
            currentRow.FontWeight = FontWeights.Bold;

            // Add cells with content to the second row.
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Location"))));
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Latitude"))));
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Longitude"))));
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Depth"))));
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Magnitude"))));
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Time"))));
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Image"))));


            for (int i = 0; i < geoJsonInfo.features.Length && i < _rowLimit; i++)
            {
                // Add the third row.
                table1.RowGroups[0].Rows.Add(new TableRow());
                currentRow = table1.RowGroups[0].Rows[i + 2];

                // Global formatting for the row.
                currentRow.FontSize = 12;
                currentRow.FontWeight = FontWeights.Normal;

                //var imageResponse = await httpClientNasa.GetAsync("imagery?api_key=izT2X3B6EikwQJSDzgCdolVarygG6TUOJ2b1gU3l" 
                //    + "&lat=" + geoJsonInfo.features[i].geometry.coordinates[1] + "&lon=" + geoJsonInfo.features[i].geometry.coordinates[0]);
                var imageUrl = "https://api.nasa.gov/planetary/earth/imagery?api_key=izT2X3B6EikwQJSDzgCdolVarygG6TUOJ2b1gU3l"
                    + "&lat=" + geoJsonInfo.features[i].geometry.coordinates[1] + "&lon=" + geoJsonInfo.features[i].geometry.coordinates[0];

                // Add cells with content to the third row.
                currentRow.Cells.Add(new TableCell(new Paragraph(new Run(geoJsonInfo.features[i].properties.place))));
                currentRow.Cells.Add(new TableCell(new Paragraph(new Run(geoJsonInfo.features[i].geometry.coordinates[1].ToString()))));
                currentRow.Cells.Add(new TableCell(new Paragraph(new Run(geoJsonInfo.features[i].geometry.coordinates[0].ToString()))));
                currentRow.Cells.Add(new TableCell(new Paragraph(new Run(geoJsonInfo.features[i].geometry.coordinates[2].ToString()))));
                currentRow.Cells.Add(new TableCell(new Paragraph(new Run(geoJsonInfo.features[i].properties.mag.ToString()))));
                currentRow.Cells.Add(new TableCell(new Paragraph(new Run(geoJsonInfo.features[i].properties.time.ToString()))));

                var imageHyperLink = new Hyperlink(new Run("Click Here"))
                { 
                    Tag = imageUrl
                };

                imageHyperLink.Click += new RoutedEventHandler(Image_HyperLink);

                currentRow.Cells.Add( new TableCell(new Paragraph(imageHyperLink)));

                // Bold the first cell.
                currentRow.Cells[0].FontWeight = FontWeights.Bold;
            }

            if(geoJsonInfo.features.Length > 10)
            { 
                table1.RowGroups[0].Rows.Add(new TableRow());
                currentRow = table1.RowGroups[0].Rows[12];

                // Global formatting for the footer row.
                currentRow.Background = Brushes.Aquamarine;
                currentRow.FontSize = 18;
                currentRow.FontWeight = FontWeights.Normal;

                // Add the header row with content,
                currentRow.Cells.Add(new TableCell(new Paragraph(new Run("This Table show only the First 10 results due to spacing." +
                    "If are missing some earthquake you want to see, narrow your serach."))));
                // and set the row to span all 6 columns.
                currentRow.Cells[0].ColumnSpan = 6;

             InfoTable.Document = flowDoc;
            }
        }
        private void Image_HyperLink(object sender, RoutedEventArgs e)
        {
            var getUrl = ((Hyperlink)sender).Tag;

            var imageWindow = new Window();

            //var httpNasaResponse = await httpClientNasa.GetAsync((string)getUrl);

            var bitImage = new BitmapImage();
            bitImage.BeginInit();
            bitImage.UriSource = new Uri((string)getUrl, UriKind.Absolute);
            bitImage.EndInit();
            var image = new Image {
                Source = bitImage
            };

            imageWindow.Content = image;

            imageWindow.Show();
        }
    }
}
