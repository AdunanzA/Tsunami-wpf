using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tsunami.Models
{
    public class AnnounceEntry
    {
        public bool CompleteSent { get; set; }
        public int Fails { get; set; }
        public int FailLimit { get; set; }
        public ErrorCode LastError { get; set; }
        public string Message { get; set; }
        public DateTime MinAnnounce { get; set; }
        public DateTime NextAannounce { get; set; }
        public int ScrapeComplete { get; set; }
        public int ScrapeDownloaded { get; set; }
        public int ScrapeIncomplete { get; set; }
        public bool SendStats { get; set; }
        public int Source { get; set; }
        public bool StartSent { get; set; }
        public int Tier { get; set; }
        public string Trackerid { get; set; }
        public bool Updating { get; set; }
        public string Url { get; set; }
        public bool Verified { get; set; }
        public bool IsWorking { get; set; }
        public int MinAnnounceIn { get; set; }
        public int NextAnnounceIn { get; set; }

        public AnnounceEntry() { /* nothing to do. just for serializator */ }

        public AnnounceEntry(Core.AnnounceEntry ae)
        {
            CompleteSent = ae.complete_sent;
            Fails = ae.fails;
            FailLimit = ae.fail_limit;
            LastError = new ErrorCode(ae.last_error);
            Message = ae.message;
            MinAnnounce = ae.min_announce;
            NextAannounce = ae.next_announce;
            ScrapeComplete = ae.scrape_complete;
            ScrapeDownloaded = ae.scrape_downloaded;
            ScrapeIncomplete = ae.scrape_incomplete;
            SendStats = ae.send_stats;
            Source = ae.source;
            StartSent = ae.start_sent;
            Tier = ae.tier;
            Trackerid = ae.trackerid;
            Updating = ae.updating;
            Url = ae.url;
            Verified = ae.verified;
            IsWorking = ae.is_working();
            MinAnnounceIn = ae.min_announce_in();
            NextAnnounceIn = ae.next_announce_in();
        }

        //public void Reset()
        //{
        //    _ae.reset();
        //}

        //public void Trim()
        //{
        //    _ae.trim();
        //}

        //public bool CanAnnounce(DateTime now, bool is_seed)
        //{
        //    return _ae.can_announce(now, is_seed);
        //}

    }
}
