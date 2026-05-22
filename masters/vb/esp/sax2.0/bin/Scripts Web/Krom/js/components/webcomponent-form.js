export class WCForm extends HTMLFormElement {

    get context() {

        return {
            detail: (data, control) => {

                this.templateView((view) => {
                   
                    const table = document.createElement('table', { is: "wc-table" });

                    table.classList.add('table');

                    table.classList.add('table-striped');

                    const caption = document.createElement('caption');

                    const findbar = document.createElement('input');

                    findbar.setAttribute('type', 'text');

                    findbar.setAttribute('placeholder', 'Buscar registro');

                    const closeButton = document.createElement('a')

                    caption.appendChild(closeButton);

                    caption.appendChild(findbar);

                    table.appendChild(caption);

                    table.appendChild(document.createElement('thead'));

                    table.appendChild(document.createElement('tbody'));

                    view.appendChild(table);

                    try {

                        const jsonData = JSON.parse(data);

                        table.dataSource = jsonData;

                        table.addEventListener("selected", (res) => {

                            control.selectedIndex = res.detail;

                            this.context.pop();

                        });

                        closeButton.addEventListener('click', () => this.context.pop());
                       
                    } catch (e) { }
                    
                });

            },
            push: (url) => {
                
                this.templateView((view) => {
                   
                    const identifier = Math.floor(Math.random() * 1000000000);

                    const iframe = document.createElement('iframe');

                    iframe.setAttribute('id', identifier);

                    iframe.setAttribute('src', url + '?is_popup=true&id=' + identifier);

                    view.append(iframe);

                });

            },
            pop: () => {

                const view = this.contexs.querySelector('div');

                view.style.left = '-100%';

                setTimeout(e => {

                    view.remove();

                    __serverObserver();

                }, 1000);

            }
        };

    }

    constructor() {

        super();

    }

    connectedCallback() {

        this.component = this.querySelector('.wc-form');

        if (this.component) {

            this.contexs = this.component.querySelector('.__contexs');

            this.navigation = this.component.querySelector('.__navigation') || false;

            this.backbutton = this.component.querySelector('.__back') || false;

            if (this.navigation) {

                const items = this.navigation.querySelectorAll('input') || [];

                items.addEventListener('change', e => this.itemMenuSelected(e));

            }

            if (this.backbutton) this.backbutton.addEventListener('click', e => this.hideBackButton(e));

            this.setScrollListener();
        }
    }

    setScrollListener() {

        function isElementInViewport(el, self) {


            if (typeof jQuery === "function" && el instanceof jQuery) {
                el = el[0];
            }

            var rect = el.getBoundingClientRect();

            let element = document.querySelector('.content-wrapper');

            let beforeFinish = element.scrollHeight - (element.scrollTop + element.offsetHeight);

            if (beforeFinish <= 100 && beforeFinish > 0) {
                return rect.top <= 450 && rect.top >= 200;
            }

            if (element.offsetHeight + element.scrollTop >= element.scrollHeight) {

                return rect.top <= rect.y && rect.top >= rect.y;

            }

            return rect.top <= 200 && rect.top >= 100;

        }


        function onVisibilityChange(el, self, callback) {

            var old_visible = false;

            var visible = isElementInViewport(el, self);

            if (visible != old_visible) {

                old_visible = visible;

                if (typeof callback == 'function') {

                    callback();

                }
            }
        }

        document.querySelector('.content-wrapper').addEventListener("scroll", (e) => {
            
            const sections = this.querySelectorAll('fieldset');
            
            sections.forEach((fieldset, i) => {

                const sectionId = fieldset.getAttribute('section-id');

                if (sectionId) {

                    onVisibilityChange(fieldset, this, () => {

                        const el = this.querySelector('[to-section="' + sectionId + '"]') || false;

                        if (el) {

                            el.checked = true;;

                        }

                    });

                }

            });
        });

    }

    itemMenuSelected(e) {
        const input = e.target;
        const nav = input.getAttribute('to-section');
        const fieldset = this.component.querySelector('[section-id="' + nav + '"]');
        const content = document.querySelector('.content-wrapper-page');


        const targetPosition = fieldset.offsetTop;
        const currentPosition = content.scrollTop;

        const duration = 2000;
        const startTime = performance.now();

        function scrollStep(timestamp) {
            const timeElapsed = timestamp - startTime;
            const progress = Math.min(timeElapsed / duration, 1);


            const currentScroll = currentPosition + (targetPosition - currentPosition) * progress;

            content.scrollTop = currentScroll;

            if (progress < 1) {

                requestAnimationFrame(scrollStep);
            }
        }


        requestAnimationFrame(scrollStep);
    }


    templateView(func) {

        document.getElementById('form1').scrollTop = 0;

        const view = document.createElement('div');

        if (typeof func === 'function') {

            func(view);

        }
        
        this.contexs.append(view);
       
        //$(view).niceScroll();
 
        setTimeout(() => { view.style.left = '0%'; }, 250);


    }

    hideBackButton(e) {

        e.preventDefault();

        const event = new Event('OnContextClose');

        window.parent.document.dispatchEvent(event);

    }

 }
