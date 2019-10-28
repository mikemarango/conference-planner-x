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
    public class SpeakerModel : PageModel
    {
        public SpeakerResponse Speaker { get; private set; }

        public async Task<ActionResult> OnGet([FromServices]ApiClient api, int id)
        {
            Speaker = await api.GetSpeakerAsync(id);
            if (Speaker == null) return NotFound();
            return Page();
        }
    }
}