using Newtonsoft.Json.Linq;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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
        private static Logger log = LogManager.GetLogger("TorrentsController");

        // GET api/torrents 
        public IEnumerable<Models.TorrentStatus> Get()
        {
            try
            {
                log.Trace("requested get()");
                return SessionManager.Instance.getTorrentStatusList();
            }
            catch (Exception ex)
            {
                if (log.IsDebugEnabled)
                {
                    log.Warn(ex);
                }
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        // GET api/torrents/5 
        public Models.TorrentStatus Get(string sha1)
        {
            try
            {
                log.Trace("requested get({0})", sha1);
                return SessionManager.Instance.getTorrentStatus(sha1);
            }
            catch (Exception ex)
            {
                if (log.IsDebugEnabled)
                {
                    log.Warn(ex);
                }
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
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
            try
            {
                log.Trace("requested pause({0})", sha1);
                return SessionManager.Instance.pauseTorrent(sha1);
            }
            catch (Exception ex)
            {
                if (log.IsDebugEnabled)
                {
                    log.Warn(ex);
                }
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        [Route("api/torrents/resume")]
        [HttpPost]
        public bool ResumeTorrent([FromBody]string sha1)
        {
            try
            {
                log.Trace("requested resume({0})", sha1);
                return SessionManager.Instance.resumeTorrent(sha1);
            }
            catch (Exception ex)
            {
                if (log.IsDebugEnabled)
                {
                    log.Warn(ex);
                }
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        [Route("api/torrents/delete/{deleteFile:bool}")]
        [HttpPost]
        public void DeleteTorrent([FromBody]string sha1, [FromUri]bool deleteFile)
        {
            try
            {
                log.Trace("requested delete({0}, deleteFile:{1})", sha1, deleteFile);
                SessionManager.Instance.deleteTorrent(sha1, deleteFile);
            }
            catch (Exception ex)
            {
                if (log.IsDebugEnabled)
                {
                    log.Warn(ex);
                }
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        [Route("api/torrents/filelist")]
        [HttpPost]
        public List<Models.FileEntry> TorrentFileList([FromBody]string sha1)
        {
            try
            {
                log.Trace("requested TorrentFileList({0})", sha1);
                return SessionManager.Instance.getTorrentFiles(sha1);
            }
            catch (Exception ex)
            {
                if (log.IsDebugEnabled)
                {
                    log.Warn(ex);
                }
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        [Route("api/torrents/add")]
        [HttpPost]
        public async Task<JObject> UploadFile(HttpRequestMessage request)
        {
            if (!request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }
            
            var data = await Request.Content.ParseMultipartAsync();

            if (data.Files.ContainsKey("inputFile[]"))
            {
                var file = data.Files["inputFile[]"].File;
                var fileName = data.Files["inputFile[]"].Filename;
                SessionManager.Instance.addTorrent(file);
            }
            JObject jret = new JObject("Torrent added");
            //jret.Add("result", "ok");
            return jret;
            //if (data.Fields.ContainsKey("description"))
            //{
            //    var description = data.Fields["description"].Value;
            //}

            //HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.OK);
            //rm.Content = new StringContent("Torrent added");
            //return rm;

            //return new HttpResponseMessage(HttpStatusCode.OK)
            //{
            //    Content = new StringContent("Torrent added")
            //};
        }
    }
}
