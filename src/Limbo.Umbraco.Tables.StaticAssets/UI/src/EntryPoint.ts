
import { manifests as tableManifest } from './Components/Table/manifests.ts';
import { manifests as modalManifest }  from './Components/Dialogs/manifests.ts';
import type {UmbEntryPointOnInit, UmbEntryPointOnUnload} from '@umbraco-cms/backoffice/extension-api';

const localizationManifests = [
    {
        type: 'localization' as const,
        alias: 'Limbo.Tables.Localization.EnUs',
        name: 'English (United States)',
        meta: { culture: 'en-us' },
        js: () => import('./Lang/en.ts'),
    },
    {
        type: 'localization' as const,
        alias: 'Limbo.Tables.Localization.DaDk',
        name: 'Danish (Denmark)',
        meta: { culture: 'da-dk' },
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