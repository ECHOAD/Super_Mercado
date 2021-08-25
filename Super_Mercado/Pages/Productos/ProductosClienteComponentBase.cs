using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;
using Super_Mercado.Service;
using Blazored.LocalStorage;
using Microsoft.JSInterop;
using Super_Mercado.Service.Interfaces;
using Entidades;
using Radzen.Blazor;
using Radzen;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Net.Http;
using BlazorInputFile;

namespace Super_Mercado.Pages.Productos
{
    public class ProductosClienteComponentBase : ComponentBase
    {

        [Inject] public IUsuarioServicio usuarioServicio { get; set; }

        [Inject] public ISuperMercadoService<Producto> SuperMercadoServicioProductos { get; set; }

        [Inject] public ILocalStorageService localStorageService { get; set; }

        [Inject] public IJSRuntime jSRuntime { get; set; }

        protected List<Producto> productos;


        protected override async Task OnInitializedAsync()
        {
            await LoadData();
        }


        protected async Task AddToCart(Producto producto)
        {
            var cart = await localStorageService.GetItemAsync<List<Producto>>("product_car");
            if (cart == null)
            {
                cart = new List<Producto>();
            }

            cart.Add(producto);
            await localStorageService.SetItemAsync("product_car", cart);

        }






        protected override async Task OnAfterRenderAsync(bool firstRender)
        {

            if (firstRender)
            {
                await LoadData();
            }
        }


        protected async Task LoadData()
        {
            productos = await SuperMercadoServicioProductos.GetAllAsync("Productos/GetProductos");

        }

    }
}

