
//Import Components
import { WCToolTip } from './components/webcomponent-tooltip.js';

import { WCForm } from './components/webcomponent-form.js';

import { WCCatalog } from './components/webcomponent-catalog.js';

import { WCFile } from './components/webcomponent-file.js';

import { WCFindbar } from './components/webcomponent-findbar.js';

import { WCFindbox } from './components/webcomponent-findbox.js';

import { WCSelect } from './components/webcomponent-select.js';

import { WCTable } from './components/webcomponent-table.js';

import { WCCollectionView } from './components/webcomponent-collectionview.js';

import { WCImage } from './components/webcomponent-image.js';

import { WCInput } from './components/webcomponent-input.js';

import { WCListbox } from './components/webcomponent-listbox.js';

import { WCPillbox } from './components/webcomponent-pillbox.js';

import { WCLabel } from './components/webcomponent-label.js';

import { WCComment } from './components/webcomponent-comment.js';

import { WCUserData } from './components/webcomponent-userdata.js';

import { WCFEditor } from './components/webcomponent-feditor.js';

import { WCLabels } from './components/webcomponent-labels.js';

import { WCIcon } from './components/webcomponent-icon.js';

import { WCNotify } from './components/webcomponent-notify.js';

//Defined Components

customElements.define('wc-tooltip', WCToolTip, { extends: 'input' });

customElements.define('wc-form', WCForm, { extends: 'form' });

customElements.define('wc-catalog', WCCatalog, { extends: 'table' });

customElements.define('wc-file', WCFile, { extends: 'input' });

customElements.define('wc-findbar', WCFindbar, { extends: 'input' });

customElements.define('wc-findbox', WCFindbox, { extends: 'input' });

customElements.define('wc-select', WCSelect, { extends: 'select' });

customElements.define('wc-table', WCTable, { extends: 'table' });

customElements.define('wc-collectionview', WCCollectionView, { extends: 'ul' });

customElements.define('wc-image', WCImage, { extends: 'div' });

customElements.define('wc-input', WCInput, { extends: 'input' });

customElements.define('wc-listbox', WCListbox, { extends: 'input' });

customElements.define('wc-pillbox', WCPillbox, { extends: 'div' });

customElements.define('gwc-label', WCLabel);

customElements.define('gwc-userdata', WCUserData);

customElements.define('gwc-comment', WCComment);

customElements.define('wc-feditor', WCFEditor, { extends: 'div' });

customElements.define('wc-icon', WCIcon);

customElements.define('wc-notify', WCNotify, { extends: 'div' });



//Initialize External Libraries

//$(".content-wrapper-page").niceScroll();

$(document).on('click', 'legend label', function (e) {

    const legend = $(this).closest('legend');

    const input = legend.next('input');

    input.click();

}); 

//DOBLE SUBMIT CONTROLLER
$('button, input:submit','#form1').click(e => e.target.disabled = true);

//DATEPICKER
$(document).on('focusin', '.datepicker', function () {
    $(this).datepicker({
        autoclose: true, format: 'yyyy-mm-dd', todayHighlight: true
    });
}).on('changeDate', '.datepicker', function (e) {
    this.dispatchEvent(new Event('change'));
});

//TIMEPICKER
$(document).on('focusin', '.timepicker', function () {
	$(this).timepicker({ showInputs: false });
}).on('change', '.timepicker', function (e) {
    this.dispatchEvent(new Event('change'));
});

//CALENDARBLOCKED
$(document).on('focusin', '.datepicker-range', function () {
    const $input = $(this);

    const minDate = $input.data('min-date');
    const maxDate = $input.data('max-date');

    $input.datepicker({
        autoclose: true,
        format: 'dd/mm/yyyy',
        todayHighlight: true,
        startDate: minDate || null,
        endDate: maxDate || null
    });
});

