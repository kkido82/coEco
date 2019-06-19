(function (angular) {
    "use strict";

    angular.module('coeco').config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {

        $stateProvider
                .state('notifications', {
                    url: '/notifications',
                    data: {
                        title: 'ניהול מסרונים'
                    },
                    template: '<ui-view></ui-view>'
                })
                .state('notifications.list', {
                    url: '/list',
                    //represent component Name
                    template: '<notifications-list/>',
                    data: {
                        title: 'רשימת ניהול מסרונים'
                    }
                })
                .state('notifications.details', {
                    url: '/details/{id}',
                    template: '<notifications-details/>',

                    data: {
                        title: 'פרטי ניהול מסרון'  
                    }
                });

        $urlRouterProvider.when('/notifications', '/notifications/list');

    }
})(angular);