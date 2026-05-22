export class WCIcon extends HTMLElement {

    constructor() {

        super();

    }

    connectedCallback() {

        this.component = this.closest('.__icon');

    }


}