(function () {
    'use strict';
    angular.module('coeco.ui').directive('printElement', printDirective);
    printDirective.$inject = ['$state'];

    function printDirective($state) {
        return {
            link: link,
            restrict: 'A',
            scope: {
                printElementRemoveClass: "="
            }
        };

        function link(scope, element, attrs, ngmodelController) {

            element.on('click', function () {
                var elemToPrint = document.getElementById(attrs.printElementId);
                if (elemToPrint) {
                    printElement(elemToPrint);
                }
            });

            function printElement(elem) {
                var elemClone = elem.cloneNode(true);

                var title = document.title;
                if ($state.current.data) {
                    title = $state.current.data.title;
                }

                var printWindow = window.open('', 'PRINT', 'height=800px,width=800px');
                var html = `<html>
                                <head>
                                    <title>${document.title}</title>
                                      <link href="/content/bootstrap.min.css" rel="stylesheet" />
                                      <link href="/app/ui/css/print.css" rel="stylesheet" />
                                </head>
                                <body>
                                    <h1 class="centered">${title}</h1>
                                    ${elemClone.outerHTML}
                                    <script>
                                          document.addEventListener('DOMContentLoaded', function() {
                                                window.print();
                                                window.close()
                                            });
                                    </script>
                                </body>
                            </html>`;

                printWindow.document.write(html);

                printWindow.document.close(); // necessary for IE >= 10
                printWindow.focus(); // necessary for IE >= 10*/

            }


        }
    }
})();