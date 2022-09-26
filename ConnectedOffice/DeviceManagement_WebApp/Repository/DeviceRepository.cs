using DeviceManagement_WebApp.Data;
using DeviceManagement_WebApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace DeviceManagement_WebApp.Repository
{
    // Repository for device
    public class DeviceRepository : GenericRepository<Device>, IDeviceRepository
    {
        // device contructor
        public DeviceRepository(ConnectedOfficeContext context) : base(context)
        {
        }

        // Gets the most recent device that was added
        public Device GetMostRecentDevice()
        {
            return _context.Device.Include(d => d.Category).Include(d => d.Zone).OrderByDescending(device => device.DateCreated).FirstOrDefault();
        }

        // Gets all devices (is different from generic method as the device table requires joins)
        public IEnumerable<Device> GetAllDevices()
        {
            var connectedOfficeContext = _context.Device.Include(d => d.Category).Include(d => d.Zone);
            return connectedOfficeContext.ToList();
        }

        // Gets device details
        public Device GetDeviceDetails(Guid id)
        {
            var device = _context.Device.Include(d => d.Category).Include(d => d.Zone).FirstOrDefault(m => m.DeviceId == id);
            return device;
        }

        // returns the zone
        public IEnumerable<Zone> GetZone()
        {
            return _context.Zone;
        }

        // returns the category
        public IEnumerable<Category> GetCategory()
        {
            return _context.Category;
        }

        // sort devices (overload method as sort in generic repository does not include functionality for Joins)
        public IEnumerable<Device> SortDevices(Expression<Func<Device, string>> expression)
        {
            return _context.Device.Include(d => d.Category).Include(d => d.Zone).OrderBy(expression);
        }
    }
}
