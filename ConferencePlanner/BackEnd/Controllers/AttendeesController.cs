using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackEnd.Data;
using BackEnd.Models;
using ConferenceDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BackEnd.Controllers
{
    [Route("api/attendees")]
    [ApiController]
    public class AttendeesController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public AttendeesController(ApplicationContext context)
        {
            _context = context;
        }

        [HttpGet("{username}")]
        public async Task<ActionResult<AttendeeResponse>> Get(string username)
        {
            var attendee = await _context.Attendees.Include(a => a.SessionAttendees)
                                                .ThenInclude(sa => sa.Session)
                                              .SingleOrDefaultAsync(a => a.UserName == username);

            if (attendee == null)
            {
                return NotFound();
            }

            var result = attendee.MapAttendeeResponse();

            return result;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<AttendeeResponse>> Post(ConferenceDTO.Attendee input)
        {
            // Check if the attendee already exists
            var existingAttendee = await _context.Attendees
                .Where(a => a.UserName == input.UserName)
                .FirstOrDefaultAsync();

            if (existingAttendee != null)
            {
                return Conflict(input);
            }

            var attendee = new Models.Attendee
            {
                FirstName = input.FirstName,
                LastName = input.LastName,
                UserName = input.UserName,
                EmailAddress = input.EmailAddress
            };

            _context.Attendees.Add(attendee);
            await _context.SaveChangesAsync();

            var result = attendee.MapAttendeeResponse();

            return CreatedAtAction(nameof(Get), new { username = result.UserName }, result);
        }

        [HttpPost("{username}/session/{sessionId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<AttendeeResponse>> AddSession(string username, int sessionId)
        {
            var attendee = await _context.Attendees.Include(a => a.SessionAttendees)
                                                .ThenInclude(sa => sa.Session)
                                              .SingleOrDefaultAsync(a => a.UserName == username);

            if (attendee == null)
            {
                return NotFound();
            }

            var session = await _context.Sessions.FindAsync(sessionId);

            if (session == null)
            {
                return BadRequest();
            }

            attendee.SessionAttendees.Add(new SessionAttendee
            {
                AttendeeId = attendee.Id,
                SessionId = sessionId
            });

            await _context.SaveChangesAsync();

            var result = attendee.MapAttendeeResponse();

            return result;
        }

        [HttpDelete("{username}/session/{sessionId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> RemoveSession(string username, int sessionId)
        {
            var attendee = await _context.Attendees.Include(a => a.SessionAttendees)
                                              .SingleOrDefaultAsync(a => a.UserName == username);

            if (attendee == null)
            {
                return NotFound();
            }

            var session = await _context.Sessions.FindAsync(sessionId);

            if (session == null)
            {
                return BadRequest();
            }

            var sessionAttendee = attendee.SessionAttendees.FirstOrDefault(sa => sa.SessionId == sessionId);
            attendee.SessionAttendees.Remove(sessionAttendee);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
