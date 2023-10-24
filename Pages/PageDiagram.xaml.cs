using appUrbanTransport.BD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace appUrbanTransport.Pages
{
    /// <summary>
    /// Логика взаимодействия для PageDiagram.xaml
    /// </summary>
    public partial class PageDiagram : Page
    {
        public PageDiagram()
        {
            InitializeComponent();

            ChartPayments.ChartAreas.Add(new ChartArea("Main"));

            var currentSeries = new Series("Скорость")
            {
                IsValueShownAsLabel = true
            };
            ChartPayments.Series.Add(currentSeries);

            ComboUser.ItemsSource = UrbanTransportEntities.GetContext().Transport.ToList();
            ComboChartTypes.ItemsSource = Enum.GetValues(typeof(SeriesChartType));
        }

        private void UpdateChart(object sender, SelectionChangedEventArgs e)
        {
            if (ComboUser.SelectedItem is Transport currentUser &&
                ComboChartTypes.SelectedItem is SeriesChartType currentType)
            {
                Series currentSeries = ChartPayments.Series.FirstOrDefault();
                currentSeries.ChartType = currentType;
                currentSeries.Points.Clear();
                var categoriesList = UrbanTransportEntities.GetContext().Transport.ToList();
                foreach (var category in categoriesList)
                {
                    currentSeries.Points.AddXY(category.name, category.speed_km_h);
                }
            }
        }
    }
}
