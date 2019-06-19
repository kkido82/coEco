(function () {
    'use strict';

    angular
        .module('coeco.common')
        .factory('breezeUriBuilder', breezeUriBuilder);

    breezeUriBuilder.$inject = ['breeze'];

    function breezeUriBuilder(breeze) {
        var interfaceRegistry = breeze.config.interfaceRegistry,
            uriBuilderInterface = interfaceRegistry.uriBuilder,
            uriBuilderCtor = uriBuilderInterface.getImpl('odata').ctor;

        return new uriBuilderCtor();


    }
})();