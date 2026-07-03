export type Table = {
    columns: Column[],
    rows: Row[],
    cells: Cell[][] | undefined,
    useFirstRowAsHeader: boolean,
    useFirstColumnAsHeader: boolean,
    useLastRowAsFooter: boolean,
};

export type Column = {
    id: string
};

export type Row = {
    id: string,
    cells: Cell[],
};

export type Cell = {
    type: string,
    scope: string | null,
    columnIndex: number,
    rowIndex: number
    value?: string,
};