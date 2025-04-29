using System;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Crypex.View;


namespace Crypex
{
    public partial class MainWindow : Window
    {
        private ContentControl? contentControl;
        public MainWindow()
        {
            InitializeComponent();
            contentControl = this.FindControl<ContentControl>("ContentControl");
            
            if (contentControl != null){
                ShowLoginPage();
            }
            else{
                Console.WriteLine("contentControl не найден");
                ShowLoginPage();
            }
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public void ShowLoginPage()
        {
            var loginPage = new LoginPage();
            contentControl.Content = loginPage;
        }

        public void ShowHomePage()
        {
            contentControl.Content = new HomePage();
        }

        public void ShowRegistrationPage()
        {
            contentControl.Content = new RegistrationPage(); // Загружаем страницу регистрации
        }

        public void NavigateTo(UserControl page)
        {
            contentControl.Content = page;
        }

    }
}

