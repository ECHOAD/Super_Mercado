using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Super_Mercado.Service;
using Blazored.LocalStorage;
using Microsoft.JSInterop;
using Super_Mercado.Service.Interfaces;
using Entidades;

namespace Super_Mercado.Pages.Categorias
{

    public class CategoriaComponentsBase : ComponentBase
    {


        [Inject] public IUsuarioServicio usuarioServicio { get;set; }
        
        
        [Inject] public ISuperMercadoService<Categoria> SuperMercadoServicio { get; set; }


        [Inject] public ILocalStorageService localStorageService { get; set; }

        [Inject] public IJSRuntime jSRuntime { get; set; }



   

        protected Categoria mCategoria { get; set; }
        protected List<Categoria> categorias { get; set; }
        protected bool isLoading = true;
        protected bool EditarMode=false;


        protected override void OnInitialized()
        {
            mCategoria = new();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await LoadCategorias();

                isLoading = false;
                StateHasChanged();
            }
        }

        protected void LoadEditCategoria(Categoria categoria)
        {

            mCategoria = categoria;

            if (mCategoria != null)
            {
                EditarMode = true;
            }

        }

        protected async Task LoadCategorias()
        {
            mCategoria = new();
            EditarMode = false;
            categorias = await SuperMercadoServicio.GetAllAsync("Categorias/GetCategorias");
        }

        protected async Task SaveCategoria()
        {
            if (mCategoria.Id== 0)
                await SuperMercadoServicio.SaveAsync("Categorias/CreateCategoria", mCategoria);
            else
                await SuperMercadoServicio.UpdateAsync("Categorias/UpdateCategoria/", mCategoria.Id, mCategoria);

            await LoadCategorias();
        }

        protected async Task DeleteCategoria(int? id)
        {
            if (id == null)
            {

            }
            else
            {
                await SuperMercadoServicio.DeleteAsync("Categorias/DeleteCategoria/", id.Value);

                await LoadCategorias();

            }
          
        }
    }
}
