const _paginaLogout = "main2.aspx"
const _paginaLogIn = "http://10.66.1.15:14326/Login.aspx"
const body = document.body;

$(".btnCerrarSesion").click(function (event) {
    event.preventDefault()
    $.ajax({
        type: "POST"
        , url: _paginaLogout + "/CerrarSesion"
        , contentType: 'application/json; charset=utf-8'
        , dataType: 'JSON'
        , beforeSend: function () {
            $("#loading").css("display", "inherit");
        }
        , error: function (xhr, ajaxOptions, thrownError) {
            console.log(xhr.status + " \n" + xhr.responseText, "\n" + thrownError);
        }
        , success: async function (response) {
            window.location.href = "/signout"
        }
        , complete: function () {
            $("#loading").css("display", "none");
        }
    });
})


$('.toggle-menu').click(function (event_) {

    $('.binnacle-aside').toggleClass('binnacle-aside-hidden', function (event_) {

        //$('.arrow-colapse').toggleClass('colapse');

        let hasClass_ = $(this).hasClass('binnacle-aside-hidden');

        if (hasClass_) {

            $('.content-wrapper').css({ 'margin-right': '0' });

            //$('.content-wrapper').css({ 'transform': 'translate(0px, 0)' });
            
            //$('.arrow-colapse').css({ 'left': '-30px', 'transform': 'scaleX(-1)' });

            $('.toggle-menu').removeClass('toggle-menu-next').addClass('toggle-menu-prev');

        } else {

            $('.content-wrapper').css({ 'margin-right': '270px' });

            //$('.content-wrapper').css({ 'transform': 'translate(-230px, 0)' });

            //$('.arrow-colapse').css({ 'left': '0', 'transform': 'scaleX(1)' });

            $('.toggle-menu').removeClass('toggle-menu-prev').addClass('toggle-menu-next');

        }

        /*setTimeout(() => {
            $(".content-wrapper-page").niceScroll();
            $(".content-wrapper-page").getNiceScroll().resize();
        }, 300);*/
        

    });

});

/*$('.arrow-colapse-menu').click(function (event_) {
    
    $('body').toggleClass('sidebar-open', function (event_) {

        let hasClass_ = $(this).hasClass('sidebar-open');

        if (hasClass_) {

            $('.arrow-colapse-menu').css({ 'left': '23rem', 'transform': 'scaleX(-1)' });

        } else {

            $('.arrow-colapse-menu').css({ 'left': '0', 'transform': 'scaleX(1)' });

        }

    });

});*/

$('.finder-bar__logo').click(function (event_) {

    $('body').toggleClass('sidebar-open', function (event_) { });

});



$('[name="right-menu"]').change(function () {

    const id = $(this).val();
    
    $('.black-item-data').addClass('d-none');

    $('[menu-section="' + id + '"]').removeClass('d-none');

});

/***********************************/
/*REDIRECCIONA AL PERFIL DE USUARIO*/
//$(".perfil-usuario").click(function (event) {
//    event.preventDefault();
//    window.location.href = "/FrontEnd/Modulos/ConfiguracionSesion/Ges003-001-Edicion.PerfilUsuario.aspx";
//});

document.querySelectorAll('.has-submenu').forEach(item => {

    item.addEventListener('click', function (e) {
        e.preventDefault();

        const parent = this.closest('.treeview');
        const level = parent.parentElement;

        const isRoot = level.classList.contains('sidebar-menu');

        level.querySelectorAll(':scope > .treeview').forEach(el => {
            if (el !== parent) {
                el.classList.remove('active');
            }
        });

        parent.classList.toggle('active');

        if (!parent.classList.contains('active')) {
            parent.querySelectorAll('.treeview.active').forEach(el => {
                el.classList.remove('active');
            });
        }

        parent.querySelectorAll('.arrow').forEach(arrow => {
            arrow.style.transform = '';
        });

    });

});


