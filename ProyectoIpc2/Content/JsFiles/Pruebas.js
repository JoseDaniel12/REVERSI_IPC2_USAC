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
    for (let y = 0; y < 8; y++) {
        for (let x = 0; x < 8; x++) {
            let id = x + "" + y
            let boton = document.getElementById(id)
            switch (tablero[y][x]) {
                case 1:
                    boton.style.backgroundColor = "black";
                    break
                case 2:
                    boton.style.backgroundColor = "whitesmoke";
                    break
                default:
                    boton.style.backgroundColor = "#009067";
                    break
            }
        }
    }

    for (let i = 0; i < tirosPosibles.length; i++) {
        id = tirosPosibles[i][0] + "" + tirosPosibles[i][1]
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

    if ((tipoPartida == "vsPc") && ((turno == 1 && jugador_negro == "PC") || (turno == 2 && jugador_blanco == "PC")) && (haTerminado == false)) {
        fetch('/Tablero/PcPlayerMove', {
            method: 'POST',
        }).then(respuesta => respuesta.json())
            .then(respuesta => renderBoard(respuesta))
    }
    
    if (haTerminado == true) {
        if (player1Points > player2Points) {
            turnState.innerHTML = "Gandor: Negro"
        } else if (player2Points > player1Points) {
            turnState.innerHTML = "Gandor: Blanco"
        } else {
            turnState.innerHTML = "EMPATE"
        }
    }

}

function casillaPresionada(e) {
    let coordenada = new FormData();
    coordenada.append("coordenada", e.id);
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
      .then(respuesta => renderBoard(respuesta));
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
    }).then(respuesta => respuesta.json())
      .then(respuesta => renderBoard(respuesta));
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

function cambiarNombre() {
    let nombre = document.getElementById("nombreIngreso").value
    if (nombre != "" && nombre != document.getElementsByClassName("black_Name")[0].innerHTML && nombre != document.getElementsByClassName("white_Name")[0].innerHTML) {
        let nombreInfo = new FormData();
        nombreInfo.append("nombre", document.getElementById("nombreIngreso").value)
        fetch('/Tablero/CambiarNombre', {
            method: 'POST',
            body: nombreInfo
        }).then(res => res.json())
            .then(function (res) {
                if (res == 1) {
                    document.getElementsByClassName("white_Name")[0].innerHTML = nombre

                } else {
                    document.getElementsByClassName("black_Name")[0].innerHTML = nombre
                }
                document.getElementById("nombreIngreso").value = "";
            });
    } else {
        alert("Nombre no valido, o ya asignado.")
    }
}

function sleep(ms) {
    return new Promise(resolve => setTimeout(resolve, ms));
}

function salir() {
    fetch('/Tablero/Salir', {
        method: 'POST',
    }).then(respuesta => location.assign("/MenuPrincipal/MenuPrincipal"));
}