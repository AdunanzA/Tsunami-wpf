/// <reference path="knockout-3.4.0.js" />
/// <reference path="jquery-2.2.2.min.js" />
/// <reference path="jquery.signalR-2.2.0.min.js" />
/// <reference path="progressbar-1.0.0.min.js" />
/// <reference path="bootstrap.min.js" />
/// <reference path="knockstrap.min.js" />
/// <reference path="toastr.min.js" />


var uri = 'api/torrents';
var twm = new TorrentViewModel;

/*
    To Convert a .net class to a knockout observable class
    http://gutek.pl/c2kjs

    Knockstrap (bind knockout and bootstrap) documentation
    http://faulknercs.github.io/Knockstrap/
*/

function Torrent(data) {
    this.Hash = ko.observable(data.Hash);
    this.Name = ko.observable(data.status.name);
    this.Progress = ko.observable(data.status.progress);
    this.QueuePosition = ko.observable(data.queue_position);
    this.State = ko.observable(data.status.State);
    this.IsPaused = ko.observable(false);

    this.updateProgress = function (progress, state) {
        this.Progress(progress * 100);
        this.State(state);
    }
}

function TorrentViewModel() {
    var self = this;
    self.Torrents = ko.observableArray([]);
    self.Busy = ko.observable(false);

    self.AddTorrent = function (trdata) {
        self.Torrents.push(trdata);
    };

    self.PauseTorrent = function (torrentItem) {
        twm.Busy(true);
        $.post('/api/torrents/pause', { '': torrentItem.Hash() })
        .done(function (paused) {
            torrentItem.IsPaused(paused);
            //torrentItem.State("Paused");
            twm.Busy(false);
        })
        .fail(function (xhr, textStatus, error) {
            toastr.error("Cannot pause torrent!", "Error: " + xhr.responseText);
            console.error("error pausing " + torrentItem.Hash());
            console.error(xhr.statusText);
            console.error(textStatus);
            console.error(error);
            twm.Busy(false);
        });
    }

    self.ResumeTorrent = function (torrentItem) {
        twm.Busy(true);
        $.post('/api/torrents/resume', { '': torrentItem.Hash() })
        .done(function (paused) {
            torrentItem.IsPaused(paused);
            twm.Busy(false);
        })
        .fail(function (xhr, textStatus, error) {
            toastr.error("Cannot resume torrent!", "Error: " + xhr.responseText);
            console.error("error resuming " + torrentItem.Hash());
            console.error(xhr.statusText);
            console.error(textStatus);
            console.error(error);
            twm.Busy(false);
        });
    }

    self.DeleteTorrent = function (torrentItem) {
        twm.Busy(true);

        $('#confirmNo').off('click').on('click', function () {
            twm.Busy(false);
            $('#confirmDeleteModal').modal('hide');
        });

        $('#confirmYes').off('click').on('click', function () {
            $.post('/api/torrents/delete', { '': torrentItem.Hash() })
            .done(function () {
                twm.Torrents.remove(torrentItem);
                twm.Busy(false);
            })
            .fail(function (xhr, textStatus, error) {
                toastr.error("Cannot delete torrent!", "Error: " + xhr.statusText);
                console.error("error deleting " + torrentItem.Hash());
                console.error(xhr.responseText);
                console.error(textStatus);
                console.error(error.responseJSON);
                twm.Busy(false);
            });
            $('#confirmDeleteModal').modal('hide');
        });

        $('#confirmDeleteModal').modal('show');
    }
};

$(document).ready(function () {
    // set up signalR connection to receive message from SessionManager
    $.connection.hub.url = location.origin + "/signalr";
    var sr = $.connection.signalRHub;

    // create knockout binding for our TorrentViewModel
    ko.applyBindings(twm);

    // retrieve torrent from WebApi
    retrieveTorrentList();

    // setting up toastr notification (http://codeseven.github.io/toastr/demo.html)
    toastr.options = {
        "closeButton": false,
        "debug": false,
        "newestOnTop": false,
        "progressBar": true,
        "positionClass": "toast-bottom-right",
        "preventDuplicates": false,
        "onclick": null,
        "showDuration": "300",
        "hideDuration": "1000",
        "timeOut": "4000",
        "extendedTimeOut": "2000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    }

    // on notification from SessionManager update progress
    sr.client.notifyUpdateProgress = function (hash, queue_position, name, progress, state) {
        if (!twm.Busy()) {
            twm.Busy(true);
            $.each(twm.Torrents(), function (k, i) {
                if (i.Hash() == hash) {
                    i.updateProgress(progress, state);
                }
            });
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
        retrieveTorrentList();
    }

    // start listening from SessionManager
    $.connection.hub.start().done(function () {
        toastr.success("Successfully connected to <b>Tsunami</b>!");
    });

});

function retrieveTorrent(hash) {
    twm.Busy(true);
    $.getJSON(uri + '/' + hash)
    .done(function (result) {
        twm.AddTorrent(new Torrent(result));
        twm.Busy(false);
        toastr.success("Torrent <b>" + result.status.name + "</b> successfully added to queue");
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
        twm.Torrents().sort();
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
