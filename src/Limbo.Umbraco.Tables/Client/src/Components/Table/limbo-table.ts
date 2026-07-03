import { customElement, LitElement, html, property, state, unsafeCSS, unsafeHTML, when, repeat } from '@umbraco-cms/backoffice/external/lit';
import styleString from './Styles.less?inline';
import { UmbChangeEvent } from '@umbraco-cms/backoffice/event';
import type {Table, Cell, Row} from "../../models/table.ts";

import {UMB_MODAL_MANAGER_CONTEXT, type UmbModalManagerContext} from '@umbraco-cms/backoffice/modal';
import { UmbElementMixin } from '@umbraco-cms/backoffice/element-api'
import {LIMBO_TABLE_MODAL} from "../Dialogs/RteModalValue.ts";
import type {UmbPropertyEditorConfigCollection, UmbPropertyEditorUiElement} from "@umbraco-cms/backoffice/property-editor";

function clone<T>(value: T): T {
    return JSON.parse(JSON.stringify(value));
}

@customElement('limbo-table')
export class LimboTable extends UmbElementMixin(LitElement) implements UmbPropertyEditorUiElement {

    private _modalContext?: UmbModalManagerContext;

    @state()
    private allowUseFirstRowAsHeader?: boolean;

    @state()
    private allowUseFirstColumnAsHeader?: boolean;

    @state()
    private allowUseLastRowAsFooter?: boolean;

    private _rteExtensions?: string[];
    private _rteToolbar?: string[][][];
    private _useTextareaEditor?: boolean;

    @state()
    private table: Table = {
        useFirstRowAsHeader: false,
        useFirstColumnAsHeader: false,
        useLastRowAsFooter: false,
        columns: [],
        rows: [],
        cells: undefined
    };

    @property({ attribute: false })
    public set config(config: UmbPropertyEditorConfigCollection) {
        this.allowUseFirstRowAsHeader = config.getValueByAlias("allowUseFirstRowAsHeader") ?? false;
        this.allowUseFirstColumnAsHeader = config.getValueByAlias("allowUseFirstColumnAsHeader") ?? false;
        this.allowUseLastRowAsFooter = config.getValueByAlias("allowUseLastRowAsFooter") ?? false;
        this._rteExtensions = config.getValueByAlias("extensions") as string[] | undefined;
        this._rteToolbar = config.getValueByAlias("toolbar") as string[][][] | undefined;
        this._useTextareaEditor = config.getValueByAlias("useTextareaEditor") as boolean | undefined;
    }

    @property({ attribute: false })
    value: undefined | Table;


    static readonly styles = [unsafeCSS(styleString)];

    constructor() {
        super();
        this.consumeContext(UMB_MODAL_MANAGER_CONTEXT, (instance) => {
            this._modalContext = instance;
        });
    }

    override async firstUpdated() {

        await this.updateComplete;

        if (!this.value) return;

        try {

            let modified: Boolean = false;

            const parsed = clone(this.value) as Table;

            if (parsed.cells) {
                parsed.rows.forEach(function (row, r) {
                    row.cells = parsed.cells ? parsed.cells[r] : [];
                });
                delete parsed.cells;
                modified = true;
            }

            parsed.columns.forEach(function (column) {
                if (!column.id) {
                    column.id = crypto.randomUUID();
                    modified = true;
                }
            });

            parsed.rows.forEach(function (row) {
                if (!row.id) {
                    row.id = crypto.randomUUID();
                    modified = true;
                }
            });

            if (modified) this.updateUi();

            this.table = parsed;

        } catch (e) {
            console.error("Failed to parse table value:", e);
        }

    }

