using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NLog;

namespace Tsunami.Models
{
    public class TorrentHandle : IDisposable
    {
        private static Logger log = LogManager.GetLogger("TorrentHandle");
        private Core.TorrentHandle _th;
        
        public TorrentHandle(Core.TorrentHandle th)
        {
            _th = th;
        }

        public void AddHttpSeed(string url)
        {
            _th.add_http_seed(url);
        }

        public void AddPiece(int piece, byte[] data, int flags)
        {
            _th.add_piece(piece, data, flags);
        }

        public void AddTracker(AnnounceEntry entry) {
            _th.add_tracker(entry.AnnounceEntryCore());
        }

        public void AddUrlSeed(string url) {
            _th.add_url_seed(url);
        }

        public void ApplyIpFilter(bool b) {
            _th.apply_ip_filter(b);
        }

        public void AutoManaged(bool b)
        {
            _th.auto_managed(b);
        }

        public void ClearError()
        {
            _th.clear_error();
        }

        public void ClearPieceDeadlines()
        {
            _th.clear_piece_deadlines();
        }

        public int DownloadLimit()
        {
            return _th.download_limit();
        }

        public int[] FilePriorities()
        {
            return _th.file_priorities();
        }

        public int FilePriority(int index)
        {
            return _th.file_priority(index);
        }

        public void FilePriority(int index, int priority)
        {
            _th.file_priority(index, priority);
        }

        public long[] FileProgress(int flags)
        {
            return _th.file_progress(flags);
        }

        public void FlushCache()
        {
            _th.flush_cache();
        }

        public void ForceDhtAnnounce()
        {
            _th.force_dht_announce();
        }

        public void ForceReannounce(int seconds, int tracker_index)
        {
            _th.force_reannounce(seconds, tracker_index);
        }

        public void ForceRecheck()
        {
            _th.force_recheck();
        }

        public List<PeerInfo> GetPeerInfo()
        {
            List<PeerInfo> pi = new List<PeerInfo>();
            foreach (Core.PeerInfo item in _th.get_peer_info())
            {
                pi.Add(new PeerInfo(item));
            }
            return pi;
        }

        public bool HasMetadata()
        {
            return _th.has_metadata();
        }

        public bool HavePiece(int index)
        {
            return _th.have_piece(index);
        }

        public string[] HttpSeeds()
        {
            return _th.http_seeds();
        }

        public string InfoHash()
        {
            string s = "";
            using (Core.Sha1Hash sh = _th.info_hash())
            {
                s = sh.ToString();
            }
            return s;
        }

        public bool IsValid()
        {
            return _th.is_valid();
        }

        public int MaxConnections()
        {
            return _th.max_connections();
        }

        public int MaxUploads()
        {
            return _th.max_uploads();
        }

        public void MoveStorage(string save_path, int flags)
        {
            _th.move_storage(save_path, flags);
        }

        public bool NeedSaveResumeData()
        {
            return _th.need_save_resume_data();
        }

        public void Pause()
        {
            _th.pause();
        }

        public int[] PieceAvailability()
        {
            return _th.piece_availability();
        }

        public int[] PiecePriorities()
        {
            return _th.piece_priorities();
        }

        public int PiecePriority(int index)
        {
            return _th.piece_priority(index);
        }

        public void PiecePriority(int index, int priority)
        {
            _th.piece_priority(index, priority);
        }

        public void PrioritizeFiles(int[] files)
        {
            _th.prioritize_files(files);
        }

        public int QueuePosition()
        {
            return _th.queue_position();
        }

        public void QueuePositionBottom()
        {
            _th.queue_position_bottom();
        }

        public void QueuePositionDown()
        {
            _th.queue_position_down();
        }

        public void QueuePositionTop()
        {
            _th.queue_position_top();
        }

        public void QueuePositionUp()
        {
            _th.queue_position_up();
        }

        public void ReadPiece(int index)
        {
            _th.read_piece(index);
        }

        public void RemoveHttpSeed(string url)
        {
            _th.remove_http_seed(url);
        }

        public void RemoveUrlSeed(string url)
        {
            _th.remove_url_seed(url);
        }

        public void RenameFile(int index, string name)
        {
            _th.rename_file(index, name);
        }

        public void ResetPieceDeadline(int index)
        {
            _th.reset_piece_deadline(index);
        }

        public bool ResolveCountries()
        {
            return _th.resolve_countries();
        }

        public void ResolveCountries(bool b)
        {
            _th.resolve_countries(b);
        }

        public void Resume()
        {
            _th.resume();
        }

        public void SaveResumeData(int flags)
        {
            _th.save_resume_data(flags);
        }

        public void ScrapeTracker()
        {
            _th.scrape_tracker();
        }

        public void SetDownloadLimit(int limit)
        {
            _th.set_download_limit(limit);
        }

        public void SetMaxConnections(int max_connections)
        {
            _th.set_max_connections(max_connections);
        }

        public void SetMaxUploads(int max_uploads)
        {
            _th.set_max_uploads(max_uploads);
        }

        public void SetMetadata(byte[] metadata)
        {
            _th.set_metadata(metadata);
        }

        public void SetPieceDeadline(int index, int deadline)
        {
            _th.set_piece_deadline(index, deadline);
        }

        public void SetPriority(int priority)
        {
            _th.set_priority(priority);
        }

        public void SetSequentialDownload(bool s)
        {
            _th.set_sequential_download(s);
        }

        public void SetShareMode(bool b)
        {
            _th.set_share_mode(b);
        }

        public void SetSslCertificate(string certificate, string private_key, string dh_params, string passphrase)
        {
            _th.set_ssl_certificate(certificate, private_key, dh_params, passphrase);
        }

        public void SetTrackerLogin(string name, string password)
        {
            _th.set_tracker_login(name, password);
        }

        public void SetUploadLimit(int limit)
        {
            _th.set_upload_limit(limit);
        }

        public void SetUploadMode(bool b)
        {
            _th.set_upload_mode(b);
        }

        public TorrentStatus Status()
        {
            return new TorrentStatus(_th.status());
        }

        public void SuperSeeding(bool on)
        {
            _th.super_seeding(on);
        }

        public TorrentInfo TorrentFile()
        {
            return new TorrentInfo(_th.torrent_file());
        }

        public List<AnnounceEntry> Trackers()
        {
            List<AnnounceEntry> ae = new List<AnnounceEntry>();
            List<Core.AnnounceEntry> _ae = _th.trackers().ToList();
            foreach (Core.AnnounceEntry item in _ae)
            {
                ae.Add(new AnnounceEntry(item));
            }
            return ae;
        }

        public int UploadLimit()
        {
            return _th.upload_limit();
        }

        public List<string> UrlSeeds()
        {
            return _th.url_seeds().ToList();
        }

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _th?.Dispose();
                }
                _th = null;
                disposedValue = true;
            }
        }

        ~TorrentHandle()
        {
            if (log.IsDebugEnabled && disposedValue == false)
            {
                log.Warn("TorrentHandle hasn't been disposed before the finalizer call");
            }
            Dispose(false);
        }

        void IDisposable.Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
