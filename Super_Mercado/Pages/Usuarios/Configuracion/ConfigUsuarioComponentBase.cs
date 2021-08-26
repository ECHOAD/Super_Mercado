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


        protected string _username = "";


        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                //await LoadCategorias();

                isLoading = false;
                StateHasChanged();

            }
        }

        protected  void LoadEditDireccion(UsuarioDireccion usuarioDireccion)
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
            try
            {
                direcciones = await SuperMercadoServicio.GetAllAsync($"DireccionUsuario/GetDirecciones/{_username}");


            }
            catch (Exception)
            {

            }

        }

        protected async Task SaveUsuarioDireccion()
        {
            mDireccionUsuario.Usuarios = new();

            mDireccionUsuario.Usuarios.User = _username;



            if (mDireccionUsuario.Id == 0)
                await SuperMercadoServicio.SaveAsync($"DireccionUsuario/CreateUsuarioDireccion/", mDireccionUsuario);
            else
                await SuperMercadoServicio.UpdateAsync("DireccionUsuario/UpdateCategoria/", mDireccionUsuario.Id, mDireccionUsuario);

            await LoadDirecciones();
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
