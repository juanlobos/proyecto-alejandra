using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Repository
{
    public interface IRepositoryGeneral
    {

        // <summary>
        /// listo todos los registros de una tabla de forma asincrona por Entity
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        Task<List<TEntity>> GetAll<TEntity>() where TEntity : class;
        /// <summary>
        /// consulta IQueryable generica para cualquier objeto por Entity
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        IQueryable<TEntity> GetQueryable<TEntity>() where TEntity : class;

        /// <summary>
        /// traigo el primer registro de una consulta de una forma asincrona por Entity de acuerdo a las condiciones
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<TEntity> GetFirst<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class;
        /// <summary>
        /// listo todos los registros de una tabla de forma asincrona de acuerdo al predicado y por Entity
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<List<TEntity>> GetList<TEntity>(Expression<System.Func<TEntity, bool>> predicate) where TEntity : class;
        /// <summary>
        /// metodo para agregar un registro en la base por Entity
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        Task Add<TEntity>(TEntity entity) where TEntity : class;
        /// <summary>
        /// eliminar un registro de la base de datos por Entity
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        Task Delete<TEntity>(TEntity entity) where TEntity : class;
        /// <summary>
        /// actualizar un registro por Entity
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        Task Update<TEntity>(TEntity entity, object id) where TEntity : class;
        /// <summary>
        /// obtener el registro de una tabla por el id por Entity
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TEntity> Find<TEntity>(object id) where TEntity : class;

    }
}
