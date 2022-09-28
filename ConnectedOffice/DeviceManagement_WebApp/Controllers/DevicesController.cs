using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DeviceManagement_WebApp.Data;
using DeviceManagement_WebApp.Models;
using Microsoft.AspNetCore.Authorization;
using DeviceManagement_WebApp.Repository;

namespace DeviceManagement_WebApp.Controllers
{
    // Authorize key word used to implement security
    [Authorize]
    public class DevicesController : Controller
    {
        private readonly IDeviceRepository _deviceRepository;

        public DevicesController(IDeviceRepository deviceRepository)
        {
            _deviceRepository = deviceRepository;
        }

        // GET: Devices - returns all items from device table
        public IActionResult Index()
        {
            var device = _deviceRepository.GetAllDevices();
            return View(device);
        }

        // GET: Devices/Details/5 - returns 1 device
        public IActionResult Details(Guid id)
        {
            var device = getDevice(id);
            if (device == null)
            {
                return NotFound();
            }

            return View(device);
        }

        // GET: Devices/Create - opens create view to add a new device
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_deviceRepository.GetCategory(), "CategoryId", "CategoryName");
            ViewData["ZoneId"] = new SelectList(_deviceRepository.GetZone(), "ZoneId", "ZoneName");
            return View();
        }

        // POST: Devices/Create - creates a new device from user input
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("DeviceId,DeviceName,CategoryId,ZoneId,Status,IsActive,DateCreated")] Device device)
        {
            device.DeviceId = Guid.NewGuid();
            _deviceRepository.Add(device);
            _deviceRepository.Save();
            return RedirectToAction(nameof(Index));
        }

        // GET: Devices/Edit/5 - returns edit view to let user edit a device
        public IActionResult Edit(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var device = _deviceRepository.GetById(id);
            if (device == null)
            {
                return NotFound();
            }

            ViewData["CategoryId"] = new SelectList(_deviceRepository.GetCategory(), "CategoryId", "CategoryName");
            ViewData["ZoneId"] = new SelectList(_deviceRepository.GetZone(), "ZoneId", "ZoneName");
            return View(device);
        }

        // POST: Devices/Edit/5 - updates a device
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, [Bind("DeviceId,DeviceName,CategoryId,ZoneId,Status,IsActive,DateCreated")] Device device)
        {
            if (id != device.DeviceId)
            {
                return NotFound();
            }
            try
            {
                _deviceRepository.Update(device);
                _deviceRepository.Save();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DeviceExists(device.DeviceId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));

        }

        // GET: Devices/Delete/5 - returns delete view to user
        public IActionResult Delete(Guid id)
        {
            var device = getDevice(id);
            if (device == null)
            {
                return NotFound();
            }

            return View(device);
        }

        // POST: Devices/Delete/5 - deletes a device
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            var device = _deviceRepository.GetById(id);
            _deviceRepository.Remove(device);
            _deviceRepository.Save();
            return RedirectToAction(nameof(Index));
        }

        // gets recent device added
        public IActionResult GetRecent()
        {
            return View(_deviceRepository.GetMostRecentDevice());
        }

        // sort devices on device name
        public IActionResult Sort()
        {
            return View(_deviceRepository.SortDevices(e => e.DeviceName));
        }

        // returns search view to user to search for a device
        public IActionResult Search()
        {
            return View();
        }

        // opens error view
        public IActionResult Error()
        {
            return View();
        }

        // search for a device
        public IActionResult SearchView([Bind("DeviceName")] Device device)
        {
            var d = _deviceRepository.FindDevice(e => e.DeviceName == device.DeviceName);
            if (d == null)
            {
                return RedirectToAction(nameof(Error));
            }
            return View(d);
        }

        // checks if a device exists
        private bool DeviceExists(Guid id)
        {
            return _deviceRepository.Exists(e => e.DeviceId == id);
        }

        // gets a device
        private Device getDevice(Guid id)
        {
            var device = _deviceRepository.GetDeviceDetails(id);
            if (id == null)
                return null;
            else if (device == null)
                return null;
            else
                return device;
        }
    }
}
