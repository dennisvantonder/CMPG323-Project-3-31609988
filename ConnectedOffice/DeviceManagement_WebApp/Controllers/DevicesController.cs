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
    [Authorize]
    public class DevicesController : Controller
    {
        private readonly IDeviceRepository _deviceRepository;

        public DevicesController(/*ConnectedOfficeContext context,*/ IDeviceRepository deviceRepository)
        {
            _deviceRepository = deviceRepository;
        }

        // GET: Devices
        public async Task<IActionResult> Index()
        {
            var device = _deviceRepository.GetAllDevices();
            return View(device);
        }

        // GET: Devices/Details/5
        public async Task<IActionResult> Details(Guid id)
        {
            var device = getDevice(id);
            if (device == null)
            {
                return NotFound();
            }

            return View(device);
        }

        // GET: Devices/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_deviceRepository.GetCategory(), "CategoryId", "CategoryName");
            ViewData["ZoneId"] = new SelectList(_deviceRepository.GetZone(), "ZoneId", "ZoneName");
            return View();
        }

        // POST: Devices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DeviceId,DeviceName,CategoryId,ZoneId,Status,IsActive,DateCreated")] Device device)
        {
            device.DeviceId = Guid.NewGuid();
            _deviceRepository.Add(device);
            _deviceRepository.Save();
            return RedirectToAction(nameof(Index));
        }

        // GET: Devices/Edit/5
        public async Task<IActionResult> Edit(Guid id)
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

        // POST: Devices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("DeviceId,DeviceName,CategoryId,ZoneId,Status,IsActive,DateCreated")] Device device)
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

        // GET: Devices/Delete/5
        public async Task<IActionResult> Delete(Guid id)
        {
            var device = getDevice(id);
            if (device == null)
            {
                return NotFound();
            }

            return View(device);
        }

        // POST: Devices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var device = _deviceRepository.GetById(id);
            _deviceRepository.Remove(device);
            _deviceRepository.Save();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> GetRecent()
        {
            return View(_deviceRepository.GetMostRecentDevice());
        }

        private bool DeviceExists(Guid id)
        {
            return _deviceRepository.Exists(e => e.DeviceId == id);
        }

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
