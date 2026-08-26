import type {ManifestPropertyEditorSchema, ManifestPropertyEditorUi} from "@umbraco-cms/backoffice/property-editor";
import type { ManifestBase } from "@umbraco-cms/backoffice/extension-api";


const ALIAS = "Limbo.Umbraco.Tables";
const NAME = "Limbo Tables";

const SCHEMA_ALIAS = ALIAS;

const UI_ALIAS = `${SCHEMA_ALIAS}.Ui`;

export const tablesSchema : ManifestPropertyEditorSchema = {
    type: 'propertyEditorSchema',
    alias: SCHEMA_ALIAS,
    name: `${NAME}: Table Property Editor Schema`,
    meta: {
        defaultPropertyEditorUiAlias: UI_ALIAS,
        settings: {
            properties: [
                {
                    alias: 'allowUseFirstRowAsHeader',
                    label: '#limboTables_useFirstRowAsHeader',
                    propertyEditorUiAlias: 'Umb.PropertyEditorUi.Toggle',
                },
                {
                    alias: 'allowUseFirstColumnAsHeader',
                    label: '#limboTables_useFirstColumnAsHeader',
                    propertyEditorUiAlias: 'Umb.PropertyEditorUi.Toggle',
                    weight: 6,
                },
                {
                    alias: 'allowUseLastRowAsFooter',
                    label: '#limboTables_useLastRowAsFooter',
                    propertyEditorUiAlias: 'Umb.PropertyEditorUi.Toggle',
                    weight: 7,
                },
                {
                    alias: 'useTextareaEditor',
                    label: '#limboTables_useTextareaEditor',
                    propertyEditorUiAlias: 'Umb.PropertyEditorUi.Toggle',
                    weight: 8,
                },
                {
                    alias: 'extensions',
                    label: '#tiptap_config_extensions',
                    propertyEditorUiAlias: 'Umb.PropertyEditorUi.Tiptap.ExtensionsConfiguration',
                    weight: 10,
                },
                {
                    alias: 'toolbar',
                    label: '#tiptap_config_toolbar',
                    propertyEditorUiAlias: 'Umb.PropertyEditorUi.Tiptap.ToolbarConfiguration',
                    weight: 15,
                },
            ],
            defaultData: [
                { alias: 'allowUseFirstRowAsHeader', value: true },
                { alias: 'allowUseFirstColumnAsHeader', value: true },
                { alias: 'allowUseLastRowAsFooter', value: true },
                { alias: 'useTextareaEditor', value: false },
                {
                    alias: 'extensions',
                    value: [
                        'Umb.Tiptap.Bold', 'Umb.Tiptap.Italic', 'Umb.Tiptap.Underline', 'Umb.Tiptap.Strike',
                        'Umb.Tiptap.BulletList', 'Umb.Tiptap.OrderedList', 'Umb.Tiptap.Heading', 'Umb.Tiptap.Link',
                    ],
                },
                {
                    alias: 'toolbar',
                    value: [[
                        ['Umb.Tiptap.Toolbar.Undo', 'Umb.Tiptap.Toolbar.Redo'],
                        ['Umb.Tiptap.Toolbar.Bold', 'Umb.Tiptap.Toolbar.Italic', 'Umb.Tiptap.Toolbar.Underline', 'Umb.Tiptap.Toolbar.Strike'],
                        ['Umb.Tiptap.Toolbar.BulletList', 'Umb.Tiptap.Toolbar.OrderedList'],
                        ['Umb.Tiptap.Toolbar.Heading1', 'Umb.Tiptap.Toolbar.Heading2'],
                        ['Umb.Tiptap.Toolbar.Link', 'Umb.Tiptap.Toolbar.Unlink'],
                    ]],
                },
            ]
        }
    }
};

const tablesUi : ManifestPropertyEditorUi = {
    type: "propertyEditorUi",
    alias: UI_ALIAS,
    name: `${NAME}: Table Property Editor UI`,
    js: () => import("./limbo-table.ts"),
    elementName: "limbo-table",
    meta: {
        "label": "Limbo Tables",
        "icon": "icon-grid color-limbo",
        "group": "Limbo",
        "propertyEditorSchemaAlias": SCHEMA_ALIAS
    }
}

export const manifests:ManifestBase[] = [tablesSchema, tablesUi];