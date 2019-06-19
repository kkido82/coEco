(function (angular) {
    "use strict";

    angular.module('coeco').config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {

        $stateProvider
                .state('permissionsProfiles', {
                    url: '/permissionsProfiles',
                    data: {
                        title: 'ניהול פרופילי הרשאות'
                    },
                    template: '<ui-view></ui-view>'
                })
                .state('permissionsProfiles.list', {
                    url: '/list',
                    //represent component Name
                    template: '<permissions-profiles-list/>',
                    data: {
                        title: 'רשימת ניהול פרופילי הרשאות'
                    }
                })
                .state('permissionsProfiles.details', {
                    url: '/details/{id}',
                    template: '<permissions-profiles-details/>',

                    data: {
                        title: 'פרטי ניהול פרופיל הרשאות'  
                    }
                });

        $urlRouterProvider.when('/permissionsProfiles', '/permissionsProfiles/list');

    }
})(angular);