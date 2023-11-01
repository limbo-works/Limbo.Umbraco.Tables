angular.module("umbraco").controller("Limbo.Umbraco.Tables.Editor.Controller", function ($scope, $routeParams, editorService, localizationService) {

	const vm = this;

	if ($scope.model.config.hideLabel === true) $scope.model.hideLabel = true;

	vm.allowUseFirstRowAsHeader = $scope.model.config.allowUseFirstRowAsHeader === true;
	vm.allowUseFirstColumnAsHeader = $scope.model.config.allowUseFirstColumnAsHeader === true;
	vm.allowUseLastRowAsFooter = $scope.model.config.allowUseLastRowAsFooter === true;

	vm.addRow = function () {

		const row = {};

		vm.table.rows.push(row);

		if (vm.table.columns.length === 0) {
			vm.addColumn();
			return;
		}

		vm.addEmptyCells();

	}

	vm.addColumn = function () {

		if (vm.table.columns.length >= 12) {
			return;
		}

		const column = {};

		vm.table.columns.push(column);

		vm.addEmptyCells();

	}

	vm.addEmptyCells = function () {

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

	vm.addNewRows = function (count) {
		for (let i = 0; i < count; i++) {
			const rows = [];
			for (let column = 0; column < vm.table.columns.length; column++) {
				const cell = vm.getEmptyCell((vm.table.rows.length - 1) + i, column);
				rows.push(cell);
			}
			vm.table.cells.push(rows);
		}
	}

	vm.getEmptyCell = function (rowIndex, columnIndex) {
		return {
			rowIndex: rowIndex,
			columnIndex: columnIndex,
			value: "",
			type: "td",
			scope: null
		};
	}

	vm.reIndexCells = function () {
		vm.table.cells.forEach(function (row, rowIndex) {
			row.forEach(function (cell, colIndex) {
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

	vm.removeColumn = function (index) {

		if (vm.table.columns.length === 1) {
			return;
		}

		vm.table.columns.splice(index, 1);
		vm.table.cells.forEach((row) => {
			row.splice(index, 1);
		});

		vm.reIndexCells();

	}

	vm.removeRow = function (index) {

		if (vm.table.rows.length === 1) {
			return;
		}

		vm.table.rows.splice(index, 1);
		vm.table.cells.splice(index, 1);
		vm.reIndexCells();

	}

	vm.editCell = function (cell) {

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
					editor: $scope.model.config.rte
				},
				value: cell.value
			},
			submit: function (model) {
				cell.value = model.prop.value;
				editorService.close();
			},
			close: function () {
				editorService.close();
			}
		};

		editorService.open(o);

		localizationService.localize("limboTables_editCellContent").then(function (value) {
			o.title = value;
		});

	}

	vm.getCssClass = function () {
		return "";
	}

	vm.getTableClas = function () {
		return "";
	}

	vm.getRowClass = function (rowIndex) {
		return "";
	}

	vm.getColumnClass = function (cell) {
		return "";
	}

	vm.showRowAndColumnSettings = function () {
		return "";
	}


	function loadTable() {
		if (!$scope.model.value || !($scope.model.value instanceof Object)) return;
		vm.table = $scope.model.value;
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

	function init() {
		initTable();
		if ($routeParams.id !== "-1") loadTable();
		$scope.$on("formSubmitting", save);
	}

	init();

});