using DeviceManagement_WebApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DeviceManagement_WebApp.Repository
{
    // Interface for Device 
    public interface IDeviceRepository : IGenericRepository<Device>
    {
        // Gets the most recent device that was added
        public Device GetMostRecentDevice();

        // Gets all devices (is different from generic method as the device table requires joins)
        public IEnumerable<Device> GetAllDevices();

        // Gets device details
        public Device GetDeviceDetails(Guid id);

        // returns the zone
        public IEnumerable<Zone> GetZone();

        // returns the category
        public IEnumerable<Category> GetCategory();

        // sort devices (overload method as sort in generic repository does not include functionality for Joins)
        public IEnumerable<Device> SortDevices(Expression<Func<Device, string>> expression);

        // find device overload method
        public Device FindDevice(Expression<Func<Device, bool>> expression);
    }
}
