function DisplayPrompt() {

    const title_ = arguments[0] || '';
    const message_ = arguments[1] || '';
    const accept_ = arguments[2] || 'Enviar';
    const cancel_ = arguments[3] || 'Cancelar';
    const func_ = arguments[4] || null;

    const template_ = document.createElement('template');

    template_.innerHTML = `
        <div class="unlock-overlay active">
  <div class="unlock-box">

    <div class="unlock-header">
      <h2 class="unlock-title">${title_}</h2>
      <button class="unlock-close">×</button>
    </div>

    <div class="unlock-body">
      <p class="unlock-text">${message_}</p>

      <input 
        type="password" 
        class="unlock-input" 
        placeholder="Ingresar clave"
      />
    </div>

    <div class="unlock-actions">
      <button class="unlock-btn cancel">
        ${cancel_}
      </button>
      <button class="unlock-btn confirm" disabled>
        ${accept_}
      </button>
    </div>

  </div>
</div>
    `;

    document.body.appendChild(template_.content.cloneNode(true));

    const modal_ = document.querySelector('.unlock-overlay');

    const btnaccept_ = modal_.querySelector('.confirm');
    const btncancel_ = modal_.querySelector('.cancel');
    const btnclose_ = modal_.querySelector('.unlock-close');
    const input_ = modal_.querySelector('.unlock-input');

    input_.addEventListener('input', () => {
        btnaccept_.disabled = input_.value.length === 0;
    });

    btnaccept_.addEventListener('click', () => {

        modal_.remove();

        if (typeof func_ === 'function') {
            func_(input_.value);
        }

    });

    btncancel_.addEventListener('click', () => {
        modal_.remove();
    });

    btnclose_.addEventListener('click', () => {
        modal_.remove();
    });

    input_.addEventListener('keypress', (e) => {
        if (e.key === 'Enter' && input_.value.length > 0) {
            btnaccept_.click();
        }
    });

    setTimeout(() => input_.focus(), 100);
}

//MOP, Nueva Ventana para confirmacion del CMF

function DisplayDialogJS(title_, message_, argument_, options_) {

    let buttonsHTML = '';

    options_.forEach((opt, index) => {

        const isPrimary = index === 0;

        const btnClass = isPrimary
            ? 'dialog-btn primary'
            : 'dialog-btn secondary';

        buttonsHTML += `
            <button
                class="${btnClass}"
                onclick="enviarRespuestaDialogo('${argument_}', '${opt.Value}')">
                ${opt.Text}
            </button>
        `;
    });

    const modalHTML = `
        <div class="dialog-overlay">
            <div class="dialog-box">

                <div class="dialog-header">
                    <h4 class="dialog-title">${title_}</h4>
                </div>

                <div class="dialog-body">
                    <p class="dialog-message">${message_}</p>
                </div>

                <div class="dialog-actions">
                    ${buttonsHTML}
                </div>

            </div>
        </div>
    `;

    document.body.insertAdjacentHTML('beforeend', modalHTML);
}


