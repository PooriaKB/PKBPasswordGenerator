using DataAccess.Models;
using System.IO;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PKBPasswordGenerator.UserControls.Views
{
   
    public partial class LoginView : UserControl
    {
        public User CurrentUser = new User();
        public Vault CurrentVault = new Vault();

        public LoginView()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string masterPassword; // Get it from PasswordBox
            if (tbPlainPassword.Visibility == Visibility.Visible)
                masterPassword = tbPlainPassword.Text; 
            else
                masterPassword = tbPassword.Password; 

            // Get the MainWindow reference
            var mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow == null) return;

            // Get the vault & user file path from MainWindow
            string vaultFilePath = mainWindow.VaultFilePath;
            string userFilePath = mainWindow.UserFilePath;
            

            try
            {
                // Call LoadUserName (returns Saved UserName)
                string userName;
                if (File.Exists(userFilePath))
                    userName = CurrentUser.LoadUserName(userFilePath);
                else
                    userName = "User";


                    // Try unlocking the vault with the master-password entered by the user
                    List<Password> passwords = CurrentVault.UnlockVault(masterPassword, vaultFilePath, out byte[] derivedKey);

                

                // Success! Store in memory
                mainWindow.CurrentUserName = userName;
                mainWindow.CurrentUser = CurrentUser;
                mainWindow.CurrentVault = CurrentVault;
                mainWindow.DerivedKey = derivedKey;
                mainWindow.CurrentPasswordDataAccess = CurrentVault.currentVaultPasswordsAccess;
                mainWindow.CurrentPasswordDataAccess.Passwords = passwords;
                mainWindow.CurrentPasswordList = passwords;  // Load the unlocked passwords

                // Open MainMenu User control
                mainWindow.ShowMainMenuView(CurrentVault.currentVaultPasswordsAccess);  
            }
            catch (CryptographicException)
            {
                PasswordBoxBorder.BorderBrush = Brushes.Red;
                PasswordBoxBorder.BorderThickness = new Thickness(2);
                MessageBox.Show("Wrong password or vault corrupted.", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                PasswordBoxBorder.BorderBrush = Brushes.Black;
                tbPassword.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Login failed: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Eye toggle button logic
        private void btnEyeToggle_Click(object sender, RoutedEventArgs e)
        {
            if (tbPlainPassword.Visibility == Visibility.Visible)
            {
                // Hide plain text → show PasswordBox
                tbPassword.Password = tbPlainPassword.Text;
                tbPlainPassword.Visibility = Visibility.Collapsed;
                tbPassword.Visibility = Visibility.Visible;
                btnTogglePassword.Content = "👁";  // Eye open icon
            }
            else
            {
                // Show plain text → hide PasswordBox
                tbPlainPassword.Text = tbPassword.Password;
                tbPassword.Visibility = Visibility.Collapsed;
                tbPlainPassword.Visibility = Visibility.Visible;
                btnTogglePassword.Content = "🕳";  // Eye closed icon (or use your icons)
            }
        }
    }
}
