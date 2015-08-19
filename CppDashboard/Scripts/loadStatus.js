angular.module("customerPaymentsDashboard", [])
        .controller("statusController", function ($scope, $http, $interval) {


            var systemLoad = function () {
                var system = $http.get(window.loadSystemUri);
                system.success(function (data) {
                    $scope.systemData = {};
                    $scope.systemData.errorSummary = data;
                    $scope.systemData.noErrors = '';
                    for (var s in data) {
                        if (data[s].ErrorCount == 0) {
                            $scope.systemData.noErrors += data[s].Service + ', ';
                        }
                    }
                });
            };
            
            var systemEvents = function () {
                var system = $http.get(window.loadEventsUri);
                system.success(function (data) {
                    $scope.systemEvents = {};
                    $scope.systemEvents.eventSummary = data;
                });
            };

            $scope.displayForKey = function(instance, channel) {
                var valueToShow = 0;
                for (var entry in instance) {
                    if (instance[entry].Channel == channel) {
                        valueToShow = instance[entry].Occurrences;
                    }
                }

                return valueToShow;
            };

            $scope.showMessage = function(message) {

                vex.open({ content: '<div style="width:400px; height:300px; overflow:scroll;">' + message + '</div>' });
        };

            var doPageFunc = function () {
                var offlineStatus = $http.get(window.loadUrl);

                offlineStatus.success(function (data, status, headers, config) {
                    $scope.isOffline = data.IsSystemOnline;
                    $scope.systemMessage = data.IsSystemOnline === true ? 'ONLINE' : 'OFFLINE';
                    $scope.logs = data.Logs;
                    $scope.throughput = data.Throughput;
                    $scope.current = data.Current;
                    $scope.paymentInfo = {};
                    $scope.paymentInfo.success = data.SuccessPayments;
                    $scope.paymentInfo.declines = data.DeclinedPayments;
                    $scope.paymentInfo.cancellations = data.CancellationsDueToGhosts;
                    $scope.paymentInfo.mkGatewayFailures = data.GatewayMkFaliures;
                    $scope.paymentInfo.mkAdyenFailures = data.AdyenMkFaliures;

                    var paymentSummary = data.SuccessPayments / (data.SuccessPayments + data.DeclinedPayments) * 100;

                    //var totalPayments = data.SuccessPayments + data.GatewayMkFaliures + data.CancellationsDueToGhosts;

                    //var refused = data.GatewayMkFaliures / totalPayments * 100;

                    //var commsFailure = data.CommsFaliures / totalPayments * 100;

                    radialProgress(document.getElementById('paymentInfo')).diameter(150).value(paymentSummary).render();
                    //radialProgress(document.getElementById('refused')).diameter(150).value(refused).render();
                    //radialProgress(document.getElementById('failures')).diameter(150).value(commsFailure).render();
                    
                    var simpleModelBuilder = new cpp.dashboard.SimpleModelBuilder();
                    var model = simpleModelBuilder.Build(data.MonitoringEvents);

                    createCancellationPie(data.CancellationsDueToGhosts, model.submittedCancellations);

                    $scope.paymentInfo.model = model;

                });
            };

            doPageFunc();
            systemLoad();
            systemEvents();
            
            $interval(function () {
                systemLoad();
                systemEvents();
            }, 10000 * 6); // every min.

            $interval(function () {
                $http.get(window.updateUrl);
                doPageFunc();
            }, 10000); // every 10s.

        });