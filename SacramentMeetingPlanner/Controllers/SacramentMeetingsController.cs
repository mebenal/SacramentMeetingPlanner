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
       public async Task<IActionResult> Index(int month, int year)
        {
            CalendarModel model = new CalendarModel {
                Meetings = await _context.SacramentMeetings.ToListAsync(),
                Calendar = buildCal(month, year),
            };
            
            return View(model);
        }

        public Calendar buildCal(int month, int year)
        {
            DateTime calBegin, calEnd;

            if (month == 0 || year == 0)
            {
                DateTime today = DateTime.Now;
                month = today.Month;
                year = today.Year;
            }

            calBegin = new DateTime(year, month, 1);
            calBegin = calBegin.AddDays( -(int)calBegin.DayOfWeek);

            calEnd = new DateTime(year, month, DateTime.DaysInMonth(year, month));
            
            return new Calendar(calBegin, calEnd, month, year);
        }


        // GET: SacramentMeetings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.SacramentMeetings == null)
            {
                return NotFound();
            }

            SacramentMeeting sacramentMeeting = await _context.SacramentMeetings
                .AsNoTracking()
                .Include(c => c.EventList)
                    .ThenInclude(c => c.EventType)
                .FirstOrDefaultAsync(m => m.SacramentMeetingId == id);

            if (sacramentMeeting == null)
            {
                return NotFound();
            }
            List<Event> events = OrderEventList(sacramentMeeting.EventList.ToList());
            List<EventView> eventViews = new(events.Count);

            SacramentMeetingView detailsView = new()
            {
                SacramentMeetingId = sacramentMeeting.SacramentMeetingId,
                SacramentMeetingDate = sacramentMeeting.SacramentMeetingDate,
                EventList = eventViews
            };

            

            foreach (Event item in events)
            {
                EventView view = new()
                {
                    EventDescription = item.EventDescription,
                    EventType = item.EventType.EventTypeName
                };
                switch (item.EventType.EventTypeName)
                {
                    case "Hymn":
                        view.Hymn = _context.Hymns.AsNoTracking().Where(h => h.HymnId == item.RowId).First().FullHymn;
                        break;
                    case "Speaker":
                        Person person1 = _context.Person.AsNoTracking().Where(p => p.PersonId == item.RowId).First();
                        view.FirstName = person1.FirstName;
                        view.LastName = person1.LastName;
                        view.Topic = item.Topic;
                        break;
                    case "Person":
                    case "Prayer":
                        Person person2 = _context.Person.AsNoTracking().Where(p => p.PersonId == item.RowId).First();
                        view.FirstName = person2.FirstName;
                        view.LastName = person2.LastName;
                        break;
                }
                eventViews.Add(view);
            }

            return View(detailsView);
        }

        public List<Event> OrderEventList(List<Event> unorderedEvents)
        {
            List<Event> orderedEvents = new(unorderedEvents.Count);
            orderedEvents.Add(unorderedEvents.Where(e => e.PrevEventId is null).First());
            for (int i = 1; i < unorderedEvents.Count; i++)
            {
                orderedEvents.Add(unorderedEvents.Where(e => e.PrevEventId == orderedEvents[i - 1].EventId).First());
            }
            return orderedEvents;
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
                        .Where(et => et.EventTypeId == item.EventTypeId)
                        .First(),
                    EventDescription = item.EventDescription
                };
                switch (newEvent.EventType.EventTypeName)
                {
                    case "Hymn":
                        newEvent.RowId = hymns.Where(h => h.HymnId == item.HymnId).First().HymnId;
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

            SacramentMeeting sacramentMeeting = await _context.SacramentMeetings
                .AsNoTracking()
                .Include(c => c.EventList)
                    .ThenInclude(c => c.EventType)
                .FirstOrDefaultAsync(m => m.SacramentMeetingId == id);

            if (sacramentMeeting == null)
            {
                return NotFound();
            }
            List<Event> events = OrderEventList(sacramentMeeting.EventList.ToList());
            List<EventView> eventViews = new(events.Count);

            foreach (Event item in events)
            {
                EventView view = new()
                {
                    EventDescription = item.EventDescription,
                    EventType = item.EventType.EventTypeName,
                    EventTypeId = item.EventTypeId,
                    EventId = item.EventId
                };
                switch (item.EventType.EventTypeName)
                {
                    case "Hymn":
                        Hymn hymn = _context.Hymns.AsNoTracking().Where(h => h.HymnId == item.RowId).First();
                        view.Hymn = hymn.FullHymn;
                        view.HymnId = hymn.HymnId;
                        break;
                    case "Speaker":
                        Person person1 = _context.Person.AsNoTracking().Where(p => p.PersonId == item.RowId).First();
                        view.FirstName = person1.FirstName;
                        view.LastName = person1.LastName;
                        view.Topic = item.Topic;
                        break;
                    case "Person":
                    case "Prayer":
                        Person person2 = _context.Person.AsNoTracking().Where(p => p.PersonId == item.RowId).First();
                        view.FirstName = person2.FirstName;
                        view.LastName = person2.LastName;
                        break;
                }
                eventViews.Add(view);
            }

            SacramentMeetingView detailsView = GetSacramentMeetingView(eventViews);
            detailsView.SacramentMeetingDate = sacramentMeeting.SacramentMeetingDate;
            detailsView.SacramentMeetingId = sacramentMeeting.SacramentMeetingId;

            return View(detailsView);
        }

        public SacramentMeetingView GetSacramentMeetingView(List<EventView>? eventList)
        {
            if (eventList is null)
            {
                eventList = new();
            }
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
                HymnsString = hymnString,
                Hymns = hymns,
                EventTypeString = eventTypeString,
                EventType = eventTypes,
                EventList = eventList
            };
            return createView;
        }

        // POST: SacramentMeetings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SacramentMeetingView sacramentMeeting)
        {
            List<EventType> eventTypes = await _context.EventTypes.ToListAsync();
            List<Hymn> hymns = await _context.Hymns.AsNoTrackingWithIdentityResolution().ToListAsync();

            SacramentMeeting existingtMeeting = await _context.SacramentMeetings
                .Include(c => c.EventList)
                    .ThenInclude(c => c.EventType)
                .FirstOrDefaultAsync(m => m.SacramentMeetingId == id);

            if (existingtMeeting is null)
            {
                return NotFound();
            }

            existingtMeeting.SacramentMeetingDate = sacramentMeeting.SacramentMeetingDate;
            await _context.SaveChangesAsync();

            List<int> eventIds = _context.Events.Select(e => e.EventId).ToList();

            int? lastEventId = null;
            foreach (EventView item in sacramentMeeting.EventList)
            {
                Event updateEvent =_context.Events.Find(item.EventId);

                if (updateEvent is null)
                {
                    updateEvent = new()
                    {
                        SacramentMeetingId = existingtMeeting.SacramentMeetingId
                    };
                } else
                {

                }
                updateEvent.PrevEventId = lastEventId;
                updateEvent.EventType = eventTypes
                                            .Where(et => et.EventTypeId == item.EventTypeId)
                                            .First();
                updateEvent.EventDescription = item.EventDescription;
                switch (updateEvent.EventType.EventTypeName)
                {
                    case "Hymn":
                        updateEvent.RowId = hymns.Where(h => h.HymnId == item.HymnId).First().HymnId;
                        break;
                    case "Speaker":
                        updateEvent.Topic = item.Topic;
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
                        updateEvent.RowId = person1.PersonId;
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
                        updateEvent.RowId = person2.PersonId;
                        break;
                }
                _context.Events.Add(updateEvent);
                _context.SaveChanges();

                lastEventId = updateEvent.EventId;
            }
            if (ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index));
            }
            return View(sacramentMeeting);
            /*
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
                            EventId = (int)childModel.EventId,
                            EventTypeId  = childModel.EventTypeId,
                            RowId  = childModel.RowId,
                            SacramentMeeting = childModel.SacramentMeeting,
                        };

                        currentMeeting.EventList.Add(newChild);
                    }
                }
                _context.SaveChanges();
            }

            return View(sacramentMeeting);*/
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
