namespace Shop.UIForms.ViewModels
{
    public class MainViewModel //Patrón MVVM (Modelo–vista vista-modelo): Es la clase principal, el objetivo es controlar las demas ViewModels
    {
        private static MainViewModel instance; //apuntador a la misma clase (Patrón Singleton ó instancia única)

        public LoginViewModel Login { get; set; }

        public ProductsViewModel Products { get; set; }

        public MainViewModel()
        {
            //this.Login = new LoginViewModel(); //Quitar, no es buena practica
            instance = this;
        }

        public static MainViewModel GetInstance()
        {
            if (instance == null)
            {
                return new MainViewModel();
            }
            return instance;
        }

    }
}
