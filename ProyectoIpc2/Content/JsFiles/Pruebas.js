var cronometro
prepararTablero()

async function renderBoard(info) {
    let tablero = JSON.parse(info["tablero"])
    let turno = JSON.parse(info["turno"])
    let player1MovesNumber = JSON.parse(info["player1MovesNumber"])
    let player2MovesNumber = JSON.parse(info["player2MovesNumber"])
    let player1Points = JSON.parse(info["player1Points"])
    let player2Points = JSON.parse(info["player2Points"])
    let tipoPartida = JSON.parse(info["tipoPartida"])
    let jugador_negro = JSON.parse(info["jugador_negro"])
    let jugador_blanco = JSON.parse(info["jugador_blanco"])
    let haTerminado = JSON.parse(info["haTerminado"])
    let tirosPosibles = JSON.parse(info["tirosPosibles"])
    let hostColor = JSON.parse(info["hostColor"])
    let ganador = JSON.parse(info["ganador"])
    let anchoTablero = JSON.parse(info["anchoTablero"])
    let altoTablero = JSON.parse(info["altoTablero"])
    let equipos = JSON.parse(info["equipos"])
    console.log(tablero)
    for (let y = 0; y < altoTablero; y++) {
        for (let x = 0; x < anchoTablero; x++) {
            let id = x + "_" + y
            let boton = document.getElementById(id)
            switch (tablero[y][x]) {
                case -1:
                    boton.style.backgroundColor = "#009067"; break // fondo de tablero
                case 1:
                    boton.style.backgroundColor = "black"; break // fondo negro
                case 2:
                    boton.style.backgroundColor = "whitesmoke"; break // fondo blanco
                case 3:
                    boton.style.backgroundColor = "red"; break // fondo rojo
                case 4:
                    boton.style.backgroundColor = "#F1C40F"; break // fondo amarillo
                case 5:
                    boton.style.backgroundColor = "#366EED"; break // fondo azul
                case 6:
                    boton.style.backgroundColor = "#ED9436"; break // fondo anaranjado
                case 7:
                    boton.style.backgroundColor = "#73ED36"; break // fondo verde
                case 8:
                    boton.style.backgroundColor = "#E236ED"; break // fondo violeta
                case 9:
                    boton.style.backgroundColor = "#36B6ED"; break // fondo celeste
                case 0:
                    boton.style.backgroundColor = "#5D6D7E"; break // fondo gris
            }
        }
    }   

    for (let i = 0; i < tirosPosibles.length; i++) {
        id = tirosPosibles[i][0] + "_" + tirosPosibles[i][1]
        document.getElementById(id).style.backgroundColor = "#307864";
    }

    let turnState = document.getElementById("turno")
    if (turno == 1) {
        turnState.innerHTML = "Turno: Negro"
    } else {
        turnState.innerHTML = "Turno: Blanco"
    }

    let player_movs1 = document.getElementById("player_movs1")
    let player_movs2 = document.getElementById("player_movs2")
    player_movs1.innerHTML = "Movimientos: " + player1MovesNumber
    player_movs2.innerHTML = "Movimientos: " + player2MovesNumber

    let player1Points_element = document.getElementById("player1Points")
    let player2Points_element = document.getElementById("player2Points")
    player1Points_element.innerHTML = "Puntos: " + player1Points
    player2Points_element.innerHTML = "Puntos: " + player2Points

    if (tipoPartida == "vsJugador") {
        document.getElementsByClassName("black_Name")[0].innerHTML = jugador_negro
        document.getElementsByClassName("white_Name")[0].innerHTML = jugador_blanco
        if (hostColor == 1) {
            document.getElementById("cambiarColor").classList.add("host_black")
        } else {
            document.getElementById("cambiarColor").classList.add("host_white")
        }
    }

    if ((tipoPartida == "vsPc" || tipoPartida == "vsPcXtreme") && ((turno == 1 && jugador_negro == "PC") || (turno == 2 && jugador_blanco == "PC")) && (haTerminado == false)) {
        fetch('/Tablero/PcPlayerMove', {
            method: 'POST',
        }).then(respuesta => respuesta.json())
            .then(respuesta => renderBoard(respuesta))
    }

    if (tipoPartida == "campeonato") {
        console.log(equipos)
        colocarEquipos(equipos)
    }
    
    if (haTerminado == true) {
        turnState.innerHTML = "Ganador: " + ganador
        clearInterval(cronometro)
        document.getElementById("cronometro").innerHTML = ""
        if (tipoPartida == "campeonato") {
            let elemento = document.getElementById("imponerGanador")
            if (elemento != null) {
                document.getElementById("info").removeChild(elemento);
            }
            let boton = document.createElement("button")
            boton.classList.add("info_option");
            boton.classList.add("host_black");
            boton.classList.add("continuar");
            boton.setAttribute("id", "cambiarColor")
            boton.setAttribute("onclick", "continuar()")
            boton.innerHTML = "Continuar"
            document.getElementById("info").appendChild(boton) 
                
            fetch('/Tablero/RefrescarFinal', {
                method: 'POST'
            }).then(respuesta => respuesta.json())
                .then((respuesta) => {
                    document.getElementById("equipo1").innerHTML = "Eq: " + respuesta["team1Name"] + " (" + respuesta["team1Points"] + "pts)"
                    document.getElementById("equipo2").innerHTML = "Eq: " + respuesta["team2Name"] + " (" + respuesta["team2Points"] + "pts)"
                })
                

        }
    }

}