document.addEventListener('DOMContentLoaded', function () {

    let permisos = JSON.parse(localStorage.getItem('user_permissions') || '[]');

    if (!permisos || permisos.length === 0) {
        console.warn('Usando permisos mock (local)');

        permisos = [
            {
                "environment_id": 1,
                "permission_id": 650,
                "token": "M1_Operacion",
                "action": 1
            },
            {
                "environment_id": 1,
                "permission_id": 651,
                "token": "M1_i_Referencias",
                "action": 1,
                "page_url": "/FrontEnd/Modulos/TraficoAA/Referencias/Ges022-001-Referencias",
                "page_type": 2,
                "podname": "SynOperations"
            },
            {
                "environment_id": 1,
                "permission_id": 652,
                "token": "M1_i_Pedimentos",
                "action": 1,
                "page_url": "/FrontEnd/Modulos/TraficoAA/MetaforaPedimento/Ges022-001-MetaforaPedimento",
                "page_type": 2,
                "podname": "SynOperations"
            },
            {
                "environment_id": 1,
                "permission_id": 653,
                "token": "M1_i_Validacion_y_pago",
                "action": 1,
                "page_url": "/FrontEnd/Modulos/TraficoAB/Validacion-pago",
                "page_type": 1,
                "podname": "Undefined"
            },
            {
                "environment_id": 1,
                "permission_id": 654,
                "token": "M1_i_Consultas",
                "action": 1
            },
            {
                "environment_id": 1,
                "permission_id": 655,
                "token": "M1_i_Expediente",
                "action": 1
            },
            {
                "environment_id": 1,
                "permission_id": 656,
                "token": "M2_i_Captura",
                "action": 1
            },
            {
                "environment_id": 1,
                "permission_id": 657,
                "token": "M2_i_Facturas_Importacion",
                "action": 1,
                "page_url": "/FrontEnd/Modulos/TraficoAA/FacturasComerciales/FacturaComercialImportacion/Ges003-001-FacturasComerciales",
                "page_type": 2,
                "podname": "SynOperations"
            },
            {
                "environment_id": 1,
                "permission_id": 658,
                "token": "M2_i_Facturas_Exportacion",
                "action": 1,
                "page_url": "/FrontEnd/Modulos/TraficoAA/FacturasComerciales/FacturaComercialExportacion/Ges022_FacturaComercialExportacion",
                "page_type": 2,
                "podname": "SynOperations"
            },
            {
                "environment_id": 1,
                "permission_id": 659,
                "token": "M2_i_Subdivision_Fac_Imp",
                "action": 1,
                "page_url": "/FrontEnd/Modulos/TraficoAB/subdivision-fac-imp",
                "page_type": 1,
                "podname": "Undefined"
            },
            {
                "environment_id": 1,
                "permission_id": 660,
                "token": "M2_i_Acuse_de_valor",
                "action": 1,
                "page_url": "/FrontEnd/Modulos/TraficoAA/AcusesValor/Ges022-001-AcuseValor",
                "page_type": 2,
                "podname": "SynOperations"
            },
            {
                "environment_id": 1,
                "permission_id": 661,
                "token": "M2_i_MVE",
                "action": 1
            },
            {
                "environment_id": 1,
                "permission_id": 662,
                "token": "M2_i_Guias_aereas",
                "action": 1,
                "page_url": "/FrontEnd/Modulos/TraficoAA/Guias/Aereas/Ges022-GuiaAerea",
                "page_type": 2,
                "podname": "SynOperations"
            },
            {
                "environment_id": 1,
                "permission_id": 663,
                "token": "M2_i_Guias_maritimas",
                "action": 1,
                "page_url": "/FrontEnd/Modulos/TraficoAA/Guias/Maritimas/Ges022-GuiaMaritima",
                "page_type": 2,
                "podname": "SynOperations"
            },
            {
                "environment_id": 1,
                "permission_id": 664,
                "token": "M2_i_Control_de_viajes",
                "action": 1,
                "page_url": "/FrontEnd/Modulos/TraficoAA/ControlViajes/Ges022-001-ControlViajes",
                "page_type": 2,
                "podname": "SynOperations"
            },
            {
                "environment_id": 1,
                "permission_id": 665,
                "token": "M2_i_Prog_Previos",
                "action": 1,
                "page_url": "/FrontEnd/Modulos/TraficoAA/ProgramacionPrevios/Ges022-001-ProgramacionPrevios",
                "page_type": 2,
                "podname": "SynOperations"
            },
            {
                "environment_id": 1,
                "permission_id": 666,
                "token": "M2_i_Procesamiento_IA",
                "action": 1
            },
            {
                "environment_id": 1,
                "permission_id": 667,
                "token": "M2_si_Procesamiento_E_Documents",
                "action": 1,
                "page_url": "/FrontEnd/Modulos/TraficoAB/Procesamiento-edocuments",
                "page_type": 1,
                "podname": "Undefined"
            },
            {
                "environment_id": 1,
                "permission_id": 668,
                "token": "M2_si_Proc_Facturas_Comerciales",
                "action": 1,
                "page_url": "/FrontEnd/Modulos/TraficoAB/Facturas-comerciales",
                "page_type": 1,
                "podname": "Undefined"
            },
            {
                "environment_id": 1,
                "permission_id": 669,
                "token": "M3_Catalogos",
                "action": 1
            },
            {
                "environment_id": 1,
                "permission_id": 670,
                "token": "M3_i_Trafico",
                "action": 1
            },
            {
                "environment_id": 1,
                "permission_id": 671,
                "token": "M3_si_Clientes",
                "action": 1,
                "page_url": "/FrontEnd/Modulos/TraficoAA/Clientes/Ges022-001-Clientes",
                "page_type": 2,
                "podname": "SynOperations"
            },
            {
                "environment_id": 1,
                "permission_id": 672,
                "token": "M3_si_Unidades_de_negocio",
                "action": 1,
                "page_url": "/FrontEnd/Modulos/TraficoAA/UnidadNegocio/Ges022-001-UnidadNegocio",
                "page_type": 2,
                "podname": "SynOperations"
            },
            {
                "environment_id": 1,
                "permission_id": 673,
                "token": "M3_si_Productos",
                "action": 1,
                "page_url": "/FrontEnd/Modulos/TraficoAA/Productos/Ges022-001-RegistroProductos",
                "page_type": 2,
                "podname": "SynOperations"
            },
            {
                "environment_id": 1,
                "permission_id": 674,
                "token": "M3_si_Prov_Extranjeros",
                "action": 1,
                "page_url": "/FrontEnd/Modulos/TraficoAA/Proveedores/ProveedorExtranjero/Ges022-001-ProveedorExtranjero",
                "page_type": 2,
                "podname": "SynOperations"
            },
            {
                "environment_id": 1,
                "permission_id": 675,
                "token": "M3_si_Prov_Nacionales",
                "action": 1,
                "page_url": "/FrontEnd/Modulos/TraficoAA/Proveedores/ProveedorNacional/Ges022-001-ProveedorNacional",
                "page_type": 2,
                "podname": "SynOperations"
            },
            {
                "environment_id": 1,
                "permission_id": 676,
                "token": "M3_si_Destinatarios",
                "action": 1,
                "page_url": "/FrontEnd/Modulos/TraficoAA/Destinatarios/Ges022-001-Destinatarios",
                "page_type": 2,
                "podname": "SynOperations"
            },
            {
                "environment_id": 1,
                "permission_id": 677,
                "token": "M3_si_Apendices",
                "action": 1,
                "page_url": "/FrontEnd/Modulos/TraficoAA/AltaApendices/Ges022-001-RegistroApendices",
                "page_type": 2,
                "podname": "SynOperations"
            },
            {
                "environment_id": 1,
                "permission_id": 678,
                "token": "M3_i_Otros",
                "action": 1
            },
            {
                "environment_id": 1,
                "permission_id": 679,
                "token": "M3_si_Ejecutivos",
                "action": 1,
                "page_url": "/FrontEnd/Modulos/TraficoAB/Ejecutivos",
                "page_type": 1,
                "podname": "Undefined"
            },
            {
                "environment_id": 1,
                "permission_id": 680,
                "token": "M3_si_Transportistas",
                "action": 1,
                "page_url": "/FrontEnd/Modulos/TraficoAB/Transportistas",
                "page_type": 1,
                "podname": "Undefined"
            },
            {
                "environment_id": 1,
                "permission_id": 681,
                "token": "M3_si_Tipos_Doc_Vucem",
                "action": 1,
                "page_url": "/FrontEnd/Modulos/TraficoAB/Tipos-doc-vucem",
                "page_type": 1,
                "podname": "Undefined"
            },
            {
                "environment_id": 1,
                "permission_id": 682,
                "token": "M4_Despacho",
                "action": 1
            },
            {
                "environment_id": 1,
                "permission_id": 683,
                "token": "M4_i_Consolidados",
                "action": 1,
                "page_url": "/FrontEnd/Modulos/TraficoAA/ControlConsolidados/Ges022-001-ControlConsolidados",
                "page_type": 2,
                "podname": "SynOperations"
            },
            {
                "environment_id": 1,
                "permission_id": 684,
                "token": "M4_i_Copias_simples",
                "action": 1,
                "page_url": "/FrontEnd/Modulos/TraficoAA/CopiaSimple/Ges022-001-CopiasSimples",
                "page_type": 2,
                "podname": "SynOperations"
            },
            {
                "environment_id": 1,
                "permission_id": 685,
                "token": "M4_i_Partes_II",
                "action": 1,
                "page_url": "/FrontEnd/Modulos/TraficoAA/PartesII/Ges022-001-PartesII",
                "page_type": 2,
                "podname": "SynOperations"
            },
            {
                "environment_id": 1,
                "permission_id": 686,
                "token": "M4_i_DODA",
                "action": 1,
                "page_url": "/FrontEnd/Modulos/TraficoAA/DODA/Ges022-001-DODA",
                "page_type": 2,
                "podname": "SynOperations"
            },
            {
                "environment_id": 1,
                "permission_id": 687,
                "token": "M5_Ajustes",
                "action": 1
            },
            {
                "environment_id": 1,
                "permission_id": 688,
                "token": "M5_i_Cubo_de_datos",
                "action": 1,
                "page_url": "/FrontEnd/Modulos/TraficoAA/CuboDatos/Ges022-001-CuboDatos",
                "page_type": 2,
                "podname": "SynOperations"
            },
            {
                "environment_id": 1,
                "permission_id": 689,
                "token": "M6_Acceso",
                "action": 1
            },
            {
                "environment_id": 1,
                "permission_id": 690,
                "token": "M6_i_Usuarios",
                "action": 1,
                "page_url": "/FrontEnd/Modulos/TraficoAB/Usuarios",
                "page_type": 1,
                "podname": "Undefined"
            },
            {
                "environment_id": 1,
                "permission_id": 691,
                "token": "M6_i_Roles",
                "action": 1,
                "page_url": "/FrontEnd/Modulos/TraficoAB/Roles",
                "page_type": 1,
                "podname": "Undefined"
            },
            {
                "environment_id": 1,
                "permission_id": 692,
                "token": "M6_i_Permisos",
                "action": 1,
                "page_url": "/FrontEnd/Modulos/TraficoAB/Permisos",
                "page_type": 1,
                "podname": "Undefined"
            },
            {
                "environment_id": 1,
                "permission_id": 693,
                "token": "M6_i_Perfiles",
                "action": 1
            },
            {
                "environment_id": 1,
                "permission_id": 694,
                "token": "M7_Web_API",
                "action": 1
            },
            {
                "environment_id": 1,
                "permission_id": 695,
                "token": "M7_i_API_Digitalizacion",
                "action": 1
            },
            {
                "environment_id": 1,
                "permission_id": 696,
                "token": "M7_i_API_CustomsBroker",
                "action": 1
            },
            {
                "environment_id": 1,
                "permission_id": 697,
                "token": "M7_i_API_Datasets",
                "action": 1
            },
            {
                "environment_id": 1,
                "permission_id": 698,
                "token": "M7_i_API_Configuration",
                "action": 1
            },
            {
                "environment_id": 1,
                "permission_id": 699,
                "token": "M7_i_API_Tasks",
                "action": 1
            },
            {
                "environment_id": 1,
                "permission_id": 500,
                "token": "M0_Environment",
                "action": 1
            },
            {
                "environment_id": 1,
                "permission_id": 501,
                "token": "M0_Configuracion_aplicacion",
                "action": 1
            },
            {
                "environment_id": 1,
                "permission_id": 502,
                "token": "M0_Can_Change_Office",
                "action": 1
            },
            {
                "environment_id": 1,
                "permission_id": 700,
                "token": "M3_si_Consignatarios",
                "action": 1,
                "page_url": "/FrontEnd/Modulos/TraficoAB/Consignatarios",
                "page_type": 1,
                "podname": "Undefined"
            },
            {
                "environment_id": 1,
                "permission_id": 701,
                "token": "M3_si_INPC",
                "action": 1,
                "page_url": "/FrontEnd/Modulos/TraficoAB/INPC",
                "page_type": 1,
                "podname": "Undefined"
            },
            {
                "environment_id": 1,
                "permission_id": 702,
                "token": "M3_si_Tipo_Cambio",
                "action": 1,
                "page_url": "/FrontEnd/Modulos/TraficoAB/TiposCambio",
                "page_type": 1,
                "podname": "Undefined"
            },
            {
                "environment_id": 1,
                "permission_id": 703,
                "token": "M3_si_Factores_Moneda",
                "action": 1,
                "page_url": "/FrontEnd/Modulos/TraficoAB/FactoresMoneda",
                "page_type": 1,
                "podname": "Undefined"
            },
            {
                "environment_id": 1,
                "permission_id": 704,
                "token": "M3_si_Catalogo_Estatus_Operacion",
                "action": 1,
                "page_url": "/FrontEnd/Modulos/TraficoAB/CatalogoEstatus",
                "page_type": 1,
                "podname": "Undefined"
            },
            {
                "environment_id": 1,
                "permission_id": 705,
                "token": "M1_i_Estatus_Operacion",
                "action": 1,
                "page_url": "/FrontEnd/Modulos/TraficoAB/EstatusOperaciones",
                "page_type": 1,
                "podname": "Undefined"
            }
        ];
    }

    const permisosMap = {};
    permisos.forEach(p => {
        permisosMap[p.token] = p;
    });

    const isLocal = window.location.hostname === 'localhost';

    isLocal ? console.warn('Se usa localhost') : null;

    document.querySelectorAll('[data-token]').forEach(el => {

        const token = el.getAttribute('data-token');
        const permiso = permisosMap[token];

        if (!permiso || permiso.action !== 1) {
            const li = el.closest('li');
            if (li) li.style.display = 'none';
            return;
        }

        if (permiso.page_url) {
            let anchor = el.tagName === 'A' ? el : el.closest('a');
            if (anchor) {
                anchor.setAttribute('href', permiso.page_url);
            }
        }
    });

    const currentUrl = window.location.pathname.toLowerCase();

    document.querySelectorAll('.sidebar-menu a[href]').forEach(link => {

        const href = link.getAttribute('href');

        if (!href || href === '#') return;

        const linkPath = href.toLowerCase();

        if (currentUrl === linkPath || currentUrl.includes(linkPath)) {

            const currentLi = link.closest('li');

            if (currentLi) {
                currentLi.classList.add('active-pink');
            }

            let parentTree = currentLi?.closest('.treeview');

            while (parentTree) {

                parentTree.classList.add('active');
                parentTree.classList.add('active-pink');

                parentTree = parentTree.parentElement.closest('.treeview');
            }
        }

    });

});

    function isSidebarCollapsed() {
        return body.classList.contains('sidebar-collapse');
    }

    function openSidebar() {
        body.classList.remove('sidebar-collapse');
    }

    function closeSidebar() {
        body.classList.add('sidebar-collapse');
    }


    document.querySelectorAll('.sidebar-menu > li > a.buttonsMenu').forEach(item => {
        item.addEventListener('click', function (e) {

            const hasSubmenu = this.classList.contains('has-submenu');

            if (hasSubmenu && isSidebarCollapsed()) {
                openSidebar();
            }

        });
    });

    document.querySelectorAll('.treeview-menu .has-submenu').forEach(item => {
        item.addEventListener('click', function (e) {

            if (isSidebarCollapsed()) {
                openSidebar();
            }

            e.stopPropagation();
        });
    });


    document.querySelectorAll('.treeview-menu li a:not(.has-submenu)').forEach(item => {
        item.addEventListener('click', function () {

            closeSidebar();
        });
    });