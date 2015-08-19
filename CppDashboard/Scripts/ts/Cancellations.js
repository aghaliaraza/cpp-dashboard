/// <reference path="../typings/lodash/lodash.d.ts" />
/// <reference path="../typings/jquery/jquery.d.ts" />
var cpp;
(function (cpp) {
    var dashboard;
    (function (dashboard) {
        var SimpleModelBuilder = (function () {
            function SimpleModelBuilder() {
            }
            SimpleModelBuilder.prototype.Build = function (monitoringEvents) {
                var cancelled = _.filter(monitoringEvents, function (entry) { return entry.EventType == 'CancelPaymentSubmitted'; });
                var pspComms = _.filter(monitoringEvents, function (entry) { return entry.EventType == 'PSPCommunicationFailed'; });
                var model = new SummaryDataViewModel();
                model.submittedCancellations = cancelled.length;
                model.pspCommunicationFaliures = pspComms.length;
                return model;
            };
            return SimpleModelBuilder;
        })();
        dashboard.SimpleModelBuilder = SimpleModelBuilder;
        var SummaryDataViewModel = (function () {
            function SummaryDataViewModel() {
            }
            return SummaryDataViewModel;
        })();
        dashboard.SummaryDataViewModel = SummaryDataViewModel;
    })(dashboard = cpp.dashboard || (cpp.dashboard = {}));
})(cpp || (cpp = {}));
//# sourceMappingURL=Cancellations.js.map