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
            return View(GetSacramentMeetingView(null));
        }
        
        // POST: SacramentMeetings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SacramentMeetingView sacramentMeeting)
        {
            List<EventType> eventTypes = await _context.EventTypes.ToListAsync();
            List<Hymn> hymns = await _context.Hymns.AsNoTrackingWithIdentityResolution().ToListAsync();

            SacramentMeeting dbMeeting = new()
            {
                SacramentMeetingDate = sacramentMeeting.SacramentMeetingDate
            };

            _context.SacramentMeetings.Add(dbMeeting);
            await _context.SaveChangesAsync();

            int? lastEventId = null;
            foreach (EventView item in sacramentMeeting.EventList)
            {
                Event newEvent = new()
                {
                    PrevEventId = lastEventId,
                    SacramentMeetingId = dbMeeting.SacramentMeetingId,
                    EventType = eventTypes
                        .Where(et => et.EventTypeName == item.EventType)
                        .First(),
                    EventDescription = item.EventDescription
                };
                switch (newEvent.EventType.EventTypeName)
                {
                    case "Hymn":
                        newEvent.RowId = hymns.Where(h => h.HymnNumber == item.Hymn).FirstOrDefault().HymnId;
                        break;
                    case "Speaker":
                        newEvent.Topic = item.Topic;
                        Person person1 = _context.Person.AsNoTrackingWithIdentityResolution()
                            .SingleOrDefault(p => p.FirstName == item.FirstName && p.LastName == item.LastName);
                        if (person1 is null)
                        {
                            person1 = new()
                            {
                                FirstName = item.FirstName,
                                LastName = item.LastName
                            };
                            _context.Person.Add(person1);
                            await _context.SaveChangesAsync();
                        }
                        newEvent.RowId = person1.PersonId;
                        break;
                    case "Person":
                    case "Prayer":
                        Person person2 = _context.Person.AsNoTrackingWithIdentityResolution()
                            .SingleOrDefault(p => p.FirstName == item.FirstName && p.LastName == item.LastName);
                        if (person2 is null)
                        {
                            person2 = new()
                            {
                                FirstName = item.FirstName,
                                LastName = item.LastName
                            };
                            _context.Person.Add(person2);
                            await _context.SaveChangesAsync();
                        }
                        newEvent.RowId = person2.PersonId;
                        break;
                }
                _context.Events.Add(newEvent);
                _context.SaveChanges();

                lastEventId = newEvent.EventId;
            }
            if (ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                _context.Remove(dbMeeting);
                await _context.SaveChangesAsync();
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

        public SacramentMeetingView GetSacramentMeetingView(List<EventView>? eventList)
        {
            List<Hymn> hymns = _context.Hymns.ToList();
            string hymnString = "";
            foreach (Hymn hymn in hymns)
            {
                hymnString += $"<option value=\"{hymn.HymnId}\">{hymn.FullHymn}</option>";
            }
            List<EventType> eventTypes = _context.EventTypes.ToList();
            string eventTypeString = "";
            foreach (EventType eventType in eventTypes)
            {
                eventTypeString += $"<option value=\"{eventType.EventTypeId}\">{eventType.EventTypeName}</option>";
            }
            SacramentMeetingView createView = new()
            {
                Hymns = hymnString,
                EventTypes = eventTypeString,
                EventList = eventList
            };
            return createView;
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
