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
    public class ZonesController : Controller
    {
        private readonly IZoneRepository _zoneRepository;

        public ZonesController(IZoneRepository zoneRepository)
        {
            _zoneRepository = zoneRepository;
        }

        // GET: Zones
        public IActionResult Index()
        {
            return View(_zoneRepository.GetAll());
        }

        // GET: Zones/Details/5
        public IActionResult Details(Guid id)
        {
            var zone = getZone(id);
            if (zone == null)
            {
                return NotFound();
            }

            return View(zone);
        }

        // GET: Zones/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Zones/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("ZoneId,ZoneName,ZoneDescription,DateCreated")] Zone zone)
        {
            zone.ZoneId = Guid.NewGuid();
            _zoneRepository.Add(zone);
            _zoneRepository.Save();
            return RedirectToAction(nameof(Index));
        }

        // GET: Zones/Edit/5
        public IActionResult Edit(Guid id)
        {
            var zone = getZone(id);
            if (zone == null)
            {
                return NotFound();
            }
            return View(zone);
        }

        // POST: Zones/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, [Bind("ZoneId,ZoneName,ZoneDescription,DateCreated")] Zone zone)
        {
            if (id != zone.ZoneId)
            {
                return NotFound();
            }

            try
            {
                _zoneRepository.Update(zone);
                _zoneRepository.Save();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ZoneExists(zone.ZoneId))
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

        // GET: Zones/Delete/5
        public IActionResult Delete(Guid id)
        {
            var zone = getZone(id);
            if (zone == null)
            {
                return NotFound();
            }

            return View(zone);
        }

        // POST: Zones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            var zone = _zoneRepository.GetById(id);
            _zoneRepository.Remove(zone);
            _zoneRepository.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult GetRecent()
        {
            return View(_zoneRepository.GetMostRecentZone());
        }

        public IActionResult Sort()
        {
            return View(_zoneRepository.Sort(e => e.ZoneName));
        }

        private bool ZoneExists(Guid id)
        {
            return _zoneRepository.Exists(e => e.ZoneId == id);
        }

        private Zone getZone(Guid id)
        {
            var zone = _zoneRepository.GetById(id);
            if (id == null)
                return null;
            else if (zone == null)
                return null;
            else
                return zone;
        }
    }
}
