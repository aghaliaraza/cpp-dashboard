/// <reference path="../typings/lodash/lodash.d.ts" />
/// <reference path="../typings/jquery/jquery.d.ts" />

module cpp.dashboard {

    export class SimpleModelBuilder {

        public Build(monitoringEvents: Array<any>): SummaryDataViewModel {

            var cancelled = _.filter(monitoringEvents,(entry) => entry.EventType == 'CancelPaymentSubmitted');
            var pspComms = _.filter(monitoringEvents, (entry) => entry.EventType == 'PSPCommunicationFailed');

            var model = new SummaryDataViewModel();
            model.submittedCancellations = cancelled.length;
            model.pspCommunicationFaliures = pspComms.length;

            return model;
        }
    }

    export class SummaryDataViewModel {
        submittedCancellations: number;
        pspCommunicationFaliures: number;
    }
}