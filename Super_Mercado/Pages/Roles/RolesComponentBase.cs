using Microsoft.AspNetCore.Components;
using Super_Mercado.Service;
using Super_Mercado.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entidades;
using Blazored.LocalStorage;
using Microsoft.JSInterop;

namespace Super_Mercado.Pages.Roles
{
    public class RolesComponentBase: ComponentBase
    {

        [Inject] public IUsuarioServicio usuarioServicio { get; set; }


        [Inject] public ISuperMercadoService<Role> SuperMercadoServicio { get; set; }


        [Inject] public ILocalStorageService localStorageService { get; set; }

        [Inject] public IJSRuntime jSRuntime { get; set; }





        protected Role mRol { get; set; }
        protected List<Role> Roles { get; set; }
        protected bool isLoading = true;
        protected bool EditarMode = false;


        protected override void OnInitialized()
        {
            mRol = new();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await LoadRoles();

                isLoading = false;
                StateHasChanged();
            }
        }

        protected void LoadEditRol(Role Role)
        {

            mRol = Role;

            if (mRol != null)
            {
                EditarMode = true;
            }

        }

        protected async Task LoadRoles()
        {
            mRol = new();
            EditarMode = false;
            Roles = await SuperMercadoServicio.GetAllAsync("Roles/GetRoles");
        }

        protected async Task SaveRol()
        {
            if (mRol.Id == 0)
                await SuperMercadoServicio.SaveAsync("Roles/CreateRole", mRol);
            else
                await SuperMercadoServicio.UpdateAsync("Roles/UpdateRole/", mRol.Id, mRol);

            await LoadRoles();
        }

        protected async Task DeleteRol(int? id)
        {
            if (id == null)
            {

            }
            else
            {
                await SuperMercadoServicio.DeleteAsync("Roles/DeleteRole/", id.Value);

                await LoadRoles();

            }

        }


    }
}
