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
    /// Логика взаимодействия для PageAddEdit.xaml
    /// </summary>
    public partial class PageAddEdit : Page
    {
        private Routes _currentRoutes = new Routes();
        public PageAddEdit(Routes selectedRoutes)
        {
            InitializeComponent();
            if (selectedRoutes != null)
                _currentRoutes = selectedRoutes;
            DataContext = _currentRoutes;
            CmbTransport.ItemsSource = UrbanTransportEntities.GetContext().Transport.ToList();
            CmbTransport.SelectedValuePath = "id_transport";
            CmbTransport.DisplayMemberPath = "name";
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (_currentRoutes.id_route == 0)
            {
                UrbanTransportEntities.GetContext().Routes.Add(_currentRoutes);
            }

            try
            {
                UrbanTransportEntities.GetContext().SaveChanges();
                MessageBox.Show("Информация сохранена!");
                BD.ClassFrame.frmObj.Navigate(new Pages.PageRoutes());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
    }
}
