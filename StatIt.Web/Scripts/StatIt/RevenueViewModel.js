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

    function RevenuesViewModel() {

        var self = this;

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

        self.dau = ko.observableArray();

        // Init Distimo IAP worker
        var iapWorker = new Worker("./Scripts/StatIt/DistimoIAPWorker.js");
        iapWorker.onmessage = function (e) {
            $('#iapLoader').hide();
            self.iapRevenues(e.data.RevenueByWeek);
            self.iapGrossRevenue(e.data.GrossRevenue);
            self.iapShareRevenue((e.data.GrossRevenue * 0.7).toFixed(2));
        }

        // Init Distimo Download worker
        var downloadWorker = new Worker("./Scripts/StatIt/DistimoDownloadWorker.js");
        downloadWorker.onmessage = function (e) {
            self.revenues(e.data.RevenueByWeek);
            self.dateStart(e.data.StartDate);
            self.grossRevenue(e.data.GrossRevenue);
            self.shareRevenue((e.data.GrossRevenue * 0.7).toFixed(2));
        }

        // Init Flurry DAU Worker
        var dauWorker = new Worker("./Scripts/StatIt/FlurryDAUWorker.js");
        dauWorker.onmessage = function (e) {
            self.dau(e.data.DailyActiveUsers);
        }


        var revFunction = function () {

            // Creat object with currently selected dates
            iapDates = {
                dateStart: self.dateStart(),
                dateEnd: self.dateEnd()
            };

            iapWorker.postMessage(iapDates);
            dauWorker.postMessage(iapDates);
            downloadWorker.postMessage(iapDates);
        };
  
        // Click handler for refresh button
        self.refreshData = revFunction;

        // Init graphs with default values
        revFunction.call();

    }
    ko.applyBindings(new RevenuesViewModel());


});