function DisplayAlert() {

    const title_ = arguments[0] || 'Confirmación';
    const message_ = arguments[1] || '';
    const accept_ = arguments[2] || 'Entendido';
    const cancel_ = arguments[3] || '';
    const argument_ = arguments[4] || null;
    const hasCancelButton = (!cancel_) ? 'd-none' : '';
    const hasAcceptButton = (!accept_) ? 'd-none' : '';

    const template_ = document.createElement('template');

    template_.innerHTML = `
            <div class="modal show">
            <div class="modal-dialog modal-sm modal-dialog-centered">
                <div class="modal-content">
                    <div class="modal-header">
                        <h4 class="modal-title">${title_}</h4>
                        <button type="button" class="close">
                            <span aria-hidden="true">×</span>
                        </button>
                    </div>
                    <div class="modal-body">
                        <p>${message_}</p>
                    </div>
                    <div class="modal-footer justify-content-between">
                        <button type="button" class="btn btn-default ${hasCancelButton}" data-dismiss="modal">${cancel_}</button>
                        <button type="button" class="btn btn-primary ${hasAcceptButton}" data-dismiss="modal">${accept_}</button>
                    </div>
                </div>
            </div>
            </div>
	        `;

    /*<button type="button" class="btn btn-primary">${accept_}</button>*/

    document.body.appendChild(template_.content.cloneNode(true));

    const modal_ = document.querySelector('.modal');

    const btnaccept_ = modal_.querySelector('.btn-primary');

    const btncancel_ = modal_.querySelector('.btn-default');

    const btnclose_ = modal_.querySelector('.close');


    btnaccept_.addEventListener('click', () => {

        document.querySelector('.modal').remove();

        if (typeof argument_ === 'function') {

            argument_(true);

        }
        else {

            PageMethods["ProcessDialogConfirmation"]({ arg: argument_, accept: true }, (data) => {

                try {

                    const res = JSON.parse(data);

                    if (res.code == 200) {

                        __serverObserver();

                    }

                } catch (e) {}

            });


        }

    });

    btncancel_.addEventListener('click', () => {

        document.querySelector('.modal').remove();

        if (typeof argument_ === 'function') {

            argument_(false);

        } else {

            PageMethods["ProcessDialogConfirmation"]({ arg: argument_, accept: false }, (data) => {

                try {

                    const res = JSON.parse(data);

                    if (res.code == 200) {

                        __serverObserver();

                    }

                } catch (e) { }

            });

        }

    });

    btnclose_.addEventListener('click', () => {

        document.querySelector('.modal').remove();

    });

}

function DisplayMessage() {

    const message_ = arguments[0] || false;

    const code_ = arguments[1] || 1;

    const template_ = document.createElement('template');

    var tintColor_;

    var title_;

    if (code_ == 1) { tintColor_ = '#1ca477'; title_ = 'Excelente'; }

    if (code_ == 2) { tintColor_ = '#750629'; title_ = 'Error'; } //#85092e, #750629 rosared, #241547 morado, BCK: a41c22

    if (code_ == 3) { tintColor_ = '#1e1873'; title_ = 'Información'; } //#1e1873 BCK: 1c8ba4

    if (code_ == 4) { tintColor_ = '#9c5c13'; title_ = 'Advertencia'; } //#9c5c13 BCK:

    template_.innerHTML = `
       <div class="wc-toast" style="--tintColor:${tintColor_}">
		  <i></i>
          <p>
            <small>${title_}</small>
            ${message_}
          </button>
       </div>
    `;

    document.querySelectorAll('.wc-toast').forEach(e => e.remove());

    document.body.append(template_.content.cloneNode(true));

    const toast = document.querySelector('.wc-toast');

    setTimeout(e => {
        toast.style.bottom = '4%';
        toast.querySelector('i').classList.add('wc-toast-animate');
        setTimeout(e => {
            toast.style.bottom = '-100%';
            setTimeout(e => toast.remove(), 1000);
        }, 5000);
    }, 0);

}

window.enviarRespuestaDialogo = function (arg, val) {
    const hdn = document.getElementById('hdnDialogResponse');
    if (hdn) {
        // Guardamos exactamente lo que el Diccionario en VB espera
        hdn.value = JSON.stringify({ "arg": arg, "value": val });
    }

    // document.querySelector('.modal-signalr').remove();
    __doPostBack('DialogConfirmation', 'DialogConfirmation');

    //__serverObserver();
};

//OK

function __serverObserver() {

    const serverObserver = document.querySelector('.__serverObserver');

    serverObserver.click();

}


//////ACTUAL MOP, DEACTIVADO, MAL, CAUSA EL PARPADEO
//function __serverObserver() {

//    alert("Alguien llamó a __serverObserver y fue desactivado (a) 2");
//    // __doPostBack(target, argument)
//    // Usamos '__Page' para que el Page_Load reciba el evento
//    if (typeof __doPostBack !== 'undefined') {
//        __doPostBack('__Page', 'DialogConfirmation');
//    } else {
//        console.error("ASP.NET __doPostBack no está definido en esta página.");
//    }
//}

function openPDF(pdfstring, title) {

    var win = window.open();

    win.document.title = title;

    var iframe = "<iframe src='" + pdfstring + "' frameborder='0' style='border:0; top:0px; left:0px; bottom:0px; right:0px; width:100%; height:100%;position:absolute;' allowfullscreen></iframe>";

    win.document.write(iframe);

}