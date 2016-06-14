/// <reference path="knockout-3.4.0.js" />
/// <reference path="jquery-2.2.2.min.js" />
/// <reference path="jquery.signalR-2.2.0.min.js" />
/// <reference path="progressbar-1.0.0.min.js" />
/// <reference path="bootstrap.min.js" />
/// <reference path="knockstrap.min.js" />
/// <reference path="toastr.min.js" />
/// <reference path="Chart.js" />
/// <reference path="fileinput.js" />

var uri = 'api/torrents';
var twm = new TorrentViewModel;
var torrentChart;

/*
    To Convert a .net class to a knockout observable class
    http://gutek.pl/c2kjs

    Knockstrap (bind knockout and bootstrap) documentation
    http://faulknercs.github.io/Knockstrap/
*/

function Torrent(data) {
    this.Hash = ko.observable(data.InfoHash);
    this.Name = ko.observable(data.Name);
    this.Progress = ko.observable(data.Progress);
    /*
    <Progress>0.069471</Progress>
    <ProgressPpm>69471</ProgressPpm>
    */
    this.QueuePosition = ko.observable(data.QueuePosition);
    this.State = ko.observable(data.State);
    this.TotalWanted = ko.observable(data.TotalWanted);
    this.TotalDone = ko.observable(data.TotalDone);
    this.DownloadRate = ko.observable(data.DownloadRate);
    this.UploadRate = ko.observable(data.UploadRate);
    this.Priority = ko.observable(data.Priority);

    this.IsPaused = ko.observable(data.Paused);
    this.IsFinished = ko.observable(false);

    this.updateProgress = function (torrentStatus) {
        this.Progress(torrentStatus.Progress * 100);
        this.State(torrentStatus.State);
        this.QueuePosition(torrentStatus.QueuePosition);
        this.IsPaused(torrentStatus.State == "Paused");
        this.IsFinished(torrentStatus.State == "Finished" || torrentStatus.State == "Seeding");
        this.TotalDone(torrentStatus.TotalDone);
        this.DownloadRate(torrentStatus.DownloadRate);
        this.UploadRate(torrentStatus.UploadRate);
        this.Priority(torrentStatus.Priority);
    }

    this.stateLabel = ko.computed(function () {
        if ($.inArray(this.State(), ['Checking Files', 'Allocating', 'Checking Resume Data', 'Queued For Checking']) >= 0) {
            return 'label-warning';
        }
        if ($.inArray(this.State(), ['Downloading Metadata', 'Downloading']) >= 0) {
            return 'label-info';
        }
        if ($.inArray(this.State(), ['Finished', 'Seeding']) >= 0) {
            return 'label-success';
        }
        if ($.inArray(this.State(), ['Error']) >= 0) {
            return 'label-danger';
        }
        if ($.inArray(this.State(), ['Paused']) >= 0) {
            return 'label-default';
        }
        return 'label-info'; // default
    }, this);

}

function FileEntry (data) {
    if (!data) {
        data = { };
    }
    this.executableAttribute = data.ExecutableAttribute;
    this.filehash = data.Filehash;
    this.fileBase = data.FileBase;
    this.hiddenAttribute = data.HiddenAttribute;
    this.mtime = data.Mtime;
    this.offset = data.Offset;
    this.padFile = data.PadFile;
    this.path = data.Path;
    this.size = data.Size;
    this.symlinkAttribute = data.SymlinkAttribute;
    this.symlinkPath = data.SymlinkPath;
    this.fileName = data.FileName;
    this.isValid = data.IsValid;
    this.pieceSize = data.PieceSize;
}

