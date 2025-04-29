using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.VisualTree;
using Avalonia.Controls.ApplicationLifetimes;
using Crypex.View;
using Crypex.Services;
using System;
using Avalonia;

namespace Crypex.View
{
    public partial class LoginPage : UserControl
    {
        private readonly AuthService _authService;
        public LoginPage()
        {
            InitializeComponent();
            string connectionString = "Host=localhost;Port=5432;Database=Crypex;Username=postgres;Password=your_password";
            _authService = new AuthService(connectionString);
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        // Обработчик нажатия на кнопку "Зарегистрироваться"
        private void OnRegisterClick(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var mainWindow = this.GetVisualRoot() as MainWindow;
            mainWindow?.ShowRegistrationPage();
        }

        private async void OnLoginButtonClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var loginTextBox = this.FindControl<TextBox>("LoginTextBox");
            var passwordTextBox = this.FindControl<TextBox>("PasswordTextBox");

            // Проверяем на null
            if (loginTextBox == null || passwordTextBox == null)
            {
                Console.WriteLine("TextBox не найдены.");
                return;
            }

            string login = loginTextBox.Text ?? "";
            string password = passwordTextBox.Text ?? "";

            bool isAuthenticated = await _authService.AuthenticateAsync(login, password);

            if (isAuthenticated)
            {
                Console.WriteLine("Авторизация успешна!");
    
                if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime lifetime &&
        lifetime.MainWindow is MainWindow mainWindow)
                {
                    mainWindow.ShowHomePage(); 
                }
            }           
            else
            {
                Console.WriteLine("Неверный логин или пароль.");
            }
        }
    }
}