/// <reference path="knockout-3.4.0.js" />
/// <reference path="jquery-2.2.2.min.js" />
/// <reference path="jquery.signalR-2.2.0.min.js" />
/// <reference path="progressbar-1.0.0.min.js" />

var uri = 'api/torrents';
var twm = new TorrentViewModel;

/*
    To Convert a .net class to a knockout observable class
    http://gutek.pl/c2kjs
*/

function Torrent(data) {
    if (!data) { data = {}; }
    var pl;

    this.Name = ko.observable(data.status.name);
    this.Progress = ko.observable(data.status.progress);
    this.QueuePosition = ko.observable(data.queue_position);

    this.updateProgress = function (progress) {
        var perc = precise_round(progress * 100, 2) + ' %';
        this.Progress(perc);
        if (this.pl == null) {
            this.pl = new ProgressBar.Line('#progress' + this.QueuePosition(), {
                color: '#FCB03C',
                trailColor: '#f4f4f4',
                height: '15px',
                text: {
                    value: perc
                }
            });
        }
        this.pl.animate(progress);
        this.pl.setText(perc);
    }
}

function TorrentViewModel() {
    var self = this;
    self.Torrents = ko.observableArray([]);
    self.Busy = ko.observable(false);

    self.AddTorrent = function (trdata) {
        self.Torrents.push(trdata);
    };//.bind(self);

    //$.getJSON(uri)
    //.done(function (results) {
    //    var torrents = $.map(results.d, function (item) { return new Torrent(item) });
    //    self.Torrents = torrents;
    //});

};

$(document).ready(function () {
    $.connection.hub.url = location.origin + "/signalr";
    var sr = $.connection.signalRHub;

    retrieveTorrentList();

    // on notification from SessionManager update progress
    sr.client.notifyUpdateProgress = function (queue_position, name, progress) {
        $.each(twm.Torrents(), function (k, i) {
            if (i.Name() == name && i.QueuePosition() == queue_position) {
                i.updateProgress(progress);
            }
        });
    }

    // on notification from SessionManager torrent added
    sr.client.notifyTorrentAdded = function (id) {
        retrieveTorrent(id);
    }

    // start listening from SessionManager
    $.connection.hub.start().done(function () {
        ko.applyBindings(twm);
    });
    //$.getJSON(uri)
    //    .done(function(results) {
    //        var torrents = $.map(results.d, function (item) { return new Torrent(item) });
    //        /*$.each(results, function (key, item) {
    //            $('<li>', { text: item.status.name }).appendTo($('#torrentList'));
    //            $('<li data-bind="text: item.status.progress">', { text: item.status.progress }).appendTo($('#torrentList'));
    //        });*/
    //    });
});

function retrieveTorrent(id) {
    twm.Busy(true);
    $.getJSON(uri + '/' + id).done(function (result) {
        //$.each(results, function (k, i) {
            //twm.AddTorrent(new Torrent(i));
        //});
        twm.AddTorrent(new Torrent(result));
        //ko.applyBindings(twm);
    });
    twm.Busy(false);
}

function retrieveTorrentList() {
    twm.Busy(true);
    $.getJSON(uri).done(function (results) {
        //var torrents = $.map(results.d, function (item) { return new Torrent(item) });
        //twm.Torrents = torrents;
        $.each(results, function (k, i) {
            twm.AddTorrent(new Torrent(i));
        });
        //ko.applyBindings(twm);
    });
    twm.Busy(false);
}

function precise_round(value, decPlaces) {
    var val = value * Math.pow(10, decPlaces);
    var fraction = (Math.round((val - parseInt(val)) * 10) / 10);
    if (fraction == -0.5) fraction = -0.6;
    val = Math.round(parseInt(val) + fraction) / Math.pow(10, decPlaces);
    return val.toFixed(decPlaces);
}