// Ventana flotante
if (!document.getElementById('custom-time-popup')) {
    const popupHtml = `
        <div id="custom-time-popup" style="
            display: none;
            position: absolute;
                background-color: #fff;
            color: #333;
            padding: 15px;
            border-radius: 10px;
            border-color: #eee;
            box-shadow: 0 8px 20px rgba(0, 0, 0, 0.3);
            z-index: 9999;
            min-width: 180px;
            font-family: 'Segoe UI', sans-serif;
        ">
            <label for="custom-time-input" style="font-size: 14px; font-weight: 600;">Selecciona hora</label>
            <input type="time" id="custom-time-input" class="form-control" style="
                margin-top: 8px;
                border: 1px solid #333;
                border-radius: 5px;
                padding: 6px 10px;
                font-size: 14px;
                width: 100%;
                color: #333;
            ">
            <button id="custom-time-ok" class="btn btn-light btn-sm" style="
                margin-top: 10px;
                width: 100%;
                font-weight: 600;
                background-color: #7e61b0;
                padding: 6px 0;
                border-radius: 5px;
                color: white;
                transition: background 0.2s;
            ">Aceptar</button>
        </div>
    `;
    document.body.insertAdjacentHTML('beforeend', popupHtml);
}

// DATETIMEPICKER
$(document).on('focusin', '.datetimepicker', function () {
    const $input = $(this);

    if (!$input.data('datetimepicker-initialized')) {
        $input.datepicker({
            autoclose: true,
            format: 'dd/mm/yyyy',
            todayHighlight: true
        }).on('changeDate', function () {
            setTimeout(() => {
                showCustomTimePopup($input);
            }, 200);
        });

        $input.data('datetimepicker-initialized', true);
    }
});

// Mostrar la ventana personalizada de hora
function showCustomTimePopup($input) {
    const popup = $('#custom-time-popup');
    const timeInput = $('#custom-time-input');
    const okBtn = $('#custom-time-ok');

    const offset = $input.offset();
    popup.css({
        top: offset.top + $input.outerHeight() + 8,
        left: offset.left,
        display: 'block'
    });

    timeInput.val('').focus();

    okBtn.off('click').on('click', function () {
        const datePart = $input.val().split(' ')[0];
        const timePart = timeInput.val();
        if (timePart) {
            $input.val(datePart + ' ' + timePart);
            $input.trigger('change');
        }
        popup.hide();
    });
}

// Ocultar la ventana si se hace clic fuera
$(document).on('click', function (e) {
    if (!$(e.target).closest('#custom-time-popup, .datetimepicker').length) {
        $('#custom-time-popup').hide();
    }
});



//FORMATS
$(document).on('focusin', '[data-mask]', function () {
    $(this).inputmask();
});

//NUMERIC FORMAT
$(document).on('propertychange input', '.numeric', function () {
    var input = $(this);
    input.val(input.val().replace(/[^\d]+/g, ''));
});

//REAL FORMAT
$(document).on('keypress', '.real', function (evt) {
        
    var input = $(this).get(0);
    
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode == 46) {

        if (input.value.indexOf('.') === -1) {
            return true;
        } else {
            return false;
        }
    } else {
        if (charCode > 31 &&
            (charCode < 48 || charCode > 57))
            return false;
    }
    return true;

});

var decimals = 0;

$(document).ready(function () {
    var elemento = $('.currency').get(0);
    if (elemento) {
        decimals = parseInt($(elemento).get(0).getAttribute('decimals'), 10);
    }
});


let doneTypingInterval = 500;

function applyCurrencyFormatting(selector, formatFunction) {
    $(document).on('input', selector, function () {
        const input = $(this);

        clearTimeout(input.data('timeout'));

        const timeout = setTimeout(() => {
            formatFunction(input);
        }, doneTypingInterval);

        input.data('timeout', timeout);
    });
}


