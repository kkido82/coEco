(function () {
    'use strict';

    var endpoint = '/api/UserAdmin';

    angular
        .module('coeco.usersAdmin')
        .factory('usersService', usersService);

    usersService.$inject = ['$http', '$window', 'common'];

    function usersService($http, $window, common) {
        var manager;

        var service = {
            listUsers: listUsers,
            getById: getById,
            changePassword: changePassword,
            save: save,
            savePermissions: savePermissions,
            listUsersExport: listUsersExport
        };

        activate();

        return service;

        function listUsers(filterContext) {
            var url = endpoint + '/Users';

            filterContext = common.clean(filterContext);
            return $http.get(url, { params: filterContext });
        }

        function listUsersExport() {
            $window.open('/File/GetUsersAdminExport');
        }

        function getById(id) {
            var url = endpoint + '/GetById/' + id;
            return $http.get(url).then(unpackResponse);
        }

        function changePassword(id, password) {

            return $http.post(endpoint + '/ChangePassword', { userId: id, password: password });
        }

        function save(user) {
            var url = endpoint + '/save';
            return $http.post(url, user).then(unpackResponse);


        }

        function savePermissions(id, roles) {
            var url = endpoint + '/savepermissions/' + id;
            return $http.post(url, roles).then(unpackResponse);

        }

        function unpackResponse(response) {
            return response.data;
        }


        function activate() {

        }

        function setupFilters(query, filterContext) {

        }
    }
})();