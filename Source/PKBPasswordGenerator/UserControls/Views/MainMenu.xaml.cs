using DataAccess;
using DataAccess.Models;
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

namespace PKBPasswordGenerator.UserControls.Views
{
    /// <summary>
    /// Interaction logic for MainMenu.xaml
    /// </summary>
    public partial class MainMenu : UserControl
    {
        public PasswordDataAccess CurrentPasswordDataAccess;
        public List<Password> CurrentPasswordList;
        public MainMenu(PasswordDataAccess currentPasswordDataAccess, List<Password> currentPasswordList, string userName)
        {
            InitializeComponent();

            this.CurrentPasswordDataAccess = currentPasswordDataAccess;
            this.CurrentPasswordList = currentPasswordList;

            

            txtGreating.Content = $"Hi {userName} ";

        }
        private void btnGeneratePass_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Window.GetWindow(this) as MainWindow;
            mainWindow?.ShowGeneratePasswordOptionsView(CurrentPasswordDataAccess);
        }
        private void btnShowOldGeneratedPasswordsList_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow?.ShowOldGeneratedPasswordsList(CurrentPasswordList, CurrentPasswordDataAccess);
        }
        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            

            var confirmation = MessageBox.Show("Are you sure you want to logout", "confirmation", MessageBoxButton.YesNo,MessageBoxImage.Question,MessageBoxResult.No);
            if (confirmation == MessageBoxResult.Yes)
            {
                var mainWindow = Window.GetWindow(this) as MainWindow;
                try
                {
                    mainWindow?.CurrentVault.SaveVault(mainWindow.DerivedKey, mainWindow.CurrentPasswordDataAccess.Passwords, mainWindow.VaultFilePath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                mainWindow?.ShowLoginView();
            }
        }
    }
}
