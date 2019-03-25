

app.controller('PickWinner', ['$scope', '$http', '$templateCache', '$rootScope', '$timeout', 'moment',/*'Entries',*/  '$q', '$window',
    function ($scope, $http, $templateCache, $rootScope, $timeout, moment, /* Entries,*/  $q, $window) {

        console.log('Pick Winners Controller Loaded.');

        $q.all(
           [$http.post(document.location.origin + "/" + "BC/ContestInfo")]
        ).then(function (responses) {

            $scope.ContestInfo = responses[0].data;


            $scope.disab = false;
            //$scope.Entries = Entries;

            var newdate = new Date();

            $scope.PickedWinner = false;

            $scope.filename = 'entries';

            $scope.ValidatePage = function ($event) {
                if (isNaN(String.fromCharCode($event.keyCode))) {
                    $event.preventDefault();
                }
            };

            $scope.GetLink = function (link) {
                $window.open(document.location.origin + "/" + "BC/SASLink?" + "Link=" + link, '_blank');
            };

            $scope.DDLOptions = ['SMS', 'Online'];


            var d = new Date();
            var n = d.getTimezoneOffset();
            $scope.Selections = {
                EntryType: 'Select All',
                StartDate: new Date($scope.ContestInfo.StartDate),
                EndDate: new Date($scope.ContestInfo.EndDate),

                isOpenStart: false,
                openCalendarStart: function (e) {
                    e.preventDefault();
                    e.stopPropagation();
                    $scope.Selections.isOpenStart = true;
                },
                isOpenEnd: false,
                openCalendarEnd: function (e) {
                    e.preventDefault();
                    e.stopPropagation();
                    $scope.Selections.isOpenEnd = true;
                },
                PastMob: true,
                PastNRIC: true,
                GroupName: "",
                NoToSelect: "",

            };


            $scope.Pick = function (form, $event) {


                console.log(form.$valid);

                if (!form.$valid) {
                    return;
                }



                $event.preventDefault();

                $event.stopPropagation();

                $http.post(document.location.origin + "/" + "BC/Pick", {
                    JsonInfo: angular.toJson($scope.Selections)
                }).then(function (response) {
                    console.log(response);
                    $scope.PickedWinner = true;
                    var data = response.data;

                    //$scope.Selections.TotalCount = data.Count;
                    //$scope.Selections.Count = (Math.ceil($scope.Selections.TotalCount / $scope.Selections.PageSize));
                    $scope.Winners = data.Winners;
                    $scope.WinnerHeaders = data.WinnerHeaders;
                }, function (error) {
                    // Do something with the error if it fails
                    var eror = error.data;
                    if (eror.Link === undefined || eror.Link === null) {
                        $scope.$parent.alert(eror).then(function (response) { console.log(response) });

                    }
                    else {
                        $scope.$parent.alert(angular.fromJson(eror)).then(function (response) { console.log(response) });
                    }

                });


            };


            $scope.callcsv = function () {
                var deferme = $q.defer();

                $http.post(document.location.origin + "/" + "BC/GetEntriesCSV", {
                    JsonInfo: angular.toJson($scope.Selections)
                }).then(function (response) {
                    console.log(response);
                    for (var t = 0; t < response.data.length; t++) {
                        var ndate = moment(response.data[t].EntryDate).format('DD/MM/YYYY hh:mm:ss A');
                        response.data[t].EntryDate = ndate;
                    }

                    deferme.resolve(response.data);
                }, function (error) {
                    $scope.HaveKeyword = false;
                    console.log(error.data)
                    // Do something with the error if it fails
                    var eror = error.data;
                    if (eror.Link === undefined || eror.Link === null) {
                        $scope.$parent.alert(eror).then(function (response) { console.log(response) });

                    }
                    else {
                        $scope.$parent.alert(angular.fromJson(eror)).then(function (response) { console.log(response) });
                    }

                });
                return deferme.promise;
            }

         });

    }]);


