/// <reference path="knockout-3.4.0.js" />
/// <reference path="jquery-2.2.2.min.js" />
/// <reference path="jquery.signalR-2.2.0.min.js" />
/// <reference path="progressbar-1.0.0.min.js" />
/// <reference path="bootstrap.min.js" />
/// <reference path="knockstrap.min.js" />
/// <reference path="toastr.min.js" />
/// <reference path="Chart.js" />


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

function TorrentViewModel() {
    var self = this;
    self.Torrents = ko.observableArray([]);
    self.Busy = ko.observable(false);
    self.Connected = ko.observable(false);
    
    self.DownloadRate = ko.observable(0);
    self.UploadRate = ko.observable(0);

    self.AddTorrent = function (trdata) {
        self.Torrents.push(trdata);
        self.Torrents().sort(function (left, right) { return left.queue_position == right.queue_position ? 0 : (left.queue_position < right.queue_position ? -1 : 1) });
    };

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
        self.Busy(true);

        $('#confirmDeleteModalBody').html('Do you really want to delete<br /><b>' + torrentItem.Name() + '</b> ?');

        $('#confirmNo').off('click').on('click', function () {
            self.Busy(false);
            $('#confirmDeleteModal').modal('hide');
        });

        $('#confirmYes').off('click').on('click', function () {
            var deleteFile = $('#checkDeleteFileToo').is(":checked");
            $.post('/api/torrents/delete/' + deleteFile, { '': torrentItem.Hash() })
            .done(function () {
                self.Torrents.remove(torrentItem);
                self.Busy(false);
            })
            .fail(function (xhr, textStatus, error) {
                toastr.error("Cannot delete torrent!", "Error: " + xhr.statusText);
                console.error("error deleting " + torrentItem.Hash());
                console.error(xhr.responseText);
                console.error(textStatus);
                console.error(error.responseJSON);
                self.Busy(false);
            });
            $('#confirmDeleteModal').modal('hide');
        });

        $('#confirmDeleteModal').modal('show');
    }

    self.AddNewTorrent = function () {
        self.Busy(true);

        $('#confirmCancel').off('click').on('click', function () {
            self.Busy(false);
            $('#addTorrentModal').modal('hide');
        });

        $('#confirmAdd').off('click').on('click', function () {
            /*$.post('/api/torrents/delete', { '': torrentItem.Hash() })
            .done(function () {
                self.Torrents.remove(torrentItem);
                self.Busy(false);
            })
            .fail(function (xhr, textStatus, error) {
                toastr.error("Cannot delete torrent!", "Error: " + xhr.statusText);
                console.error("error deleting " + torrentItem.Hash());
                console.error(xhr.responseText);
                console.error(textStatus);
                console.error(error.responseJSON);
                self.Busy(false);
            });*/
            $('#addTorrentModal').modal('hide');
        });

        $('#addTorrentModal').modal('show');
    }
};

$(document).ready(function () {
    // set up signalR connection to receive message from SessionManager
    $.connection.hub.url = location.origin + "/signalr";
    var sr = $.connection.signalRHub;

    // create knockout binding for our TorrentViewModel
    ko.applyBindings(twm);

    // retrieve torrent from WebApi
    //retrieveTorrentList();

    // http://nnnick.github.io/Chart.js/docs-v2/
    Chart.defaults.global.legend.display = false;
    Chart.defaults.global.responsive = true;
    Chart.defaults.global.maintainAspectRatio = false;
    Chart.defaults.global.defaultFontFamily = "monospace";
    Chart.defaults.global.tooltips.enabled = false;
    //Chart.defaults.global.elements.line.tension = 0;
    Chart.defaults.global.elements.point.radius = 0;
    Chart.defaults.global.elements.point.borderWidth = 0;
    Chart.defaults.global.elements.point.hoverRadius = 0;
    Chart.defaults.global.elements.point.hoverBorderWidth = 0;



    var ctx = $("#torrentChart");
    torrentChart = new Chart(ctx, {
        type: 'line',
        data: {
            labels: [],
            datasets: [{
                label: '',
                data: [0]
            }]
        },
        options: {
            scales: {
                yAxes: [{
                    ticks: {
                        beginAtZero: true
                    }
                }],
                xAxes: [{
                    display: false,
                    ticks: {
                        min: 0,
                        max: 20000,
                        autoSkip: true
                    }
                }]
            }
        }
    });

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

    sr.client.notifySessionStatistics = function (sessionStatistics) {
        if (!twm.Busy() && twm.Torrents().length > 0) {
            twm.Busy(true);
            twm.DownloadRate(sessionStatistics.DownloadRate);
            twm.UploadRate(sessionStatistics.UploadRate);
            if (torrentChart.data.datasets[0].data.length == 100) {
                torrentChart.data.datasets[0].data.shift();
                torrentChart.data.labels.shift();
            }
            torrentChart.data.datasets[0].data.push(sessionStatistics.DownloadRate/1000);
            torrentChart.data.labels.push(torrentChart.data.datasets[0].data.length);
            torrentChart.update();
            twm.Busy(false);
        }
    }

    // on notification from SessionManager torrent added
    sr.client.notifyTorrentAdded = function (hash) {
        toastr.info("Receiving new torrent from <b>Tsunami</b>");
        retrieveTorrent(hash);
    }

    // on torrent deleted SessionManager request to refresh torrent list
    sr.client.refreshList = function () {
        toastr.info("Receiving refresh list from <b>Tsunami</b>");
        retrieveTorrentList();
    }

    // start listening from SessionManager
    $.connection.hub.start().done(function () {
        twm.Connected(true);
        toastr.success("Successfully connected to <b>Tsunami</b>!");
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
        console.error(xhr.statusText);
        console.error(textStatus);
        console.error(error);
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
        console.error("error retrieving " + uri + '/' + hash);
        console.error(xhr.statusText);
        console.error(textStatus);
        console.error(error);
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
