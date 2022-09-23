using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq.Expressions;
using System;
using DeviceManagement_WebApp.Data;
using System.Linq;

namespace DeviceManagement_WebApp.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly ConnectedOfficeContext _context;

        // Constructor
        public GenericRepository(ConnectedOfficeContext context)
        {
            _context = context;
        }

        // Adds a new item
        public void Add(T entity)
        {
            _context.Set<T>().Add(entity);
        }

        // Update an item
        public void Update(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }

        // Adds a range of items (currently not implemented)
        public void AddRange(IEnumerable<T> entities)
        {
            _context.Set<T>().AddRange(entities);
        }

        // Finds items based on an expression passed through (currently not implemented)
        public T Find(Expression<Func<T, bool>> expression)
        {
            return _context.Set<T>().FirstOrDefault(expression);
        }

        // Gets all items from a table
        public IEnumerable<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }

        // Gets an item for a specific id
        public T GetById(Guid id)
        {
            return _context.Set<T>().Find(id);
        }

        // Removes an item
        public void Remove(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        // Removes a range of items (currently not implemented)
        public void RemoveRange(IEnumerable<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
        }

        // Saves work to database
        public void Save()
        {
            _context.SaveChanges();
        }

        // Check if an item exists
        public bool Exists(Expression<Func<T, bool>> expression)
        {
            return _context.Set<T>().Any(expression);
        }

        // Sorts items
        public IEnumerable<T> Sort(Expression<Func<T, string>> expression)
        {
            return _context.Set<T>().OrderBy(expression);
        }
    }
}
