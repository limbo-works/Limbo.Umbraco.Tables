export type Table = {
    rows: object[],
    columns: object[],
    cells: Row[],
    useFirstRowAsHeader: boolean,
    useFirstColumnAsHeader: boolean
};
export type Cell = {
    type: string,
    scope:string | null,
    columnIndex: number,
    rowIndex: number
    value?: string,
};
export type Row = {
    cells: Cell[],
};