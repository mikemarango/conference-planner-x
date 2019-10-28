using ConferenceDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace FrontEnd.Services
{
    public class ApiClient
    {
        private readonly HttpClient http;

        public ApiClient(HttpClient http)
        {
            this.http = http;
        }

        public async Task<bool> AddAttendeeAsync(Attendee attendee)
        {
            var response = await http.PostAsJsonAsync("attendees", attendee);
            return response.IsSuccessStatusCode;
        }

        public async Task<AttendeeResponse> GetAttendeeAsync(string name)
        {
            var response = await http.GetAsync($"attendees{name}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<AttendeeResponse>();
        }

        public async Task<SessionResponse> GetSessionAsync(int id)
        {
            var response = await http.GetAsync($"sessions/{id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<SessionResponse>();
        }

        public async Task<List<SessionResponse>> GetSessionsAsync()
        {
            var response = await http.GetAsync("sessions");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<List<SessionResponse>>();
        }

        public async Task DeleteSessionAsync(int id)
        {
            var response = await http.DeleteAsync($"sessions/{id}");
            response.EnsureSuccessStatusCode();
        }

        public async Task<SpeakerResponse> GetSpeakerAsync(int id)
        {
            var response = await http.GetAsync($"speakers/{id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<SpeakerResponse>();
        }

        public async Task<List<SpeakerResponse>> GetSpeakersAsync()
        {
            var response = await http.GetAsync("speakers");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<List<SpeakerResponse>>();
        }

        public async Task PutSessionAsync(Session session)
        {
            var response = await http
                .PutAsJsonAsync($"sessions/{session.Id}", session);
            response.EnsureSuccessStatusCode();
        }

        public async Task<List<SearchResult>> SearchAsync(string query)
        {
            var term = new SearchTerm { Query = query };

            var response = await http.PostAsJsonAsync($"search", term);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<List<SearchResult>>();
        }
    }
}
