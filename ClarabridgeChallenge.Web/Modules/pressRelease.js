/// <reference path="~/scripts/angular.js" />
(function ($) {
    var pressReleaseApp = angular
        .module("pressReleaseModule", ['ngSanitize', 'ngDialog'])
        .controller("listController", function ($scope, $rootScope, $http) {

            function initSignalR() {
                var hubConnection = $.hubConnection();
                var hubPressRelease = hubConnection.createHubProxy('pressReleaseNotification');
                hubPressRelease.on('onPressReleaseListRefresh', function (data) {
                    $rootScope.$broadcast(appEvents.pressRelease.listRefresh);
                });
                hubConnection.start();
            }

            $scope.$on(appEvents.pressRelease.listRefresh, function () {
                refreshList($scope, $rootScope, $http);
            })

            function refreshList($scope, $rootScope, $http) {
                $http.get(urlList.pressRelease)
                .then(function successCallback(response) {
                    $scope.items = response.data;
                    $scope.view = function (id) {
                        if (canScopePerformClick($scope)) {
                            $rootScope.$broadcast(appEvents.pressRelease.detailShowDialog, { id: id });
                        }
                        return false;
                    }
                    $scope.add = function () {
                        if (canScopePerformClick($scope)) {
                            $rootScope.$broadcast(appEvents.pressRelease.addUpdateDeleteShowDialog, { id: null });
                        }
                        return false;
                    }
                    $scope.update = function (id) {
                        if (canScopePerformClick($scope)) {
                            $rootScope.$broadcast(appEvents.pressRelease.addUpdateDeleteShowDialog, { id: id });
                        }
                        return false;
                    }
                }, function errorCallback(response) {
                    $rootScope.$broadcast(appEvents.messageBox.showErrorMessage, { "message": resources.error.errorOccurred + (response.data.Message ? response.data.Message : response.statusText) });
                });
            }

            initSignalR();
            $rootScope.$broadcast(appEvents.pressRelease.listRefresh, {});

        })
        .controller("detailController", function ($scope, $rootScope, $http, ngDialog) {
            $scope.update = function ($scope) {
                if (canScopePerformClick($scope)) {
                    $rootScope.$broadcast(appEvents.pressRelease.addUpdateDeleteShowDialog, { id: $scope.item.Id });
                    $scope.closeThisDialog();
                }
                return false;
            },
            $scope.cancelItem = function ($scope) {
                if (canScopePerformClick($scope)) {
                    $scope.closeThisDialog();
                }
            };

            $scope.$on(appEvents.pressRelease.detailShowDialog, function (event, data) {
                if (data.id) {
                    $http.get(urlList.pressRelease + data.id)
                    .then(function successCallback(response) {
                        $scope.item = response.data;
                        ngDialog.open({
                            className: 'pressReleaseDetailDialog ngdialog-theme-default',
                            template: constants.templates.pressRelease.detail,
                            scope: $scope,
                            showClose: false
                        });
                    }, function errorCallback(response) {
                        $rootScope.$broadcast(appEvents.messageBox.showErrorMessage, { "message": resources.error.errorOccurred + (response.data.Message ? response.data.Message : response.statusText) });
                    });
                }
            })
        })
        .controller("addUpdateDeleteController", function ($scope, $rootScope, $http, ngDialog) {
            $scope.saveItem = function ($scope) {
                if (canScopePerformClick($scope)) {
                    $http.post(urlList.pressRelease, $scope.item)
                    .then(function successCallback(response) {
                        $scope.closeThisDialog();
                        $rootScope.$broadcast(appEvents.messageBox.showSuccessMessage, { "message": resources.pressRelease.addUpdateDelete.PressReleaseSaved });
                        $rootScope.$broadcast(appEvents.pressRelease.detailShowDialog, { "id": response.data.Id });
                    }, function errorCallback(response) {
                        $rootScope.$broadcast(appEvents.messageBox.showErrorMessage, { "message": resources.error.errorOccurred + (response.data.Message ? response.data.Message : response.statusText) });
                    });
                }
                return false;
            },
            $scope.deleteItem = function ($scope) {
                if (canScopePerformClick($scope)) {
                    $http.delete(urlList.pressRelease + $scope.item.Id, $scope.item)
                    .then(function successCallback(response) {
                        $scope.closeThisDialog();
                        $rootScope.$broadcast(appEvents.messageBox.showSuccessMessage, { "message": resources.pressRelease.addUpdateDelete.PressReleaseDeleted });
                    }, function errorCallback(response) {
                        $rootScope.$broadcast(appEvents.messageBox.showErrorMessage, { "message": resources.error.errorOccurred + (response.data.Message ? response.data.Message : response.statusText) });
                    });
                }
                return false;
            },
            $scope.cancelItem = function ($scope) {
                if (canScopePerformClick($scope)) {
                    $scope.closeThisDialog();
                    $rootScope.$broadcast(appEvents.pressRelease.detailShowDialog, { "id": $scope.item.Id });
                }
            };

            $scope.$on(appEvents.pressRelease.addUpdateDeleteShowDialog, function (event, data) {
                $scope.item = null;
                if (data.id) {
                    $http.get(urlList.pressRelease + data.id)
                    .then(function successCallback(response) {
                        $scope.item = response.data,
                        loadAddUpdateDialog($scope, ngDialog);

                    }, function errorCallback(response) {
                        $(document).trigger(appEvents.messageBox.showErrorMessage, [{ "message": "Error Occurred" + ": " + response.data.Message }]);
                    });
                }
                else {
                    loadAddUpdateDialog($scope, ngDialog);
                }
            });
            function loadAddUpdateDialog($scope, ngDialog) {
                $scope.Heading = $scope.item ? resources.pressRelease.addUpdateDelete.UpdatePressRelease : resources.pressRelease.addUpdateDelete.AddNewPressRelease
                ngDialog.openConfirm({
                    className: 'pressReleaseAddUpdateDeleteDialog ngdialog-theme-default',
                    template: constants.templates.pressRelease.addUpdateDelete,
                    scope: $scope,
                    showClose: false,
                    backdrop: 'static',
                    keyboard: false
                });
            }
        })
        .controller("messageBoxController", function ($scope, $timeout) {
            $scope.$on(appEvents.messageBox.showSuccessMessage, function (event, data) {
                messageBox.showSuccessMessage($scope, $timeout, data.message);
            });
            $scope.$on(appEvents.messageBox.showErrorMessage, function (event, data) {
                messageBox.showErrorMessage($scope, $timeout, data.message);
            });
        })
        .directive("datetimepicker", dateTimePickerDirective)
        .directive('ckEditor', ckEditorDirective);

    function canScopePerformClick($scope) {
        if (!$scope.buttonDisabled) {
            $scope.buttonDisabled = true;
            setTimeout(function () { $scope.buttonDisabled = false; }, 500);
            return true;
        }
        return false;
    }
}(jQuery));