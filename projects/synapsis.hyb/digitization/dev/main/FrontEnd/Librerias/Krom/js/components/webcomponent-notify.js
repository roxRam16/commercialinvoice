export class WCNotify extends HTMLDivElement {

    constructor() {
        super();
    }

    connectedCallback() {
        this.component = this.closest('.wc-notify, .wc-notifySmall');

        if (this.component) {

            const closeButtons = [

                ...this.component.querySelectorAll('.iconClose-normal, .iconClose-advertencia, .iconClose-alerta, .iconCloseSmall-normal, .iconCloseSmall-advertencia, .iconCloseSmall-alerta')

            ];

            const actionButtons = [

                ...this.component.querySelectorAll('.btnAction-normal, .btnActions-normal, .btnAction-advertencia, .btnActions-advertencia, .btnAction-alerta, .btnActions-alerta')

            ];

            closeButtons.forEach(button => button.addEventListener('click', e => this.closeNotify(e)));

            actionButtons.forEach(button => button.addEventListener('click', e => this.closeNotify(e)));

        } else {

            console.error('No se ha encontrado el componente wc-notify o wc-notifySmall');
        }

    }

    closeNotify(e) {

        const button = e.currentTarget;

        const evt = new Event('change');

        button.dispatchEvent(evt);

        const notifyBox = button.closest('.notifyBox, .notifyBoxSmall');

        const wcNotify = this.closest('.wc-notify, .wc-notifySmall');

        if (!notifyBox || !wcNotify) {

            console.error('No se encontró el contenedor de la notificación o el componente wc-notify');

            return;

        }

        notifyBox.classList.add('fade-out');

        wcNotify.classList.add('fade-out');

        setTimeout(() => {
            notifyBox.remove();
            wcNotify.remove();
        }, 1000);
    }


}