applyCurrencyFormatting('.currencyDecimal', formatCurrencyDecimal);
applyCurrencyFormatting('.currencyInteger', formatCurrencyInterger);
applyCurrencyFormatting('.currency', formatCurrency);


//MONEY FORMAT 2 DECIMALS REQUIRED
function formatCurrencyDecimal(input) {
    var input_val = input.val() || ""; 
    0
    if (input_val === "") { return; }

    var original_len = input_val.length;
    var caret_pos = input.prop("selectionStart");

    if (typeof input_val !== 'string') {
        return;
    }

    input_val = input_val.replace(/[^\d.]/g, "");

    if (input_val === "") {
        input_val = "0";
    }

    input_val = formatNumberTwo(input_val);

    input_val = "$" + input_val;

    if (input_val.indexOf(".") === input_val.length - 1) {
        input_val = input_val + "00";
    }

    if (!input_val.includes(".")) {
        input_val = input_val + ".00";
    }

    var decimal_pos = input_val.indexOf(".");
    if (decimal_pos !== -1) {
        var decimal_part = input_val.substring(decimal_pos);
        input_val = input_val.substring(0, decimal_pos + 3);
        if (decimal_part.length === 1 || decimal_part.length === 2) {
            input_val = input_val + "0";
        }
        if (decimal_part !== .00) {
            input_val = input_val.substring(0, decimal_pos) + ".00";
        }
    }

    input.val(input_val);

    var updated_len = input_val.length;
    caret_pos = updated_len - original_len + caret_pos;
    if (caret_pos >= 0 && caret_pos <= input_val.length) {
         input[0].setSelectionRange(caret_pos, caret_pos);
    }
}

//MONEY FORMAT INTERGER
function formatCurrencyInterger(input, blur) {
    var input_val = input.val() || "";

    if (input_val === "") { return; }

    var original_len = input_val.length;
    var caret_pos = input.prop("selectionStart");

    if (typeof input_val !== 'string') {
        return;
    }

    input_val = input_val.replace(/\D/g, "");

    input_val = formatNumber(input_val);

    if (input_val === "") {
        input_val = "0";
    }

    input_val = "$" + input_val;

    if (blur === "blur") {
        input_val = input_val + ".00";
    }

    input.val(input_val);

    var updated_len = input_val.length;
    caret_pos = updated_len - original_len + caret_pos;
    if (caret_pos >= 0 && caret_pos <= input_val.length) {
        input[0].setSelectionRange(caret_pos, caret_pos);
    }
}

//MONEY FORMAT 2 DECIMALS NORMAL
function formatCurrency(input, blur) {
    var input_val = input.val() || "";

    if (input_val === "") { return; }

    var original_len = input_val.length;
    var caret_pos = input.prop("selectionStart");

    if (typeof input_val !== 'string') {
        return;
    }

    if (input_val.indexOf(".") >= 0) {
        var decimal_pos = input_val.indexOf(".");
        var left_side = input_val.substring(0, decimal_pos);
        var right_side = input_val.substring(decimal_pos + 1);

        left_side = formatNumber(left_side);

        if (right_side.length > decimals) {
            right_side = right_side.substring(0, decimals);
        }

        if (blur === "blur") {
            while (right_side.length < decimals) {
                right_side += "0";
            }
        }

        input_val = "$" + left_side + "." + right_side;

    } else {
        input_val = formatNumber(input_val);
        input_val = "$" + input_val;

        if (blur === "blur") {
            input_val += "." + "0".repeat(decimals);
        }
    }

    input.val(input_val);

    var updated_len = input_val.length;
    caret_pos = updated_len - original_len + caret_pos;
    if (caret_pos >= 0 && caret_pos <= input_val.length) {
        input[0].setSelectionRange(caret_pos, caret_pos);
    }
}

function formatNumber(n) {
    return n.replace(/\D/g, "").replace(/\B(?=(\d{3})+(?!\d))/g, ",")
}

