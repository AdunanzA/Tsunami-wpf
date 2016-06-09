#pragma once

#include <libtorrent/add_torrent_params.hpp>
#include <libtorrent/torrent_info.hpp>
#include "enums.h"

namespace Tsunami
{
	namespace Core
	{
		ref class TorrentInfo;
		ref class Sha1Hash;
		ref class ErrorCode;
		
		public ref class AddTorrentParams
		{
		public:
			AddTorrentParams();
			~AddTorrentParams();

			property ATPFlags flags
			{
				ATPFlags get();
				void set(ATPFlags value);
			}

			property System::String^ name
			{
				System::String^ get();
				void set(System::String^ value);
			}

			property System::String^ url
			{
				System::String^ get();
				void set(System::String^ value);
			}

			property System::String^ save_path
			{
				System::String^ get();
				void set(System::String^ value);
			}

			property TorrentInfo^ ti
			{
				TorrentInfo^ get();
				void set(TorrentInfo^ value);
			}

			property Sha1Hash^ info_hash
			{
				Sha1Hash ^ get();
				void set(Sha1Hash ^ value);
			}

			property cli::array<System::String ^> ^ trackers
			{
				cli::array<System::String ^> ^ get();
				void set(cli::array<System::String ^> ^ value);
			}

			property cli::array<int> ^ tracker_tiers
			{
				cli::array<int> ^ get();
				void set(cli::array<int> ^ value);
			}

			property storage_mode_t storage_mode
			{
				storage_mode_t get();
				void set(storage_mode_t value);
			}

			property cli::array<unsigned> ^ file_priorities
			{
				cli::array<unsigned> ^ get();
				void set(cli::array<unsigned> ^ value);
			}

			/// <summary>
			/// ``max_uploads``, ``max_connections``, ``upload_limit``,
			/// ``download_limit`` correspond to the ``set_max_uploads()``,
			/// ``set_max_connections()``, ``set_upload_limit()`` and
			/// ``set_download_limit()`` functions on torrent_handle. These values let
			/// you initialize these settings when the torrent is added, instead of
			/// calling these functions immediately following adding it.
			///
			/// -1 means unlimited on these settings just like their counterpart
			/// functions on torrent_handle
			/// </summary>
			property int max_uploads
			{
				int get() {	return params_->max_uploads; }
				void set(int value) { params_->max_uploads = value; }
			}

			property int max_connections
			{
				int get() { return params_->max_connections; }
				void set(int value) { params_->max_connections = value; }
			}

			property int upload_limit
			{
				int get() { return params_->upload_limit; }
				void set(int value) { params_->upload_limit = value; }
			}

			property int download_limit
			{
				int get() { return params_->download_limit; }
				void set(int value) { params_->download_limit = value; }
			}

			/// <summary>
			/// the total number of bytes uploaded and downloaded by this 
			/// torrent so far.
			/// </summary>
			property long long total_uploaded
			{
				long long get() { return params_->total_uploaded; }
				void set(long long value) { params_->total_uploaded = value; }
			}

			property long long total_downloaded
			{
				long long get() { return params_->total_downloaded; }
				void set(long long value) { params_->total_downloaded = value; }
			}
			
			/// <summary>
			/// the number of seconds this torrent has spent in started, finished and
			/// seeding state so far, respectively.
			/// </summary>
			property int active_time
			{
				int get() { return params_->active_time; }
				void set(int value) { params_->active_time = value; }
			}

			property int finished_time
			{
				int get() { return params_->finished_time; }
				void set(int value) { params_->finished_time = value; }
			}

			property int seeding_time
			{
				int get() { return params_->seeding_time; }
				void set(int value) { params_->seeding_time = value; }
			}

			/// <summary>
			/// if set to a non-zero value, this is the posix time of when this torrent
			/// was first added, including previous runs/sessions. If set to zero, the
			/// internal added_time will be set to the time of when add_torrent() is
			/// called.
			/// </summary>
			property System::DateTime added_time
			{
				System::DateTime get();
			}

			property System::DateTime completed_time
			{
				System::DateTime get();
			}

			/// <summary>
			/// if set to non-zero, initializes the time (expressed in posix time) when
			/// we last saw a seed or peers that together formed a complete copy of the
			/// torrent. If left set to zero, the internal counterpart to this field
			/// will be updated when we see a seed or a distributed copies >= 1.0.
			/// </summary>
			property System::DateTime last_seen_complete
			{
				System::DateTime get();
			}

			/// <summary>
			/// these field can be used to initialize the torrent's cached scrape data.
			/// The scrape data is high level metadata about the current state of the
			/// swarm, as returned by the tracker (either when announcing to it or by
			/// sending a specific scrape request). ``num_complete`` is the number of
			/// peers in the swarm that are seeds, or have every piece in the torrent.
			/// ``num_inomplete`` is the number of peers in the swarm that do not have
			/// every piece. ``num_downloaded`` is the number of times the torrent has
			/// been downloaded (not initiated, but the number of times a download has
			/// completed).
			///
			/// Leaving any of these values set to -1 indicates we don't know, or we
			/// have not received any scrape data.
			/// </summary>

			property int num_complete
			{
				int get() { return params_->num_complete; }
				void set(int value) { params_->num_complete = value; }
			}

			property int num_incomplete
			{
				int get() { return params_->num_incomplete; }
				void set(int value) { params_->num_incomplete = value; }
			}

			property int num_downloaded
			{
				int get() { return params_->num_downloaded; }
				void set(int value) { params_->num_downloaded = value; }
			}

			/// <summary>
			/// URLs can be added to these two lists to specify additional web
			/// seeds to be used by the torrent. If the ``flag_override_web_seeds``
			/// is set, these will be the _only_ ones to be used. i.e. any web seeds
			/// found in the .torrent file will be overridden.
			///
			/// http_seeds expects URLs to web servers implementing the original HTTP
			/// seed specification `BEP 17`_.
			///
			/// url_seeds expects URLs to regular web servers, aka "get right" style,
			/// specified in `BEP 19`_.
			/// </summary>

			property cli::array<System::String^> ^ http_seed
			{
				cli::array<System::String ^> ^ get();
				void set(cli::array<System::String ^> ^ value);
			}

			property cli::array<System::String^> ^ url_seeds
			{
				cli::array<System::String ^> ^ get();
				void set(cli::array<System::String ^> ^ value);
			}

			/// <summary>
			/// The optional parameter, ``resume_data`` can be given if up to date
			/// fast-resume data is available. The fast-resume data can be acquired
			/// from a running torrent by calling save_resume_data() on
			/// torrent_handle. See fast-resume_. The ``vector`` that is passed in
			/// will be swapped into the running torrent instance with
			/// ``std::vector::swap()``.
			/// </summary>

			property cli::array<char> ^ resume_data
			{
				cli::array<char> ^ get();
				void set(cli::array<char> ^ value);
			}

			/// <summary>
			/// to support the deprecated use case of reading the resume data into
			/// resume_data field and getting a reject alert, any parse failure is
			/// communicated forward into libtorrent via this field. If this is set, a
			/// fastresume_rejected_alert will be posted.
			property ErrorCode ^ internal_resume_data_error
			{
				ErrorCode ^ get();
			}

			/*

			// peers to add to the torrent, to be tried to be connected to as
			// bittorrent peers.
			std::vector<tcp::endpoint> peers;

			// peers banned from this torrent. The will not be connected to
			std::vector<tcp::endpoint> banned_peers;

			// this is a map of partially downloaded piece. The key is the piece index
			// and the value is a bitfield where each bit represents a 16 kiB block.
			// A set bit means we have that block.
			std::map<int, bitfield> unfinished_pieces;

			// this is a bitfield indicating which pieces we already have of this
			// torrent.
			bitfield have_pieces;

			// when in seed_mode, pieces with a set bit in this bitfield have been
			// verified to be valid. Other pieces will be verified the first time a
			// peer requests it.
			bitfield verified_pieces;

			// this sets the priorities for each individual piece in the torrent. Each
			// element in the vector represent the piece with the same index. If you
			// set both file- and piece priorities, file priorities will take
			// precedence.
			std::vector<boost::uint8_t> piece_priorities;

			// if this is a merkle tree torrent, and you're seeding, this field must
			// be set. It is all the hashes in the binary tree, with the root as the
			// first entry. See torrent_info::set_merkle_tree() for more info.
			std::vector<sha1_hash> merkle_tree;

			// this is a map of file indices in the torrent and new filenames to be
			// applied before the torrent is added.
			std::map<int, std::string> renamed_files;

			#ifndef TORRENT_NO_DEPRECATE
			// deprecated in 1.2
			// if ``uuid`` is specified, it is used to find duplicates. If another
			// torrent is already running with the same UUID as the one being added,
			// it will be considered a duplicate. This is mainly useful for RSS feed
			// items which has UUIDs specified.
			std::string uuid;

			// should point to the URL of the RSS feed this torrent comes from, if it
			// comes from an RSS feed.
			std::string source_feed_url;

			*/

		internal:
			libtorrent::add_torrent_params* ptr();
			AddTorrentParams(libtorrent::add_torrent_params & params);
		private:
			libtorrent::add_torrent_params* params_;
		};
	}
}
