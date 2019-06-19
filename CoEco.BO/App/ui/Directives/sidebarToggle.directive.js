(function (angular) {
    angular.module('coeco.ui').directive('sidebarToggle', directive);

    directive.$inject = ['navService', '$rootScope'];
    function directive(navService, $rootScope) {
        return {
            link: function (scope,element) {
                element.on('click', () => {
               
                    if (navService.stateHasSubMenu()) {
                        $rootScope.submenuOpen = !$rootScope.submenuOpen;
                    }
                    return true;
                });
            }
        };
    }

})(angular);