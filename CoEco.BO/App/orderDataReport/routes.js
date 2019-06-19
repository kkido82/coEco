(function (angular) {
    "use strict";

    angular.module('coeco').config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {

        $stateProvider
                .state('orderDataReport', {
                    url: '/orderDataReport',
                    data: {
                        title: 'ניהול דוח פרטי הזמנה'
                    },
                    template: '<ui-view></ui-view>'
                })
                .state('orderDataReport.list', {
                    url: '/list',
                    //represent component Name
                    template: '<order-data-report-list/>',
                    data: {
                        title: 'רשימת ניהול פרטי הזמנה'
                    }
                });

        $urlRouterProvider.when('/orderDataReport', '/orderDataReport/list');

    }
})(angular);