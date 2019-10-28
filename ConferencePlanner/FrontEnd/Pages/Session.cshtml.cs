using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConferenceDTO;
using FrontEnd.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FrontEnd.Pages
{
    public class SessionModel : PageModel
    {
        public SessionResponse Session { get; set; }
        public int? DayOffset { get; set; }
        public async Task<ActionResult> OnGetAsync(
            [FromServices]ApiClient apiClient, int id)
        {
            Session = await apiClient.GetSessionAsync(id);

            if (Session == null) return RedirectToPage("/Index");

            var allSessions = await apiClient.GetSessionsAsync();

            var startDate = allSessions.Min(s => s.StartTime?.Date);

            DayOffset = Session.StartTime?
                .Subtract(startDate ?? DateTimeOffset.MinValue).Days;

            return Page();
        }

    }
}