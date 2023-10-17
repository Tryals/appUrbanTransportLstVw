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
    /// Логика взаимодействия для PageRoutes.xaml
    /// </summary>
    public partial class PageRoutes : Page
    {
        public PageRoutes()
        {
            InitializeComponent();
            dtgRoutes.ItemsSource = UrbanTransportEntities.GetContext().Routes.ToList();

            CmbStart.ItemsSource = UrbanTransportEntities.GetContext().Routes.Select(x => x.route_start).Distinct().ToList();
        }

        private void CmbStart_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string Route_start = (string)CmbStart.SelectedValue;
            dtgRoutes.ItemsSource = UrbanTransportEntities.GetContext().Routes.Where(x => x.route_start == Route_start).Distinct().ToList();
        }

        private void TxtSearchEnd_TextChanged(object sender, TextChangedEventArgs e)
        {
            string search = TxtSearchEnd.Text;
            dtgRoutes.ItemsSource = UrbanTransportEntities.GetContext().Routes.
            Where(x => x.route_end.Contains(search)).ToList();
        }
        private void BtnResetFiltr_Click(object sender, RoutedEventArgs e)
        {
            dtgRoutes.ItemsSource = UrbanTransportEntities.GetContext().Routes.ToList();
        }

        private void BtnNew_Click(object sender, RoutedEventArgs e)
        {
            BD.ClassFrame.frmObj.Navigate(new Pages.PageAddEdit(null));
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
                var Remove = dtgRoutes.SelectedItems.Cast<Routes>().ToList();

                if (MessageBox.Show($"Вы точно хотите удалить следующие {Remove.Count()} элементов?", "Внимание",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        UrbanTransportEntities.GetContext().Routes.RemoveRange(Remove);
                        UrbanTransportEntities.GetContext().SaveChanges();
                        MessageBox.Show("Данные удалены!");

                        dtgRoutes.ItemsSource = UrbanTransportEntities.GetContext().Routes.ToList();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString());
                    }
                }
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            BD.ClassFrame.frmObj.Navigate(new Pages.PageAddEdit((sender as Button).DataContext as Routes));
        }
    }
}