function TorrentViewModel() {
    var self = this;
    self.Torrents = ko.observableArray([]);
    self.Files = ko.observableArray([]);
    self.Busy = ko.observable(false);
    self.Connected = ko.observable(false);
    self.Debug = ko.observable(false);
    
    self.AllowedUploadSlots = ko.observable(0);
    self.DhtDownloadRate = ko.observable(0);
    self.DhtGlobalNodes = ko.observable(0);
    self.DhtNodes = ko.observable(0);
    self.DhtNodeCache = ko.observable(0);
    self.DhtTorrents = ko.observable(0);
    self.DhtTotalAllocations = ko.observable(0);
    self.DhtUploadRate = ko.observable(0);
    self.DiskReadQueue = ko.observable(0);
    self.DiskWriteQueue = ko.observable(0);
    self.DownloadRate = ko.observable(0);
    self.DownBandwidthBytesQueue = ko.observable(0);
    self.DownBandwidthQueue = ko.observable(0);
    self.HasIncomingConnections = ko.observable(0);
    self.IpOverheadDownloadRate = ko.observable(0);
    self.IpOverheadUploadRate = ko.observable(0);
    self.NumPeers = ko.observable(0);
    self.NumUnchoked = ko.observable(0);
    self.OptimisticUnchokeCounter = ko.observable(0);
    self.PayloadDownloadRate = ko.observable(0);
    self.PayloadUploadRate = ko.observable(0);
    self.PeerlistSize = ko.observable(0);
    self.TotalDhtDownload = ko.observable(0);
    self.TotalDhtUpload = ko.observable(0);
    self.TotalDownload = ko.observable(0);
    self.TotalFailedBytes = ko.observable(0);
    self.TotalIpOverheadDownload = ko.observable(0);
    self.TotalIpOverheadUpload = ko.observable(0);
    self.TotalPayloadDownload = ko.observable(0);
    self.TotalPayloadUpload = ko.observable(0);
    self.TotalRedundantBytes = ko.observable(0);
    self.TotalTrackerDownload = ko.observable(0);
    self.TotalTrackerUpload = ko.observable(0);
    self.TotalUpload = ko.observable(0);
    self.TrackerDownloadRate = ko.observable(0);
    self.TrackerUploadRate = ko.observable(0);
    self.UnchokeCounter = ko.observable(0);
    self.UploadRate = ko.observable(0);
    self.UpBandwidthBytesQueue = ko.observable(0);
    self.UpBandwidthQueue = ko.observable(0);

    self.updateSessionStatistics = function (data) {
        self.AllowedUploadSlots(data.AllowedUploadSlots);
        self.DhtDownloadRate(data.DhtDownloadRate);
        self.DhtGlobalNodes(data.DhtGlobalNodes);
        self.DhtNodes(data.DhtNodes);
        self.DhtNodeCache(data.DhtNodeCache);
        self.DhtTorrents(data.DhtTorrents);
        self.DhtTotalAllocations(data.DhtTotalAllocations);
        self.DhtUploadRate(data.DhtUploadRate);
        self.DiskReadQueue(data.DiskReadQueue);
        self.DiskWriteQueue(data.DiskWriteQueue);
        self.DownloadRate(data.DownloadRate);
        self.DownBandwidthBytesQueue(data.DownBandwidthBytesQueue);
        self.DownBandwidthQueue(data.DownBandwidthQueue);
        self.HasIncomingConnections(data.HasIncomingConnections);
        self.IpOverheadDownloadRate(data.IpOverheadDownloadRate);
        self.IpOverheadUploadRate(data.IpOverheadUploadRate);
        self.NumPeers(data.NumPeers);
        self.NumUnchoked(data.NumUnchoked);
        self.OptimisticUnchokeCounter(data.OptimisticUnchokeCounter);
        self.PayloadDownloadRate(data.PayloadDownloadRate);
        self.PayloadUploadRate(data.PayloadUploadRate);
        self.PeerlistSize(data.PeerlistSize);
        self.TotalDhtDownload(data.TotalDhtDownload);
        self.TotalDhtUpload(data.TotalDhtUpload);
        self.TotalDownload(data.TotalDownload);
        self.TotalFailedBytes(data.TotalFailedBytes);
        self.TotalIpOverheadDownload(data.TotalIpOverheadDownload);
        self.TotalIpOverheadUpload(data.TotalIpOverheadUpload);
        self.TotalPayloadDownload(data.TotalPayloadDownload);
        self.TotalPayloadUpload(data.TotalPayloadUpload);
        self.TotalRedundantBytes(data.TotalRedundantBytes);
        self.TotalTrackerDownload(data.TotalTrackerDownload);
        self.TotalTrackerUpload(data.TotalTrackerUpload);
        self.TotalUpload(data.TotalUpload);
        self.TrackerDownloadRate(data.TrackerDownloadRate);
        self.TrackerUploadRate(data.TrackerUploadRate);
        self.UnchokeCounter(data.UnchokeCounter);
        self.UploadRate(data.UploadRate);
        self.UpBandwidthBytesQueue(data.UpBandwidthBytesQueue);
        self.UpBandwidthQueue(data.UpBandwidthQueue);
    }

    self.AddTorrent = function (trdata) {
        self.Torrents.push(trdata);
        self.Torrents().sort(function (left, right) { return left.queue_position == right.queue_position ? 0 : (left.queue_position < right.queue_position ? -1 : 1) });
    }

    self.PauseTorrent = function (torrentItem) {
        self.Busy(true);
        $.post('/api/torrents/pause', { '': torrentItem.Hash() })
        .done(function (paused) {
            self.Busy(false);
        })
        .fail(function (xhr, textStatus, error) {
            toastr.error("Cannot pause torrent!", "Error: " + xhr.responseText);
            console.error("error pausing " + torrentItem.Hash());
            console.error(xhr.statusText);
            console.error(textStatus);
            console.error(error);
            self.Busy(false);
        });
    }
    self.PauseAllTorrent = function () {
        $.each(self.Torrents(), function (k, i) {
            //if (i.State() == "Downloading") {
                self.PauseTorrent(i);
            //}
        });
    }
    self.ResumeAllTorrent = function () {
        $.each(self.Torrents(), function (k, i) {
            //if (i.State() == "Paused") {
                self.ResumeTorrent(i);
            //}
        });
    }
    self.ResumeTorrent = function (torrentItem) {
        self.Busy(true);
        $.post('/api/torrents/resume', { '': torrentItem.Hash() })
        .done(function (paused) {
            self.Busy(false);
        })
        .fail(function (xhr, textStatus, error) {
            toastr.error("Cannot resume torrent!", "Error: " + xhr.responseText);
            console.error("error resuming " + torrentItem.Hash());
            console.error(xhr.statusText);
            console.error(textStatus);
            console.error(error);
            self.Busy(false);
        });
    }
    self.DeleteTorrent = function (torrentItem) {

        $('#confirmDeleteModalBody').html('Do you really want to delete<br /><b>' + torrentItem.Name() + '</b> ?');

        $('#confirmNo').off('click').on('click', function () {
            $('#confirmDeleteModal').modal('hide');
        });

        $('#confirmYes').off('click').on('click', function () {
            var deleteFile = $('#checkDeleteFileToo').is(":checked");
            $.post('/api/torrents/delete/' + deleteFile, { '': torrentItem.Hash() })
            .done(function () {
                self.Torrents.remove(torrentItem);
            })
            .fail(function (xhr, textStatus, error) {
                toastr.error("Cannot delete torrent!", "Error: " + xhr.statusText);
                console.error("error deleting " + torrentItem.Hash());
                console.error(xhr.responseText);
                console.error(textStatus);
                console.error(error.responseJSON);
            });
            $('#confirmDeleteModal').modal('hide');
        });

        $('#confirmDeleteModal').modal('show');
    }

    self.AddNewTorrent = function () {
        $('#inputFile').fileinput('clear');

        $('#addTorrentModalClose').off('click').on('click', function () {
            $('#addTorrentModal').modal('hide');
        });

        //$('#confirmAdd').off('click').on('click', function () {
        //    $.post('/api/torrents/delete', { '': torrentItem.Hash() })
        //    .done(function () {
        //        self.Torrents.remove(torrentItem);
        //    })
        //    .fail(function (xhr, textStatus, error) {
        //        toastr.error("Cannot delete torrent!", "Error: " + xhr.statusText);
        //        console.error("error deleting " + torrentItem.Hash());
        //        console.error(xhr.responseText);
        //        console.error(textStatus);
        //        console.error(error.responseJSON);
        //    });
        //    $('#addTorrentModal').modal('hide');
        //});

        $('#addTorrentModal').modal('show');
    }

    self.ShowFiles = function (torrentItem) {

        $('#showFileModalClose').off('click').on('click', function () {
            $('#showFileModal').modal('hide');
        });

        self.Files.removeAll();
        $.post('/api/torrents/filelist', { '': torrentItem.Hash() })
            .done(function (results) {
                $.each(results, function (k, i) {
                    self.Files.push(new FileEntry(i));
                    //fe = new FileEntry(i);
                    //console.debug(fe.fileName);
                });
                //twm.Busy(false);
            })
            .fail(function (xhr, textStatus, errorThrown) {
                $('#showFileModal').modal('hide');
                toastr.error("Cannot retrieve filelist!", "Error: " + xhr.responseText);
                console.error("error retrieving " + uri + '/' + hash);
                console.error(xhr.statusText);
                console.error(textStatus);
                console.error(error);
                twm.Busy(false);
            });

        $('#showFileModal').modal('show');
    }
};

