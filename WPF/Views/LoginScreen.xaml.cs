using System.Windows;
using System.Windows.Controls;
using SeaBattle.ViewModels;

namespace SeaBattle.Views;
public partial class LoginScreen : Window
{
    public LoginScreen()
    {
        InitializeComponent();
    }

    private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
    {
        if (DataContext is LoginViewModel viewModel)
        {
            viewModel.Password = ((PasswordBox)sender).Password;
        }
    }
}

