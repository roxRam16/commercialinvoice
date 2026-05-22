<%@ Page Title="" Language="vb" AutoEventWireup="false" CodeBehind="404.aspx.vb" Inherits=".PageNotFound" %>

<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="utf-8" />
    <title>404 | Página no encontrada</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />

    <link rel="stylesheet" href="/FrontEnd/Librerias/BootstrapV3/dist/css/bootstrap.min.css" />

    <style>
        html, body {
            height: 100%;
            margin: 0;
        }

        body {
            background: linear-gradient(45deg, #722c62 0%, #730059 45%, #360056 100%);
            display: flex;
            align-items: center;
            justify-content: center;
            font-family: system-ui, -apple-system, "Segoe UI", sans-serif;
        }

        .error-box {
            max-width: 500px;
            padding: 44px 36px;
            text-align: center;
            color: #fff;
            animation: fadeUp .6s ease-out;
        }

        .error-code {
            font-size: clamp(48px, 7vw, 72px);
            font-weight: 700;
            letter-spacing: -0.5px;
            opacity: .7;
            margin-bottom: 10px;
        }

        .error-title {
            font-size: 23px;
            font-weight: 700;
            margin-bottom: 8px;
        }

        .error-description {
            font-size: 12.5px;
            line-height: 1.6;
            color: rgba(255,255,255,.85);
            margin-bottom: 26px;
            font-style: italic;
        }

        .btn-home {
            padding: 11px 30px;
            border-radius: 26px;
            background: rgba(255,255,255,.14);
            color: #fff;
            border: 1px solid rgba(255,255,255,.35);
            font-size: 13.5px;
            transition: all .25s ease;
            text-decoration: none;
        }

        .btn-home:hover {
            background: #fff;
            color: #360056;
            text-decoration: none;
        }

        .imgPrincipal{
            width:18em;
            animation: fadeUp .6s ease-out;
        }

        .zoneError{
            display: flex;
            flex-direction: row;
            width: 100%;
            align-items: center;
            text-align: center;
        }

        .icon-back {
            margin-right: 8px;
            font-size: 14px;
            position: relative;
            top: 1px;
            transition: transform .25s ease;
        }

        .btn-home:hover .icon-back {
            transform: translateX(-4px);
        }

        @keyframes fadeUp {
            from { opacity: 0; transform: translateY(14px); }
            to { opacity: 1; transform: translateY(0); }
        }

        @media (max-width: 768px) {

            .zoneError {
                flex-direction: column;
            }

            .imgPrincipal {
                width: 14em;      
                margin-bottom: 20px;
                animation: fadeUp .6s ease-out;
            }

            .error-box {
                padding: 24px 18px;
            }
        
        }

    </style>
</head>

<body>

    <section class="containerError">

        <div class="zoneError">
            <image class="imgPrincipal" src="/FrontEnd/Recursos/Imgs/404.png" />
            <section class="error-box">
                <div class="error-code"></div>

                <div class="error-title">
                    Ups... esta página se perdio
                </div>

                <div class="error-description">
                    Tal vez el enlace está roto o la ruta no existe
                </div>

                <a href="#" class="btn-home" onclick="history.back(); return false;">
                    <span class="glyphicon glyphicon-arrow-left icon-back"></span>
                    Volver
                </a>

            </section>

        </div>

    </section>

</body>
</html>