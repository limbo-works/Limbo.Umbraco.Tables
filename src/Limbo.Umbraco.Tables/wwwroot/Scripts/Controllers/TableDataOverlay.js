﻿angular.module("umbraco").controller("Limbo.Umbraco.Tables.Overlay.Controller", function ($scope, formHelper) {
    $scope.save = function () {
        if (formHelper.submitForm({ scope: $scope })) {
            formHelper.resetForm({ scope: $scope });
            $scope.model.submit($scope.model);
        }
    };
});