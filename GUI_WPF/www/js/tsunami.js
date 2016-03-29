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
    this.Name = ko.observable(data.status.name);
    this.Progress = ko.observable(data.status.progress);
}

function TorrentViewModel() {
    var self = this;
    self.Torrents = ko.observableArray([]);
    //self.Name = ko.observable();
    //self.Progress = ko.observable();

    self.AddTorrent = function (trdata) {
        self.Torrents.push(trdata);
    }.bind(self);

    //$.getJSON(uri)
    //.done(function (results) {
    //    var torrents = $.map(results.d, function (item) { return new Torrent(item) });
    //    self.Torrents = torrents;
    //});

};

$(document).ready(function () {
    $.connection.hub.url = "http://localhost:4242/signalr";
    var sr = $.connection.signalRHub;

    var line = new ProgressBar.Line('#progress', {
        color: '#FCB03C',
        trailColor: '#f4f4f4',
        text: {
            value: '0 %'
        }
    });

    $.getJSON(uri).done(function (results) {
        //var torrents = $.map(results.d, function (item) { return new Torrent(item) });
        //twm.Torrents = torrents;
        $.each(results, function (k, i) {
            twm.AddTorrent(new Torrent(i));
        });
        ko.applyBindings(twm);
    });

    sr.client.notifyUpdateProgress = function (name, progress) {
        $.each(twm.Torrents(), function (k, i) {
            if (i.Name() == name) {
                var perc = precise_round(progress * 100, 2) + ' %';
                i.Progress(perc);
                line.animate(progress);
                line.setText(perc);
            }
        });
    }

    $.connection.hub.start().done(function () {

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

function precise_round(value, decPlaces) {
    var val = value * Math.pow(10, decPlaces);
    var fraction = (Math.round((val - parseInt(val)) * 10) / 10);
    if (fraction == -0.5) fraction = -0.6;
    val = Math.round(parseInt(val) + fraction) / Math.pow(10, decPlaces);
    return val;
}
