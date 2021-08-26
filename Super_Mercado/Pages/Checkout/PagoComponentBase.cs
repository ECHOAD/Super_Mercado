using Blazored.LocalStorage;
using Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using Super_Mercado.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Super_Mercado.Pages.Checkout
{
    public class PagoComponentBase : ComponentBase
    {

        [Inject] public ISuperMercadoService<Ordenes> SuperMercadoServicioProductos { get; set; }

        [Inject] public ISuperMercadoService<UsuarioDireccion> SuperMercadoServicioDirecciones { get; set; }


        [Inject] public ILocalStorageService localStorageService { get; set; }

        [Inject] public IJSRuntime jSRuntime { get; set; }


        [Inject] IAuthorizationService authorizationService { get; set; }

        protected List<Producto> productosEnCart { get; set; }

        protected double Sum = 0;

        protected string _userId;

        [CascadingParameter]
        protected Task<AuthenticationState> authenticationStateTask { get; set; }
        protected string _username = "";
        protected List<UsuarioDireccion> lst_direcciones { get; set; }



        protected override async Task OnInitializedAsync()
        {
            productosEnCart = await localStorageService.GetItemAsync<List<Producto>>("product_car");

            if (productosEnCart != null)
                foreach (var item in productosEnCart)
                {
                    Sum += item.Precio;
                }



            var user = (await authenticationStateTask).User;

            if (user != null)
            {
                var i = user.Claims;


                foreach (var claim in i)
                {
                    if (claim.Type == "User")
                    {

                        _username = claim.Value;

                        break;
                    }
                }
            }
            try
            {
                lst_direcciones = await SuperMercadoServicioDirecciones.GetAllAsync($"DireccionUsuario/GetDirecciones/{_username}");


            }
            catch (Exception)
            {

            }




        }

        protected async void DeleteCart(Producto producto)
        {

            productosEnCart.Remove(producto);

            
            await localStorageService.SetItemAsync("product_car", lst_direcciones);

        }




    }
}
