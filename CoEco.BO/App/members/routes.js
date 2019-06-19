(function (angular) {
    "use strict";

    angular.module('coeco').config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {

        $stateProvider
                .state('members', {
                    url: '/members',
                    data: {
                        title: 'ניהול משתמשים'
                    },
                    template: '<ui-view></ui-view>'
                })
                .state('members.list', {
                    url: '/list',
                    //represent component Name
                    template: '<members-list/>',
                    data: {
                        title: 'רשימת ניהול משתמשים'
                    }
                })
                .state('members.details', {
                    url: '/details/{id}',
                    template: '<members-details/>',

                    data: {
                        title: 'פרטי ניהול משתמש'  
                    }
                });

        $urlRouterProvider.when('/members', '/members/list');

    }
})(angular);