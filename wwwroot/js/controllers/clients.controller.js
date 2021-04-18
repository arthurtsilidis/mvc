
angular.module("clientsApp", ['ngAnimate', 'ngSanitize', 'ui.bootstrap']);

angular.module('clientsApp').controller("ClientsController", function ($scope, $http, $uibModal) {
    $scope.ClientsList = null;
    $scope.ClientModel = null;
    $scope.apiCallResult = null;
    $scope.apiSuccess = false;
    $scope.itemToDelete = false;
    getAllClients();

    var $ctrl = this;
   

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
        $http.delete(`/api/clients/${id}`, id).then(function (response) {

        }, function (response) {

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
    };

    deleteConfirm = function (id) {
        var modalInstance = $uibModal.open({
            animation: true,
            ariaLabelledBy: 'modal-title',
            ariaDescribedBy: 'modal-body',
            templateUrl: 'modal.confirmDelete.html',
            controller: 'ModalController',
            controllerAs: '$ctrl',
            resolve: {
                itemToDelete: function () {
                    return $scope.itemToDelete;
                }
            }
        });

        modalInstance.result.then(function () {
            deleteClientApi($scope.itemToDelete.id);
            var deleted = $scope.ClientsList.indexOf($scope.itemToDelete);
            $scope.ClientsList.splice(deleted, 1);
            console.log($scope.ClientsList);
        });
    };

    $scope.deleteClient = function (item) {
        $scope.itemToDelete = item;
        deleteConfirm(item.id);

        //deleteClientApi(item.id);
        //if ($scope.apiCallResult.success) {
        //    alert("ok");
        //    var deleted = $scope.ClientsList.indexOf(item);
        //    $scope.ClientsList.splice(index, 1);
        //}
        //else {
        //    alert(`An error has occured while deleting item. ${$scope.apiCallResult.message}`);
        //}
    }

    $scope.addNewClient = function () {
        let client = {
            firstName: $scope.clientFirstName,
            lastName: $scope.clientLastName,
            address: $scope.clientAddress,
            email: $scope.clientEmail
        }
        insertClientApi(client);
        getAllClients();
        console.log($scope.ClientModel);
        var modalInstance = $uibModal.open({
            animation: true,
            ariaLabelledBy: 'modal-title',
            ariaDescribedBy: 'modal-body',
            templateUrl: 'modal.template.html',
            controller: 'ModalController',
            controllerAs: '$ctrl',
            resolve: {}
        });

        modalInstance.result.then(function () {

            //deleteClientApi(item.id);
            //getAllClients();
        });
    };
})
    .controller("ModalController", function ($scope, $uibModalInstance) {

        $scope.ok = function () {
            $uibModalInstance.close("Modal dialog successfully closed!");
        }

        $scope.cancel = function () {
            $uibModalInstance.dismiss("cancel");
        }
    });

