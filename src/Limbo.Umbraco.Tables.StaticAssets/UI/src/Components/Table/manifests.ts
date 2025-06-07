import type {ManifestPropertyEditorSchema, ManifestPropertyEditorUi} from "@umbraco-cms/backoffice/dist-cms/packages/core/property-editor";
import type {ManifestBase} from "@umbraco-cms/backoffice/dist-cms/libs/extension-api/types/manifest-base.interface";
export const styledTextSchema : ManifestPropertyEditorSchema = {
    type: 'propertyEditorSchema',
    name: 'Styled textbox',
    alias: 'styled.textbox',
    meta: {
        defaultPropertyEditorUiAlias: 'styled.textbox.ui',
        settings: {
            properties: [
                {
                    alias: 'styleValue',
                    label: 'Styles',
                    description: 'Styles to apply to the box',
                    propertyEditorUiAlias: 'Umb.PropertyEditorUi.TextArea'
                }
            ],
            defaultData: [
                {
                    alias: 'styleValue',
                    value: 'font-size: 20px;\r\nborder:none; border-bottom: 1px solid #444;'
                }
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
        "propertyEditorSchemaAlias": "Umbraco.Plain.Json"
    }
}

export const manifests:ManifestBase[] = [styledTextSchema, styledTextUi];