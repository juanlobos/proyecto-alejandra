using Domain.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class RepositoryGeneral : IRepositoryGeneral
    {
        private readonly ConexionContext _con;

        public RepositoryGeneral(ConexionContext con)
        {
            _con = con;
        }
        /// <summary>
        /// metodo para agregar un registro en la base por Entity
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public async Task Add<TEntity>(TEntity entity) where TEntity : class
        {
            _con.Set<TEntity>().Add(entity);
            await _con.SaveChangesAsync();
        }
        /// <summary>
        /// eliminar un registro de la base de datos por Entity
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public async Task Delete<TEntity>(TEntity entity) where TEntity : class
        {
            _con.Set<TEntity>().Remove(entity);
            await _con.SaveChangesAsync();
        }
        // <summary>
        /// traigo el primer registro de una consulta de una forma asincrona por el id por Entity
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public async Task<TEntity> Find<TEntity>(object id) where TEntity : class
        {
            return await _con.Set<TEntity>().FindAsync(id);
        }
        // <summary>
        /// listo todos los registros de una tabla de forma asincrona por Entity
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public async Task<List<TEntity>> GetAll<TEntity>() where TEntity : class
        {
            return await _con.Set<TEntity>().ToListAsync();
        }
        /// <summary>
        /// traigo el primer registro de una consulta de una forma asincrona por Entity de acuerdo a las condiciones
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task<TEntity> GetFirst<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            return await _con.Set<TEntity>().FirstOrDefaultAsync(predicate);
        }
        /// <summary>
        /// listo todos los registros de una tabla de forma asincrona de acuerdo al predicado y por Entity
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task<List<TEntity>> GetList<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            return await _con.Set<TEntity>().Where(predicate).ToListAsync();
        }
        /// <summary>
        /// consulta IQueryable generica para cualquier objeto por Entity
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public IQueryable<TEntity> GetQueryable<TEntity>() where TEntity : class
        {
            return _con.Set<TEntity>().AsQueryable();
        }


        /// <summary>
        /// actualizar un registro
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task Update<TEntity>(TEntity entity, object id) where TEntity : class
        {
            var entidad = await _con.Set<TEntity>().FindAsync(id);
            if (entidad != null)
            {
                _con.Entry(entidad).CurrentValues.SetValues(entity);
                await _con.SaveChangesAsync();
            }
        }
      
    }
}