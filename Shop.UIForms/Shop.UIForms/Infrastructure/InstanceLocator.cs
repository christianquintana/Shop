namespace Shop.UIForms.Infrastructure
{
    using ViewModels;

    public class InstanceLocator //Patrón Locator: permite mantener una unica instancia durante la ejecucion de la ViewModel durante la vida del proyecto
    {
        public MainViewModel Main { get; set; }

        public InstanceLocator()
        {
            this.Main = new MainViewModel();
        }
    }
}
