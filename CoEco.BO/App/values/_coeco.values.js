(function () {
    'use strict';

   var app= angular.module('coeco.values', [
        // Angular modules


        // Custom modules
        'coeco.common','coeco.ui',
        // 3rd Party Modules
        'breeze.angular', 'ui.router','asideModule'
        
   ]);

   app.config(['$urlRouterProvider', function ($urlRouterProvider) {
       $urlRouterProvider.when('/values', '/values/list');
        $urlRouterProvider.when('/values/list', ['$state', function ($state) {
            $state.go('values.list', { entityType: 'Street' });
        }]);
    }]);


})();
