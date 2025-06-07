import { UmbModalToken } from "@umbraco-cms/backoffice/modal";

export interface RteModalData {
    headline: string;
    content: string;
}
export interface RteModalValue {
    content: string
}


export const LIMBO_TABLE_MODAL = new UmbModalToken<RteModalData, RteModalValue>(
    "limbo.table.modal",
    {
        modal: {
            type: 'sidebar',
            size: 'medium'
        }
    }
);