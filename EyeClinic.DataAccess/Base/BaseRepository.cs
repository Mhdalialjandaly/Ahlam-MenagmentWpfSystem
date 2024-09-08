using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using EyeClinic.Core;
using EyeClinic.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace EyeClinic.DataAccess.Base
{
    public class BaseRepository<TModel, TEntity> : IBaseRepository<TModel, TEntity>
        where TEntity : class, IEntityModel
        where TModel : class
    {
        private readonly IMapper _mapper;
        private readonly EyeClinicContext _context;

        public BaseRepository(IMapper mapper, EyeClinicContext context) {
            _mapper = mapper;
            _context = context;

            var user = Global.GetValue(GlobalKeys.LoggedUser);
            if (user is Model.UserDto loggedInUser)
                LoggedInUser = loggedInUser.UserName;
        }
        public string LoggedInUser { get; set; }
        public virtual IQueryable<TEntity> Get() {
            return _context.Set<TEntity>();
        }

        public virtual async Task<List<TModel>> GetAll() {
            var result = await _context.Set<TEntity>().AsNoTracking()
                .Where(e => e.DeletedDate == null)
                .ToListAsync();
            return _mapper.Map<List<TModel>>(result);
        }

        public virtual async Task<TModel> GetById(object id) {
            var result = await _context.Set<TEntity>().FindAsync(id);
            return _mapper.Map<TModel>(result);
        }

        public virtual async Task<List<TModel>> GetByKey(Expression<Func<TEntity, bool>> predicate) {
            var result = await _context.Set<TEntity>().AsNoTracking()
                .Where(e => e.DeletedDate == null)
                .Where(predicate)
                .ToListAsync();
            return _mapper.Map<List<TModel>>(result);
        }

        public virtual async Task<TModel> Add(TModel modelToAdd) {
            var entityToAdd = _mapper.Map<TEntity>(modelToAdd);
            var result = await _context.AddAsync(entityToAdd);

            var changedEntriesCopy = _context.ChangeTracker.Entries()
                .Where(e => e.State is EntityState.Added or
                        EntityState.Modified or EntityState.Deleted)
                .ToList();

            await SaveChangesAsync();

            foreach (var entry in changedEntriesCopy)
                entry.State = EntityState.Detached;

            return _mapper.Map<TModel>(result.Entity);
        }

        public virtual async Task AddRange(IList<TModel> modelsToAdd) {
            var entitiesToAdd = _mapper.Map<List<TEntity>>(modelsToAdd);
            await _context.AddRangeAsync(entitiesToAdd);

            var changedEntriesCopy = _context.ChangeTracker.Entries()
                .Where(e => e.State is EntityState.Added or
                    EntityState.Modified or EntityState.Deleted)
                .ToList();

            await SaveChangesAsync();

            foreach (var entry in changedEntriesCopy)
                entry.State = EntityState.Detached;
        }

        public virtual async Task Update(TModel modelToUpdate) {
            var entityToUpdate = _mapper.Map<TEntity>(modelToUpdate);
            entityToUpdate.LastModifiedDate = DateTime.Now;
            _context.Update(entityToUpdate);

            var changedEntriesCopy = _context.ChangeTracker.Entries()
                .Where(e => e.State is EntityState.Added or
                    EntityState.Modified or EntityState.Deleted)
                .ToList();

            await SaveChangesAsync();

            foreach (var entry in changedEntriesCopy)
                entry.State = EntityState.Detached;
        }

        public async Task UpdateRange(IList<TModel> modelsToUpdate) {
            var entitiesToUpdate = _mapper.Map<List<TEntity>>(modelsToUpdate);
            entitiesToUpdate.ForEach(e => e.LastModifiedDate = DateTime.Now);
            _context.UpdateRange(entitiesToUpdate);

            var changedEntriesCopy = _context.ChangeTracker.Entries()
                .Where(e => e.State is EntityState.Added or
                    EntityState.Modified or EntityState.Deleted)
                .ToList();

            await SaveChangesAsync();
            foreach (var entry in changedEntriesCopy)
                entry.State = EntityState.Detached;
        }

        public virtual async Task Delete(int id) {
            var entityToDelete = await _context.Set<TEntity>().FindAsync(id);
            entityToDelete.DeletedDate = DateTime.Now;
            entityToDelete.DeletedBy = LoggedInUser;
            await SaveChangesAsync();
        }

        public virtual async Task Delete(Expression<Func<TEntity, bool>> predicate) {
            var entityToDelete = await _context.Set<TEntity>()
                .Where(predicate).ToListAsync();
            foreach (var entityModel in entityToDelete) {
                entityModel.DeletedDate = DateTime.Now;
                entityModel.DeletedBy = LoggedInUser;
            }
            await SaveChangesAsync();
        }

        public async Task<int> SaveChangesAsync() {
            return await _context.SaveChangesAsync();
        }
    }
}
