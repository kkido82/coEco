(function (angular) {
    "use strict";

    var app = angular.module('coeco');


    app.config(['$stateProvider', '$urlRouterProvider', 'coecoValuesConfig', 'pluralize',
       function ($stateProvider, $urlRouterProvider, coecoValuesConfig, pluralize) {
           var entityTypeResolver = [
           '$stateParams', function ($stateParams) {
               var et = $stateParams.entityType;
               var entityType = {
                   single: et,
                   plural: pluralize(et)
               };
               return entityType;
           }
           ];

           $urlRouterProvider.when('/manageItems', '/manageItems/list')
                             .when('/units', '/units/list')
                             .when('/ratingQuestions', '/ratingQuestions/list')
                             .when('/notifications', '/notifications/list')
                             .when('/permissionsProfiles', '/permissionsProfiles/list')
                             .when('/members', '/members/list')
                             .when('/orderDataReport', '/orderDataReport/list')
                             .otherwise('/');

           $stateProvider
               .state('login', {
                   url: '/login',
                   data: {
                       hideInNavigation: true
                   },
                   controller: 'loginCtrl',
                   templateUrl: '/app/auth/login/login.tmpl.html',
                   controllerAs: 'vm'
               })
               .state('home', {
                   url: '/',
                   data: {
                       hideInNavigation: true
                   },
                   template: '<nav-dashboard/>'
               })




               .state('usersAdmin', {
                   url: '/usersAdmin',
                   data: {
                       requiredPermission: 'usersAdmin',
                       icon: 'suitcase',
                       title: 'מנהלי מערכת'
                   },
                   controller: 'usersAdminListCtrl',
                   templateUrl: '/app/usersAdmin/list/userAdminList.tmpl.html',
                   controllerAs: 'vm'
               })
               .state('userAdminEdit', {
                   url: '/userAdmin/:id',
                   controller: 'usersAdminEditCtrl',
                   templateUrl: '/app/usersAdmin/edit/userAdminEdit.tmpl.html',
                   controllerAs: 'vm',
                   data: {
                       title: 'עריכת מנהלן',
                       parent: 'usersAdmin'
                   }
               });
              // .state('values', {
              //     url: '/values',
              //     data: {
              //         requiredPermission: 'values',
              //         icon: 'link',
              //         title: 'מאפייני מערכת',
              //         hasSubMenu: 'true'
              //     }, views: {
              //         'submenu': {
              //             template: '<values-navbar/>'
              //         },
              //         '': {
              //             template: '<ui-view/>'
              //         }
              //     }
              // })
              // .state('values.list', {
              //     url: '/list/{entityType}',
              //     data: {
              //         title: 'ניהול'
              //     },
              //     controller: 'listValueCtrl',
              //     templateUrl: 'app/values/list/listValue.tmpl.html',
              //     resolve: {
              //         entityType: entityTypeResolver
              //     },
              //     controllerAs: 'vm'
              // })
              // .state('values.edit', {
              //     url: '/edit/{entityType}/{id}',
              //     controller: 'valueEditorCtrl',
              //     templateUrl: 'app/values/edit/valueEditor.tmpl.html',
              //     data: {
              //         title: 'עריכה'
              //     },
              //     resolve: {
              //         entityType: entityTypeResolver
              //     },
              //     controllerAs: 'vm'
              // })
              //.state('reportingService', {
              //    url: '/reportingService{entityType}/{id}',
              //    controller: 'reportingServiceCtrl',
              //    templateUrl: 'app/reportingService/reportingService/reportingService.tmpl.html',
              //    data: {
              //        title: 'דוחות'
              //    },
              //    resolve: {
              //        entityType: entityTypeResolver
              //    },
              //    controllerAs: 'vm'
              //})
              //.state('sync', {
              //    url: '/sync',
              //    templateUrl: 'app/sync/sync.menu.tmpl.html',
              //    data: {
              //        title: 'סנכרון'
              //    }
              //})
           //.state('logs',
           //{
           //    url: '/logs',
           //    data: {
           //        requiredPermission: 'logsAndReports',
           //        icon: 'laptop',
           //        title: 'לוגים'
           //    },
           //    templateUrl: '/app/logs/logs.html',
           //    controller: 'logsCtrl',
           //    controllerAs: 'vm'
           //});




       }
    ]);


})(angular);