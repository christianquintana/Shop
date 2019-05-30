namespace Shop.UIForms.ViewModels
{ 
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Shop.Common.Models;
    using Shop.Common.Services;
    using Xamarin.Forms;

    
    public class ProductsViewModel : BaseViewModel // Hereda  
    {
        private ApiService ApiService;

        // Propiedad Privada
        private ObservableCollection<Product> products; 
        private bool isRefreshing;

        //public ObservableCollection<Product> Products { get; set; }

        // Propiedad Publica
        // Cualquier propiedad que se necesite cambiar en la ViewModel y que requiera verse reflejado en la View debemos hacer lo mismo
        public ObservableCollection<Product> Products 
        {
            // Cuando se pida el valor de Products devuelve la propiedad Privada
            get { return this.products; }

            // BaseViewModel.SetValue<T> 
            // Cuando se establezca el valor de Products tambien refresca la vista 
            set { this.SetValue(ref this.products, value); } 
        }

        // Propiedad publica
        // Cualquier propiedad que se necesite cambiar en la ViewModel y que requiera verse reflejado en la View debemos hacer lo mismo
        public bool IsRefreshing 
        {
            // Cuando se pida el valor de IsRefreshing devuelve la propiedad Privada
            get { return this.isRefreshing; }

            // BaseViewModel.SetValue<T>             
            // Cuando se establezca el valor de IsRefreshing tambien refresca la vista             
            set { this.SetValue(ref this.isRefreshing, value); } 
        }

        // Constructor
        public ProductsViewModel()
        {
            this.ApiService = new ApiService();
            this.LoadProducts();
        }

        // Método para cargar los productos
        private async void LoadProducts()
        {
            this.IsRefreshing = true;

            //ApiService.GetListAsync<T>(string urlBase, string servicePrefix, string controller): Método que devuelve lo mismo que Postman
            var response = await this.ApiService.GetListAsync<Product>("https://shopceqn.azurewebsites.net", "/api", "/Products");

            this.IsRefreshing = false;

            // Se obtiene un valor booleano que indica si la respuesta HTTP fue exitosa  
            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "Accept");
                return;
            }

            // Convertimos el resultado en una lista de Product
            var myProducts = (List<Product>)response.Result;

            // Pintamos el ObservableCollection <ListView ItemsSource = "{Binding Products}" >
            // La unica forma de que al cambiar el ObservableCollection (Products) la View se de cuenta es implementando la INotifyPropertyChanged
            // significa que cuando cambia algo en la ViewModel se va refrescar automaticamente en la View
            this.Products = new ObservableCollection<Product>(myProducts);
        }
    }
}
