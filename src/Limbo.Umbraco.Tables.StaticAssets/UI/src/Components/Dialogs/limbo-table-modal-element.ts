import { customElement, html } from "@umbraco-cms/backoffice/external/lit";
import { UmbModalBaseElement } from "@umbraco-cms/backoffice/modal";
import { UmbChangeEvent } from "@umbraco-cms/backoffice/event";
import { UmbPropertyEditorConfigCollection } from "@umbraco-cms/backoffice/property-editor";
import type { RteModalData, RteModalValue } from "./RteModalValue";
import '@umbraco-cms/backoffice/tiptap';

@customElement('limbo-table-modal')
export class LimboTableModal extends UmbModalBaseElement<RteModalData, RteModalValue> {

    #rteConfig!: UmbPropertyEditorConfigCollection;

    override connectedCallback(): void {
        super.connectedCallback();
        this.updateValue({ content: this.data?.content ?? '' });
        this.#rteConfig = new UmbPropertyEditorConfigCollection([
            {
                alias: 'extensions',
                value: this.data?.rteExtensions ?? [
                    'Umb.Tiptap.Bold', 'Umb.Tiptap.Italic', 'Umb.Tiptap.Underline', 'Umb.Tiptap.Strike',
                    'Umb.Tiptap.BulletList', 'Umb.Tiptap.OrderedList', 'Umb.Tiptap.Heading', 'Umb.Tiptap.Link',
                ],
            },
            {
                alias: 'toolbar',
                value: this.data?.rteToolbar ?? [[
                    ['Umb.Tiptap.Toolbar.Undo', 'Umb.Tiptap.Toolbar.Redo'],
                    ['Umb.Tiptap.Toolbar.Bold', 'Umb.Tiptap.Toolbar.Italic', 'Umb.Tiptap.Toolbar.Underline', 'Umb.Tiptap.Toolbar.Strike'],
                    ['Umb.Tiptap.Toolbar.BulletList', 'Umb.Tiptap.Toolbar.OrderedList'],
                    ['Umb.Tiptap.Toolbar.Heading1', 'Umb.Tiptap.Toolbar.Heading2'],
                    ['Umb.Tiptap.Toolbar.Link', 'Umb.Tiptap.Toolbar.Unlink'],
                ]],
            },
        ]);
    }

    #onValueChange(e: UmbChangeEvent) {
        const editor = e.target as HTMLElement & { value: string };
        this.updateValue({ content: editor.value });
    }

    #handleConfirm() {
        this.modalContext?.submit();
    }

    #handleCancel() {
        this.modalContext?.reject();
    }

    #onTextareaChange(e: Event) {
        const textarea = e.target as HTMLTextAreaElement & { value: string };
        this.updateValue({ content: textarea.value });
    }

    render() {
        return html`
            <umb-body-layout .headline=${this.data?.headline ?? 'Edit cell'}>
                <uui-box>
                    ${this.data?.useTextareaEditor
                        ? html`<uui-textarea
                            .value=${this.value?.content ?? ''}
                            style="width:100%;min-height:200px;"
                            @change=${this.#onTextareaChange}>
                          </uui-textarea>`
                        : html`<umb-input-tiptap
                            .value=${this.value?.content ?? ''}
                            .configuration=${this.#rteConfig}
                            @change=${this.#onValueChange}>
                          </umb-input-tiptap>`
                    }
                </uui-box>

                <div slot="actions">
                    <uui-button id="cancel" label="Cancel" @click=${this.#handleCancel}>Cancel</uui-button>
                    <uui-button
                        id="submit"
                        color="positive"
                        look="primary"
                        label="Submit"
                        @click=${this.#handleConfirm}>Submit</uui-button>
                </div>
            </umb-body-layout>
        `;
    }

}

export default LimboTableModal;
