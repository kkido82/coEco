(function (angular) {
    "use strict";

    angular.module('coeco').config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {

        $stateProvider
                .state('units', {
                    url: '/units',
                    data: {
                        title: 'ניהול יחידות'
                    },
                    template: '<ui-view></ui-view>'
                })
                .state('units.list', {
                    url: '/list',
                    //represent component Name
                    template: '<units-list/>',
                    data: {
                        title: 'רשימת ניהול יחידות'
                    }
                })
            .state('units.details', {
                url: '/details/{id}',
                template: '<units-details/>',

                data: {
                    title: 'פרטי ניהול יחידה'
                }
            })
            .state('units.distances', {
                url: '/distances',
                template: '<units-distances />',

                data: {
                    title: 'ניהול מרחקים בין יחידות'
                }
            });

        $urlRouterProvider.when('/units', '/units/list');

    }
})(angular);