// Code goes here
angular.module("clientsApp", ["ui.bootstrap"])
    .controller("Controller", function ($scope, $uibModal) {
        $scope.open = function () {
            console.log('modal');
            $uibModal.open({
                ariaLabelledBy: 'modal-title',
                ariaDescribedBy: 'modal-body',
                templateUrl: 'modal.template.html',
                controller: 'ModalController',
                resolve: {
                    users: function () {
                        return [
                            { id: 1, name: "aaa" },
                            { id: 2, name: "bbb" },
                            { id: 3, name: "ccc" }
                        ];
                    }
                }
            }).result.then(
                function (result) {
                    console.log(result);
                },
                function (err) {
                    console.log("Modal dialog dismissed!", err);
                }
            );
        }
    })
    .controller("ModalController", function ($scope, $uibModalInstance, users) {
        $scope.users = users;
        $scope.ok = function () {
            $uibModalInstance.close("Modal dialog successfully closed!");
        }

        $scope.cancel = function () {
            $uibModalInstance.dismiss("cancel");
        }
    });
