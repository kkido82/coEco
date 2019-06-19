(function (angular) {
    "use strict";

    angular.module('coeco.ui').directive('coecoTableRow', coecoTableRow);

    coecoTableRow.$inject = ['$compile'];
    function coecoTableRow($compile) {
        var directive = {
            link: link,
            restrict: 'A',
            scope: {
                entity: '=',
                toggleDisable: '&',
                entityTypeDefintions: '='
            }

        };

        return directive;

        function link(scope, element, attrs) {
            var tmpl = '';
            if (!scope.entityTypeDefintions || !scope.entityTypeDefintions.fields || !angular.isArray(scope.entityTypeDefintions.fields)) {
                return;
            }

            angular.forEach(scope.entityTypeDefintions.fields, function (field) {
                tmpl += "<td>{{entity." + field.value + "}}</td>";
            });
            tmpl += "<td>{{entity.CreatedBy}}</td>" +
                "       <td>{{entity.CreatedOn | date:'medium'}}</td>" +
                "       <td>{{entity.UpdatedBy}}</td>" +
                "       <td>{{entity.UpdatedOn | date:'medium'}}</td>" +
                "       <td>" +
                "           <a class=\"btn btn-info btn-sm\"  ng-class=\"{'btn-primary': !entity.Disabled}\"  ng-if=\"!entity.Disabled\"  ui-sref='values.edit({id:entity.ID,entityType:entity.entityType.shortName})' >ערוך</a>" +
                "           <button class=\"btn btn-sm {{entity.Disabled ? 'btn-success': 'btn-danger'}}\" ng-click=\"toggleDisable()\" >{{entity.Disabled ? 'אפשר' : 'חסום'}}</button>" +
                "       </td>";

            var compiledTemplate = $compile(tmpl);
            compile();
            scope.$watch('entity', function (n, o) {
                if (n && !o) {
                    compile();
                }
            });

            function compile() {
                if (!scope.entity) {
                    return;
                }
                var result = compiledTemplate(scope);
                element.append(result);
            }



        }
    }
})(angular);