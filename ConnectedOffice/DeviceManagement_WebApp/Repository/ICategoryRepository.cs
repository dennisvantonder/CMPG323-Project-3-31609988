using DeviceManagement_WebApp.Models;
using System.Collections.Generic;

namespace DeviceManagement_WebApp.Repository
{
    // Category repository interface
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        public Category GetMostRecentCategory();
    }
}
