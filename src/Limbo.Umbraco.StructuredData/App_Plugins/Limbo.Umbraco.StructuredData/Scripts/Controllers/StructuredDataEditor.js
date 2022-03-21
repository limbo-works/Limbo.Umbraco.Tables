function tablesEditorController($scope, $routeParams, editorService) {
	var vm = this;

	function _addRow() {
		var row = {
			//backgroundColor: 'none'
		};

		vm.table.rows.push(row);

		if (vm.table.columns.length === 0) {
			_addColumn();
			return;
		}

		_addEmptyCells();
	}

	function _addColumn() {
		if (vm.table.columns.length >= 12) {
			return;
		}

		var column = {
			//backgroundColor: 'none'
		};

		vm.table.columns.push(column);
		_addEmptyCells();
	}

	function _addEmptyCells() {
		if (vm.table.cells.length === 0) {
			_addNewRows(vm.table.rows.length);
		}
		else {
			// get column difference 
			var firstCell = vm.table.cells[0];
			var diffColumns = vm.table.columns.length - firstCell.length;

			if (diffColumns < 0) {
				// remove columns
				vm.table.cells.forEach((row) => {
					row.splice(row.length - diffColumns, diffColumns);
				});
			}
			else if (diffColumns > 0) {
				// add columns
				vm.table.cells.forEach((row, index) => {
					for (var x = 0; x < diffColumns; x++) {
						row.push(_getEmptyCell(index, (vm.table.columns.length - 1) + x));
					}
				});
			}

			// get row difference
			var diffRows = vm.table.rows.length - vm.table.cells.length;

			if (diffRows < 0) {
				// remove rows
				vm.table.cells.splice(diffRows, diffRows);
			}
			else if (diffRows > 0) {
				_addNewRows(diffRows);
			}
		}

	}

	function _addNewRows(count) {
		for (var i = 0; i < count; i++) {
			var rows = [];

			for (var column = 0; column < vm.table.columns.length; column++) {
				var cell = _getEmptyCell((vm.table.rows.length - 1) + i, column);
				rows.push(cell);
			}

			vm.table.cells.push(rows);
		}
	}

	function _getEmptyCell(rowIndex, columnIndex) {
		return {
			rowIndex: rowIndex,
			columnIndex: columnIndex,
			value: '',
			type: 'td',
			scope: null
		};
	}

	function _reIndexCells() {
		vm.table.cells.forEach(function (row, rowIndex) {
			row.forEach(function (cell, colIndex) {
				if (vm.table.useFirstRowAsHeader && vm.table.useFirstColumnAsHeader) {
					if (rowIndex == 0 && colIndex != 0) {
						cell.type = 'th';
						cell.scope = 'col';
					}
					if (rowIndex != 0 && colIndex == 0) {
						cell.type = 'th';
						cell.scope = 'row';
					}
				}
				else if (vm.table.useFirstRowAsHeader && rowIndex == 0) {
					cell.type = 'th';
					cell.scope = 'col';
				}
				else if (vm.table.useFirstColumnAsHeader && colIndex == 0) {
					cell.type = 'th';
					cell.scope = 'row';
				}
				else {
					cell.type = 'td';
					cell.scope = null;
				}

				cell.columnIndex = colIndex;
				cell.rowIndex = rowIndex;
			});
		});
	}

	function _removeColumn(index) {
		if (vm.table.columns.length === 1) {
			return;
		}

		vm.table.columns.splice(index, 1);
		vm.table.cells.forEach((row) => {
			row.splice(index, 1);
		});
		_reIndexCells();
	}

	function _removeRow(index) {
		if (vm.table.rows.length === 1) {
			return;
		}

		vm.table.rows.splice(index, 1);
		vm.table.cells.splice(index, 1);
		_reIndexCells();
	}

	function _editCell(cell) {
		editorService.open( {
			view: "/App_Plugins/Limbo.Umbraco.StructuredData/Views/StructuredDataOverlay.html",
			show: true,
			title: "Edit cell value",
			size: "medium",
			prop: {
				alias: "value",
				label: "",
				view: "rte",
				config: {
					editor: {
						toolbar: [
							"ace",
							//	"removeformat",
							//"styleselect",
							"bold",
							"italic",
							//	"underline",
							//	"strikethrough",
							"alignleft",
							"aligncenter",
							"alignright",
							//	"alignjustify",
							"bullist",
							"numlist",
							"outdent",
							"indent",
							"link",
							"unlink",
							//"umbmediapicker",
							//	"umbmacro",
							//	"umbembeddialog",
							//	"hr",
							//	"subscript",
							//	"superscript",
							//	"charmap",
							//	"rtl",
							//	"ltr"
						],
						dimensions: {
							height: 500,
							width: 0
						}
					}
				},
				value: cell.value
			},
			submit: function (model) {
				cell.value = model.prop.value;
				editorService.close();
			},
			close: function (model) {
				editorService.close();
			}
		});
	}

	function _getCssClass(backgroundColour) {
		return '';
	}

	function _getTableClass() {
		return '';
	}

	function _getRowClass(rowIndex) {
		return '';
	}

	function _getColumnClass(cell) {
		return '';
	}

	function _loadTable() {

		if ($scope.model.value && $scope.model.value instanceof Object) {
			console.log($scope.model.value);
			vm.table = $scope.model.value;
		}
	}

	function _save() {
		console.log('saving', vm.table);
		_reIndexCells();

		//save
		$scope.model.value = vm.table;
	}

	function _showRowAndColumnSettings() {
		return '';
	}

	function _initTable() {
		vm.table = {
			rows: [],
			columns: [],
			cells: [],
			useFirstRowAsHeader: false,
			useFirstColumnAsHeader: false
		};
		_addRow();
	}

	function _init() {
		_initTable();

		if ($routeParams.id !== "-1") {
			_loadTable();
		}

		$scope.addRow = _addRow;
		$scope.addColumn = _addColumn;
		$scope.removeColumn = _removeColumn;
		$scope.removeRow = _removeRow;
		$scope.editCell = _editCell;
		$scope.reIndexCells = _reIndexCells;
		$scope.$on("formSubmitting", _save);
		$scope.getColumnClass = _getColumnClass;
		$scope.getRowClass = _getRowClass;
		$scope.showRowAndColumnSettings = _showRowAndColumnSettings;
		$scope.getTableClass = _getTableClass;
	}

	_init();
}

angular.module("umbraco").controller("Limbo.Umbraco.StructuredData.Editor.Controller", tablesEditorController);