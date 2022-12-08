﻿using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SacramentMeetingPlanner.Data;
using SacramentMeetingPlanner.Models;

namespace SacramentMeetingPlanner.Controllers
{
    public class SacramentMeetingsController : Controller
    {
        private readonly SacramentMeetingPlannerContext _context;

        public SacramentMeetingsController(SacramentMeetingPlannerContext context)
        {
            _context = context;
        }

        // GET: SacramentMeetings
        
        // start and end of the whole cal display
        public async Task<IActionResult> Index(int month, int year)
        {
            CalendarModel model = new CalendarModel {
                Meetings = await _context.SacramentMeeting.ToListAsync(),
                Calendar = buildCal(month, year),
            };
            
            return View(model);
        }

        public Calendar buildCal(int month, int year)
        {
            DateTime calBegin, calEnd;

            //DateTime today = DateTime.Now;
            calBegin = new DateTime(year, month, 1);
            calBegin = calBegin.AddDays( -(int)calBegin.DayOfWeek);

            calEnd = new DateTime(year, month, DateTime.DaysInMonth(year, month));
            
            return new Calendar(calBegin, calEnd, month, year);
        }


        // GET: SacramentMeetings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.SacramentMeeting == null)
            {
                return NotFound();
            }

            var sacramentMeeting = await _context.SacramentMeeting
                                            .Include(i => i.EventList)
                                            .FirstAsync(i => i.Id == id);
            if (sacramentMeeting == null)
            {
                return NotFound();
            }

            return View(sacramentMeeting);
        }

        // GET: SacramentMeetings/Create
        public IActionResult Create()
        {
            var meeting = new SacramentMeeting();
            return View(meeting);
        }
        
        // POST: SacramentMeetings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SacramentMeeting sacramentMeeting)
        {

            foreach (var item in sacramentMeeting.EventList)
            {
                item.Meeting = sacramentMeeting;
            }
            if (ModelState.IsValid)
            {
                _context.Add(sacramentMeeting);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(sacramentMeeting);
        }

        // GET: SacramentMeetings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.SacramentMeeting == null)
            {
                return NotFound();
            }
    
            var sacramentMeeting = await _context.SacramentMeeting
                                            .Include(i => i.EventList)
                                            .FirstAsync(i => i.Id == id);
            if (sacramentMeeting == null)
            {
                return NotFound();
            }

            return View(sacramentMeeting);
        }

        // POST: SacramentMeetings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SacramentMeeting sacramentMeeting)
        {

            var currentMeeting = await _context.SacramentMeeting
                .Where(p => p.Id == sacramentMeeting.Id)
                .Include(p => p.EventList)
                .FirstAsync(i => i.Id == id);

            if (currentMeeting != null)
            {
                _context.Entry(currentMeeting).CurrentValues.SetValues(sacramentMeeting);

                foreach (var existingChild in currentMeeting.EventList.ToList())
                {
                    if (!sacramentMeeting.EventList.Any(c => c.Id == existingChild.Id))
                        _context.Event.Remove(existingChild);
                }

                foreach (var childModel in sacramentMeeting.EventList)
                {
                    var existingChild = currentMeeting.EventList
                        .Where(c => c.Id == childModel.Id && c.Id != default(int))
                        .SingleOrDefault();

                    if (existingChild != null)
                        // Update child
                        _context.Entry(existingChild).CurrentValues.SetValues(childModel);
                    else
                    {
                        // Insert child
                        var newChild = new Event
                        {
                            Id = childModel.Id,
                            localId = childModel.localId,
                            EventType  = childModel.EventType,
                            EventDetails  = childModel.EventDetails,
                            Meeting = childModel.Meeting,
                        };

                        currentMeeting.EventList.Add(childModel);
                    }
                }
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));

            return View(sacramentMeeting);
        }

        // GET: SacramentMeetings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.SacramentMeeting == null)
            {
                return NotFound();
            }

            var sacramentMeeting = await _context.SacramentMeeting
                                                .Include(p => p.EventList)
                                                .FirstOrDefaultAsync(m => m.Id == id);
            if (sacramentMeeting == null)
            {
                return NotFound();
            }

            return View(sacramentMeeting);
        }

        // POST: SacramentMeetings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            
            if (_context.SacramentMeeting == null)
            {
                return Problem("Entity set 'SacramentMeetingPlannerContext.SacramentMeeting'  is null.");
            }
            var sacramentMeeting = await _context.SacramentMeeting
                                                .Include(p => p.EventList)
                                                .FirstOrDefaultAsync(m => m.Id == id);
            if (sacramentMeeting != null)
            {
                foreach (var existingChild in sacramentMeeting.EventList.ToList())
                {
                    if (!sacramentMeeting.EventList.Any(c => c.Id == existingChild.Id))
                        _context.Event.Remove(existingChild);
                }
                _context.SacramentMeeting.Remove(sacramentMeeting);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SacramentMeetingExists(int id)
        {
          return _context.SacramentMeeting.Any(e => e.Id == id);
        }
    }
}
