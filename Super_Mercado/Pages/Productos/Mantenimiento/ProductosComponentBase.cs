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

namespace Super_Mercado.Pages.Productos.Mantenimiento
{
    public class ProductosComponentBase : ComponentBase
    {

        [Inject] public IUsuarioServicio usuarioServicio { get; set; }


        [Inject] public ISuperMercadoService<Producto> SuperMercadoServicioProductos { get; set; }
        [Inject] public ISuperMercadoService<Categoria> SuperMercadoServicioCategoria { get; set; }



        [Inject] public ILocalStorageService localStorageService { get; set; }

        [Inject] public IJSRuntime jSRuntime { get; set; }

        [Inject] private IWebHostEnvironment _host { get; set; }
        [Inject] private IHttpContextAccessor _contextAcessor { get; set; }



        protected RadzenDataGrid<Producto> productosGrid;
        protected List<Producto> productos;
        protected List<Categoria> categorias;
        private byte[] _filebytes;
        private IFileListEntry file;


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


        protected void EditRow(Producto order)
        {
            productosGrid.EditRow(order);
        }


        protected async void HandleFileSelection(IFileListEntry[] value)
        {
            if (value is not null)
            {

                file = null;
                file = value[0];

                using var ms = new MemoryStream();

                await file.Data.CopyToAsync(ms);
                _filebytes = ms.ToArray();



            }
        }

        protected void OnError(UploadErrorEventArgs args, string name)
        {

        }
        protected async Task LoadData()
        {
            productos = await SuperMercadoServicioProductos.GetAllAsync("Productos/GetProductos");

            categorias = await SuperMercadoServicioCategoria.GetAllAsync("Categorias/GetCategorias");
        }

        protected void SaveRow(Producto producto)
        {
            productosGrid.UpdateRow(producto);
        }

        protected void CancelEdit(Producto producto)
        {

            productosGrid.CancelEditRow(producto);



        }




        protected async void DeleteRow(Producto producto)
        {
            await SuperMercadoServicioProductos.DeleteAsync("Producto/DeleteProducto", producto.Id);
        }

        protected void InsertRow()
        {

            var producto = new Producto();
            producto.Categorias = new Categoria();

            productosGrid.InsertRow(producto);
        }

        private async Task SaveProducto(Producto producto)
        {



            if (file is not null || producto.Id != 0)
            {



                if (file is not null )
                {
                    var path = "";


                    var extension = System.IO.Path.GetExtension(file.Name);

                    if (!Directory.Exists($"{_host.WebRootPath}/Images/Productos"))
                    {

                        Directory.CreateDirectory($"{_host.WebRootPath}/Images/Productos");





                    }

                    path = $"{_host.WebRootPath}/Images/Productos/Producto_{producto.Nombre}{extension}";


                    var fileStream = new FileStream(path, FileMode.Create);


                    fileStream.Write(_filebytes, 0, _filebytes.Length);




                    producto.Imagen = path;


                }





                //producto.Categorias= await SuperMercadoServicioCategoria.GetByIdAsync("Categorias/GetCategoria/", producto.Id_Categoria);


                if (producto.Id == 0)
                {


                    await SuperMercadoServicioProductos.SaveAsync("Productos/CreateProducto", producto);

                }
                else
                {


                    await SuperMercadoServicioProductos.UpdateAsync("Productos/UpdateProducto/", producto.Id, producto);

                }

            }





        }









        //producto.Photo = new FormFile(stream, 0, stream.Length, producto.Nombre, File.Name)
        //{
        //    ContentType = File.ContentType,
        //    ContentDisposition = "form=data"


        //};




        protected async void OnCreateRow(Producto producto)
        {
            await SaveProducto(producto);

        }

        protected async void OnUpdateRow(Producto producto)
        {
            await SaveProducto(producto);

        }
    }
}

