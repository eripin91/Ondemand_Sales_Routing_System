
app.controller('TA', ['$scope', '$http', '$templateCache', '$rootScope', '$timeout', 'KW',
    function ($scope, $http, $templateCache, $rootScope, $timeout, KW) {

        console.log('TestAPI Controller Loaded.');
        $scope.disab = false;

        $scope.KW = KW;
        var newdate = new Date();

        $scope.Entry = {
            MobileNo: '', EntryDate: /*'dd/mm/yyyy'*/newdate, EntryText: ''
        };

        $scope.isOpenTest = false;

        $scope.openCalendarTest = function (e) {
            e.preventDefault();
            e.stopPropagation();

            $scope.isOpenTest = true;
        };

        $scope.Submit = function (form, $event) {

            console.log(form.$valid);

            if (!form.$valid) {
                $scope.disab = false;
                return;
            }

            $scope.disab = true;
            $event.preventDefault();

            $event.stopPropagation();


            console.log($scope.Entry.EntryText);
            console.log($scope.Entry.MobileNo);

            $http.post(document.location.origin + "/" + "SAREST/Add", {
                createdon: $scope.Entry.EntryDate,
                MobileNo: $scope.Entry.MobileNo,
                Message: $scope.Entry.EntryText,
            }).then(function (response) {
                $scope.disab = false;
                //force user to relogin
                console.log(response);
                $scope.$parent.alert(response.data).then(function (response) { console.log(response) });
            }, function (error) {
                $scope.disab = false;
                console.log(error.data)
                // Do something with the error if it fails
                var eror = error.data;
                if (eror == undefined || eror == null) {
                        $scope.$parent.alert(eror).then(function (response) { console.log(response) });
                }
                else {
                      $scope.$parent.alert(angular.fromJson(eror)).then(function (response) { console.log(response)  });
                }
            });

        };
    }]);


