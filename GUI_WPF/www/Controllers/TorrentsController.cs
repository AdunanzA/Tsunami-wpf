using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

/*
    Info on routing
    http://www.asp.net/web-api/overview/web-api-routing-and-actions/routing-in-aspnet-web-api
*/

namespace Tsunami.Gui.Wpf.www.Controllers
{
    public class TorrentsController : ApiController
    {
        /*[HttpPost("api/torrent/list")]
        public IEnumerable<Models.Torrent> GiveList()
        {

        }*/

        // GET api/torrents 
        public IEnumerable<Models.TorrentItem> Get()
        {
            Core.TorrentHandle[] th = SessionManager.TorrentSession.get_torrents();
            List<Models.TorrentItem> res = new List<Models.TorrentItem>();
            Core.TorrentStatus ts;
            Core.TorrentInfo ti;
            Models.TorrentItem mi;
            foreach (Core.TorrentHandle t in th)
            {
                ts = t.status();
                ti = t.torrent_file();
                mi = new Models.TorrentItem();
                mi.queue_position = t.queue_position();
                mi.status = new Models.TorrentStatus();
                mi.torrent_file = new Models.TorrentInfo();
                mi.status.name = ts.name;
                mi.status.progress = ts.progress;
                res.Add(mi);
            }
            return res;
        }

        // GET api/torrents/5 
        public Models.TorrentItem Get(int id)
        {
            Core.TorrentHandle[] th = SessionManager.TorrentSession.get_torrents();
            Models.TorrentItem mi = new Models.TorrentItem();
            mi.status = new Models.TorrentStatus();
            mi.torrent_file = new Models.TorrentInfo();
            foreach (Core.TorrentHandle t in th)
            {
                if (t.queue_position() == id)
                {
                    mi.queue_position = id;
                    mi.status.name = t.status().name;
                    mi.status.progress = t.status().progress;
                }
            }
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
        public void Delete(int id)
        {
        }

    }
}
