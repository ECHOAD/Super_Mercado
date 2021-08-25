using Blazored.LocalStorage;
using Entidades;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;
using Super_Mercado.Service;
using Super_Mercado.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Super_Mercado.Pages.Usuarios
{
    public class UsuariosComponent : ComponentBase
    {

        [Inject] public IUsuarioServicio usuarioServicio { get; set; }

        [Inject] public ISuperMercadoService<Usuario> UsuarioServicio { get; set; }

        [Inject] public ISuperMercadoService<Role> SuperMercadoServicioCategoria { get; set; }

        [Inject] public ILocalStorageService localStorageService { get; set; }

        [Inject] public IJSRuntime jSRuntime { get; set; }

        [Inject] private IWebHostEnvironment _host { get; set; }
        [Inject] private IHttpContextAccessor _contextAcessor { get; set; }



        protected RadzenDataGrid<Usuario> UsuariosGrid;
        
        protected List<Usuario> Usuarios;

        protected List<Role> Roles;



        protected override async Task OnInitializedAsync()
        {
            await LoadData();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {

            if (firstRender)
            {

            }
        }


        protected void EditRow(Usuario usuario)
        {
            UsuariosGrid.EditRow(usuario);
        }




        protected void OnError(UploadErrorEventArgs args, string name)
        {

        }


        protected async Task LoadData()
        {
            Usuarios = await UsuarioServicio.GetAllAsync("Usuarios/GetAllUsuarios");

            Roles = await SuperMercadoServicioCategoria.GetAllAsync("Roles/GetRoles");
        }

        protected void SaveRow(Usuario usuario)
        {
            UsuariosGrid.UpdateRow(usuario);
        }

        protected void CancelEdit(Usuario usuario)
        {
            UsuariosGrid.CancelEditRow(usuario);
        }

        protected async void DeleteRow(Usuario producto)
        {
            await UsuarioServicio.DeleteAsync("Usuarios/GetUserDetails/", producto.Id);
        }

        protected void InsertRow()
        {
            var usuario = new Usuario();
            usuario.Roles = new Role();

            UsuariosGrid.InsertRow(usuario);
        }

        private async Task SaveUsuario(Usuario usuario)
        {
            if (usuario.Id == 0)
            {

            }
            else
            {
                await UsuarioServicio.UpdateAsync("Productos/UpdateUser/", usuario.Id, usuario);
            }

        }


        protected async void OnUpdateRow(Usuario usuario)
        {
            await SaveUsuario(usuario);

        }
    }
}



