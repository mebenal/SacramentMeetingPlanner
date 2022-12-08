using System;
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
        public async Task<IActionResult> Index()
        {
            CalendarModel model = new()
            {
                Meetings = await _context.SacramentMeetings.ToListAsync(),
                Calendar = BuildCal(),
            };
            return View(model);
        }

        public Calendar BuildCal()
        {
            DateTime calBegin, calEnd;

            DateTime today = DateTime.Now;
            calBegin = new DateTime(today.Year, today.Month, 1);
            calBegin = calBegin.AddDays( -(int)calBegin.DayOfWeek);

            calEnd = new DateTime(today.Year, today.Month, DateTime.DaysInMonth(today.Year, today.Month));
            
            return new Calendar(calBegin, calEnd);
        }


        // GET: SacramentMeetings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.SacramentMeetings == null)
            {
                return NotFound();
            }

            SacramentMeeting sacramentMeeting = await _context.SacramentMeetings
                .Include(c => c.EventList)
                    .ThenInclude(c => c.EventType)
                .FirstOrDefaultAsync(m => m.SacramentMeetingId == id);


            if (sacramentMeeting == null)
            {
                return NotFound();
            }

            return View(sacramentMeeting);
        }

        // GET: SacramentMeetings/Create
        public IActionResult Create()
        {
            SelectList hymns = new(_context.Hymns.ToList(), "HymnId", "FullHymn");
            SelectList eventTypes = new(_context.EventTypes.ToList(), "EventTypeId", "EventName");
            CreateView createView = new()
            {
                Hymns = hymns,
                EventTypes = eventTypes
            };
            return View(createView);
        }
        
        // POST: SacramentMeetings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SacramentMeetingView sacramentMeeting)
        {
            List<Event> sacramentMeetingEvents = new();
            foreach (var item in sacramentMeeting.EventList)
            {
                sacramentMeetingEvents.Add(
                    new()
                    {
                    });;
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
            if (id == null || _context.SacramentMeetings == null)
            {
                return NotFound();
            }
    
            var sacramentMeeting = await _context.SacramentMeetings
                                            .Include(i => i.EventList)
                                            .FirstAsync(i => i.SacramentMeetingId == id);
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

            var currentMeeting = await _context.SacramentMeetings
                .Where(p => p.SacramentMeetingId == sacramentMeeting.SacramentMeetingId)
                .Include(p => p.EventList)
                .FirstAsync(i => i.SacramentMeetingId == id);

            if (currentMeeting != null)
            {
                _context.Entry(currentMeeting).CurrentValues.SetValues(sacramentMeeting);

                foreach (var existingChild in currentMeeting.EventList.ToList())
                {
                    if (!sacramentMeeting.EventList.Any(c => c.EventId == existingChild.EventId))
                        _context.Events.Remove(existingChild);
                }

                foreach (var childModel in sacramentMeeting.EventList)
                {
                    var existingChild = currentMeeting.EventList
                        .Where(c => c.EventId == childModel.EventId && c.EventId != default(int))
                        .SingleOrDefault();

                    if (existingChild != null)
                        // Update child
                        _context.Entry(existingChild).CurrentValues.SetValues(childModel);
                    else
                    {
                        // Insert child
                        var newChild = new Event
                        {
                            EventId = childModel.EventId,
                            EventTypeId  = childModel.EventTypeId,
                            RowId  = childModel.RowId,
                            SacramentMeeting = childModel.SacramentMeeting,
                        };

                        currentMeeting.EventList.Add(newChild);
                    }
                }
                _context.SaveChanges();
            }

            return View(sacramentMeeting);
        }

        // GET: SacramentMeetings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.SacramentMeetings == null)
            {
                return NotFound();
            }

            var sacramentMeeting = await _context.SacramentMeetings
                .FirstOrDefaultAsync(m => m.SacramentMeetingId == id);
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
            if (_context.SacramentMeetings == null)
            {
                return Problem("Entity set 'SacramentMeetingPlannerContext.SacramentMeeting'  is null.");
            }
            var sacramentMeeting = await _context.SacramentMeetings.FindAsync(id);
            if (sacramentMeeting != null)
            {
                _context.SacramentMeetings.Remove(sacramentMeeting);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SacramentMeetingExists(int id)
        {
          return _context.SacramentMeetings.Any(e => e.SacramentMeetingId == id);
        }
    }
}
