

app.controller('Winners', ['$scope', '$http', '$templateCache', '$rootScope', '$timeout', 'moment',/*'Entries',*/ '$q', 'WinnerGroupNames', '$window',
    function ($scope, $http, $templateCache, $rootScope, $timeout, moment, /* Entries,*/  $q, WinnerGroupNames, $window) {

        console.log('View Winners Controller Loaded.');
        $scope.disab = false;
        //$scope.Entries = Entries;
        $scope.WinnerGroupNames = WinnerGroupNames;

        $q.all(
               [$http.post(document.location.origin + "/" + "BC/ContestInfo")]
              ).then(function (responses) {

                  console.log(responses);

                  $scope.ContestInfo = responses[0].data;

                  var newdate = new Date();

                  $scope.headers = [];
                  $scope.getHeader = function () {
                      return $scope.headers;
                  }

                  $scope.ShowWinners = false;

                  $scope.filename = 'winners';

                  $scope.ValidatePage = function ($event) {
                      if (isNaN(String.fromCharCode($event.keyCode))) {
                          $event.preventDefault();
                      }
                  };

                  $scope.GetLink = function (link) {
                      $window.open(document.location.origin + "/" + "BC/SASLink?" + "Link=" + link, '_blank');
                  };
                
                  var d = new Date();
                  var n = d.getTimezoneOffset();
                  $scope.Selections = {
                      WinnerGroupName: 'Select All',
                      Page: 1,
                      StartingIndex: 0,
                      Count: 0,
                      TotalCount: 0,
                      PageSize: 50,
                      WStartDate: new Date($scope.ContestInfo.StartDate),
                      WEndDate: new Date($scope.ContestInfo.EndDate),
                      StartDate: new Date($scope.ContestInfo.StartDate),
                      EndDate: new Date($scope.ContestInfo.EndDate),
                      ValidOnly: 0,
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
                      wisOpenStart: false,
                      wopenCalendarStart: function (e) {
                          e.preventDefault();
                          e.stopPropagation();
                          $scope.Selections.wisOpenStart = true;
                      },
                      wisOpenEnd: false,
                      wopenCalendarEnd: function (e) {
                          e.preventDefault();
                          e.stopPropagation();
                          $scope.Selections.wisOpenEnd = true;
                      },
                      GMT: n,
                  };



                  $scope.Filter = function () {

                      $http.post(document.location.origin + "/" + "BC/GetWinners", {
                          JsonInfo: angular.toJson($scope.Selections)
                      }).then(function (response) {
                          console.log(response);
                          $scope.ShowWinners = true;
                          var data = response.data;

                          $scope.Selections.TotalCount = data.Count;
                          var c = (Math.ceil($scope.Selections.TotalCount / $scope.Selections.PageSize));
                          $scope.Selections.Count = c <= 0 ? 1 : c;
                          $scope.Winners = data.Winners;
                          $scope.WinnerHeaders = data.WinnerHeaders;
                      }, function (error) {
                          $scope.ShowWinners = false;
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

                      $http.post(document.location.origin + "/" + "BC/GetWinnersCSV", {
                          JsonInfo: angular.toJson($scope.Selections)
                      }).then(function (response) {
                          console.log(response);
                          var data = response.data["data"];

                          for (var t = 0; t < data.length; t++) {
                              var ndate = moment(data[t].DateEntry).format('DD/MM/YYYY hh:mm:ss A');
                              data[t].DateEntry = ndate;

                              var ndate2 = moment(data[t].DateWon).format('DD/MM/YYYY hh:mm:ss A');
                              data[t].DateWon = ndate2;
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

                      $scope.$parent.confirm("Purge All Winners?!").then(function (response) {
                          if (!response) {
                              return;
                          }

                          else {
                              $http.post(document.location.origin + "/" + "BC/PurgeWinners").then(function (response) {
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


