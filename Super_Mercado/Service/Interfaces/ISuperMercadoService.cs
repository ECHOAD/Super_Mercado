using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Super_Mercado.Service.Interfaces
{
    public interface ISuperMercadoService<T>
    {
        Task<List<T>> GetAllAsync(string requestUri);
        Task<T> GetByIdAsync(string requestUri, int Id);
        Task<T> SaveAsync(string requestUri, T obj);
        Task<T> UpdateAsync(string requestUri, int Id, T obj);
        Task<bool> DeleteAsync(string requestUri, int Id);
      


    }
}
