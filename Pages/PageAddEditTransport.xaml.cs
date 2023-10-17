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
    /// Логика взаимодействия для PageAddEditTransport.xaml
    /// </summary>
    public partial class PageAddEditTransport : Page
    {
        private Transport _currentTransport = new Transport();
        public PageAddEditTransport(Transport selectedTransport)
        {
            InitializeComponent();
            if (selectedTransport != null)
                _currentTransport = selectedTransport;
            DataContext = _currentTransport;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (_currentTransport.id_transport == 0)
            {
                UrbanTransportEntities.GetContext().Transport.Add(_currentTransport);
            }

            try
            {
                UrbanTransportEntities.GetContext().SaveChanges();
                MessageBox.Show("Информация сохранена!");
                BD.ClassFrame.frmObj.Navigate(new Pages.PageTransport());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
    }
}
