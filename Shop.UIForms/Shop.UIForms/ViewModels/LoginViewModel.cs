namespace Shop.UIForms.ViewModels
{ 
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using Shop.UIForms.Views;
    using Xamarin.Forms;

    public class LoginViewModel
    {
        public string Email { get; set; }

        public string Password { get; set; }

        // Propiedad que define un comando (ICommand), para implementarlo (RelayCommand) se debe instalar el paquete NuGet "MvvmLightLibsStd10"
        // Delegado solo lectura
        public ICommand LoginCommand => new RelayCommand(Login);

        //public ICommand LoginCommand // Es lo mismo que el delegado
        //{
        //    get
        //    {
        //        return new RelayCommand(Login);
        //    }
        //} 

        // Constructor
        public LoginViewModel()
        {
            // Se pueden inicializar los campos para no digitarlos a cada ingreso, esto debido a que se tiene una coneccion directa entre la ViewModel y la View 
            // Solo funciona en el constructor

            //this.Email = "ceqn_20@hotmail.com";
            //this.Password = "123456";
        }

        // Método para iniciar sesión
        private async void Login()
        {
            // A diferencia con el patrón MVC, con el Patrón MVVM los objetos ya estan bindeados por lo que ya no debo enviar modelos
            // DisplayAlert: Presenta un diálogo de alerta al usuario de la aplicación con un botón de aceptar y cancelar. DisplayAlert (title, message, cancel, accept)
            // DisplayActionSheet: Muestra una hoja de acción de la plataforma nativa, que permite al usuario de la aplicación elegir entre varios botones.

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

            // Antes de llamar (instanciar) a la Page, instanciamos en memoria la ProductsViewModel ligada a esa Page
            MainViewModel.GetInstance().Products = new ProductsViewModel();

            // PushAsync: Asíncronamente agrega una página a la parte superior de la pila de navegación
            // PopAsync: Elimina de forma asíncrona la página más reciente de la pila de navegación
            // NavigationPage: Es una página que gestiona la navegación y la experiencia de usuario de un stack (pila, pagina encima de otra) de páginas.
            await Application.Current.MainPage.Navigation.PushAsync(new ProductsPage());

        }

    }
}
