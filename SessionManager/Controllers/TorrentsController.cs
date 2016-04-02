using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

/*
    Info on routing
    http://www.asp.net/web-api/overview/web-api-routing-and-actions/routing-in-aspnet-web-api
*/

namespace Tsunami.Controllers
{
    public class TorrentsController : ApiController
    {
        // GET api/torrents 
        public IEnumerable<Models.TorrentItem> Get()
        {
            List<Models.TorrentItem> res = new List<Models.TorrentItem>();
            Core.TorrentStatus ts;
            Core.TorrentInfo ti;
            Models.TorrentItem mi;
            foreach (KeyValuePair<string, Core.TorrentHandle> kvt in SessionManager.TorrentHandles)
            {
                Core.TorrentHandle t = kvt.Value;
                ts = t.status();
                ti = t.torrent_file();
                mi = new Models.TorrentItem();
                mi.Hash = t.info_hash().ToString();
                mi.queue_position = t.queue_position();
                mi.status = new Models.TorrentStatus();
                mi.torrent_file = new Models.TorrentInfo();
                mi.status.name = ts.name;
                mi.status.progress = ts.progress;
                mi.status.State = Models.TorrentStatus.GiveMeStateFromEnum(ts.state);
                res.Add(mi);
            }
            return res;
        }

        // GET api/torrents/5 
        public Models.TorrentItem Get(string sha1)
        {
            Core.TorrentHandle th1;
            if (!SessionManager.TorrentHandles.TryGetValue(sha1, out th1))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            Models.TorrentItem mi = new Models.TorrentItem();
            mi.status = new Models.TorrentStatus();
            mi.torrent_file = new Models.TorrentInfo();
            mi.Hash = sha1;
            mi.queue_position = th1.queue_position();
            mi.status.name = th1.status().name;
            mi.status.progress = th1.status().progress;
            mi.status.State = Models.TorrentStatus.GiveMeStateFromEnum(th1.status().state);

            return mi;
        }

        // POST api/torrents 
        public void Post([FromBody]string value)
        {
        }

        // PUT api/torrents/5 
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/torrents/5 
        public void Delete(string sha1)
        {
        }

        [Route("api/torrents/pause")]
        [HttpPost]
        public bool PauseTorrent([FromBody]string sha1)
        {
            Core.TorrentHandle th;
            if (!SessionManager.TorrentHandles.TryGetValue(sha1, out th))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            //Core.TorrentHandle th2 = SessionManager.TorrentSession.find_torrent(th1.info_hash());
            //th2.pause();
            th.pause();
            return (th.status().paused == true);
        }

        [Route("api/torrents/resume")]
        [HttpPost]
        public bool ResumeTorrent([FromBody]string sha1)
        {
            Core.TorrentHandle th;
            if (!SessionManager.TorrentHandles.TryGetValue(sha1, out th))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            th.resume();
            return (th.status().paused == true);
        }

        [Route("api/torrents/delete")]
        [HttpPost]
        public void DeleteTorrent([FromBody]string sha1)
        {
            Core.TorrentHandle th;
            if (!SessionManager.TorrentHandles.TryGetValue(sha1, out th))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            SessionManager.TorrentSession.remove_torrent(th, 1);
            SessionManager.TorrentHandles.TryRemove(sha1, out th);
        }
    }
}
