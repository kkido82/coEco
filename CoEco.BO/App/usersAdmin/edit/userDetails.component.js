(function (angular) {
    "use strict";

    var app = angular.module('coeco.usersAdmin');

    app.component('userDetails', {
        bindings: {
            editedUser: '<',
            onsave: '&',
            onreset: '&',
            isNew:'<'
        },
        templateUrl: '/App/usersAdmin/edit/userDetails.tmpl.html',
        controllerAs: 'vm',
        controller: Controller


    });
    Controller.$inject = ['coecoMetaContext', 'datePickerLocale'];
    function Controller(coecoMetaContext, datePickerLocale) {
        var vm = this;
        vm.validationMessages = coecoMetaContext.validationMessages;
        vm.showError = showError;
        vm.getMessage = getMessage;
        vm.locale = datePickerLocale;
        
        vm.$onInit = onInit;
    

        function showError(key) {

            return !vm.editForm[key].$valid && vm.editForm[key].$dirty;
        }

        function getMessage(key, displayName) {
            var message = vm.validationMessages[key];
            if (!message) {
                return undefined;
            }
            return message.replace('%displayName%', displayName);
        }

        function onInit() {
           }

       
        
    }

   
})(angular);