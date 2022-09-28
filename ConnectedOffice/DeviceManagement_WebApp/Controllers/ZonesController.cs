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
    // Authorize key word used to implement security on controller
    [Authorize]
    public class ZonesController : Controller
    {
        private readonly IZoneRepository _zoneRepository;

        public ZonesController(IZoneRepository zoneRepository)
        {
            _zoneRepository = zoneRepository;
        }

        // GET: Zones - returns all items from zone table
        public IActionResult Index()
        {
            return View(_zoneRepository.GetAll());
        }

        // GET: Zones/Details/5 - returns 1 zone
        public IActionResult Details(Guid id)
        {
            var zone = getZone(id);
            if (zone == null)
            {
                // created an error view to show when no item exists
                return RedirectToAction(nameof(Error));
            }

            return View(zone);
        }

        // GET: Zones/Create - opens create view to add a new zone
        public IActionResult Create()
        {
            return View();
        }

        // POST: Zones/Create - creates a new zone from user input
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

        // GET: Zones/Edit/5 - opens edit view to update a zone
        public IActionResult Edit(Guid id)
        {
            var zone = getZone(id);
            if (zone == null)
            {
                return RedirectToAction(nameof(Error));
            }
            return View(zone);
        }

        // POST: Zones/Edit/5 - updates a zone and saves to database
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, [Bind("ZoneId,ZoneName,ZoneDescription,DateCreated")] Zone zone)
        {
            if (id != zone.ZoneId)
            {
                return RedirectToAction(nameof(Error));
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
                    return RedirectToAction(nameof(Error));
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));

        }

        // GET: Zones/Delete/5 - opens delete view to delete a zone
        public IActionResult Delete(Guid id)
        {
            var zone = getZone(id);
            if (zone == null)
            {
                return RedirectToAction(nameof(Error));
            }

            return View(zone);
        }

        // POST: Zones/Delete/5 - deletes a zone
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            var zone = _zoneRepository.GetById(id);
            _zoneRepository.Remove(zone);
            _zoneRepository.Save();
            return RedirectToAction(nameof(Index));
        }

        // Gets recently added zone
        public IActionResult GetRecent()
        {
            return View(_zoneRepository.GetMostRecentZone());
        }

        // Sorts all items in zone table in asc order
        public IActionResult Sort()
        {
            return View(_zoneRepository.Sort(e => e.ZoneName));
        }

        // Opens error view
        public IActionResult Error()
        {
            return View();
        }

        // Opens search view to search for a zone
        public IActionResult Search()
        {
            return View();
        }

        // shows the zone which user searched for
        public IActionResult SearchView([Bind("ZoneName")] Zone zone)
        {
            var z = _zoneRepository.Find(e => e.ZoneName == zone.ZoneName);
            if (z == null)
            {
                return RedirectToAction(nameof(Error));
            }
            return View(z);
        }

        // Checks if a zone exists
        private bool ZoneExists(Guid id)
        {
            return _zoneRepository.Exists(e => e.ZoneId == id);
        }

        // Gets a zone from an id passed through
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
