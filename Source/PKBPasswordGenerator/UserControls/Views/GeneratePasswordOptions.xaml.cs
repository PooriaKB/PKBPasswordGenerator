using DataAccess;
using DataAccess.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace PKBPasswordGenerator.UserControls.Views
{
   
    public partial class GeneratePasswordOptions : UserControl
    {
        PasswordDataAccess CurrentPasswordDataAccess;
        public GeneratePasswordOptions(PasswordDataAccess currentPasswordDataAccess)
        {
            InitializeComponent();

            this.CurrentPasswordDataAccess = currentPasswordDataAccess;

            // Define the keyboard's enter key default behavior 
            this.KeyDown += (s, e) =>
            {
                if (e.Key == Key.Enter && Keyboard.FocusedElement is not TextBox)
                {
                    btnGeneratePass_Click(btnGeneratePass, new RoutedEventArgs());
                    e.Handled = true;
                }
            };
        }
        private void btnBackward_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Window.GetWindow(this) as MainWindow;
            var confirmation = MessageBox.Show("Are you sure you want to go back to main menu?\nYour settings won't be saved","Confirmation",MessageBoxButton.YesNo,MessageBoxImage.Question,MessageBoxResult.No);
            if (confirmation == MessageBoxResult.Yes)
                mainWindow?.ShowMainMenuView(CurrentPasswordDataAccess);
        }
        
        private void btnGeneratePass_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Window.GetWindow(this) as MainWindow;

            // Get password's name & characters options from GUI
            var passName= tbPassName.Text.Trim();
            var passLength = Convert.ToInt16(tbPassLength.Text);
            var useAlphabets = chkAlaphChars.IsChecked ?? false;
            var useNumbers = chkNumbers.IsChecked ?? false;
            var useSpecialChars = chkSpecialChars.IsChecked ?? false;

            // Main logic of the button & some password's name validation & checkbox choice check
            if (passName==string.Empty)
            {
                tbPassName.BorderBrush = Brushes.Red;
                tbPassName.BorderThickness = new Thickness(2);
                MessageBox.Show("Please enter a valid password name.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Warning);
                tbPassName.BorderBrush = Brushes.Black;
                tbPassName.Focus();

            }
            else if(IsNameNotValid(passName))
            {
                tbPassName.BorderBrush = Brushes.Red;
                tbPassName.BorderThickness = new Thickness(2);
                MessageBox.Show("Password name can't contain !@#$%^&*()+=", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Warning);
                tbPassName.BorderBrush = Brushes.Black;
                tbPassName.Focus();
            }
            else if (passName.Length > 10)
            {
                tbPassName.BorderBrush = Brushes.Red;
                tbPassName.BorderThickness = new Thickness(2);
                MessageBox.Show("Password name can't be more than 10 char", "Pass Name too big", MessageBoxButton.OK, MessageBoxImage.Warning);
                tbPassName.BorderBrush = Brushes.Black;
                tbPassName.Focus();
            }
            else if (!useAlphabets && !useNumbers && !useSpecialChars)
            {
                chkAlaphChars.BorderBrush = Brushes.Red;
                chkNumbers.BorderBrush = Brushes.Red;
                chkSpecialChars.BorderBrush = Brushes.Red;
                MessageBox.Show("Please select at least one character type.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Warning);
                chkAlaphChars.BorderBrush = Brushes.Black;
                chkNumbers.BorderBrush = Brushes.Black;
                chkSpecialChars.BorderBrush = Brushes.Black;

            }
            else
            {
                // Generate a new random password with the selected options
                if(CurrentPasswordDataAccess.Passwords.Exists(p => p.Name.Equals(passName, StringComparison.OrdinalIgnoreCase)))
                {
                    MessageBox.Show("A password with this name already exists. Please choose a different name.", "Duplicate Name", MessageBoxButton.OK, MessageBoxImage.Warning);
                    tbPassName.Focus();
                    return;
                }
                Password currentPassword = new Password(passName, passLength, useAlphabets, useNumbers, useSpecialChars);
                currentPassword.Value = currentPassword.GeneratePassword(passLength, useAlphabets, useNumbers, useSpecialChars);
                
                // Add the generated password to the list of passwords
                mainWindow?.CurrentPasswordDataAccess.AddPassword(currentPassword);
                mainWindow?.CurrentPasswordList = mainWindow?.CurrentPasswordDataAccess.Passwords;

                // Show generation successful message with the generated password 
                mainWindow?.ShowGeneratedPasswordView(CurrentPasswordDataAccess, currentPassword,mainWindow.CurrentPasswordList);
            }

                
        }

        // Logic for name validation
        private bool IsNameNotValid(string name)
        {
            bool result = false;
            for (var index = 0; index < name.Length; index++)
            {
                if (!char.IsLetterOrDigit(name[index]) && name[index] != '_' && name[index] != '-' && name[index]!= ' ')
                {
                    result = true;
                    break;
                }
            }
            return result;
            
        }
    }
}