    #shouldShowToolbar() {
        return this.allowUseFirstRowAsHeader || this.allowUseFirstColumnAsHeader || this.allowUseLastRowAsFooter;
    }

    #renderToolbar() {
        if (!this.#shouldShowToolbar()) return html``;
        return html`
            <div class="toolbar">
                <div class="toolbar__buttons is-fullwidth">
                    ${when(this.allowUseFirstRowAsHeader, () => html`
                          <div >
                            <uui-toggle pristine="" label="label" .checked="${this.table.useFirstRowAsHeader}" @change="${this.switchedUseFirstRowAsHeader}">
                              <umb-localize key="limboTables_useFirstRowAsHeader">Use first row as header</umb-localize>
                            </uui-toggle>
                          </div>
                    `)}
                    ${when(this.allowUseFirstColumnAsHeader, () => html`
                        <div>
                            <uui-toggle pristine="" label="label" .checked="${this.table.useFirstColumnAsHeader}" @change="${this.switchedUseFirstColumnAsHeader}">
                                <umb-localize key="limboTables_useFirstColumnAsHeader">Use first column as header</umb-localize>
                            </uui-toggle>
                        </div>
                    `)}
                    ${when(this.allowUseLastRowAsFooter, () => html`
                          <div>
                            <uui-toggle pristine="" label="label" .checked="${this.table.useLastRowAsFooter}" @change="${this.switchedAllowUseLastRowAsFooter}">
                              <umb-localize key="limboTables_useLastRowAsFooter">Use last row as footer</umb-localize>
                            </uui-toggle>
                          </div>
                    `)}
                </div>
            </div>
        `;
    }

    #renderActions() {
        return html`
            <div class="toolbar">
                <div class="toolbar__buttons is-fullwidth">
                    <uui-button pristine="" label="${this.localize.term("limboTables_addRow")}" look="secondary" @click="${this.addRow}">
                        <uui-icon name="icon-add" ></uui-icon>
                        ${this.localize.term("limboTables_addRow")}
                    </uui-button>
                    <uui-button pristine="" label="${this.localize.term("limboTables_addColumn")}" look="secondary" @click="${this.addColumn}">
                        <uui-icon name="icon-add"></uui-icon>
                        ${this.localize.term("limboTables_addColumn")}
                    </uui-button>
                    <uui-button pristine="" label="Reset" look="secondary" @click="${this.reset}">
                        <uui-icon name="icon-add"></uui-icon>
                        Reset
                    </uui-button>
                </div>
            </div>
        `;
    }

    render() {
        return html`
            <div class="limbo-tables">
                <div class="table-editor">
                    <div class="umb-scrollable row-fluid">
                        <div class="editor">
                            ${this.#renderToolbar()}
                            ${this.#renderActions()}
                            <div class="table">
                                <div class="controls">
                                    ${this.table.columns.length == 1 ? html`
                                        <uui-button pristine="" label="Delete" look="secondary"  disabled="true">
                                            <uui-icon name="icon-trash"></uui-icon>
                                        </uui-button>` : html`
                                        ${Object.entries(this.table.columns).map(([key, _]) => {
                                            return html`
                                                <div class="controls__control" ng-repeat="column in vm.table.columns">
                                                    <uui-button pristine="" label="Delete" look="secondary"  @click="${() => this.removeColumn(parseInt(key))}">
                                                      <uui-icon name="icon-trash"></uui-icon>
                                                    </uui-button>
                                                </div>
                                            `
                                        })}
                                    `}
                                    <div class="controls__control no-opacity">
                                        <uui-button pristine="" label="Move" look="secondary">
                                            <uui-icon name="icon-navigation"></uui-icon>
                                        </uui-button>
                                        <uui-button pristine="" label="Delete" look="secondary">
                                            <uui-icon name="icon-trash"></uui-icon>
                                        </uui-button>
                                    </div>
                                </div>
                                <div ui-sortable="sortableOptions" class="table-element ${this.getTableClass()}">
                                    ${repeat(this.table.rows, (_, index) => index, (row, index) => html`
                                        <div class="table__row--wrapper" >
                                            <div class="table__row ${this.getRowClass(index)}">
                                                ${repeat(row.cells, (_, cellIndex) => cellIndex, (cell) => html`
                                                    <div class="table__column ${this.getColumnClass(cell)}" @click="${() => this.editCell(cell)}">
                                                    ${when(cell.value?.length == 0, () => html`
                                                        <div class="table__column--placeholder" >
                                                            <div>
                                                                <p>
                                                                    <umb-localize key="limboTables_addContent">Add content</umb-localize>
                                                                </p>
                                                            </div>
                                                        </div>
                                                    `, () => html`
                                                        <div class="table__column--content">
                                                            ${unsafeHTML(cell.value)}
                                                        </div>
                                                    `)}
                                                    </div>
                                                `)}
                                            </div>
                                            <div class="buttons">
                                                <uui-button pristine="" label="Move" look="secondary" >
                                                    <uui-icon name="icon-navigation"></uui-icon>
                                                </uui-button>
                                                <uui-button pristine="" label="Delete row" look="secondary" @click="${() => this.removeRow(index)}">
                                                    <uui-icon name="icon-trash"></uui-icon>
                                                </uui-button>
                                            </div>
                                        </div>
                                    `)}
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        `;
    }

    reset() {
        this.table = {
            useFirstRowAsHeader: false,
            useFirstColumnAsHeader: false,
            useLastRowAsFooter: false,
            columns: [],
            rows: [],
            cells: undefined
        };
        this.updateUi();
    }

    //on change callbacks
    switchedUseFirstRowAsHeader() {
        this.table.useFirstRowAsHeader = !this.table.useFirstRowAsHeader;
        this.reIndexCells();
        this.updateUi();
    }

    switchedUseFirstColumnAsHeader() {
        this.table.useFirstColumnAsHeader = !this.table.useFirstColumnAsHeader;
        this.reIndexCells();
        this.updateUi();
    }

    switchedAllowUseLastRowAsFooter() {
        this.table.useLastRowAsFooter = !this.table.useLastRowAsFooter;
        this.reIndexCells();
        this.updateUi();
    }

    //helpers
    addRow() {
        const row: Row = {
            id: crypto.randomUUID(),
            cells: []
        };
        this.table.rows.push(row);
        if (this.table.columns.length === 0) {
            this.addColumnAction();
            this.updateUi();
            return;
        }
        this.addEmptyCells();
        this.updateUi();
    }

    addColumn() {
        this.addColumnAction();
        this.updateUi();
    }

    addColumnAction() {
        this.table.columns.push({id: crypto.randomUUID()});
        this.addEmptyCells();
    }

    getEmptyCell(rowIndex: number, columnIndex: number) {
        return {
            rowIndex: rowIndex,
            columnIndex: columnIndex,
            value: "",
            type: "td",
            scope: null
        };
    }

    addEmptyCells() {

        if (this.table.rows.length === 0) {
            this.addNewRows(1);
            return;
        }

        this.table.rows.forEach((row: Row, index: number) => {
            const diffColumns = this.table.columns.length - row.cells.length;
            if (diffColumns < 0) {
                row.cells.splice(row.cells.length - diffColumns, diffColumns);
            } else if (diffColumns > 0) {
                for (let x = 0; x < diffColumns; x++) {
                    row.cells.push(this.getEmptyCell(index, (this.table.columns.length - 1) + x));
                }
            }
        });

    }

    addNewRows(count: number) {
        for (let i = 0; i < count; i++) {
            const row : Row = { id: crypto.randomUUID(), cells : []};
            for (let column = 0; column < this.table.columns.length; column++) {
                const cell = this.getEmptyCell((this.table.rows.length - 1) + i, column);
                row.cells.push(cell);
            }
            this.table.rows.push(row);
        }
        this.updateUi()
    }

    reIndexCells() {
        const table = this.table;
        table.rows.forEach(function (row:Row, rowIndex:number) {
            row.cells.forEach(function (cell:Cell, colIndex:number) {
                if (table.useFirstRowAsHeader && table.useFirstColumnAsHeader) {
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

    removeColumn(index: number) {
        if (this.table.columns.length === 1) return;
        this.table.columns.splice(index, 1);
        this.table.rows.forEach((row: Row) => {
            row.cells.splice(index, 1);
        });
        this.reIndexCells();
        this.updateUi()
    }

    removeRow(index:number) {
        if (this.table.rows.length === 1) return;
        this.table.rows.splice(index, 1);
        this.reIndexCells();
        this.updateUi();
    }

    updateUi() {
        this.#dispatchChangeEvent();
        this.requestUpdate();
    }

    async editCell(cell: Cell) {
        const customContext = this._modalContext?.open(this, LIMBO_TABLE_MODAL, {
            data: {
                headline: "Edit cell",
                content: cell.value ?? "",
                rteExtensions: this._rteExtensions,
                rteToolbar: this._rteToolbar,
                useTextareaEditor: this._useTextareaEditor
            }
        });
        const data = await customContext?.onSubmit();
        if (!data) return;
        cell.value = data.content;
        this.updateUi()
    }

    getTableClass() {
        return "";
    }

    getRowClass(rowIndex: number) {
        if (this.table.useFirstRowAsHeader && rowIndex === 0) return "header";
        if (this.table.useLastRowAsFooter && rowIndex === this.table.rows.length - 1) return "footer";
        return "";
    }

    getColumnClass(cell: Cell) {
        if (this.table.useFirstColumnAsHeader && cell.columnIndex === 0) return "header";
        return "";
    }

    #dispatchChangeEvent() {
        this.value = JSON.parse(JSON.stringify(this.table)) as Table;
        this.dispatchEvent(new UmbChangeEvent());
    }

}

declare global {
    interface HTMLElementTagNameMap {
        "limbo-table": LimboTable
    }
}

export default LimboTable;