async function casillaPresionada(e) {
    let coordenada = new FormData();
    coordenada.append("coordenada", e.id);

    let res = await fetch('/Tablero/ValidarTiro', {
        method: 'POST',
        body: coordenada
    }).then(res => res.json())
    .then(res => {
        if (res == true) {
            activarCronometro()
        }
    })

    fetch('/Tablero/CasillaPresionada', {
        method: 'POST',
        body: coordenada
    }).then(respuesta => respuesta.json())
      .then(respuesta => renderBoard(respuesta));
}

function guardarPartida() {
    let tableroInfo = new FormData();
    fetch('/Tablero/GuardarPartida', {
        method: 'POST',
        body: tableroInfo
    }).then(respuesta => alert("Se ha guardado tu partida"));
}

function cargarTablero(ruta) {
    let fileRoot = new FormData()
    fileRoot.append("fileRoot", ruta)

    fetch('/Tablero/cargarTablero', {
        method: 'POST',
        body: fileRoot
    }).then(respuesta => respuesta.json())
        .then(respuesta => construirTablero(respuesta))
}

function processSelectedFiles(fileInput) {
    let fileName = fileInput.files[0].name
    document.getElementById("choosedFile").innerHTML = fileName
}

function cargarPartida() {
    let fileRoot = "C:\\Users\\josed\\Downloads\\" + document.getElementById("choosedFile").innerHTML;
    let rootForm = new FormData();
    rootForm.append("fileRoot", fileRoot)
    fetch('/Tablero/CargarPartida', {
        method: 'POST',
        body: rootForm
    }).then(respuesta => location.assign("/Tablero/Tablero"))
}

function cambiarColor() {
    fetch('/Tablero/CambiarColor', {
        method: 'POST',
    }).then(respuesta => respuesta.json())
        .then(
            function (colorInfo) {
                let isChanged = JSON.parse(colorInfo["isChanged"])
                let hostColor = JSON.parse(colorInfo["hostColor"])
                if (isChanged) {
                    let nombreNegro = document.getElementsByClassName("black_Name")[0].innerHTML
                    let nombreBlanco = document.getElementsByClassName("white_Name")[0].innerHTML
                    document.getElementsByClassName("black_Name")[0].innerHTML = nombreBlanco
                    document.getElementsByClassName("white_Name")[0].innerHTML = nombreNegro
                    let boton = document.getElementById("cambiarColor")
                    if (hostColor == 2) {
                        boton.innerHTML = "Color del Host: Blanco"
                        boton.style.backgroundColor = "whitesmoke"
                        boton.style.color = "black"
                    } else {
                        boton.innerHTML = "Color del Host: Negro"
                        boton.style.backgroundColor = "black"
                        boton.style.color = "whitesmoke"
                    }
                } else {
                    alert("No puedes cambiar el color del host al ya haber empezado a jugar")
                }
            }
        );
        

}

function salir() {
    fetch('/Tablero/Salir', {
        method: 'POST',
    }).then(respuesta => location.assign("/MenuPrincipal/MenuPrincipal"));
}

