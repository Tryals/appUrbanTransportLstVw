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
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace appUrbanTransport.Pages
{
    /// <summary>
    /// Логика взаимодействия для PageTransport.xaml
    /// </summary>
    public partial class PageTransport : Page
    {
        public PageTransport()
        {
            InitializeComponent();
            dtgTransport.ItemsSource = UrbanTransportEntities.GetContext().Transport.ToList();

            CmbSpeed.ItemsSource = UrbanTransportEntities.GetContext().Transport.Select(x => x.speed_km_h).Distinct().ToList();
        }

        private void CmbSpeed_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int Speed_km_h = (int)CmbSpeed.SelectedValue;
            dtgTransport.ItemsSource = UrbanTransportEntities.GetContext().Transport.Where(x => x.speed_km_h == Speed_km_h).Distinct().ToList();
        }

        private void TxtSearchName_TextChanged(object sender, TextChangedEventArgs e)
        {
            string search = TxtSearchName.Text;
            dtgTransport.ItemsSource = UrbanTransportEntities.GetContext().Transport.
            Where(x => x.name.Contains(search)).ToList();
        }

        private void BtnResetFiltr_Click(object sender, RoutedEventArgs e)
        {
            dtgTransport.ItemsSource = UrbanTransportEntities.GetContext().Transport.ToList();
        }

        private void new_Click(object sender, RoutedEventArgs e)
        {
            BD.ClassFrame.frmObj.Navigate(new Pages.PageAddEditTransport(null));
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            var Remove = dtgTransport.SelectedItems.Cast<Transport>().ToList();

            if (MessageBox.Show($"Вы точно хотите удалить следующие {Remove.Count()} элементов?", "Внимание",
            MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    UrbanTransportEntities.GetContext().Transport.RemoveRange(Remove);
                    UrbanTransportEntities.GetContext().SaveChanges();
                    MessageBox.Show("Данные удалены!");

                    dtgTransport.ItemsSource = UrbanTransportEntities.GetContext().Transport.ToList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            BD.ClassFrame.frmObj.Navigate(new Pages.PageAddEditTransport((sender as Button).DataContext as Transport));
        }
    }
}
