(function (angular) {
    "use strict";

    angular.module('coeco').config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {

        $stateProvider
                .state('ratingQuestions', {
                    url: '/ratingQuestions',
                    data: {
                        title: 'ניהול שאלות דירוג'
                    },
                    template: '<ui-view></ui-view>'
                })
                .state('ratingQuestions.list', {
                    url: '/list',
                    //represent component Name
                    template: '<rating-questions-list/>',
                    data: {
                        title: 'רשימת ניהול שאלות דירוג'
                    }
                })
                .state('ratingQuestions.details', {
                    url: '/details/{id}',
                    template: '<rating-questions-details/>',

                    data: {
                        title: 'פרטי ניהול שאלת דירוג'  
                    }
                });

        $urlRouterProvider.when('/ratingQuestions', '/ratingQuestions/list');

    }
})(angular);