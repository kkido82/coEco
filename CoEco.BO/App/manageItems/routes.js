(function (angular) {
    "use strict";

    angular.module('coeco').config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {

        $stateProvider
                .state('manageItems', {
                    url: '/manageItems',
                    data: {
                        title: 'ניהול פריטים'
                    },
                    template: '<ui-view></ui-view>'
                })
                .state('manageItems.list', {
                    url: '/list',
                    //represent component Name
                    template: '<manage-items-list/>',
                    data: {
                        title: 'רשימת ניהול פריטים'
                    }
                })
                .state('manageItems.details', {
                    url: '/details/{id}',
                    template: '<manage-items-details/>',

                    data: {
                        title: 'פרטי ניהול פריט'  
                    }
                });

        $urlRouterProvider.when('/manageItems', '/manageItems/list');

    }
})(angular);