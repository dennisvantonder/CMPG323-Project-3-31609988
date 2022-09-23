using DeviceManagement_WebApp.Models;
using System;
using System.Collections.Generic;

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
    }
}
