(function (angular) {
    "use strict";

    var app = angular.module('coeco');

    app.run(['breeze', 'coecoMetaContext', initValidationMessages]);

    function initValidationMessages(breeze, coecoMetaContext) {
        var messages = coecoMetaContext.validationMessages;
        for (var key in messages) {
            if (messages.hasOwnProperty(key)) {
                breeze.Validator.messageTemplates[key] = messages[key];
            }
        }
    }

    app.run([
        'coecoDataService', initDisplayNames]);

    function initDisplayNames(coecoDataService) {
        init(coecoDataService);      
     
    }

    function init(service) {
        var manager = service.manager;
        manager.fetchMetadata().then(function (md) {
            md.schema ? angular.forEach(md.schema.entityType, function (et) {
                try {
                    var etype = manager.metadataStore.getEntityType(et.name);
                    angular.forEach(et.property,
                        function (p) {

                            var prop = etype.getProperty(p.name);
                            prop.displayName = p.displayName;

                        });
                } catch (e) {
                    console.log(et, e);
                }

            }) : null;


        });
    }

})(angular);