
import { manifests as tableManifest } from './Components/Table/manifests.ts';
import { manifests as modalManifest }  from './Components/Dialogs/manifests.ts';
import type {UmbEntryPointOnInit, UmbEntryPointOnUnload} from '@umbraco-cms/backoffice/extension-api';

const ALIAS = "Limbo.Umbraco.Tables";
const NAME = "Limbo Tables";

const localizationManifests = [
    {
        type: 'localization' as const,
        alias: `${ALIAS}.Localization.EnUs`,
        name: `${NAME}: English (en-US)`,
        meta: { culture: 'en' },
        js: () => import('./Lang/en.ts'),
    },
    {
        type: 'localization' as const,
        alias: `${ALIAS}.Localization.DaDk`,
        name: `${NAME}: Danish (da-DK)`,
        meta: { culture: 'da' },
        js: () => import('./Lang/da.ts'),
    },
];

export const onInit: UmbEntryPointOnInit = (_, extensionRegistry) => {
    extensionRegistry.registerMany([
        ...tableManifest,
        ...modalManifest,
        ...localizationManifests,
    ]);
};

export const onUnload: UmbEntryPointOnUnload = (_, extensionRegistry) => {
    extensionRegistry.unregisterMany([
        ...tableManifest,
        ...modalManifest,
        ...localizationManifests,
    ].map(m => m.alias));
}