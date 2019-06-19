(function (angular) {
    "use strict";

    var app = angular.module('coeco.common');

    app.factory('userService', userService);

    userService.$inject = ['$http', '$q'];
    function userService($http, $q) {

        return {
            permissions: permissions
        };

        function permissions() {
            return $q.when({
                values: true,
                courses: true,
                meshartim: true,
                mutavim: true,
                cms: true,
                notification: true,
                applications: true,
                paychecks: true,
                usersAdmin: true,
                sync: true,
                reports: true,
                logs: true,
                nofashim: true,
                nofashimAllocations: true,
                nofashimHotels: true,
                nofashimVacationCycles: true,
                nofashimCaseInquiry: true,
                nofashimEntitledMesharet: true,
                nofashimReservation: true,
                nofashimSettings: true,
                nofashimReport: true,
                nofashimNotifications: true,
                nofashimEditEntitledMesharet: true,
                nofashimPenaltyCancellation: true,
                nofashimUploadFiles: true,
            });


        }

    }


})(angular);
