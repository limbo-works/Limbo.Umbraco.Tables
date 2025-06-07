import { customElement, html, state } from "@umbraco-cms/backoffice/external/lit";
import { UmbModalBaseElement } from "@umbraco-cms/backoffice/modal";
import {type RteModalData, type RteModalValue} from "./RteModalValue";
import { UUIInputEvent } from "@umbraco-cms/backoffice/external/uui";

@customElement('limbo-table-modal')
export class LimboTableModal extends
    UmbModalBaseElement<RteModalData, RteModalValue>
{
    constructor() {
        super();
    }

    connectedCallback(): void {
        super.connectedCallback();
        this.updateValue({content: this.data?.content});
    }

    @state()
    content: string = '';

    #handleConfirm() {
        this.value = { content: this.value?.content ?? ''} ;
        this.modalContext?.submit();
    }

    #handleCancel() {
        this.modalContext?.reject();
    }

    #contentChange(event: UUIInputEvent) {
        console.log(event);
        console.log(event.target.value);
        this.updateValue({content: event.target.value.toString()});
    }

    render() {
        return html`
            <umb-body-layout .headline=${this.data?.headline ?? 'Custom dialog'}>
                <uui-box>
                    <umb-property  alias="cell-content"
                                   label="Cell Content"
                                   .value=${this.data?.content}
                                   property-editor-ui-alias="Umb.PropertyEditorUi.Tiptap" >
                    </umb-property>
                    <uui-textarea label="content" 
                        rows=10
                        .value=${this.data?.content}
                        @input=${this.#contentChange}>
                    </uui-textarea>
                </uui-box>
                <uui-box>
                    <h2>Return Value</h2>
                    <pre>${this.value?.content}</pre>
                </uui-box>

                <div slot="actions">
                        <uui-button id="cancel" label="Cancel" @click="${this.#handleCancel}">Cancel</uui-button>
                        <uui-button
                            id="submit"
                            color='positive'
                            look="primary"
                            label="Submit"
                            @click=${this.#handleConfirm}></uui-button>
            </div>
            </umb-body-layout>
        `;
    }

}

export default LimboTableModal;