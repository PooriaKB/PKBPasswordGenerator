using DataAccess;
using DataAccess.Models;
using PKBPasswordGenerator.UserControls.Controls;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PKBPasswordGenerator.UserControls.Views
{
    

    public partial class OldGeneratedPasswordsList : UserControl
    {
        PasswordDataAccess CurrentPasswordDataAccess;
        List<Password> CurrentPasswords;

        public OldGeneratedPasswordsList(List<Password> currentPasswords , PasswordDataAccess currentPasswordDataAccess)
        {
            
            InitializeComponent();

            this.CurrentPasswords = currentPasswords;
            this.CurrentPasswordDataAccess = currentPasswordDataAccess;

            if (CurrentPasswords.Count == 0)
            {
                PasswordStackPanel.Children.Clear();
                PasswordStackPanel.Children.Add(new Label
                {
                    Content = "No passwords generated yet.",
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Foreground = Brushes.Red,
                    Margin = new Thickness(0,150,0,0)
                });

            }
            else 
            {
                var PasswordsSortedList = CurrentPasswords.OrderByDescending(p => p.Id).ToList();
                PasswordTextBox_Load(PasswordsSortedList);
                rbtn_LastToFirst.IsChecked = true;
            }


        }

        private void PasswordTextBox_Load(List<Password> passwords) 
        {
            PasswordStackPanel.Children.Clear();

            for (int i = 0; i < passwords.Count; i++)
            {
                var pwd = passwords[i];

                var passwordValue = pwd.Value;      // Real password
                var passwordName = pwd.Name;        // Name or identifier
                var passwordID = pwd.Id;            // Password ID

                var row = new PasswordTextBox(passwordID,passwordValue, passwordName, CurrentPasswordDataAccess, CurrentPasswords)
                {
                    
                    Margin = new Thickness(0, 5, 0, 5)
                };

                PasswordStackPanel.Children.Add(row);
            }


        }


        

        private void btnBackToMainMenu_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            this.CurrentPasswordDataAccess = mainWindow.CurrentPasswordDataAccess;
            mainWindow?.ShowMainMenuView(CurrentPasswordDataAccess);
        }

        private void rbtn_FirstToLast_Click(object sender, RoutedEventArgs e)
        {

            var PasswordsSortedList = CurrentPasswords.OrderBy(p => p.Id).ToList();
            PasswordTextBox_Load(PasswordsSortedList);
        }
        private void rbtn_LastToFirst_Click(object sender, RoutedEventArgs e)
        {

            var PasswordsSortedList = CurrentPasswords.OrderByDescending(p => p.Id).ToList();
            PasswordTextBox_Load(PasswordsSortedList);
        }

        private void rbtn_NameOrder_Click(object sender, RoutedEventArgs e)
        {
            var PasswordsSortedList = CurrentPasswords.OrderBy(p => p.Name).ToList();
            PasswordTextBox_Load(PasswordsSortedList);

        }
    }
}
