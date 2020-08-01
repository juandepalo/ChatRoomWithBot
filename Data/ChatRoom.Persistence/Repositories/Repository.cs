
using ChatRoom.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatRoom.Persistence.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, new()
    {
        private readonly ChatRoomDbContext _chatRoomDbContext;

        public Repository(ChatRoomDbContext chatRoomDbContext)
        {
            _chatRoomDbContext = chatRoomDbContext;
        }

        public IQueryable<TEntity> GetAll()
        {
            try
            {
                return _chatRoomDbContext.Set<TEntity>();
            }
            catch (Exception)
            {
                throw new Exception("Couldn't retrieve entities");
            }
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException($"{nameof(AddAsync)} entity must not be null");
            }

            try
            {
                await _chatRoomDbContext.AddAsync(entity);
                await _chatRoomDbContext.SaveChangesAsync();

                return entity;
            }
            catch (Exception)
            {
                throw new Exception($"{nameof(entity)} could not be saved");
            }
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException($"{nameof(AddAsync)} entity must not be null");
            }

            try
            {
                _chatRoomDbContext.Update(entity);
                await _chatRoomDbContext.SaveChangesAsync();

                return entity;
            }
            catch (Exception)
            {
                throw new Exception($"{nameof(entity)} could not be updated");
            }
        }

        public async Task UpdateRangeAsync(List<TEntity> entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException($"{nameof(UpdateRangeAsync)} entities must not be null");
            }

            try
            {
                _chatRoomDbContext.UpdateRange(entities);
                await _chatRoomDbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new Exception($"{nameof(entities)} could not be updated");
            }
        }
    }
}
