namespace Shop.UIForms.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Shop.Common.Models;
    using Shop.Common.Services;
    using Xamarin.Forms;

    public class ProductsViewModel : BaseViewModel
    {
        private ApiService ApiService;
        private ObservableCollection<Product> products; //propiedad privada
        private bool isRefreshing; 

        //public ObservableCollection<Product> Products { get; set; }
        public ObservableCollection<Product> Products //propiedad publica
        {
            get { return this.products; } //devuelve la propiedad privada
            set { this.SetValue(ref this.products, value); } //establece la propiedad y refresca la vista
        }

        public bool IsRefreshing //propiedad publica
        {
            get { return this.isRefreshing; } //devuelve la propiedad privada
            set { this.SetValue(ref this.isRefreshing, value); } //establece la propiedad y refresca la vista
        }

        public ProductsViewModel()
        {
            this.ApiService = new ApiService();
            this.LoadProducts();
        }

        private async void LoadProducts()
        {
            this.IsRefreshing = true;

            //ApiService.GetListAsync: Método que devuelve lo mismo que Postman
            var response = await this.ApiService.GetListAsync<Product>("https://shopceqn.azurewebsites.net", "/api", "/Products");

            this.IsRefreshing = false;

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "Accept");
                return;
            }

            var myProducts = (List<Product>)response.Result;

            this.Products = new ObservableCollection<Product>(myProducts);
        }
    }
}
