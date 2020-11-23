

app.controller('Entries', ['$scope', '$http', '$templateCache', '$rootScope', '$timeout', 'moment',/*'Entries',*/  '$q', '$ngBootbox', '$window',
    function ($scope, $http, $templateCache, $rootScope, $timeout, moment, /* Entries,*/  $q, $ngBootbox, $window) {

        console.log('View Entries Controller Loaded.');

        $q.all(
         [$http.post(document.location.origin + "/" + "BC/ContestInfo")]
        ).then(function (responses) {

            $scope.ContestInfo = responses[0].data;

            $scope.disab = false;
            //$scope.Entries = Entries;

            var newdate = new Date();

            console.log($scope.ContestInfo);
            $scope.headers = [];
            $scope.getHeader = function () {
                return $scope.headers;
            }

            $scope.ShowEntries = false;

            $scope.filename = 'entries';

            $scope.GetLink = function (link) {
                $window.open(document.location.origin + "/" + "BC/SASLink?" + "Link=" + link, '_blank');
            }

            $scope.ValidatePage = function ($event) {
                if (isNaN(String.fromCharCode($event.keyCode))) {
                    $event.preventDefault();
                }
            }

            var d = new Date();
            var n = d.getTimezoneOffset();
            $scope.Selections = {
                Page: 1,
                StartingIndex: 0,
                Count: 0,
                TotalCount: 0,
                PageSize: 50,
                StartDate: new Date($scope.ContestInfo.StartDate),
                EndDate: new Date($scope.ContestInfo.EndDate),
                ValidOnly: 'Select All',
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
                GMT: n,
            };


            $scope.DDLOptions = ['Valid', 'Invalid'];



            $scope.Filter = function () {

                $http.post(document.location.origin + "/" + "BC/GetEntries", {
                    JsonInfo: angular.toJson($scope.Selections)
                }).then(function (response) {
                    console.log(response);
                    $scope.ShowEntries = true;
                    var data = response.data;

                    $scope.Selections.TotalCount = data.Count;
                    var c = (Math.ceil($scope.Selections.TotalCount / $scope.Selections.PageSize));
                    $scope.Selections.Count = c <= 0 ? 1 : c;
                    $scope.Entries = data.Entries;
                    $scope.EntryHeaders = data.EntriesHeader;
                }, function (error) {
                    $scope.ShowEntries = false;
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
                    var data = response.data["data"];

                    for (var t = 0; t < data.length; t++) {
                        var ndate = moment(data[t].DateEntry).format('DD/MM/YYYY hh:mm:ss A');
                        data[t].DateEntry = ndate;
                    }
                    $scope.headers = response.data["headers"];
                    console.log($scope.headers);
                    data.unshift($scope.headers);
                    deferme.resolve(data);
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
            };




            $scope.Purge = function () {

                $scope.$parent.confirm("Purge All Entries?!").then(function (response) {
                    if (!response) {
                        return;
                    }

                    else {
                        $http.post(document.location.origin + "/" + "BC/PurgeEntries").then(function (response) {
                            console.log(response);
                            $scope.Filter();
                            $scope.$parent.alert(response.data).then(function (response) { console.log(response) });
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
                    }
                });

            };

            $scope.Check = false;

            $scope.CheckAll = function () {
                for (var r = 0; r < $scope.Entries.length; r++) {
                    $scope.Entries[r].Checked = $scope.Check;
                }
            }


            $scope.PurgeSelected = function () {


                $scope.$parent.confirm("Purge Selected Entries?!").then(function (response) {
                    if (!response) {
                        return;
                    }

                    else {
                        var EntryIDs = [];

                        for (var j = 0; j < $scope.Entries.length; j++) {
                            if ($scope.Entries[j].Checked) {
                                EntryIDs.push($scope.Entries[j].EntryID);
                            }
                        }


                        $http.post(document.location.origin + "/" + "BC/PurgeSelectedEntries", {
                            JsonInfo: angular.toJson(EntryIDs)
                        }).then(function (response) {
                            console.log(response);
                            $scope.Filter();
                            $scope.$parent.alert(response.data).then(function (response) { console.log(response) });
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

                    }
                });



            };



        });

    }]);


