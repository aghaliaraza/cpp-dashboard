/// <reference path="../typings/lodash/lodash.d.ts" />
/// <reference path="../typings/jquery/jquery.d.ts" />
var cpp;
(function (cpp) {
    var dashboard;
    (function (dashboard) {
        var TotalCancellationFinder = (function () {
            function TotalCancellationFinder() {
            }
            TotalCancellationFinder.prototype.GetAllCancellations = function (monitoringEvents) {
                var items = _.filter(monitoringEvents, function (entry) { return entry.EventType == 'CancelPaymentSubmitted'; });
                return items.length;
            };
            return TotalCancellationFinder;
        })();
        dashboard.TotalCancellationFinder = TotalCancellationFinder;
    })(dashboard = cpp.dashboard || (cpp.dashboard = {}));
})(cpp || (cpp = {}));
//# sourceMappingURL=Cancellations.js.map