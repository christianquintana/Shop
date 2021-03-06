﻿namespace Shop.UIForms
{
    using Shop.UIForms.ViewModels;
    using Shop.UIForms.Views;   
    using Xamarin.Forms;
    
    public partial class App : Application
    {
        // Contructor
        public App()
        {
            InitializeComponent();

            // Antes de llamar (instanciar) a la Page, instanciamos en memoria la LoginViewModel ligada a esa Page
            MainViewModel.GetInstance().Login = new LoginViewModel();

            // Se establece la página raíz (inicial) de la aplicación.
            // NavigationPage: Es una página que gestiona la navegación y la experiencia de usuario de un stack (pila, pagina encima de otra) de páginas.
            this.MainPage = new NavigationPage(new LoginPage());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
