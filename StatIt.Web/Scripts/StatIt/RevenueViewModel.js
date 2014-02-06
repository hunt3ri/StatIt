$(document).ready(function () {

    function RevenuesViewModel() {

        var self = this;

        self.revenues = ko.observableArray();

        self.iain = ko.observable('Iain Test');

        $.ajax({
            url: '/Home/GetRevenues?from=all&revenue=total&view=line&breakdown=application,appstore',
            beforeSend: function () {
                $('#loading').show();
                $('#chartContainer').hide();
            },
            complete: function () {
                $('#loading').hide();

            }
        })
        .done(function (data) {
            //var parsedJson = JSON.parse(data);
            $('#chartContainer').show();
            self.revenues(data.RevenueByWeek);
        })
        .error(function () {
            $('#target').append('Failed');
        });
    }
    ko.applyBindings(new RevenuesViewModel());


});