/// <reference path="../typings/lodash/lodash.d.ts" />
/// <reference path="../typings/jquery/jquery.d.ts" />

module cpp.dashboard {

    export class TotalCancellationFinder {

        public GetAllCancellations(monitoringEvents: Array<any>): number {

            var items = _.filter(monitoringEvents,(entry) => entry.EventType == 'CancelPaymentSubmitted');

            return items.length;
        }
    }
}