function formatNumberTwo(n) {

    if (n.length <= 3) return n;
    return n.replace(/\B(?=(\d{3})+(?!\d))/g, ",");
}


//OPEN BLOCKED INPUT MODAL
$('body').on('click', '.lock-input', function (e) {
    if ($(this).get(0).hasAttribute('pbref')) {

        e.preventDefault();

        DisplayPrompt('Desbloquear', ' Ingrese el c&oacute;digo de seguridad para desbloquear este control', null, null, (value_) => {

            if (value_) {

                $(this).get(0).setAttribute('onclick', $(this).attr('pbref').replace("__argument", value_));

                $(this).get(0).removeAttribute('pbref');

                setTimeout(() => $(this).click(), 0);
            }
        });
    }
});

$(document).click((event) => {
    if (!$(event.target).closest('.__reminder').length) {
        $('.__reminder').remove();
    }
});



$('body').on('click', '.__down', function (e) {

    const component = e.target.closest('.wc-collectionview') || false;

    if (component) {

        if (!e.target.hasAttribute('in-process')) {

            e.preventDefault();

            e.target.setAttribute('in-process', 'true');

            const element = e.target.closest('[fieldid]');

            const jsonString = component.querySelector('.__data');

            const indice = element.parentNode.querySelector('.__item').getAttribute('itemid');

            try {

                const data = JSON.parse(jsonString.value);

                let result = data.filter(a => a[component.getAttribute('keyfield')] == indice);

                result[0][element.getAttribute('fieldid')]['Dropdowm'] = new Boolean(true);

                jsonString.value = JSON.stringify(data);

                e.target.click();

            } catch (e) { console.log(e) }

        }

    }

});

$('body').on('change', '.wc-collectionview input[id], .wc-collectionview textarea[id]', (e) => {

    const component = e.target.closest('.wc-collectionview') || false;

    if (component) {

        OnCollectionViewControlChanged(e.target);

    }

});

window.onSelectChange = (e) => {

    const component = e.closest('.wc-collectionview') || false;

    if (component) {

        OnCollectionViewControlChanged(e);

    }
    
};

function OnCollectionViewControlChanged(e) {

    const element = e.closest('[fieldid]')

    if (element.classList.contains('wc-catalog') == false) {

        const component = e.closest('.wc-collectionview');

        const jsonString = component.querySelector('.__data');

        const indice = element.parentNode.querySelector('.__item').getAttribute('itemid');

        try {

            const data = JSON.parse(jsonString.value);

            let result = data.filter(a => a[component.getAttribute('keyfield')] == indice);

            if (e.type.toLowerCase() == 'select-one') {

                result[0][element.getAttribute('fieldid')]['Value'] = e.value;

                result[0][element.getAttribute('fieldid')]['Text'] = e.options[e.selectedIndex].text;

                result[0][element.getAttribute('fieldid')]['Dropdowm'] = !result[0][element.getAttribute('fieldid')]['Dropdowm'];

            } else {

                result[0][element.getAttribute('fieldid')] = e.value;

            }

            jsonString.value = JSON.stringify(data);

        } catch (e) { console.log(e) }

    }

}

window.onCatalogChange = (e) => OnCatalogControlChange(e); 

function OnCatalogControlChange(e) {
    
    const collection = e.closest('.wc-collectionview') || false;

    if (collection) {

        const fieldid = e.parentNode.getAttribute('fieldid');

        const dataCatalogo = e.jsonString.value;

        const data = collection.querySelectorAll('.__data');

        const jsonString = data[data.length - 1];

        const indice = e.parentNode.parentNode.querySelector('.__item').getAttribute('itemid');

        try {

            const data = JSON.parse(jsonString.value);

            let result = data.filter(a => a[collection.getAttribute('keyfield')] == indice);

            result[0][fieldid] = dataCatalogo;

            jsonString.value = JSON.stringify(data);

        } catch (e) { console.log(e) }

    }

}