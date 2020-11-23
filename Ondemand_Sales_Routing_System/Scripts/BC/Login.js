app.controller('Login', ['$scope', '$http', '$templateCache', '$rootScope', '$timeout', '$q', 'Factory',
    function ($scope, $http, $templateCache, $rootScope, $timeout, $q, Factory) {

        console.log('Login Controller Loaded.');
        $scope.disab = false;

        $scope.User = { UserName: "", PassWord: "" };


        //OnClientClick?

        function validate_form() {
            console.log('Validate');
             Factory.alert('????!').then(
                  function (response) {
                      console.log(response);
                      __doPostBack('Login', 'OnClick');
                  }
                  )
        }
    }]);


