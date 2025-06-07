import type {ManifestPropertyEditorSchema, ManifestPropertyEditorUi} from "@umbraco-cms/backoffice/dist-cms/packages/core/property-editor";
import type {ManifestBase} from "@umbraco-cms/backoffice/dist-cms/libs/extension-api/types/manifest-base.interface";
export const styledTextSchema : ManifestPropertyEditorSchema = {
    type: 'propertyEditorSchema',
    name: 'Limbo Table',
    alias: 'limbo.table',
    meta: {
        defaultPropertyEditorUiAlias: 'limbo.tables.propertyEditor',
        settings: {
            properties: [
              
                {
                    alias: 'allowUseFirstRowAsHeader',
                    label: '#allowUseFirstRowAsHeader',
                    description: '{#allowUseFirstColumnAsHeader_description}',
                    propertyEditorUiAlias: 'Umb.PropertyEditorUi.Toggle',
             
                },
                {
                    alias: 'allowUseFirstColumnAsHeader',
                    label: '#allowUseFirstColumnAsHeader',
                    description: '{#allowUseFirstColumnAsHeader_description}',
                    propertyEditorUiAlias: 'Umb.PropertyEditorUi.Toggle',
                    weight: 6,
                },
                {
                    alias: 'allowUseLastRowAsFooter',
                    label: '#rte_config_allowUseLastRowAsFooter',
                    description: '{#allowUseLastRowAsFooter_description}',
                    propertyEditorUiAlias: 'Umb.PropertyEditorUi.Toggle',
                    weight: 7,
                },
            ],
            defaultData: [
              
                { alias: 'allowUseFirstRowAsHeader', value: true },
                { alias: 'allowUseFirstColumnAsHeader', value: true },
                { alias: 'allowUseLastRowAsFooter', value: true },
            ]
        }
    }
};

const styledTextUi : ManifestPropertyEditorUi = {
    type: 'propertyEditorUi',
    alias: 'limbo.tables.propertyEditor',
    name: 'Limbo Table Property Editor',
    js: () => import('./limbo-table.ts'),
    elementName: 'limbo-table',
    meta: {
        "label": "Table",
        "icon": "icon-grid color-limbo",
        "group": "common",
        "propertyEditorSchemaAlias": 'limbo.table'
    }
}

export const manifests:ManifestBase[] = [styledTextSchema, styledTextUi];