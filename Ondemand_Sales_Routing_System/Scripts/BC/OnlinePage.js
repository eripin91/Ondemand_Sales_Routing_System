var app = angular.module('OnlineApp', ["ngSanitize", 'ui.bootstrap', 'ui.bootstrap.datetimepicker', 'ngBootbox', 'ngFileUpload']);

app.controller('OA', ['$scope', '$http', '$templateCache', '$rootScope', '$timeout', '$q', '$ngBootbox', 'Upload',
    function ($scope, $http, $templateCache, $rootScope, $timeout, $q, $ngBootbox, Upload) {

        console.log('OnlinePage Controller Loaded.');
        $scope.disab = false;



        $scope.alert = function (messages) {
            var deferme = $q.defer();
            var stri = false;
            console.log(messages);
            $ngBootbox.alert(messages)
                     .then(function () {
                         stri = true
                         deferme.resolve(stri);
                     });
            return deferme.promise;
        }

        $scope.confirm = function (messages) {
            var deferme = $q.defer();
            var stri = false;
            console.log(messages);

            $ngBootbox.confirm(messages)
                .then(function () {
                    stri = true
                    deferme.resolve(stri);
                }, function () {
                    stri = false
                    deferme.resolve(stri);
                });

            return deferme.promise;
        }


        $scope.Gender = ["Male", "Female"];

        var newdate = new Date();

        $scope.Mob = "65";
        $scope.MobileNo = "";

        $scope.Entry = {
            Name: null, NRIC: '', MobileNo: '', Gender: '', DOB: null, Email: '', ReceiptNo: '', Amount: '', Privacy: false, ReceiveInfo: true, File: { filename: ''}
        };

        $scope.isOpenTest = false;

        $scope.openCalendarTest = function (e) {
            e.preventDefault();
            e.stopPropagation();

            $scope.isOpenTest = true;
        };


        $scope.click = function () {
            document.querySelector('#image').click();
        }

      



        $scope.uploadFiles = function (file, errFiles) {
           
            $scope.errFile = errFiles && errFiles[0];

            if(file!= undefined && file != null)
            {
                $scope.f = file;
                $scope.Entry.File.filename = $scope.f.name;

            }
      
            //Upload.base64DataUrl(file).then(function (urls) {
            //    $scope.Entry.File.filebase64 = urls;
            //    console.log($scope.Entry.File.filebase64);
            //}, function (evt) {
            //    var progressPercentage = parseInt(100.0 * evt.loaded / evt.total);
            //    console.log('progress: ' + progressPercentage + '% ' + evt.config.data.file.name);
            //});

        }

        $scope.Submit = function (form, $event) {

            console.log(form.$valid);

            if (!form.$valid) {
                $scope.disab = false;
                return;
            }

            $scope.disab = true;
            $event.preventDefault();

            $event.stopPropagation();


            if ($scope.Entry.Privacy == false) {
                $scope.alert('Please accept the privacy policy in order to continue with the submission.').then(function (response) { console.log(response) });
                $scope.disab = false;
                return;
            }


            if ($scope.Entry.File.filename == "") {
                $scope.alert('Please upload a file.').then(function (response) { console.log(response) });
                $scope.disab = false;
                return;
            }

            $scope.Entry.MobileNo = $scope.Mob + $scope.MobileNo

            var formdata = new FormData(this);
            $scope.data = formdata;

            $scope.data.append('file', $scope.f, $scope.f.name);
            $scope.data.append('JsonInfo', angular.toJson($scope.Entry));

            $http.post(document.location.origin + "/" + "BC/InsertEntries", $scope.data, {
                transformRequest: angular.identity,
                headers: { 'Content-Type': undefined }
            }).then(function (response) {
                $scope.disab = false;
                //force user to relogin
                console.log(response);
                $scope.alert(response.data).then(function (response) { console.log(response) });
            }, function (error) {
                $scope.disab = false;
                console.log(error)
                // Do something with the error if it fails
                var eror = error.data;
                if (eror == undefined || eror == null) {
                    $scope.alert(eror).then(function (response) { console.log(response) });
                }
                else {
                    $scope.alert(eror).then(function (response) { console.log(response) });
                }
            });

        };
    }]);


