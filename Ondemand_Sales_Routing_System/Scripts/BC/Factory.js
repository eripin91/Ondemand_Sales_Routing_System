app.factory('Factory', function ($http, $q, $ngBootbox) {

    var Factory = {};

    //Private function, not required to be exposed
    function get(url) {
        //$http itself returns a promise, so no need to explicitly create another deferred object
        return $http.post(url).then(function (response) {

            var rtnObj = {
                tableParams: response.data,
                itemsByPage: 10
            };
            rtnObj.pagestoshow = Math.ceil(response.data.length / rtnObj.itemsByPage);

            console.log(rtnObj);

            return rtnObj;


        }, function (error, code) {

            // Do something with the error if it fails
            return { error: error, code: code };
        });


    }



    //BOOT BOX 
    $ngBootbox.setDefaults({
        size: 'small',
    });

    Factory.alert = function (messages) {
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

    Factory.confirm = function (messages) {
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


    //HTTP GET METHODS
    Factory.getLogs = function () {
        //DRY
        return get(document.location.origin + "/" + "Home/GetLogs");
    };



    return Factory;

});