function activarCronometro() {
    let s = document.getElementById("segundos")
    let m = document.getElementById("minutos")
    let tiempo = new FormData();
    tiempo.append("segundos", parseInt(s.innerHTML))
    tiempo.append("minutos", parseInt(m.innerHTML))
    fetch('/Tablero/RegistrarTiempo', {
        method: 'POST',
        body: tiempo
    })
    clearInterval(cronometro)

    s.innerHTML = "0"
    m.innerHTML = "0"
    let contador_s = 0
    let contador_m = 0
    cronometro = window.setInterval(function () {
        if (contador_s == 60) {
            contador_s = 0
            contador_m++
            m.innerHTML = contador_m
        }
        contador_s++
        s.innerHTML = contador_s
    }
    , 1000)
}


function prepararTablero() {
    fetch('/Tablero/PrepararTablero', {
        method: 'POST',
    }).then(respuesta => respuesta.json())
      .then(respuesta => construirTablero(respuesta))
}

function construirTablero(info) {
    let ancho = JSON.parse(info["anchoTablero"])
    let alto = JSON.parse(info["altoTablero"])
    let contenedor = document.getElementById("contendorTablero")
    contenedor.innerHTML = ""
    let numsText = ["cero", "uno", "dos", "tres", "cuatro", "cinco", "seis", "siete", "ocho", "nueve"]
    let abcdario = ["", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T","V"]

    for (let y = 0; y < alto + 2; y++) {
        let fila = document.createElement("div")
        fila.classList.add("fila")
        fila.setAttribute("id", numsText[y])
        for (let x = 0; x < ancho + 2; x++) {
            let contenedorCasilla = document.createElement("div")
            contenedorCasilla.classList.add("box")
            contenedorCasilla.setAttribute("id", numsText[x])
            if (x != 0 && x != ancho + 1 && y != 0 && y != alto + 1) {
                let boton = document.createElement("button")
                boton.classList.add("coin")
                boton.setAttribute("id", (x - 1) + "_" + (y - 1))
                boton.setAttribute("onclick", "casillaPresionada(this)")
                contenedorCasilla.appendChild(boton)
            } else {
                let texto = document.createElement("div")
                if (x > 0 && x < ancho + 1) {
                    texto.innerHTML = abcdario[x]
                } else if (y > 0 && y < alto + 1) {
                    texto.innerHTML = y
                }
                contenedorCasilla.appendChild(texto)
                if (x == 0 && y == 0) {
                    contenedorCasilla.classList.add("esquina_izq_sup")
                } else if (x == ancho + 1 && y == 0) {
                    contenedorCasilla.classList.add("esquina_der_sup")
                } else if (x == 0 && y == alto + 1) {
                    contenedorCasilla.classList.add("esquina_izq_inf")
                } else if (x == ancho + 1 && y == alto + 1) {
                    contenedorCasilla.classList.add("esquina_der_inf")
                }
                contenedorCasilla.classList.add("borde")
            }
            contenedorCasilla.style.width = (508 / (ancho + 2) - 1).toString() + "px"
            contenedorCasilla.style.height = (508 / (ancho + 2) - 1).toString() + "px"
            fila.appendChild(contenedorCasilla)
        }
        contenedor.appendChild(fila)
    }
    renderBoard(info)
}

function continuar() {
    fetch('/Tablero/Continuar', {
        method: 'POST',
    }).then(respuesta => respuesta.json())
        .then(
            (estadosCampeonato) => {
                //document.getElementById("info").removeChild(document.getElementById("cambiarColor"));
                if (estadosCampeonato["haTerminado"] == true) {
                    location.assign("/GanadorCampeonato/GanadorCampeonato")
                } else {
                    if (estadosCampeonato["hayEmpate"] == true) {
                        location.assign("/Empate/Empate")
                    } else {
                        location.assign("/Tablero/Tablero")
                    }
                }
            }
        )
}

function imponerGanador() {
    let jugadorGanador = document.getElementById("jugadorGanador").value
    let formGanador = new FormData();
    formGanador.append("jugadorGanador", jugadorGanador)
    fetch('/Tablero/ImponerGanador', {
        method: 'POST',
        body: formGanador
    }).then(respuesta => respuesta.json())
        .then(
            (estadosCampeonato) => {
                if (estadosCampeonato["ganadorCorrecto"] == true) {
                    if (estadosCampeonato["haTerminado"] == true) {
                        location.assign("/GanadorCampeonato/GanadorCampeonato")
                    } else {
                        location.assign("/Tablero/Tablero")
                    }
                } else {
                    alert("Nombre invalido, intente de nuevo.")
                }
            }
        )

}