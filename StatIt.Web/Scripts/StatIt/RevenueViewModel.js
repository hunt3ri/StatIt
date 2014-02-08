﻿$(document).ready(function () {

    // Initialise datepicker fields
    $('#sandbox-container .input-daterange').datepicker({
        format: "dd/mm/yyyy",
        autoclose: true,
    });

    function GetDate(weekCount)
    {
        // Calculate num of days we need to subtract
        var daysToSubtract = weekCount * 7;

        var today = new Date();
        today.setDate(today.getDate() - daysToSubtract);
        var dd = today.getDate();
        var mm = today.getMonth() + 1; //January is 0!

        var yyyy = today.getFullYear();
        if (dd < 10) { dd = '0' + dd } if (mm < 10) { mm = '0' + mm } today = dd + '/' + mm + '/' + yyyy;
        return today;
    }

    function RefreshData(dateStart, dateEnd, callback)
    {


        $.ajax({
            url: '/Home/GetRevenues?appId=FairySchool&dateStart=' + dateStart + '&dateEnd=' + dateEnd,
             //url: '/Home/GetRevenues?from=all&revenue=total&view=line&breakdown=application,appstore',
            beforeSend: function () {
                $('#loading').show();
                $('#chartContainer').hide();
            },
            complete: function () {
                $('#loading').hide();
            }
        })
        .done(function (data) {
            $('#chartContainer').show();
            callback(data.RevenueByWeek);
        })
        .error(function () {
            $('#target').append('Failed');
        });
    }

    function RevenuesViewModel() {

        var self = this;

        self.myMessage = ko.observable();
        self.myMessage('Test');

        // Get default values showing last 6 weeks worth of data
        var toDate = GetDate(0);
        var fromDate = GetDate(6);

        self.dateStart = ko.observable(fromDate);
        self.dateEnd = ko.observable(toDate);
        self.revenues = ko.observableArray();

        // Refresh Click handler
        self.refreshData = function () {
            RefreshData(self.dateStart(), self.dateEnd(), function (data) {
                self.revenues(data);
            });
        };

        // Display Graph for default vaules
        RefreshData(self.dateStart(), self.dateEnd(), function (data) {
            self.revenues(data);
        });

    }
    ko.applyBindings(new RevenuesViewModel());


});