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

namespace Super_Mercado.Pages.ImagesWeb.Mantenimiento
{
    public class ImageWebComponentBase : ComponentBase
    {

        [Inject] public IUsuarioServicio usuarioServicio { get; set; }

        [Inject] public ISuperMercadoService<ImagenWebPage> SuperMercadoServicioProductos { get; set; }

        [Inject] public ILocalStorageService localStorageService { get; set; }

        [Inject] public IJSRuntime jSRuntime { get; set; }

        [Inject] private IWebHostEnvironment _host { get; set; }
        [Inject] private IHttpContextAccessor _contextAcessor { get; set; }



        protected RadzenDataGrid<ImagenWebPage> imagenesGrid;
        protected List<ImagenWebPage> imagenes;
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


        protected void EditRow(ImagenWebPage img)
        {
            imagenesGrid.EditRow(img);
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
            imagenes = await SuperMercadoServicioProductos.GetAllAsync("ImagesWeb/GetImages");
        }

        protected void SaveRow(ImagenWebPage img)
        {
            imagenesGrid.UpdateRow(img);
        }

        protected void CancelEdit(ImagenWebPage img)
        {

            imagenesGrid.CancelEditRow(img);

        }

        protected async void DeleteRow(ImagenWebPage img)
        {
            await SuperMercadoServicioProductos.DeleteAsync("ImagesWeb/DeleteImage/", img.Id);

            await OnInitializedAsync();
        }

        protected void InsertRow()
        {

            var producto = new ImagenWebPage();

            imagenesGrid.InsertRow(producto);
        }

        private async Task SaveImagen(ImagenWebPage img)
        {



            if (file is not null || img.Id != 0)
            {



                if (file is not null)
                {
                    var path = "";


                    var extension = System.IO.Path.GetExtension(file.Name);

                    if (!Directory.Exists($"{_host.WebRootPath}/Images/ImagenesWeb"))
                    {

                        Directory.CreateDirectory($"{_host.WebRootPath}/Images/ImagenesWeb");

                    }

                    path = $"{_host.WebRootPath}/Images/ImagenesWeb/Img_{img.Titulo}{extension}";


                    var fileStream = new FileStream(path, FileMode.Create);


                    fileStream.Write(_filebytes, 0, _filebytes.Length);


                    img.Path = path;


                }



                //producto.Categorias= await SuperMercadoServicioCategoria.GetByIdAsync("Categorias/GetCategoria/", producto.Id_Categoria);


                if (img.Id == 0)
                {


                    await SuperMercadoServicioProductos.SaveAsync("ImagesWeb/CreateImage", img);

                }
                else
                {


                    await SuperMercadoServicioProductos.UpdateAsync("ImagesWeb/UpdateImage/", img.Id, img);

                }

            }





        }









        //producto.Photo = new FormFile(stream, 0, stream.Length, producto.Nombre, File.Name)
        //{
        //    ContentType = File.ContentType,
        //    ContentDisposition = "form=data"


        //};




        protected async void OnCreateRow(ImagenWebPage producto)
        {
            await SaveImagen(producto);

        }

        protected async void OnUpdateRow(ImagenWebPage producto)
        {
            await SaveImagen(producto);

        }
    }
}

