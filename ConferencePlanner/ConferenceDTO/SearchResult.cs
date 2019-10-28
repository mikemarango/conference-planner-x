using System;
using System.Collections.Generic;
using System.Text;

namespace ConferenceDTO
{
    public enum SearchResultType
    {
        Session,
        Speaker
    }
    public class SearchResult
    {
        public SearchResultType Type { get; set; }
        public SessionResponse Session { get; set; }
        public SpeakerResponse Speaker { get; set; }
    }
}
