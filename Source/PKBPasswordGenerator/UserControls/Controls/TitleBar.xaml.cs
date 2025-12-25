using DataAccess;
using DataAccess.Models;
using PKBPasswordGenerator.UserControls.Views;
using System.Windows;
using System.Windows.Controls;

namespace PKBPasswordGenerator.UserControls.Controls
{
   
    public partial class TitleBar : UserControl
    {



        public TitleBar()
        {
            InitializeComponent();

        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Window.GetWindow(this) as MainWindow;
            mainWindow?.Close();

        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this)!.WindowState = WindowState.Minimized;
        }
    }
}
