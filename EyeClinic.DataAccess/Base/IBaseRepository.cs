using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EyeClinic.DataAccess.Entities;

namespace EyeClinic.DataAccess.Base
{
    public interface IBaseRepository<TModel, TEntity>
        where TModel : class
        where TEntity : class, IEntityModel
    {
        #region Get
        IQueryable<TEntity> Get();
        Task<List<TModel>> GetAll();
        Task<TModel> GetById(object id);
        Task<List<TModel>> GetByKey(Expression<Func<TEntity, bool>> predicate);
        #endregion

        #region Add
        Task<TModel> Add(TModel modelToAdd);
        Task AddRange(IList<TModel> modelsToAdd);
        #endregion

        #region Update
        Task Update(TModel modelToUpdate);
        Task UpdateRange(IList<TModel> modelsToUpdate);
        #endregion

        #region Delete
        Task Delete(int id);
        Task Delete(Expression<Func<TEntity, bool>> predicate);
        #endregion


        Task<int> SaveChangesAsync();
    }
}
