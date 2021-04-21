
angular.module("clientsApp", ['ngAnimate', 'ngSanitize', 'ui.bootstrap']);

angular.module('clientsApp').controller("ClientsController", function ($http, $uibModal) {
    
    //getClients();
    var $ctrl = this;
    $ctrl.ClientsList = null;

    

    // regex patterns
    //$ctrl.emailPattern = /^(([^<>()[\]\.,;:\s@\"]+(\.[^<>()[\]\.,;:\s@\"]+)*)|(\".+\"))@(([^<>()[\]\.,;:\s@\"]+\.)+[^<>()[\]\.,;:\s@\"]{2,})$/i;
    $ctrl.emailPattern = /^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$/;
    $ctrl.namePattern = /^(?=.{2,50}$)[a-z]+(?:['_.\s][a-z]+)*$/i;
    $ctrl.phoneNumberPattern = /^[\+]?[(]?[0-9]{3}[)]?[-\s\.]?[0-9]{3}[-\s\.]?[0-9]{4,6}$/im;

    $ctrl.getClients = function () {
        getClients();
    }

    $ctrl.addClient = function () {
        var modalInstance = $uibModal.open({
            animation: true,
            ariaLabelledBy: 'modal-title',
            ariaDescribedBy: 'modal-body',
            templateUrl: 'clientModal.html',
            controller: 'CreateClientController',
            controllerAs: '$ctrl'
        });

        modalInstance.result.then(function (item) {
            addClientApi(item);
        });
    };

    $ctrl.editClient = function (item) {
        $ctrl.client = item;
        console.log(item);
        var modalInstance = $uibModal.open({
            animation: true,
            ariaLabelledBy: 'modal-title',
            ariaDescribedBy: 'modal-body',
            templateUrl: 'clientModal.html',
            controller: 'EditClientController',
            controllerAs: '$ctrl',
            resolve: {
                client: function () {
                    return $ctrl.client;
                }
            }
        });

        modalInstance.result.then(function (model) {
            console.log(model);
            $ctrl.client = model;
            editClientApi(model);
        }, function () {

        });
    };

    $ctrl.deleteClient = function (item) {
        $ctrl.client = item;
        var modalInstance = $uibModal.open({
            animation: true,
            ariaLabelledBy: 'modal-title',
            ariaDescribedBy: 'modal-body',
            templateUrl: 'deleteClientModal.html',
            controller: 'DeleteConfirmController',
            controllerAs: '$ctrl',
            resolve: {
                client: function () {
                    return $ctrl.client;
                }
            }
        });

        modalInstance.result.then(function (model) {
            deleteClientApi(model.id);
            $ctrl.ClientsList.splice($ctrl.ClientsList.findIndex(item => item.id === model.id), 1);
        });
    };


    function showDialog(result) {
        console.log('show dialog called');
        console.log(result);

        var modalInstance = $uibModal.open({
            animation: true,
            ariaLabelledBy: 'modal-title',
            ariaDescribedBy: 'modal-body',
            templateUrl: 'dialogModal.html',
            controller: 'DialogController',
            controllerAs: '$ctrl',
            resolve: {
                client: function () {
                    return result;
                }
            }
        });

        //modalInstance.result.then(function (model) {
        //});
    }

    function getClients() {
        console.log('dd');
        $http({
            method: 'GET',
            url: '/api/clients/'
        }).then(function (response) {
            $ctrl.ClientsList = response.data.map(item => ({
                id: item.id,
                lastName: item.lastName,
                firstName: item.firstName,
                address: item.address,
                email: item.email,
                homePhones: item.phoneNumbers.filter((item) => { return item.type === 'Home' }).map((item) => { return item.number }).join(", "),
                mobilePhones: item.phoneNumbers.filter((item) => { return item.type === 'Mobile' }).map((item) => { return item.number }).join(", "),
                workPhones: item.phoneNumbers.filter((item) => { return item.type === 'Work' }).map((item) => { return item.number }).join(", "),
            }));
            console.log($ctrl.ClientsList);
        }, function (error) {
            console.log(error);
        });
    };

    function addClientApi(item) {
        var homePhones = item.homePhones ? [{ type: "Home", number: item.homePhones }] : []
        var mobilePhones = item.mobilePhones ? [{ type: "Mobile", number: item.mobilePhones }] : []
        var workPhones = item.workPhones ? [{ type: "Work", number: item.workPhones }] : []
        var client = {
            firstName: item.firstName,
            lastName: item.lastName,
            address: item.address,
            email: item.email,
            phoneNumbers: [...homePhones, ...mobilePhones, ...workPhones
            ]
        };

        $http.post('/api/clients', JSON.stringify(client))
            .then(function (response) {
                if (response.data) {
                    $ctrl.client = {
                        id: response.data.id,
                        lastName: response.data.lastName,
                        firstName: response.data.firstName,
                        address: response.data.address,
                        email: response.data.email,
                        homePhones: response.data.phoneNumbers.filter((item) => { return item.type === 'Home' }).map((item) => { return item.number }).join(", "),
                        mobilePhones: response.data.phoneNumbers.filter((item) => { return item.type === 'Mobile' }).map((item) => { return item.number }).join(", "),
                        workPhones: response.data.phoneNumbers.filter((item) => { return item.type === 'Work' }).map((item) => { return item.number }).join(", "),
                    }
                    $ctrl.ClientsList.push($ctrl.client);
                }
            }, function (response) {

            });
    }

    function editClientApi(item) {
        var homePhones = item.homePhones ? [{ type: "Home", number: item.homePhones }] : []
        var mobilePhones = item.mobilePhones ? [{ type: "Mobile", number: item.mobilePhones }] : []
        var workPhones = item.workPhones ? [{ type: "Work", number: item.workPhones }] : []
        var client = {
            id: item.id,
            firstName: item.firstName,
            lastName: item.lastName,
            address: item.address,
            email: item.email,
            phoneNumbers: [...homePhones, ...mobilePhones, ...workPhones
            ]
        };

        $http.put('/api/clients', JSON.stringify(client))
            .then(function (response) {
                if (response.data) {
                    $ctrl.client = {
                        id: response.data.id,
                        lastName: response.data.lastName,
                        firstName: response.data.firstName,
                        address: response.data.address,
                        email: response.data.email,
                        homePhones: response.data.phoneNumbers.filter((item) => { return item.type === 'Home' }).map((item) => { return item.number }).join(", "),
                        mobilePhones: response.data.phoneNumbers.filter((item) => { return item.type === 'Mobile' }).map((item) => { return item.number }).join(", "),
                        workPhones: response.data.phoneNumbers.filter((item) => { return item.type === 'Work' }).map((item) => { return item.number }).join(", "),
                    }
                    $ctrl.ClientsList[$ctrl.ClientsList.findIndex(item => item.id === $ctrl.client.id)] = $ctrl.client;
                }
            }, function (response) {

            });
    }

    function deleteClientApi(id) {
        $http.delete(`/api/clients/${id}`, id).then(function (response) {

            //showDialog({ state: 'success', message: 'success message' });

        }, function (response) {
                //showDialog({ state: 'failure', message: 'failure message' });
        });
    };
});

angular.module('clientsApp').controller("CreateClientController",
    function ($uibModalInstance) {
        var $ctrl = this;
        $ctrl.clientModel = null;
        
        $ctrl.ok = function () {
            $uibModalInstance.close($ctrl.clientModel);
        };

        $ctrl.cancel = function () {
            $uibModalInstance.dismiss("cancel");
        };
    });

angular.module('clientsApp').controller("DeleteConfirmController",
    function ($uibModalInstance, client) {
        var $ctrl = this;
        $ctrl.clientModel = {
            firstName: "",
            lastName: "",
            address: "",
            email: "",
            homePhones: "",
            mobilePhones: "",
            workPhones: ""
        };

        $ctrl.ok = function () {
            $uibModalInstance.close($ctrl.clientModel);
        }

        $ctrl.cancel = function () {
            $uibModalInstance.dismiss("cancel");
        }
    });

angular.module('clientsApp').controller("EditClientController",
    function ($uibModalInstance, client) {

        var $ctrl = this;
        $ctrl.clientModel = {
            id: client.id,
            firstName: client.firstName,
            lastName: client.lastName,
            address: client.address,
            email: client.email,
            homePhones: client.homePhones,
            mobilePhones: client.mobilePhones,
            workPhones: client.workPhones
        };
        

        $ctrl.ok = function () {
            $uibModalInstance.close($ctrl.clientModel);
        }

        $ctrl.cancel = function () {
            $uibModalInstance.dismiss("cancel");
        }
    });

angular.module('clientsApp').controller("DialogController",
    function ($uibModalInstance, result) {
        console.log('dialog');
        console.log(result);
        var $ctrl = this;
        $ctrl.result = result;

        $ctrl.ok = function () {
            $uibModalInstance.close($ctrl.result);
        }

        $ctrl.cancel = function () {
            $uibModalInstance.dismiss("cancel");
        }
    });

