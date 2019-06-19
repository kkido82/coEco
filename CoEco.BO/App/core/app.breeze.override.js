(function (angular) {
    "use strict";

    var app = angular.module('coeco');

    app.run(['breeze', initBreezeOverride]);

    function initBreezeOverride(breeze) {

        breeze.DataType.parseDateFromServer = function (source) {
            var date = moment(source);
            return date.toDate();
        };
    }

})(angular);