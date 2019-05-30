namespace Shop.UIForms.ViewModels
{
    // Implementando el Patrón MVVM (model–view–viewmodel / Modelo–vista-modelo de vista): 
    // Es un patrón de arquitectura de software. Se caracteriza por tratar de desacoplar lo máximo posible la interfaz de usuario de la lógica de la aplicación.

    // La MainViewModel es la clase principal del proyecto, su objetivo es controlar las demas ViewModels
    public class MainViewModel 
    {        
        // Apuntador a la misma clase
        private static MainViewModel instance; // Patrón Singleton ó instancia única

        public LoginViewModel Login { get; set; }

        public ProductsViewModel Products { get; set; }

        // Contructor
        public MainViewModel()
        {
            // Comentar posteriormente: No es buena practica instanciar en la MainViewModel las otras ViewModel, solo referenciarlas
            //this.Login = new LoginViewModel(); 
            instance = this;
        }

        // Patrón Singleton ó instancia única
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
