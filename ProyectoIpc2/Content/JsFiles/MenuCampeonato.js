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
        .then(respuesta => location.assign("/Tablero/Tablero"));
}

function empezarCampeonato() {
    let nombreCampeonato = document.getElementById("nombreCampeonato").value;
    let numeroEquipos = document.getElementById("numeroEquipos").options[document.getElementById("numeroEquipos").selectedIndex].text
    let campeonatoInfo = new FormData()
    if (nombreCampeonato != "") {
        campeonatoInfo.append("nombreCampeonato", nombreCampeonato)
        campeonatoInfo.append("numeroEquipos", numeroEquipos)
        fetch('/MenuCampeonato/IniciarCampeonato', {
            method: 'POST',
            body: campeonatoInfo
        }).then(respuesta => location.assign("/Tablero/Tablero"));
    } else {
        alert("El nombre del campeonato no puede estar vacio.")
    }
}

function regresar() {
    location.assign("/MenuPrincipal/MenuPrincipal")
}