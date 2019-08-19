using data.damn;
using model.damn;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace logic.UintaPine
{
    public class TodoLogic
    {
        private MongoContext _db { get; set; }
        public TodoLogic(MongoContext context)
        {
            _db = context;
        }

        async public Task<List<Todo>> GetTodosByUserId(string id)
        {
            return await _db.Todos.Find(t => t.Owner == id).ToListAsync();
        }

        async public Task InsertTodoByUserId(Todo todo)
        {
            await _db.Todos.InsertOneAsync(todo);
        }

        async public Task UpdateTodoByUserIdItemId(bool completed, string userId, string itemId)
        {
            var update = Builders<Todo>.Update.Set(t => t.Completed, completed);

            await _db.Todos.UpdateOneAsync(t => t.Owner == userId && t.Id == itemId, update);
        }

        async public Task<Todo> GetTodoByUserIdItemId(string userId, string itemId)
        {
            return await _db.Todos.Find(t => t.Owner == userId && t.Id == itemId).FirstOrDefaultAsync();
        }

        async public Task TaskDeleteTodoByUserIdItemId(string userId, string itemId)
        {
            await _db.Todos.DeleteOneAsync(t => t.Owner == userId && t.Id == itemId);
        }
    }
}
