
// load up the manifests here.
import { manifests as tableManifest } from './Components/Table/manifests.ts';
import { manifests as modalManifest }  from './Components/Dialogs/manifests.ts';
import type {UmbEntryPointOnInit, UmbEntryPointOnUnload} from '@umbraco-cms/backoffice/extension-api';


export const onInit: UmbEntryPointOnInit = (_, extensionRegistry) => {
console.log("I am here");
    // register them here. 
    extensionRegistry.registerMany([
        ...tableManifest,
        ...modalManifest
    ]);

  

};


export const onUnload: UmbEntryPointOnUnload = (_, extensionRegistry) => {
    // Unregister the extension (optional)
    extensionRegistry.unregisterMany([
        ...tableManifest,
        ...modalManifest
    ]);
}