$(document).ready(function () {
    // setting up toastr notification (http://codeseven.github.io/toastr/demo.html)
    toastr.options = {
        "closeButton": false,
        "debug": false,
        "newestOnTop": false,
        "progressBar": true,
        "positionClass": "toast-bottom-center",
        "preventDuplicates": false,
        "onclick": null,
        "showDuration": "300",
        "hideDuration": "1000",
        "timeOut": "3500",
        "extendedTimeOut": "2000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    }

    // set up signalR connection to receive message from SessionManager
    $.connection.hub.url = location.origin + "/signalr";
    var sr = $.connection.signalRHub;

    // create knockout binding for our TorrentViewModel
    ko.applyBindings(twm);

    // http://www.bootstraptoggle.com/
    // debug check box
    $('#checkDebug').change(function () {
        var isAdvanced = $(this).prop('checked');
        twm.Debug(isAdvanced);
        $.connection.hub.logging = isAdvanced;
    })

    // retrieve torrent from WebApi
    //retrieveTorrentList();

    // set modal event (to manage twm busy state)
    $('.modal').on('shown.bs.modal', function (e) {
        twm.Busy(true);
    })

    // initialize file input
    $("#inputFile2").fileinput({
        'allowedFileExtensions': ["torrent"],
        'allowedPreviewTypes': false,
        'showUpload': false,
        'showPreview': false,
        'showCaption': false,
        'uploadAsynch': false,
        'uploadUrl': uri + '/add'
    });


    // initialize file input
    $("#inputFile").fileinput({
        'allowedFileExtensions': ["torrent"],
        'allowedPreviewTypes': false,
        'showUpload': true,
        'showPreview': true,
        'showCaption': true,
        'uploadUrl': uri + '/add',
        'layoutTemplates': {
            progress: '<div></div>',
            actions: '<span></span>'
        }
    });
    $('#inputFile').on('filebatchuploadcomplete', function (event, files, extra) {
        console.log('File batch upload complete');
        $('#inputFile').fileinput('clear');
    });

    $('.modal').on('hidden.bs.modal', function (e) {
        twm.Busy(false);
    })

    // http://nnnick.github.io/Chart.js/docs-v2/ default chart
    Chart.defaults.global.legend.display = false;
    Chart.defaults.global.responsive = true;
    Chart.defaults.global.maintainAspectRatio = true;
    Chart.defaults.global.defaultFontFamily = "monospace";
    Chart.defaults.global.tooltips.enabled = false;
    //Chart.defaults.global.elements.line.tension = 0;
    Chart.defaults.global.elements.point.radius = 0;
    Chart.defaults.global.elements.point.borderWidth = 0;
    Chart.defaults.global.elements.point.hoverRadius = 0;
    Chart.defaults.global.elements.point.hoverBorderWidth = 0;

    // create chart element
    torrentChartD = new Chart($("#torrentChartD"), {
        type: 'line',
        data: {
            labels: [],
            datasets: [
                {
                    label: '',
                    backgroundColor: "rgba(0,220,0,0.2)",
                    tension: 0.1,
                    data: [0]
                }
            ]
        },
        options: {
            scales: {
                yAxes: [{
                    ticks: {
                        min: 1,
                        beginAtZero: true
                    }
                }],
                xAxes: [{
                    display: false,
                    ticks: {
                        autoSkip: true
                    }
                }]
            }
        }
    });
    torrentChartU = new Chart($("#torrentChartU"), {
        type: 'line',
        data: {
            labels: [],
            datasets: [
                {
                    label: '',
                    backgroundColor: "rgba(220,50,50,0.2)",
                    data: [0]
                }
            ]
        },
        options: {
            scales: {
                yAxes: [{
                    ticks: {
                        min: 1,
                        beginAtZero: true
                    }
                }],
                xAxes: [{
                    display: false,
                    ticks: {
                        autoSkip: true
                    }
                }]
            }
        }
    });

    // on notification from SessionManager update progress
    sr.client.notifyUpdateProgress = function (torrentStatus) {
        if (!twm.Busy() && twm.Torrents().length > 0) {
            twm.Busy(true);
            for (var i = 0, len = twm.Torrents().length; i < len; i++) {
                if (twm.Torrents()[i].Hash() == torrentStatus.InfoHash) {
                    twm.Torrents()[i].updateProgress(torrentStatus);
                    break;
                }
            }
            //$.each(twm.Torrents(), function (k, i) {
            //    if (i.Hash() == hash) {
            //        i.updateProgress(progress, state, queue_position);
            //    }
            //});
            twm.Busy(false);
        }
    }

    // on sessionstatistics update from SessionManager, update TorrentViewModel
    sr.client.notifySessionStatistics = function (sessionStatistics) {
        if ( !twm.Busy() ) {//&& twm.Torrents().length > 0) {
            twm.Busy(true);
            twm.updateSessionStatistics(sessionStatistics);

            // updating download chart
            if (torrentChartD.data.datasets[0].data.length == 30) {
                torrentChartD.data.datasets[0].data.shift();
                torrentChartD.data.labels.shift();
            }
            torrentChartD.data.datasets[0].data.push(sessionStatistics.DownloadRate/1000);
            torrentChartD.data.labels.push(torrentChartD.data.datasets[0].data.length);
            torrentChartD.update();

            // updating upload chart
            if (torrentChartU.data.datasets[0].data.length == 30) {
                torrentChartU.data.datasets[0].data.shift();
                torrentChartU.data.labels.shift();
            }
            torrentChartU.data.datasets[0].data.push(sessionStatistics.UploadRate / 1000);
            torrentChartU.data.labels.push(torrentChartU.data.datasets[0].data.length);
            torrentChartU.update();
            twm.Busy(false);
        }
    }

    // on notification from SessionManager torrent added
    sr.client.notifyTorrentAdded = function (hash) {
        toastr.info("Receiving new torrent from <b>Tsunami</b>");
        retrieveTorrent(hash);
    }

    sr.client.notifyError = function (errorCode) {
        toastr.error("Error from Tsunami: " + errorCode.Message);
    }

    // on torrent deleted SessionManager request to refresh torrent list
    sr.client.refreshList = function () {
        toastr.info("Receiving refresh list from <b>Tsunami</b>");
        retrieveTorrentList();
    }

    // wire up hub events
    $.connection.hub.error(function (error) {
        toastr.error("Error received from <b>Tsunami</b>: " + error);
    });
    $.connection.hub.connectionSlow(function () {
        toastr.warning("Slow connection with <b>Tsunami</b>");
    });
    $.connection.hub.reconnecting(function () {
        twm.Connected(false);
        toastr.warning("Reconnecting to <b>Tsunami</b>");
    });
    $.connection.hub.reconnected(function () {
        twm.Connected(true);
        toastr.success("Connected to <b>Tsunami</b>");
    });
    $.connection.hub.disconnected(function () {
        twm.Connected(false);
        toastr.error("Disconnected from <b>Tsunami</b>");
    });

    // start listening from SessionManager
    $.connection.hub.start()
        .done(function () {
            twm.Connected(true);
            toastr.success("Successfully connected to <b>Tsunami</b>!");
        })
        .fail(function () {
            twm.Connected(false);
            toastr.error("Cannot connect to <b>Tsunami</b>!");
        });
});

function retrieveTorrent(hash) {
    twm.Busy(true);
    $.getJSON(uri + '/' + hash)
    .done(function (result) {
        twm.AddTorrent(new Torrent(result));
        twm.Busy(false);
        toastr.success("Torrent <b>" + result.Name + "</b> successfully added to queue");
    })
    .fail(function (xhr, textStatus, errorThrown) {
        toastr.error("Cannot retrieve torrent!", "Error: " + xhr.responseText);
        console.error("error retrieving " + uri + '/' + hash);
        console.error(xhr.responseText);
        console.error(errorThrown);
        twm.Busy(false);
    });
}

function retrieveTorrentList() {
    twm.Busy(true);
    $.getJSON(uri)
    .done(function (results) {
        twm.Torrents.removeAll();
        $.each(results, function (k, i) {
            twm.AddTorrent(new Torrent(i));
        });
        //twm.Torrents().sort(function (left, right) { return left.queue_position == right.queue_position ? 0 : (left.queue_position < right.queue_position ? -1 : 1) });
        twm.Busy(false);
    })
    .fail(function (xhr, textStatus, errorThrown) {
        toastr.error("Cannot retrieve torrent!", "Error: " + xhr.responseText);
        console.error("error retrieving " + uri);
        console.error(xhr.responseText);
        console.error(errorThrown);
        twm.Busy(false);
    });
}

function precise_round(value, decPlaces) {
    var val = value * Math.pow(10, decPlaces);
    var fraction = (Math.round((val - parseInt(val)) * 10) / 10);
    if (fraction == -0.5) fraction = -0.6;
    val = Math.round(parseInt(val) + fraction) / Math.pow(10, decPlaces);
    return val;
}

//function msg(txtMsg) {
//    toastr.info(txtMsg)
    //var n = $('.notifContainer').noty({
    //    text: txtMsg,
    //    type: 'warning',
    //    layout: 'bottom',
    //    theme: 'relax',
    //    timeout: 2000,
    //    animation: {
    //        open: 'animated flipInX', // Animate.css class names
    //        close: 'animated flipInX', // Animate.css class names
    //        easing: 'swing', // unavailable - no need
    //        speed: 500 // unavailable - no need
    //    }
    //});
//}
