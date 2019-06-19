/// <reference path="controller.js" />
/// <reference path="dataService.js" />
(function (angular) {
    "use strict";

    angular.module('coeco.values').controller('listValueCtrl', Controller);

    Controller.$inject = ['entityController', 'entityType', 'entityTableDefinitions'];
    function Controller(entityController, entityType, entityTableDefinitions) {
    
        var vm = this;
        entityController(vm, entityType.single,entityType.plural,entityTableDefinitions[entityType.single.toLowerCase()]);

        activate();

        function activate() {
            vm.activate();
         
        }
            

    }

})(angular);