<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Login.aspx.vb" Inherits=".Login" %>

<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Synapsis</title>
    <link href="https://fonts.googleapis.com/css2?family=Montserrat:wght@300;400;600;700&display=swap" rel="stylesheet">
    <link href="https://fonts.googleapis.com/icon?family=Material+Icons" rel="stylesheet">
    <link rel="icon" href="/FrontEnd/Recursos/Imgs/ico_Synapsis.png" type="image/x-icon">
    <link rel="stylesheet" href="/FrontEnd/Librerias/Krom/css/stylesnew.css">
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0-beta3/css/all.min.css" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/aos/2.3.4/aos.css" />
    <link href="https://cdn.jsdelivr.net/npm/aos@2.3.4/dist/aos.css" rel="stylesheet">
</head>
<body>

    <form method="post" id="loginForm">

        <video autoplay muted loop playsinline id="background-video">
            <source src="/FrontEnd/Recursos/Videos/background-login.mp4" type="video/mp4" />
            Tu navegador no soporta videos HTML5.
        </video>


        <div id="gradient-overlay"></div>
        
        <div class="loader-overlay" id="loader">
            <div class="loader">

                <svg viewBox="-1 0 26 26"
                     xmlns="http://www.w3.org/2000/svg"
                     class="container">

                    <path class="track"
                          d="M333,744 C331.23,744 329.685,744.925 328.796,746.312 L323.441,743.252 C323.787,742.572 324,741.814 324,741 C324,740.497 323.903,740.021 323.765,739.563 L329.336,736.38 C330.249,737.37 331.547,738 333,738 C335.762,738 338,735.762 338,733 C338,730.238 335.762,728 333,728 C330.238,728 328,730.238 328,733 C328,733.503 328.097,733.979 328.235,734.438 L322.664,737.62 C321.751,736.631 320.453,736 319,736 C316.238,736 314,738.238 314,741 C314,743.762 316.238,746 319,746 C320.14,746 321.179,745.604 322.02,744.962 L328.055,748.46 C328.035,748.64 328,748.814 328,749 C328,751.762 330.238,754 333,754 C335.762,754 338,751.762 338,749 C338,746.238 335.762,744 333,744"
                          transform="translate(-314, -728)" />

                    <path class="car"
                          d="M333,744 C331.23,744 329.685,744.925 328.796,746.312 L323.441,743.252 C323.787,742.572 324,741.814 324,741 C324,740.497 323.903,740.021 323.765,739.563 L329.336,736.38 C330.249,737.37 331.547,738 333,738 C335.762,738 338,735.762 338,733 C338,730.238 335.762,728 333,728 C330.238,728 328,730.238 328,733 C328,733.503 328.097,733.979 328.235,734.438 L322.664,737.62 C321.751,736.631 320.453,736 319,736 C316.238,736 314,738.238 314,741 C314,743.762 316.238,746 319,746 C320.14,746 321.179,745.604 322.02,744.962 L328.055,748.46 C328.035,748.64 328,748.814 328,749 C328,751.762 330.238,754 333,754 C335.762,754 338,751.762 338,749 C338,746.238 335.762,744 333,744"
                          transform="translate(-314, -728)" />
                </svg>
            </div>
        </div>

        <div class="contenedor">

            <div class="contenedor-base">

                <span class="contenedor-logo">
                    <div class="logo-wrapper">
                        <div class="logo-out">
                            <img src="/FrontEnd/Recursos/Imgs/logo.png" alt="Logo" class="logo-img glow-animated" data-aos="fade-right" />
                        </div>
                        <div class="logo-text-out">
                            <p class="logo-text glow-animated" data-aos="fade-right" data-aos-delay="200">¡Bienvenidos al futuro!</p>
                        </div>

                    </div>
                </span>

                <div class="contenedor-login">

                    <div class="bloque-login">
                        <div class="text-out-titulo bloque-login-text contenedor-general align-mid fade-in-right delay-1">
                            <h2>Iniciar sesión</h2>
                        </div>

                               <div class="contenedor-general correo-input-out fade-in-right delay-2">
                                      <div class="form-group correo-input-out">
                                           <label for="email" class="input-label">Correo</label>
                                                     <input type="email"
                                                            placeholder="user.name@kromaduanal.com"
                                                            class="input-field"
                                                            id="user"
                                                            name="user"
                                                            value="<%= If(Request.Form("user") Is Nothing, "", HttpUtility.HtmlEncode(Request.Form("user").ToString())) %>" /> <%-- This is where the email value is injected --%>
                                                           <span class="error-message" id="emailError"></span>
                                    </div>
                        </div>

                        <div class="contenedor-general password-input-out password-wrapper fade-in-right delay-3 ">
                            <div class="form-group password-input-out" >
                                <label for="password" class="input-label">Contraseña</label>
                                <div class="input-wrapper">
                                    <input type="password"
                                           placeholder="••••••••"
                                           id="password"
                                           name="password"
                                           class="input-field" />
                                    <span class="material-icons toggle-password" onclick="togglePassword()">visibility_off</span>
                                </div>

                                <span class="error-message" id="passwordError"></span>
                            </div>
                        </div>

                        <div class="contenedor-general bloque-switch-out fade-in-right delay-4">
                            <div class="checkbox-group">
                                <div class="left-group">
                                    <input class="switch" type="checkbox" id="remember-switch" checked>
                                    <label for="remember-switch" class="switch-label">Recordarme</label>
                                </div>
                                <a href="#" class="forgot-password forgot-password-desktop">¿Olvidaste tu contraseña?</a>
                            </div>
                        </div>

                        <div class="contenedor-general acceder-out">
                            <button class="btn-acceder fade-in-right delay-5"
                                    onclick="handleLogin()">
                                Acceder
                            </button>
                        </div>

                        <a href="#" class="forgot-password forgot-password-mobile">¿Olvidaste tu contraseña?</a>
                        
                    </div>

                    <div class="bloque-redes">
                        <div class="contenedor-general contenedor-redes redes-out align-mid">
                                <a class="button" href="http://kromaduanal.com/" target="_blank">
                                <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 132 126" fill="currentColor">
                                    <path d="M1,82c0-10.7,0-21.4,0.33-32.24c1.89-3.97,3.05-8.01,5.03-11.53C16.36,20.85,30.24,8.21,50.04,2.9
                                    c0.46-0.12,0.65-1.24,0.96-1.9h33.24c0.65,0.77,1.05,1.45,1.66,1.62c23.41,7.07,37.88,22.69,44.27,46.1
                                    c0.13,0.46,1.25,0.66,1.83,0.98v30.24c-1.45,3.11-2.47,6.09-3.71,8.99c-6.11,14.33-15.87,25.54-29.03,33.86
                                    c-0.52,0.31-0.65,1.26-0.96,1.93H88.76c-2.89-6.4-2.26-7.44,3.72-10.89c9.26-5.35,16.81-12.49,21.3-21.38
                                    c-3.12-2.06-5.88-3.88-8.6-5.68c-11.33,22.46-38.79,28.32-54.99,19.66c1.71-3.05,3.43-6.11,5.11-9.11
                                    c27.35,8.05,50.56-15.16,42.66-42.01c-1.52,1.1-3.12,2.92-5.09,3.56c-2.83,0.91-4.58-0.83-6.16-3.47
                                    c-7.32-12.22-17.72-13.34-27.68-10.31c-10.34,3.13-15.69,13.5-14.15,24.08c1.23,8.47,6.38,13.45,13.09,17.37
                                    c-7.28,12.42-14.49,24.72-21.73,37.07c-1.09-0.51-2.58-1.05-3.89-1.86C17.41,112.72,7.37,99.97,2.9,82.97
                                    C2.78,82.51,1.65,82.32,1,82z" />
                                </svg>
                            </a>

                                <a class="button" href="https://www.facebook.com/kromaduanalylogistica/?locale=es_LA" target="_blank">
                                <svg viewBox="38.657999999999994 12.828 207.085 207.085" xmlns="http://www.w3.org/2000/svg" fill="currentColor">
                                    <g id="SVGRepo_bgCarrier" stroke-width="0"></g>
                                    <g id="SVGRepo_tracerCarrier" stroke-linecap="round" stroke-linejoin="round"></g>
                                    <g id="SVGRepo_iconCarrier">
                                        <path d="M158.232 219.912v-94.461h31.707l4.747-36.813h-36.454V65.134c0-10.658 2.96-17.922 18.245-17.922l19.494-.009V14.278c-3.373-.447-14.944-1.449-28.406-1.449-28.106 0-47.348 17.155-47.348 48.661v27.149H88.428v36.813h31.788v94.461l38.016-.001z" fill="currentColor">
                                        </path>
                                    </g>
                                </svg>
                            </a>

                                <a href="https://www.linkedin.com/company/krom-aduanal-y-log%C3%ADstica/?originalSubdomain=mx" target="_blank" class="button">
                                <svg xmlns="http://www.w3.org/2000/svg" fill="currentColor" viewBox="-271 283.9 256 235.1">
                                    <g>
                                        <rect x="-264.4" y="359.3" width="49.9" height="159.7" />
                                        <path d="M-240.5,283.9c-18.4,0-30.5,11.9-30.5,27.7c0,15.5,11.7,27.7,29.8,27.7h0.4c18.8,0,30.5-12.3,30.4-27.7
      C-210.8,295.8-222.1,283.9-240.5,283.9z" />
                                        <path d="M-78.2,357.8c-28.6,0-46.5,15.6-49.8,26.6v-25.1h-56.1c0.7,13.3,0,159.7,0,159.7h56.1v-86.3c0-4.9-0.2-9.7,1.2-13.1
      c3.8-9.6,12.1-19.6,27-19.6c19.5,0,28.3,14.8,28.3,36.4V519h56.6v-88.8C-14.9,380.8-42.7,357.8-78.2,357.8z" />
                                    </g>
                                </svg>
                            </a>                                

                        </div>
                    </div>
                </div>

            </div>
        </div>
    </form>

        <script src="https://cdnjs.cloudflare.com/ajax/libs/aos/2.3.4/aos.js"></script>
        
        <script>
            AOS.init();
        </script>

        <script>
            AOS.init({
                once: true,
                duration: 1200,
                easing: 'ease-out',
            });
        </script>

        <script>

            function togglePassword() {

              

                const passwordField = document.getElementById("password");

                const toggleIcon = document.querySelector(".toggle-password");

                if (passwordField.type === "password") {

                    passwordField.type = "text";

                    toggleIcon.textContent = "visibility";

                } else {

                    passwordField.type = "password";

                    toggleIcon.textContent = "visibility_off";

                }

            }

            function handleLogin() {

                

                event.preventDefault();

                //Validación del formulario

                

                const emailInput = document.getElementById('user');

                const passwordInput = document.getElementById('password');

                const emailError = document.getElementById('emailError');

                const passwordError = document.getElementById('passwordError');

                emailError.textContent = '';

                passwordError.textContent = '';

                emailInput.classList.remove('error');

                passwordInput.classList.remove('error');

                let isValid = true;

                const email = emailInput.value.trim();

                const password = passwordInput.value.trim();

                const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

                if (!email) {

                    emailError.textContent = 'El correo es obligatorio. ';

                    emailInput.classList.add('error');

                    isValid = false;

                } else if (!emailRegex.test(email)) {

                    emailError.textContent = 'El formato del correo no es válido.';

                    emailInput.classList.add('error');

                    isValid = false;

                }


                if (!password) {

                    passwordError.textContent = 'La contraseña es obligatoria.';

                    passwordInput.classList.add('error');

                    isValid = false;

                }

                if (isValid) {

                    //Animaciones de desvanecido al acceder

                    const logoImg = document.querySelector('.logo-out');

                    const logoText = document.querySelector('.logo-text-out');

                    const contenedorLogin = document.querySelector('.contenedor-login');

                    const titulo = document.querySelector('.text-out-titulo');

                    const correoInput = document.querySelector('.correo-input-out');

                    const bloqueSwitch = document.querySelector('.bloque-switch-out');

                    const btnAcceder = document.querySelector('.acceder-out');

                    const redes = document.querySelector('.redes-out');

                    const loader = document.getElementById('loader');

                    const loginForm = document.getElementById('loginForm');

                    logoImg.classList.add('fade-out-left', 'fade-out-left-0');

                    logoText.classList.add('fade-out-left');

                    contenedorLogin.classList.add('fade-out-right');

                    titulo.classList.add('fade-out-right', 'fade-out-right-0');

                    correoInput.classList.add('fade-out-right', 'fade-out-right-1');

                    bloqueSwitch.classList.add('fade-out-right', 'fade-out-right-3');

                    btnAcceder.classList.add('fade-out-right', 'fade-out-right-4');

                    redes.classList.add('fade-out-right', 'fade-out-right-5');

                                              

                  setTimeout(() => {
                        loader.classList.add('active');
                        if (loginForm) {

                              <% Session("tbLogin_") = If(Request.Form("user") Is Nothing, "", Request.Form("user").ToString) %>

                            loginForm.submit();
                        }
                    }, 500);
                }

            }

            // Solución del navegador Edge

            const isEdge = /Edg/.test(navigator.userAgent);

            if (isEdge) {

                const toggleIcon = document.querySelector('.toggle-password');

                if (toggleIcon) {

                    toggleIcon.style.display = 'none';

                }

            }


        </script>

    <%-- Asegúrate de que 'mensaje' sea una propiedad pública o de ámbito de página en tu Code-Behind --%>
<% If Not String.IsNullOrEmpty(Session("fallaLogin_")) Then %>
    <div id="toast-container" role="alert" aria-live="assertive" aria-atomic="true">
        <div class="toast-header">
            <span class="material-icons icon">error_outline</span>
            <strong class="toast-title">Error</strong>
            <button aria-label="Cerrar notificación" class="toast-close" onclick="closeToast()">
                <span class="material-icons">close</span>
            </button>
        </div>
        <div class="toast-body"><%=Session("fallaLogin_") %></div> <%-- Se imprime directamente la variable --%>
    </div>

    <script>
        document.addEventListener("DOMContentLoaded", function () {
            const toast = document.getElementById("toast-container");

            toast.classList.add("show");

            window.closeToast = function () {
                toast.classList.remove("show");
                toast.classList.add("toast-hidden");
            }

            setTimeout(() => {
                toast.classList.remove("show");
                toast.classList.add("toast-hidden");
            }, 6000);
        });
    </script>
<% End If %>

</body>
</html>