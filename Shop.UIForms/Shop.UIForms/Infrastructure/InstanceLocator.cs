namespace Shop.UIForms.Infrastructure
{
    using ViewModels;

    // Implementando el Patrón Locator: permite mantener una unica instancia durante la ejecucion de la ViewModel durante la vida del proyecto
    public class InstanceLocator 
    {
        // Creamos una propiedad Main de tipo MainViewModel
        public MainViewModel Main { get; set; }

        // Contructor: inicializamos el objeto 
        public InstanceLocator()
        {
            this.Main = new MainViewModel();
        }
    }
}
