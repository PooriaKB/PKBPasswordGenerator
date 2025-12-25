using DataAccess;
using DataAccess.Models;
using PKBPasswordGenerator.UserControls.Controls;
using System.Windows;
using System.Windows.Controls;

namespace PKBPasswordGenerator.UserControls.Views
{
    
    public partial class ShowGeneratedPasswordView : UserControl
    {
        
        public PasswordDataAccess CurrentPasswordDataAccess;
        public Password CurrentPassword;
        public List<Password> CurrentPasswordList;
        public ShowGeneratedPasswordView(PasswordDataAccess currentPasswordDataAccess,Password currentPassword, List<Password> currentPasswordList)
        {
            InitializeComponent();
            this.CurrentPasswordDataAccess = currentPasswordDataAccess;
            this.CurrentPassword = currentPassword ;
            this.CurrentPasswordList = currentPasswordList;
            PasswordTextBox_Loaded();
        }
        private void PasswordTextBox_Loaded()
        {
            PasswordStackPanel.Children.Clear();

            var passwordValue = CurrentPassword.Value;       // The password value
            var passwordName = CurrentPassword.Name;
            var passwordId = CurrentPassword.Id;            // Name or identifier


            var row = new PasswordTextBox(passwordId, passwordValue, passwordName, CurrentPasswordDataAccess,CurrentPasswordList)
            {
                Margin = new Thickness(0, 5, 0, 5)
            };
            
            PasswordStackPanel.Children.Add(row);
        }
        
        private void btnGeneratePass_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow?.ShowGeneratePasswordOptionsView(CurrentPasswordDataAccess);
        }

        private void btnShowPasswordsList_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow?.ShowOldGeneratedPasswordsList(CurrentPasswordList,CurrentPasswordDataAccess);
        }
    }
}
