(function(angular) {
	"use strict";

	var module = angular.module('coeco.auth', ['coeco.common', 'LocalStorageModule', 'ui.router', 'ui.gravatar']);

	module.run(['authService','$rootScope', function(authService,$rootScope) {
	    authService.fillAuthData();
	    $rootScope.$on('mft:authSet', () => $rootScope.userState = angular.copy(authService.state));

	    $rootScope.$broadcast('mft:authSet');


	}]);


})(angular);