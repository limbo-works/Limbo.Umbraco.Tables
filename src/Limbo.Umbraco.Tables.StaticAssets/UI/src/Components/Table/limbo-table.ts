import { customElement, LitElement, html, css,  property, state } from '@umbraco-cms/backoffice/external/lit';

import style from './Styles.less?inline';
import { UmbChangeEvent } from '@umbraco-cms/backoffice/event';
import type {Table, Cell, Row} from "../../models/table.ts";

import {UMB_MODAL_MANAGER_CONTEXT, UmbModalManagerContext} from '@umbraco-cms/backoffice/modal';
import { UmbElementMixin } from '@umbraco-cms/backoffice/element-api'
import {LIMBO_TABLE_MODAL} from "../Dialogs/RteModalValue.ts";
/**
 * An example element.
 *
 * @slot - This element has a slot
 * @csspart button - The button
 */
@customElement('limbo-table')
// @ts-ignore
export class LimboTable extends UmbElementMixin(LitElement) {
  private _modalContext?: UmbModalManagerContext;
  constructor() {
    super();
    this.consumeContext(UMB_MODAL_MANAGER_CONTEXT, (_instance) => {
      this._modalContext = _instance;
    });
  }
  /**
   * Copy for the read the docs hint.
   */
  @property()
  docsHint = 'Click on the Vite and Lit logos to learn more'

  /**
   * The number of times the button has been clicked.
   */
  @property({ type: Number })
  count = 0

  @state()
  private table : Table = {
    rows: [],
    columns: [],
    cells: [],
    useFirstRowAsHeader: false,
    useFirstColumnAsHeader: false
  };
  
  
  
  static readonly styles = [style, css`
    .foo {
      color: white;
    }
  `];
  RenderToolBar() {
    return html`
      <div class="toolbar" ng-if="">
      <div class="toolbar__buttons is-fullwidth">
        <div ng-if="vm.allowUseFirstRowAsHeader">
          <input type="checkbox" id="useFirstRowAsHeader" value="${this.table.useFirstRowAsHeader}" @change="vm.reIndexCells()" />
          <label for="useFirstRowAsHeader">
            <localize key="limboTables_useFirstRowAsHeader">Use first row as header</localize>
          </label>
        </div>
        <div ng-if="vm.allowUseFirstColumnAsHeader">
          <input type="checkbox" id="useFirstColumnAsHeader" value="vm.table.useFirstColumnAsHeader" @change="vm.reIndexCells()" />
          <label for="useFirstColumnAsHeader">
            <localize key="limboTables_useFirstColumnAsHeader">Use first column as header</localize>
          </label>
        </div>
        <div ng-if="vm.allowUseLastRowAsFooter">
          <input type="checkbox" id="useLastRowAsFooter" value="vm.table.useLastRowAsFooter" @change="vm.reIndexCells()" />
          <label for="useLastRowAsFooter">
            <localize key="limboTables_useLastRowAsFooter">Use last row as footer</localize>
          </label>
        </div>
      </div>
    </div>
    `
  }
  render() {
    return html`
      <div class="limbo-tables">
        <div class="table-editor">
            <div class="umb-scrollable row-fluid">
                <div class="editor">
                    ${this.ShouldShowToolBar() ?
                      this.RenderToolBar() :
                      html``
                      }
                    
                    <div class="toolbar">
                        <div class="toolbar__buttons is-fullwidth">
                          <uui-button pristine="" label="Add row" look="secondary" @click="${this.addRow}">
                            <uui-icon name="add" ></uui-icon> Add row
                          </uui-button>
                          <uui-button pristine="" label="Add column" look="secondary" @click="${this.addColumn}">
                            <uui-icon name="add"></uui-icon> Add column
                          </uui-button>
                           
                        </div>
                    </div>
                    <div class="table">
                        <div class="controls">
                          ${this.table.columns.length == 1 ? 
                              html`
                                <uui-button pristine="" label="Delete" look="secondary"  disabled="true">
                                  <uui-icon name="trash"></uui-icon> Delete
                                </uui-button>` : 
                            html`
                                 ${Object.entries(this.table.columns).map( ([key, _]) =>
                                  {
                                    
                                    return html`
                                        <div class="controls__control" ng-repeat="column in vm.table.columns">
                                          <uui-button pristine="" label="Delete" look="secondary"  @click="${()=>this.removeColumn(parseInt(key))}">
                                            <uui-icon name="trash"></uui-icon> Delete
                                          </uui-button>
                                          
                                        </div>
                                      `}
                            
                                 )}
                            `}
                            <div class="controls__control no-opacity">
                              <uui-button pristine="" label="Move" look="secondary"  >
                                <uui-icon name="navigation"></uui-icon>
                              </uui-button>
                              <uui-button pristine="" label="Delete" look="secondary"  >
                                <uui-icon name="trash"></uui-icon>
                              </uui-button>
                              
                            </div>
                        </div>
                        <div ui-sortable="sortableOptions" class="table-element" class="${this.getTableClass()}">
                          ${JSON.stringify(this.table)}
                             ${Object.entries(this.table.cells).map( ([key, val]) => 
                                {
                                  return html` test
                                    <div class="table__row--wrapper" >
                                        <div class="table__row" ng-class="vm.getRowClass(${key})">
                                            ${Object.entries(val.cells).map( ([_, cell]) =>
                                                {return html`
                                                  <div class="table__column" ng-class="vm.getColumnClass(${cell})" ng-repeat="cell in row; track by $index" @click="${()=>this.editCell(cell)}">
                                                      <div class="table__column--placeholder" ng-if="cell.value.length === 0">
                                                          <div>
                                                              <p>
                                                                  <localize key="limboTables_addContent">Add content</localize>
                                                              </p>
                                                          </div>
                                                      </div>
                                                      <div class="table__column--content">
                                                        ${cell.value}
                                                      </div>
                                                  </div>
                                                `})}
                                        </div>
                                        <div class="buttons">
                                          <uui-button pristine="" label="Move" look="secondary" @click="removeRow($index)">
                                            <uui-icon name="navigation"></uui-icon>
                                          </uui-button>
                                          <uui-button pristine="" label="" look="secondary" @click="removeRow($index)">  
                                            <uui-icon name="trash"></uui-icon>
                                          </uui-button>
                                            
                                        </div>
                                    </div>
                                  `})}
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    `
  }
  addRow() {

    const row = {};

    this.table.rows.push(row);

    if (this.table.columns.length === 0) {
      this.addColumnAction();
      return;
    }

    this.addEmptyCells();

  }
  addColumn() {
    this.addColumnAction();
    this.updateUi();
  }
  addColumnAction() {

    this.table.columns.push({id:Date.now()});
    this.addEmptyCells();

  }
  loadTable= function() {
   // if (!$scope.model.value || !($scope.model.value instanceof Object)) return;
   // this.table = $scope.model.value;
  }
  getEmptyCell(rowIndex:number, columnIndex:number) {
    return {
      rowIndex: rowIndex,
      columnIndex: columnIndex,
      value: "",
      type: "td",
      scope: null
    };
  }
  addEmptyCells () {

    if (this.table.cells.length === 0) {
      this.addNewRows(this.table.rows.length);
      return;
    }

    // Get column difference
    const firstCell = this.table.cells[0];
    const diffColumns = this.table.columns.length - firstCell.cells.length;

    if (diffColumns < 0) {
      // Remove columns
      this.table.cells.forEach((row : Row) => {
        row.cells.splice(row.cells.length - diffColumns, diffColumns);
      });
    } else if (diffColumns > 0) {
      // Add columns
      this.table.cells.forEach((row : Row, index:number) => {
        for (let x = 0; x < diffColumns; x++) {
          row.cells.push(this.getEmptyCell(index, (this.table.columns.length - 1) + x));
        }
      });
    }

    // Get row difference
    const diffRows = this.table.rows.length - this.table.cells.length;

    if (diffRows < 0) {
      // Remove rows
      this.table.cells.splice(diffRows, diffRows);
    } else if (diffRows > 0) {
      this.addNewRows(diffRows);
    }

  }
  addNewRows(count:number) {
    for (let i = 0; i < count; i++) {
      const rows :Row= { cells : []};
      for (let column = 0; column < this.table.columns.length; column++) {
        const cell = this.getEmptyCell((this.table.rows.length - 1) + i, column);
        rows.cells.push(cell);
      }
      this.table.cells.push(rows);
    }

    this.updateUi()
  }
 
