using DataAccess;
using DataAccess.Models;
using PKBPasswordGenerator.UserControls.Views;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace PKBPasswordGenerator
{


    public partial class MainWindow : Window
    {
        // App memory properties
        public PasswordDataAccess CurrentPasswordDataAccess;
        public List<Password> CurrentPasswordList;
        public Vault CurrentVault;
        public User CurrentUser;
        public string CurrentUserName;
        public string VaultFilePath;
        public string UserFilePath;
        public byte[] DerivedKey;


        public MainWindow()
        {
            InitializeComponent();

            // === Define the correct paths FIRST ===
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData); // e.g., C:\User\YourName\AppData\Roaming
            string appFolder = Path.Combine(appDataPath, "PKBPasswordGenerator"); // The App Folder
            string usersFolder = Path.Combine(appFolder, "Users"); // Folder For Usernames
            string userFilePath = Path.Combine(usersFolder, "username.txt"); // Simple text file for username
            string vaultsFolder = Path.Combine(appFolder, "Vaults");
            string vaultFilePath = Path.Combine(vaultsFolder, $"userVault.vault");
            this.UserFilePath = userFilePath;
            this.VaultFilePath = vaultFilePath;


            // === Create safe directories ===
            Directory.CreateDirectory(usersFolder);
            Directory.CreateDirectory(vaultsFolder);



            // === Decide: Show Login or Register? ===
            bool vaultAlreadyExists = File.Exists(vaultFilePath);  // This file tells us if someone registered


            if (vaultAlreadyExists)
            {
                // User exists → show Login
                MainContentArea.Content = new LoginView();

            }
            else
            {
                // First time → show Register
                MainContentArea.Content = new RegistrerView(vaultFilePath, userFilePath);
            }

            Closing += MainWindow_Closing;

        }

        private void MainWindow_Closing(object? sender, CancelEventArgs e)
        {
            var mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow == null) return;

            // Show confirmation popup
            var confirmation = MessageBox.Show(
                "Are you sure you want to close the app?",
                "Confirm Exit",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            // If user clicks "No", cancel the closing
            if (confirmation == MessageBoxResult.Yes)
            {
                if (mainWindow.MainContentArea.Content is LoginView || mainWindow.MainContentArea.Content is RegistrerView)
                {
                    mainWindow.Close();
                }
                else
                {
                    try
                    {
                        mainWindow.CurrentVault.SaveVault(mainWindow.DerivedKey, mainWindow.CurrentPasswordDataAccess.Passwords, mainWindow.VaultFilePath);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                    mainWindow.Close();
                }

            }
            else
            {
                e.Cancel = true; // Cancel the closing event
            }
        }

        internal void ShowLoginView()
        {
            MainContentArea.Content = new LoginView();
        }
        internal void ShowMainMenuView(PasswordDataAccess currentPasswordDataAccess)
        {
            MainContentArea.Content = new MainMenu(currentPasswordDataAccess, CurrentPasswordList, CurrentUserName);
        }
        internal void ShowGeneratePasswordOptionsView(PasswordDataAccess currentPasswordDataAccess)
        {
            MainContentArea.Content = new GeneratePasswordOptions(currentPasswordDataAccess);
        }
        internal void ShowOldGeneratedPasswordsList(List<Password> currentPasswords, PasswordDataAccess currentPasswordDataAccess)
        {
            MainContentArea.Content = new OldGeneratedPasswordsList(currentPasswords, currentPasswordDataAccess);
        }
        internal void ShowGeneratedPasswordView(PasswordDataAccess currentPasswordDataAccess, Password password, List<Password> currentPasswordList)
        {
            MainContentArea.Content = new ShowGeneratedPasswordView(currentPasswordDataAccess, password, currentPasswordList);
        }
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        



    }
}