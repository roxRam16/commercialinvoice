export class WCLabels extends HTMLSpanElement {
    constructor() {
        super();
    }

    connectedCallback() {
        this.component = this.closest('.wc-labels');
    }

    static copyToClipboard(labelId) {

        const label = document.getElementById(labelId);

        if (!label) return;

        const tempInput = document.createElement("textarea");

        tempInput.style.position = "absolute";

        tempInput.style.left = "-9999px";

        tempInput.value = label.innerText || label.textContent;

        document.body.appendChild(tempInput);

        tempInput.select();

        document.execCommand("copy");

        document.body.removeChild(tempInput);

        DisplayMessage("Texto copiado al portapapeles", 1);
    }
}

try {

    if (!customElements.get('wc-labels')) {

        customElements.define('wc-labels', WCLabels, { extends: 'span' });

    }

} catch (e) {

    console.warn('wc-labels ya estaba definido:', e.message);

}

window.WCLabels = WCLabels;
