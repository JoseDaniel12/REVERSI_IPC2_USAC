﻿function renderBoard(tablero) {
    for (let y = 0; y < 8; y++) {
        for (let x = 0; x < 8; x++) {
            let id = x + "" + y
            let boton = document.getElementById(id);
            switch (tablero[x][y]) {
                case 1:
                    boton.style.backgroundColor = "whitesmoke";
                    break
                case 2:
                    boton.style.backgroundColor = "black";
                    break
                default:
                    boton.style.backgroundColor = "";
                    break
            }
        }
    }
}

function casillaPresionada(e) {
    let url = '/Tablero/CaillaPresionada';
    let coordenada = new FormData();
    coordenada.append("coordenada", e.id);

    fetch(url, {
        method: 'POST',
        body: coordenada
    }).then(respuesta => respuesta.json())
      .then(respuesta => renderBoard(respuesta));
}

function guardarPartida() {
    let tableroInfo = new FormData();
    for (let y = 0; y < 8; y++) {
        for (let x = 0; x < 8; x++) {
            let id = x + "" + y
            let boton = document.getElementById(id)
            switch (boton.style.backgroundColor) {
                case "whitesmoke":
                    tableroInfo.append(id, 1)
                    break
                case "black":
                    tableroInfo.append(id, 2)
                    break
                default:
                    tableroInfo.append(id, -1)
            }
        }
    }

    fetch('/Tablero/GuardarPartida', {
        method: 'POST',
        body: tableroInfo
    }).then(respuesta => respuesta.json());
    alert("Se ha guardado tu partida")
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