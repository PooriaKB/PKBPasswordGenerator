using DataAccess.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PKBPasswordGenerator.UserControls.Views
{

    public partial class RegistrerView : UserControl
    {
        public string UserFilePath;
        public string VaultFilePath;
        public User CurrentUser = new User();
        public Vault CurrentVault = new Vault();


        public RegistrerView(string vaultFilePath, string userFilePath)
        {
            InitializeComponent();

            this.VaultFilePath = vaultFilePath;
            this.UserFilePath = userFilePath;
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            // Take username and password from the GUI
            var currentUserName = tbUserName.Text.Trim();
            string masterPassword;
            if (tbPlainPassword.Visibility == Visibility.Visible)
                masterPassword = tbPlainPassword.Text.Trim();
            else
                masterPassword = tbPassword.Password.Trim();
            // Some username and password validation
            if (currentUserName == string.Empty)
            {
                TextBoxBorder.BorderBrush = Brushes.Red;
                TextBoxBorder.BorderThickness = new Thickness(2);
                MessageBox.Show("Username can't be empty", "Invalid Username", MessageBoxButton.OK, MessageBoxImage.Warning);
                TextBoxBorder.BorderBrush = Brushes.Black;
                tbUserName.Focus();
            }
            else if (IsNameNotValid(currentUserName))
            {
                TextBoxBorder.BorderBrush = Brushes.Red;
                TextBoxBorder.BorderThickness = new Thickness(2);
                MessageBox.Show("Username can't contain !@#$%^&*()+=", "Invalid Username", MessageBoxButton.OK, MessageBoxImage.Warning);
                TextBoxBorder.BorderBrush = Brushes.Black;
                tbUserName.Focus();
            }
            else if (IsNameFirstLetterANumber(currentUserName))
            {
                TextBoxBorder.BorderBrush = Brushes.Red;
                TextBoxBorder.BorderThickness = new Thickness(2);
                MessageBox.Show("Username's first letter can't be a number", "Invalid Username", MessageBoxButton.OK, MessageBoxImage.Warning);
                TextBoxBorder.BorderBrush = Brushes.Black;
                tbUserName.Focus();
            }
            else if (masterPassword == string.Empty)
            {
                PasswordBoxBorder.BorderBrush = Brushes.Red;
                PasswordBoxBorder.BorderThickness = new Thickness(2);
                MessageBox.Show("Password can't be empty", "Password Error", MessageBoxButton.OK, MessageBoxImage.Error);
                PasswordBoxBorder.BorderBrush = Brushes.Black;
                tbPassword.Focus();

            }
            else if (masterPassword.Contains(" "))
            {
                PasswordBoxBorder.BorderBrush = Brushes.Red;
                PasswordBoxBorder.BorderThickness = new Thickness(2);
                MessageBox.Show("Password can't contain white spaces", "Password Error", MessageBoxButton.OK, MessageBoxImage.Error);
                PasswordBoxBorder.BorderBrush = Brushes.Black;
                tbPassword.Focus();

            }
            else if (masterPassword.Length < 4 || masterPassword.Length > 8)
            {
                PasswordBoxBorder.BorderBrush = Brushes.Red;
                PasswordBoxBorder.BorderThickness = new Thickness(2);
                if (masterPassword.Length < 4)
                    MessageBox.Show("Password can't be less than 4 charecters", "Password Error", MessageBoxButton.OK, MessageBoxImage.Error);
                else if (masterPassword.Length > 8)
                    MessageBox.Show("Password can't be more than 8 charecters", "Password Error", MessageBoxButton.OK, MessageBoxImage.Error);
                PasswordBoxBorder.BorderBrush = Brushes.Black;
                tbPassword.Focus();
            }
            else
            {

                // === NEW USER: Register! ===
                try
                {
                    // Save the username & master-password in memory
                    CurrentUser.UserName = currentUserName;
                    CurrentUser.MasterPassword = masterPassword;



                    // Save ONLY username to file
                    CurrentUser.SaveUserName(UserFilePath);

                    var mainWindow = Window.GetWindow(this) as MainWindow;



                    // Create the encrypted vault (this uses Vault class — master password in memory only)
                    CurrentVault.CreateVault(masterPassword, VaultFilePath); // This creates empty vault with salt + encryption

                    MessageBox.Show($"Vault created securely.", "Success!", MessageBoxButton.OK, MessageBoxImage.Information);

                    // Open main window (pass paths and master password and PasswordDataAccess in memory)
                    mainWindow?.DerivedKey = CurrentVault.derivedKey;
                    mainWindow?.CurrentPasswordDataAccess = CurrentVault.currentVaultPasswordsAccess;
                    mainWindow?.CurrentPasswordList = CurrentVault.currentVaultPasswordsAccess.Passwords;
                    mainWindow?.CurrentUserName = currentUserName;
                    mainWindow?.CurrentVault = CurrentVault;
                    mainWindow?.CurrentUser = CurrentUser;
                    mainWindow?.ShowMainMenuView(CurrentVault.currentVaultPasswordsAccess);


                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Registration failed: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

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

        #region SomeMethodsForNameValidation
        private bool IsNameNotValid(string name)
        {
            bool result = false;
            for (var index = 0; index < name.Length; index++)
            {
                if (!char.IsLetterOrDigit(name[index]) && name[index] != '_' && name[index] != '-' && name[index] != ' ')
                {
                    result = true;
                    break;
                }
            }
            return result;

        }
        private bool IsNameFirstLetterANumber(string name)
        {
            string numbers = "1234567890";
            foreach (var number in numbers)
            {
                if (number == name[0])
                    return true;
            }
            return false;

        }
        #endregion
    }
}
