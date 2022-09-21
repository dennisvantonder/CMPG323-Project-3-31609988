using DeviceManagement_WebApp.Data;
using DeviceManagement_WebApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace DeviceManagement_WebApp.Repository
{
    public class DeviceRepository : GenericRepository<Device>, IDeviceRepository
    {
        public DeviceRepository(ConnectedOfficeContext context) : base(context)
        {
        }

        /*public Device GetMostRecentDevice()
        {
            return _context.Device.OrderByDescending(device => device.DateCreated).FirstOrDefault();
        }*/

        public IEnumerable<Device> GetAllDevices()
        {
            var connectedOfficeContext = _context.Device.Include(d => d.Category).Include(d => d.Zone);
            return connectedOfficeContext.ToList();
        }

        public Device GetDeviceDetails(Guid id)
        {
            var device = _context.Device.Include(d => d.Category).Include(d => d.Zone).FirstOrDefault(m => m.DeviceId == id);
            return device;
        }

        public IEnumerable<Zone> GetZone()
        {
            return _context.Zone;
        }

        public IEnumerable<Category> GetCategory()
        {
            return _context.Category;
        }
    }
}
