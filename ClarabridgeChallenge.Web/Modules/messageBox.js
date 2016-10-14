var messageBox = (function(){

    function showMessage($scope, $timeout, message, timeout) {
        $scope.message = message;
        $scope.showMessage = true;
        if (timeout) {
            $timeout(function () {
                $scope.showMessage = false;
            }, timeout);
        }
    }
    return {
        showSuccessMessage: function ($scope, $timeout, message) {
            $scope.messageType = "success";
            showMessage($scope, $timeout, message, 2000);
        },
        showErrorMessage: function ($scope, $timeout, message) {
            $scope.messageType = "error";
            showMessage($scope, $timeout, message, 5000);
        }
    }

}());
