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
    public class SearchModel : PageModel
    {
        public string Term { get; set; }
        public List<SearchResult> SearchResults { get; set; }
        public async Task OnGetAsync([FromServices]ApiClient api, string term)
        {
            Term = term;
            SearchResults = await api.SearchAsync(term);
        }
    }
}