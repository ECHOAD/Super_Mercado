using Blazored.LocalStorage;
using Entidades;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using Super_Mercado.Service;
using Super_Mercado.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;


namespace Super_Mercado.Pages.Usuarios.Configuracion
{
    public class ConfigUsuarioComponentBase : ComponentBase
    {
        [Inject] public IUsuarioServicio usuarioServicio { get; set; }


        [Inject] public ISuperMercadoService<UsuarioDireccion> SuperMercadoServicio { get; set; }


        [Inject] public ILocalStorageService localStorageService { get; set; }

        [Inject] public IJSRuntime jSRuntime { get; set; }

        [CascadingParameter]
        protected Task<AuthenticationState> authenticationStateTask { get; set; }



        protected UsuarioDireccion mDireccionUsuario { get; set; }
        protected List<UsuarioDireccion> direcciones { get; set; }


        protected bool isLoading = true;
        protected bool EditarMode = false;


        private string _username = "";


        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                //await LoadCategorias();

                isLoading = false;
                StateHasChanged();

            }
        }

        protected async void LoadEditCategoria(UsuarioDireccion usuarioDireccion)
        {




            mDireccionUsuario = usuarioDireccion;

            if (mDireccionUsuario != null)
            {
                EditarMode = true;
            }

        }

        protected override async Task OnInitializedAsync()
        {
            await LoadDirecciones();
        }

        protected async Task LoadDirecciones()
        {
            mDireccionUsuario = new();
            EditarMode = false;


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

            direcciones = await SuperMercadoServicio.GetAllAsync($"DireccionUsuario/GetDireccionUsuario/?usuario={_username}");
        }

        protected async Task SaveCategoria()
        {
            //if (mCategoria.Id == 0)
            //    await SuperMercadoServicio.SaveAsync("Categorias/CreateCategoria", mCategoria);
            //else
            //    await SuperMercadoServicio.UpdateAsync("Categorias/UpdateCategoria/", mCategoria.Id, mCategoria);

            //await LoadCategorias();
        }

        protected async Task DeleteCategoria(int? id)
        {
            if (id == null)
            {

            }
            else
            {
                await SuperMercadoServicio.DeleteAsync("Categorias/DeleteCategoria/", id.Value);

                //await LoadCategorias();

            }

        }

    }
}
