// Code goes here
angular.module("clientsApp", ["ui.bootstrap"])
    .controller("ClientsController", function ($scope, $http, $uibModal) {
        $scope.ClientsList = null;
        $scope.ClientModel = null;
        $scope.apiCallResult = null;
        $scope.apiSuccess = false;
        getAllClients();


        $scope.emailPattern = /^(([^<>()[\]\.,;:\s@\"]+(\.[^<>()[\]\.,;:\s@\"]+)*)|(\".+\"))@(([^<>()[\]\.,;:\s@\"]+\.)+[^<>()[\]\.,;:\s@\"]{2,})$/i;
        $scope.namePattern = /^(?=.{1,50}$)[a-z]+(?:['_.\s][a-z]+)*$/i;
        $scope.phoneNumberPattern = /^[\+]?[(]?[0-9]{3}[)]?[-\s\.]?[0-9]{3}[-\s\.]?[0-9]{4,6}$/im;

        function getAllClients() {
            console.log('dd');
            $http({
                method: 'GET',
                url: '/api/clients/'
            }).then(function (response) {
                $scope.ClientsList = response.data;
                console.log(response.data);
            }, function (error) {
                console.log(error);
            });
        };

        function insertClientApi(data) {
            $http.post('/api/clients', JSON.stringify(data))
                .then(function (response) {
                    if (response.data) {
                        $scope.ClientModel = response.data;
                    }
                }, function (response) {
                    $scope.apiSuccess = false;
                    $scope.apiMessage = response
                });
        }

        function deleteClientApi(id) {
            $http({
                method: 'DELETE',
                url: `/api/clients/${id}`
            }).then(function (response) {
                $scope.apiCallResult = {
                    success: response.success
                };
            }, function (error) {
                $scope.apiCallResult = {
                    success: response.success,
                    message: response.message
                };
            });
        };

        $scope.submit = function () {
            let client = {
                firstName: $scope.clientFirstName,
                lastName: $scope.clientLastName,
                address: $scope.clientAddress,
                email: $scope.clientEmail
            } 
            console.log(client);
            insertClientApi(client);
            getAllClients();
            console.log($scope.ClientModel);
        }

        $scope.deleteClient = function (item) {
            deleteClientApi(item.id);
            if ($scope.apiCallResult.success) {
                alert("ok");
                var deleted = $scope.ClientsList.indexOf(item);
                $scope.ClientsList.splice(index, 1);
            }
            else {
                alert(`An error has occured while deleting item. ${$scope.apiCallResult.message}`);
            }
        }

        //$scope.closeDialog = function () {
        //    $rootScope.$emit("closeParentDialog", {});
        //}

        $scope.addNewClient = function () {
            console.log('modal');
            $uibModal.open({
                ariaLabelledBy: 'modal-title',
                ariaDescribedBy: 'modal-body',
                templateUrl: 'modal.template.html',
                controller: 'ClientsController',
                resolve: {}
            
            }).result.then(
                function (result) {
                    console.log(result);
                },
                function (err) {
                    console.log("Modal dialog dismissed!", err);
                }
            );
        }

        $scope.deleteConfirm = function (id) {
            $uibModal.open({
                ariaLabelledBy: 'modal-title',
                ariaDescribedBy: 'modal-body',
                templateUrl: 'modal.confirmDelete.html',
                controller: 'ClientsController',
                resolve: {
                    message: function () {
                        return `Are you sure you want to delete the item with id ${id}`;
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

        //$scope.ok = function () {
        //    $uibModalInstance.close("Modal dialog successfully closed!");
        //}

        //$scope.cancel = function () {
        //    $uibModalInstance.dismiss("cancel");
        //}

    })
    .controller("ModalController", function ($scope, $rootScope, $uibModalInstance, message) {

    $scope.ok = function () {
        $uibModalInstance.close("Modal dialog successfully closed!");
    }

    $scope.cancel = function () {
        $uibModalInstance.dismiss("cancel");
    }

    //$rootScope.$on("closeParentDialog", function () {
    //    $scope.cancel();
    //}
});

