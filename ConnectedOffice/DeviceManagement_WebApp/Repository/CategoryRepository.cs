using DeviceManagement_WebApp.Data;
using DeviceManagement_WebApp.Models;
using System.Collections.Generic;
using System.Linq;

namespace DeviceManagement_WebApp.Repository
{
    // Category repository to handle data operation tasks
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(ConnectedOfficeContext context) : base(context)
        {
        }

        // returns most recent category added
        public Category GetMostRecentCategory()
        {
            return _context.Category.OrderByDescending(category => category.DateCreated).FirstOrDefault();
        }
    }
}
