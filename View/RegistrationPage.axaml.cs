using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Crypex.Services;
using Avalonia.Interactivity;
using System;
using System.Threading.Tasks;

namespace Crypex.View
{
    public partial class RegistrationPage : UserControl
    {
        private readonly AuthService _authService;

        private TextBox _loginTextBox;
        private TextBox _firstNameTextBox;
        private TextBox _lastNameTextBox;
        private TextBox _passwordTextBox;
        private TextBox _passwordReplaceTextBox;

        public RegistrationPage()
        {
            InitializeComponent();
            string connectionString = "Host=localhost;Port=5432;Database=Crypex;Username=postgres;Password=your_password";
            _authService = new AuthService(connectionString);
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            // Привязка элементов по x:Name
            _loginTextBox = this.FindControl<TextBox>("LoginTextBox");
            _firstNameTextBox = this.FindControl<TextBox>("FirstNameTextBox");
            _lastNameTextBox = this.FindControl<TextBox>("LastNameTextBox");
            _passwordTextBox = this.FindControl<TextBox>("PasswordTextBox");
            _passwordReplaceTextBox = this.FindControl<TextBox>("PasswordReplaceTextBox");
        }

        private async void RegisterClick(object sender, RoutedEventArgs e)
        {
            string login = _loginTextBox.Text;
            string firstName = _firstNameTextBox.Text;
            string lastName = _lastNameTextBox.Text;
            string password = _passwordTextBox.Text;
            string confirmPassword = _passwordReplaceTextBox.Text;

            if (string.IsNullOrWhiteSpace(login) ||
                string.IsNullOrWhiteSpace(firstName) ||
                string.IsNullOrWhiteSpace(lastName) ||
                string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(confirmPassword))
            {
                await ShowMessageAsync("Пожалуйста, заполните все поля.");
                return;
            }

            if (password != confirmPassword)
            {
                await ShowMessageAsync("Пароли не совпадают.");
                return;
            }

            try
            {
                bool registered = await _authService.RegisterAsync(login, firstName, lastName, password);

                if (registered)
                {
                    await ShowMessageAsync("Регистрация успешна.");
                    if (VisualRoot is Window window)
                        window.Content = new LoginPage(); // переход к странице входа
                }
                else
                {
                    await ShowMessageAsync("Ошибка регистрации. Возможно, пользователь с таким логином уже существует.");
                }
            }
            catch (Exception ex)
            {
                await ShowMessageAsync("Произошла ошибка: " + ex.Message);
            }
        }

        private async Task ShowMessageAsync(string message)
        {
            var dialog = new Window
            {
                Width = 300,
                Height = 150,
                Title = "Сообщение",
                Content = new TextBlock
                {
                    Text = message,
                    VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
                    HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                    FontSize = 16,
                }
            };

            await dialog.ShowDialog((Window)this.VisualRoot);
        }

        private void LoginClick(object sender, RoutedEventArgs e)
        {
            if (VisualRoot is Window window)
                window.Content = new LoginPage();
        }
    }
}