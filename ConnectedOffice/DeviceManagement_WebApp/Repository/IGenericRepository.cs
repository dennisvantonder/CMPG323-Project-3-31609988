using System.Collections.Generic;
using System.Linq.Expressions;
using System;

namespace DeviceManagement_WebApp.Repository
{
    // Generic interface that is used by Category, zone and device repositories
    public interface IGenericRepository<T> where T : class
    {
        // Gets an item for a specific id
        T GetById(Guid id);

        // Gets all items from a table
        IEnumerable<T> GetAll();

        // Finds items based on an expression passed through
        T Find(Expression<Func<T, bool>> expression);

        // Adds a new item
        void Add(T entity);

        // Update an item
        void Update(T entity);

        // Adds a range of items (currently not implemented)
        void AddRange(IEnumerable<T> entities);

        // Removes an item
        void Remove(T entity);

        // Removes a range of items (currently not implemented)
        void RemoveRange(IEnumerable<T> entities);

        // Saves work to database
        void Save();

        // Check if an item exists
        bool Exists(Expression<Func<T, bool>> expression);

        // Sorts items
        IEnumerable<T> Sort(Expression<Func<T, string>> expression);
    }
}
