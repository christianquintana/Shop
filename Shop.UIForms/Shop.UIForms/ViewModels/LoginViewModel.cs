namespace Shop.UIForms.ViewModels
{
    using System;
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using Shop.UIForms.Views;
    using Xamarin.Forms;

    public class LoginViewModel
    {
        public string Email { get; set; }

        public string Password { get; set; }

        // Propiedad que define un comando
        // Delegado solo lectura, instalar paquete NuGet MvvmLightLibsStd10
        public ICommand LoginCommand => new RelayCommand(Login); 

        //public ICommand LoginCommand // Es lo mismo que el delegado
        //{
        //    get
        //    {
        //        return new RelayCommand(Login);
        //    }
        //} 

        public LoginViewModel() //constructor para inicializar los campos y no estar digitando
        {
            this.Email = "ceqn_20@hotmail.com";
            this.Password = "123456";
        }

        private async void Login()
        {
            if (string.IsNullOrEmpty(this.Email))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "You must enter an email.", "Accept");
                return;
            }
            if (string.IsNullOrEmpty(this.Password))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "You must enter an password.", "Accept");
                return;
            }

            if (!this.Email.Equals("ceqn_20@hotmail.com") || !this.Password.Equals("123456"))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Use or password wrong.", "Accept");
                return;
            }

            //await Application.Current.MainPage.DisplayAlert("Ok", "Fuck Yeah!!!..", "Accept");

            MainViewModel.GetInstance().Products = new ProductsViewModel(); //antes de llamar(instanciar) a la Page, instanciamos en memoria la ProductsViewModel ligada a esa Page

            await Application.Current.MainPage.Navigation.PushAsync(new ProductsPage());

        }

    }
}
