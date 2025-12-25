using DataAccess;
using DataAccess.Models;
using System.Windows;
using System.Windows.Controls;

namespace PKBPasswordGenerator.UserControls.Controls
{
    
    public partial class PasswordTextBox : UserControl
    {

        public PasswordDataAccess CurrentPasswordDataAccess;
        public List<Password> CurrentPasswordList;
        public string PasswordValue { get; set; }
        public string PasswordName { get; set; }
        int CurrentPasswordID { get; set; }

        // This property controls what is shown (stars or real text)
        private bool _isVisible = false;

        public PasswordTextBox(int currentPasswordID,string passwordValue,string passwordName ,PasswordDataAccess currentPasswordDataAccess,List<Password> currentPasswordList)
        {
            InitializeComponent();
            this.CurrentPasswordID = currentPasswordID;
            this.PasswordValue = passwordValue;
            this.PasswordName = passwordName;
            this.CurrentPasswordDataAccess = currentPasswordDataAccess;
            this.CurrentPasswordList = currentPasswordList;

            ShowAsHidden();
        }
        private void ShowAsHidden()
        {
            PasswordTextBlock.Text = new string('\u25CF', PasswordValue.Length);
            PasswordTextBlockName.Text = PasswordName + " :";
            _isVisible = false;
        }

        private void BtnEye_Click(object sender, RoutedEventArgs e)
        {
            if (_isVisible)
            {
                PasswordTextBlock.Text = new string('\u25CF', PasswordValue.Length);
                PasswordTextBlockName.Text = PasswordName + " :";
                ((Button)sender).Content = "👁️";
            }
            else
            {
                PasswordTextBlock.Text = PasswordValue;
                PasswordTextBlockName.Text = PasswordName + " :";
                ((Button)sender).Content = "🕳";
            }
            _isVisible = !_isVisible;
        }

        private void BtnCopy_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(PasswordValue);
            var btn = (Button)sender;
            var original = btn.Content;
            btn.Content = "✅";
            Task.Delay(1000).ContinueWith(_ => Dispatcher.Invoke(() => btn.Content = original));
        }
        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var deletedPassword = CurrentPasswordDataAccess.Passwords.First(x => x.Id == CurrentPasswordID);
            var confirmation = MessageBox.Show($"Are you sure you want to delete \"{PasswordName}\" password", "Confirm Deletion", MessageBoxButton.YesNo, MessageBoxImage.Question ,MessageBoxResult.No);
            if (confirmation == MessageBoxResult.Yes)
            {
                CurrentPasswordDataAccess.RemovePassword(deletedPassword.Id);
                var parent = (Panel)this.Parent;
                parent.Children.Remove(this);
                
            }
        }
    }
}
