$(document).ready(function () {

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
            callback(data);
        })
        .error(function () {
            $('#target').append('Failed');
        });
    }

    function RefreshIAPData(dateStart, dateEnd, callback)
    {
        $.ajax({
            url: '/Home/GetIAPRevenues?appId=FairySchool&dateStart=' + dateStart + '&dateEnd=' + dateEnd,
            //url: '/Home/GetRevenues?from=all&revenue=total&view=line&breakdown=application,appstore',
            beforeSend: function () {
                $('#iapLoader').show();
                $('#iapWeekChart').hide();
            },
            complete: function () {
                $('#iapLoader').hide();
            }
        })
        .done(function (data) {
            $('#iapWeekChart').show();
            callback(data);
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
        self.iapRevenues = ko.observableArray();
        self.iapGrossRevenue = ko.observable();
        self.iapShareRevenue = ko.observable();
        self.grossRevenue = ko.observable();
        self.shareRevenue = ko.observable();

        var iapFunction = function () {
            RefreshIAPData(self.dateStart(), self.dateEnd(), function (data) {
                self.iapRevenues(data.RevenueByWeek);
                self.iapGrossRevenue(data.GrossRevenue);
                self.iapShareRevenue((data.GrossRevenue * 0.7).toFixed(2));
            });
        }

        var revFunction = function () {
            RefreshData(self.dateStart(), self.dateEnd(), function (data) {
                self.revenues(data.RevenueByWeek);
                self.dateStart(data.StartDate);
                self.grossRevenue(data.GrossRevenue);
                self.shareRevenue((data.GrossRevenue * 0.7).toFixed(2));
            });

            iapFunction.call();
        };

        // Click handler for refresh button
        self.refreshData = revFunction;

        // Init graphs with default values
        //iapFunction.call();
        revFunction.call();

    }
    ko.applyBindings(new RevenuesViewModel());


});