angular.module("umbraco").controller("Limbo.Umbraco.Tables.CacheLevel", function ($scope, $http, editorService) {

    const vm = this;

    vm.levels = [
        { alias: "Element", name: "Element", description: "Indicates that the property value can be cached at the element level, i.e. it can be cached until the element itself is modified." },
        { alias: "Elements", name: "Elements", description: "Indicates that the property value can be cached at the elements level, i.e. it can be cached until any element is modified." },
        { alias: "Snapshot", name: "Snapshot", description: "Indicates that the property value can be cached at the snapshot level, i.e. it can be cached for the duration of the current snapshot.", remarks: "In most cases, a snapshot is created per request, and therefore this is equivalent to cache the value for the duration of the request." },
        { alias: "None", name: "None", description: "Indicates that the property value cannot be cached and has to be converted each time it is requested." }
    ];

    vm.defaultLevel = vm.levels[1];

    vm.selected = vm.defaultLevel;

    if ($scope.model.value) {
        vm.selected = vm.levels.find(x => x.alias === $scope.model.value) ?? vm.selected;
        vm.levels.forEach(function (l) {
            l.active = l.alias === vm.selected.alias;
        });
    } else {
        vm.selected.active = true;
    }

    vm.select = function (level) {

        vm.selected = level;

        $scope.model.value = level.alias;

        vm.levels.forEach(function (l) {
            l.active = l.alias === level.alias;
        });

    }

});