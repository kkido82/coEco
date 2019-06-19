(function () {
    'use strict';

    var permissionItems = {
        manageItems: 'ניהול פריטים',
        usersAdmin: 'ניהול מנהלנים',
        userAdminEdit: 'עריכת מנהלן',
        units: 'ניהול יחידות',
        ratingQuestions: 'ניהול שאלות דירוג',
        notifications: 'ניהול מסרונים',
        permissionsProfiles: 'ניהול פרופילי הרשאות',
        members: 'ניהול משתמשים',
        orderDataReport: 'ניהול דוח פרטי הזמנה',
        problemSolving: 'פתרון בעיות'
    };

    var endpointPrefix = '/api/Account/';
    angular
        .module('coeco.auth')
        .factory('authService', auth);

    auth.$inject = ['$http', 'localStorageService', '$rootScope', '$q', '$cacheFactory', 'coecoMetaContext'];

    function auth($http, localStorageService, $rootScope, $q, $cacheFactory, coecoMetaContext) {
        var _authentication = {
            isAuth: false,
            userName: ""
        };

        var service = {
            login: login,
            signout: logout,
            fillAuthData: fillAuthData,
            privileges: privileges,
            authentication: _authentication,
            getPermissionItems: getPermissionItems,
            get state() {
                return _authentication;
            }
        };

        return service;

        function login(loginData) {

            return $http.post('/api/account/login', loginData)
                .then(unpackResponse)
                .then(r => {
                    localStorageService.set('userData', r);
                    _authentication = {
                        isAuth: true,
                        userData: r
                    };
                })
                .then(() =>$rootScope.$broadcast('mft:authSet'));



        }

        function logout() {

            //return $http.post('/api/account/signout').then(unpackResponse).then(r => {
            //    localStorageService.remove('userData');
            //    _authentication.isAuth = false;
            //    _authentication.userData = null;
            //    $rootScope.$broadcast('mft:authSet');
            //    $cacheFactory.get('$http').removeAll();
            //    return r;
            //});

            return $http.post('/api/account/signout').then(unpackResponse).then((r => {
                localStorageService.remove('userData');
                _authentication.isAuth = false;
                _authentication.userData = null;
                $rootScope.$broadcast('mft:authSet');
                $cacheFactory.get('$http').removeAll();
                return r;

            }), function (err) {
                localStorageService.remove('userData');
                _authentication.isAuth = false;
                _authentication.userData = null;
                $rootScope.$broadcast('mft:authSet');
                $cacheFactory.get('$http').removeAll();

            });


        }

        function fillAuthData() {
            if (coecoMetaContext.userData) {
                localStorageService.set('userData', coecoMetaContext.userData);
            }

            var userData = localStorageService.get('userData');
            if (userData) {
                _authentication.isAuth = true;
                _authentication.userData = userData;
            }

        }

        function privileges() {

            return $http.get(endpointPrefix + 'Priveleges', { cache: true }).then(unpackResponse);
        }

        function unpackResponse(response) {
            return response.data;
        }

        function getPermissionItems() {
            return $q.when(permissionItems);
        }


    }
})();