angular.module("umbraco").controller("Limbo.Umbraco.Tables.Editor.Controller", 
    function ($scope, $routeParams, editorService, localizationService) {

        const vm = this;

        //editRawValue capabilities adapted from https://github.com/leekelleher/umbraco-contentment/blob/dev/v4.x/src/Umbraco.Community.Contentment/DataEditors/Notes/notes.js
        // & from https://github.com/leekelleher/umbraco-contentment/blob/dev/v4.x/src/Umbraco.Community.Contentment/DataEditors/_/_dev-mode.js */

        
        if ($scope.model.config.hideLabel === true) $scope.model.hideLabel = true;

        vm.allowUseFirstRowAsHeader = $scope.model.config.allowUseFirstRowAsHeader === true;
        vm.allowUseFirstColumnAsHeader = $scope.model.config.allowUseFirstColumnAsHeader === true;
        vm.allowUseLastRowAsFooter = $scope.model.config.allowUseLastRowAsFooter === true;

        vm.addRow = function() {

            const row = {};

            vm.table.rows.push(row);

            if (vm.table.columns.length === 0) {
                vm.addColumn();
                return;
            }

            vm.addEmptyCells();

        }

        vm.addColumn = function() {

            if (vm.table.columns.length >= 12) {
                return;
            }

            const column = {};

            vm.table.columns.push(column);

            vm.addEmptyCells();

        }

        vm.addEmptyCells = function() {

            if (vm.table.cells.length === 0) {
                vm.addNewRows(vm.table.rows.length);
                return;
            }

            // Get column difference
            const firstCell = vm.table.cells[0];
            const diffColumns = vm.table.columns.length - firstCell.length;

            if (diffColumns < 0) {
                // Remove columns
                vm.table.cells.forEach((row) => {
                    row.splice(row.length - diffColumns, diffColumns);
                });
            } else if (diffColumns > 0) {
                // Add columns
                vm.table.cells.forEach((row, index) => {
                    for (let x = 0; x < diffColumns; x++) {
                        row.push(vm.getEmptyCell(index, (vm.table.columns.length - 1) + x));
                    }
                });
            }

            // Get row difference
            const diffRows = vm.table.rows.length - vm.table.cells.length;

            if (diffRows < 0) {
                // Remove rows
                vm.table.cells.splice(diffRows, diffRows);
            } else if (diffRows > 0) {
                vm.addNewRows(diffRows);
            }

        }

        vm.addNewRows = function(count) {
            for (let i = 0; i < count; i++) {
                const rows = [];
                for (let column = 0; column < vm.table.columns.length; column++) {
                    const cell = vm.getEmptyCell((vm.table.rows.length - 1) + i, column);
                    rows.push(cell);
                }
                vm.table.cells.push(rows);
            }
        }

        vm.getEmptyCell = function(rowIndex, columnIndex) {
            return {
                rowIndex: rowIndex,
                columnIndex: columnIndex,
                value: "",
                type: "td",
                scope: null
            };
        }

        vm.reIndexCells = function() {
            vm.table.cells.forEach(function(row, rowIndex) {
                row.forEach(function(cell, colIndex) {
                    if (vm.table.useFirstRowAsHeader && vm.table.useFirstColumnAsHeader) {
                        if (rowIndex === 0 && colIndex !== 0) {
                            cell.type = "th";
                            cell.scope = "col";
                        }
                        if (rowIndex !== 0 && colIndex === 0) {
                            cell.type = "th";
                            cell.scope = "row";
                        }
                    } else if (vm.table.useFirstRowAsHeader && rowIndex === 0) {
                        cell.type = "th";
                        cell.scope = "col";
                    } else if (vm.table.useFirstColumnAsHeader && colIndex === 0) {
                        cell.type = "th";
                        cell.scope = "row";
                    } else {
                        cell.type = "td";
                        cell.scope = null;
                    }
                    cell.columnIndex = colIndex;
                    cell.rowIndex = rowIndex;
                });
            });
        }

        vm.removeColumn = function(index) {

            if (vm.table.columns.length === 1) {
                return;
            }

            vm.table.columns.splice(index, 1);
            vm.table.cells.forEach((row) => {
                row.splice(index, 1);
            });

            vm.reIndexCells();

        }

        vm.removeRow = function(index) {

            if (vm.table.rows.length === 1) {
                return;
            }

            vm.table.rows.splice(index, 1);
            vm.table.cells.splice(index, 1);
            vm.reIndexCells();

        }

        vm.editCell = function(cell) {

            const o = {
                view: "/App_Plugins/Limbo.Umbraco.Tables/Views/TableOverlay.html",
                show: true,
                title: "Edit cell content",
                size: "medium",
                prop: {
                    alias: "value",
                    label: "",
                    view: "rte",
                    config: {
                        editor: $scope.model.config.rte,
                        overlaySize: $scope.model.config.overlaySize
                    },
                    value: cell.value
                },
                submit: function(model) {
                    cell.value = model.prop.value?.markup ? model.prop.value.markup : model.prop.value;
                    editorService.close();
                },
                close: function() {
                    editorService.close();
                }
            };

            editorService.open(o);

            localizationService.localize("limboTables_editCellContent").then(function(value) {
                o.title = value;
            });

        }

        vm.getCssClass = function() {
            return "";
        }

        vm.getTableClas = function() {
            return "";
        }

        vm.getRowClass = function(rowIndex) {
            return "";
        }

        vm.getColumnClass = function(cell) {
            return "";
        }

        vm.showRowAndColumnSettings = function() {
            return "";
        }


        function loadTable() {

            if (!$scope.model.value || !($scope.model.value instanceof Object)) return;

            const t = $scope.model.value;

            // If the current model doesn't contain a "cells" array or the array is empty, it means
            // that we have an invalid model, which means we can stop further parsing of the model
            if (!Array.isArray(t.cells) || t.cells.length === 0) {
                vm.table = null;
                return;
            }

            // If the "rows" array is missing, we can create it from the "cells" array. The "rows"
            // array would for instance be missing if the value originates from the "Imulus.TableEditor",
            // in which the data format is slightly different than this package.
            let rows;
            if (Array.isArray(t.rows)) {
                rows = t.rows;
            } else {
                rows = [];
                for (let i = 0; i < t.cells.length; i++) {
                    rows.push({});
                }
            }

            // Similar to the "rows" array, we need to create the "columns" array is missing in the
            // current table model
            let columns;
            if (Array.isArray(t.columns)) {
                columns = t.columns;
            } else {
                columns = [];
                if (rows.length > 0) {
                    for (let i = 0; i < t.cells[0].length; i++) {
                        columns.push({});
                    }
                }
            }

            // Initialize the overall table object used by the property editor
            vm.table = {
                useFirstRowAsHeader: t.useFirstRowAsHeader === true,
                useFirstColumnAsHeader: t.useFirstColumnAsHeader === true,
                useLastRowAsFooter: t.useLastRowAsFooter === true,
                columns,
                rows,
                cells: t.cells
            };

        }

        function save() {

            // Re-index all the cells
            vm.reIndexCells();

            // Determine whether all cells are currently empty
            let allEmpty = true;
            for (let i = 0; i < vm.table.cells.length; i++) {
                if (!allEmpty) break;
                for (let j = 0; j < vm.table.cells[i].length; j++) {
                    if (vm.table.cells[i][j].value) {
                        allEmpty = false;
                        break;
                    }
                }
            }

            // Persist the table model back to the property value, but not if all cells are empty
            $scope.model.value = allEmpty ? "" : vm.table;

        }

        function initTable() {
            vm.table = {
                rows: [],
                columns: [],
                cells: [],
                useFirstRowAsHeader: false,
                useFirstColumnAsHeader: false
            };
            vm.addRow();
        }

        function editRawTableData() {
            const o = {
                view: "/App_Plugins/Limbo.Umbraco.Tables/Views/_json-editor.html",
                show: true,
                title: "Edit raw value",
                size: "medium",
                value: Utilities.toJson($scope.model.value, true),
                ace: {
                    showGutter: true,
                    useWrapMode: true,
                    useSoftTabs: true,
                    theme: "chrome",
                    mode: "javascript",
                    advanced: {
                        fontSize: "14px",
                        wrap: true
                    }
                    //onLoad: function (_editor) {
                    //    $timeout(() => _editor.focus());
                    //}
                },
                submit: function (value) {

                    if (value=="") 
                    {
                        $scope.model.value = value;
                        initTable();
                    } else {
                        $scope.model.value = Utilities.fromJson(value);
                        loadTable();
                    }

                    editorService.close();
                    
                },
                
                close: function() {
                    editorService.close();
                }
            };

            editorService.open(o);

            localizationService.localize("limboTables_editRawValue").then(function(value) {
                o.title = value;
            });
        };

        function init() {
            initTable();
            if ($routeParams.id !== "-1") loadTable();


            if ($scope.model.config.enableDevMode === true && $scope.umbProperty) {
                $scope.umbProperty.setPropertyActions([
                    //{
                    //    labelKey: "limboTables_editRawValue",
                    //    icon: "brackets",
                    //    method: () => devModeService.editValue($scope.model, setDirty)
                    //}
                    {
                        labelKey: "limboTables_editRawValue",
                        icon: "brackets",
                        method: () => editRawTableData()
                    }
                ]);

                $scope.$on("formSubmitting", save);
            }

            //init();
        }


        init();
    }
);