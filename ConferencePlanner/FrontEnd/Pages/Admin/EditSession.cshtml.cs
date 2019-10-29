using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConferenceDTO;
using FrontEnd.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FrontEnd.Pages.Admin
{
    public class EditSessionModel : PageModel
    {
        private readonly ApiClient api;
        public bool ShowMessage 
            => !string.IsNullOrEmpty(Message);

        public EditSessionModel(ApiClient api)
        {
            this.api = api;
        }

        [TempData]
        public string Message { get; set; }

        [BindProperty]
        public Session Session { get; set; }

        public async Task OnGetAsync(int id)
        {
            var session = await api.GetSessionAsync(id);
            Session = new Session
            {
                Id = session.Id,
                TrackId = session.TrackId,
                Title = session.Title,
                Abstract = session.Abstract,
                StartTime = session.StartTime,
                EndTime = session.EndTime
            };
        }

        public async Task<ActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();


            await api.PutSessionAsync(Session);
            Message = "Session updated successfully";

            return Page();
        }

        public async Task<ActionResult> OnPostDeleteAsync(int id)
        {
            if (!await api.DeleteSessionAsync(id))
            {
                return Page();
            }
            Message = "Session deleted successfully!";
            return RedirectToPage("/Index");
        }
    }
}