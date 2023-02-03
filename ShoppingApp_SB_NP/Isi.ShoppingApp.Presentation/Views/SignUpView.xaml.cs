using Isi.ShoppingApp.Presentation.ViewModels;
using Isi.Utility.Authentication;
using System.Windows;

//SHARMAINE
namespace Isi.ShoppingApp.Presentation.Views
{
    public partial class SignUpView : Window
    {
        SignUpViewModel viewModel;
        public SignUpView()
        {
            viewModel = new SignUpViewModel();
            InitializeComponent();
            DataContext = viewModel;
            viewModel.SignUpSucceeded += OnSignUpSucceeded;
        }

        private void OnSignUpSucceeded(string message)
        {
            LoginView loginView = new LoginView();
            loginView.Show();
            this.Close();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            viewModel.SignUpCommand.NotifyCanExecuteChanged();
            viewModel.HashedPassword = PasswordHasher.HashPassword(passwordBox.Password);
        }
    }
}