  reIndexCells() {
    var table = this.table;
    table.cells.forEach(function (row:Row, rowIndex:number) {
      row.cells.forEach(function (cell:Cell, colIndex:number) {
        if ( table.useFirstRowAsHeader && table.useFirstColumnAsHeader) {
          if (rowIndex === 0 && colIndex !== 0) {
            cell.type = "th";
            cell.scope = "col";
          }
          if (rowIndex !== 0 && colIndex === 0) {
            cell.type = "th";
            cell.scope = "row";
          }
        } else if ( table.useFirstRowAsHeader && rowIndex === 0) {
          cell.type = "th";
          cell.scope = "col";
        } else if ( table.useFirstColumnAsHeader && colIndex === 0) {
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
  removeColumn (index:number) {
    if (this.table.columns.length === 1) {
      return;
    }

    this.table.columns.splice(index, 1);
    this.table.cells.forEach((row : Row) => {
      row.cells.splice(index, 1);
    });

    this.reIndexCells();
    this.updateUi()
  }

 removeRow(index:number) {

    if (this.table.rows.length === 1) {
      return;
    }

   this.table.rows.splice(index, 1);
   this.table.cells.splice(index, 1);
   this.reIndexCells();
   this.updateUi()
  }
  updateUi(){
    this.#dispatchChangeEvent();
    this.requestUpdate();
  }
  async editCell(cell:Cell){
    const customContext = this._modalContext?.open(this, LIMBO_TABLE_MODAL, {
      data: {
        headline: 'A Custom modal',
        content: cell.value ?? ""
      }
    });
    const data = await customContext?.onSubmit();

    if (!data) return;
    console.log(data);
    cell.value = data.content;
    this.updateUi()
  }

  getCssClass () {
    return "";
  }

  getTableClass() {
    return "";
  }

  getRowClass(rowIndex:number) {
    console.log(rowIndex);
    return "";
  }

  getColumnClass(cell:Cell) {
    console.log(cell);
    return "";
  }

  showRowAndColumnSettings() {
    return "";
  }
  #dispatchChangeEvent() {
    this.dispatchEvent(new UmbChangeEvent());
  }
  ShouldShowToolBar = function() {
    //<!-- vm.allowUseFirstRowAsHeader || vm.allowUseFirstColumnAsHeader || vm.allowUseLastRowAsFooter -->
    return true;
  }
}

declare global {
  interface HTMLElementTagNameMap {
    'limbo-table': LimboTable
  }
}
