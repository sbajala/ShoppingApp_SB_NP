using Isi.ShoppingApp.Core.Entities;
using Isi.ShoppingApp.Domain.Services;
using Isi.ShoppingApp.Presentation.ViewModels;
using Isi.Utility.Authentication;
using System;
using System.Diagnostics;
using System.Windows;

//SHARMAINE
namespace Isi.ShoppingApp.Presentation.Views
{
    public partial class LoginView : Window
    {
        LoginViewModel viewModel;
        UserService service;
        public LoginView()
        {
            InitializeComponent();
            viewModel = new LoginViewModel();
            DataContext = viewModel;

            viewModel.LoginSucceeded += OnLoginSucceeded;
            viewModel.FailedLogin += OnLoginFailed;
            signUpButton.Click += OnSignUpButtonClicked;

            service = new UserService();
        }

        private void OnLoginFailed(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            ClearTextbox();
        }

        private void OnSignUpButtonClicked(object sender, RoutedEventArgs e)
        {
            passwordBox.Clear();
            SignUpView signUpView = new SignUpView();
            signUpView.Show();
            this.Close();
        }

        private void OnLoginSucceeded()
        {
            MessageBox.Show("Logged in. Opening main window"); //FOR TESTING PURPOSES
            User user = service.GetUser(viewModel.Username);
            if (viewModel.IsAdmin)
            {
                //open admin window and pass user
                this.Close();
            }
            else
            {
                //MainWindow mainWindow = new MainWindow(user);
                //mainWindow.Show();
                this.Close();
            }
        }

        private void passwordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            viewModel.LoginCommand.NotifyCanExecuteChanged();
            viewModel.Password = passwordBox.Password;
        }

        private void ClearTextbox()
        {
            passwordBox.Clear();
            usernameTextBox.Text = "";
        }
    }
}
