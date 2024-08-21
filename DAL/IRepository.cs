using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public interface IRepository : IDisposable
    {
        // Agregar una nueva entidad a la base de datos 
        Task<TEntity> CreateAsync<TEntity>(TEntity toCreate) where TEntity : class;

        //Para Eliminar una entidad
        Task<bool> DeleteAsync<TEntity>(TEntity toDelete) where TEntity : class;

        // Para actualizar una entidad
        Task<bool> UpdateAsync<TEntity>(TEntity toUpdate) where TEntity : class;

        //Para recuperar una entidad con base en un criterio 
        Task<TEntity> RetrieveAsync<TEntity>(Expression<Func<TEntity, bool>> criteria) where TEntity : class;

        //Para recuperar un conjunto de entidades con base en un criterio de busqueda 

        Task<List<TEntity>> FilterAsync<TEntity>(Expression<Func<TEntity, bool>> criteria) where TEntity : class;

    }
}

