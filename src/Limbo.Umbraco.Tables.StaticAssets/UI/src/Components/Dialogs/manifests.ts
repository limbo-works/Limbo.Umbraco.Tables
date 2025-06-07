import type { ManifestModal } from '@umbraco-cms/backoffice/modal';
import type {ManifestBase} from "@umbraco-cms/backoffice/dist-cms/libs/extension-api/types/manifest-base.interface";

const modals: Array<ManifestModal> = [
    {
        type: 'modal',
        alias: 'limbo.table.modal',
        name: 'Limbo Table modal',
        js: () => import('./limbo-table-modal-element.ts')
    }
];

export const manifests:ManifestBase[] = [...modals];