function vsPc() {
    let opcion = new FormData()
    opcion.append("opcion", "vsPc");
    fetch('/MenuPrincipal/opcionSeleccionada', {
        method: 'POST',
        body: opcion
    }).then(res => location.assign("/Tablero/Tablero"))
}

function vsJugador() {
    let opcion = new FormData()
    opcion.append("opcion", "vsJugador");
    fetch('/MenuPrincipal/opcionSeleccionada', {
        method: 'POST',
        body: opcion
    }).then(res => location.assign("/Tablero/Tablero"))
}

function vsPcXtreme() {
    let opcion = new FormData()
    opcion.append("opcion", "vsPcXtreme");
    fetch('/MenuPrincipal/opcionSeleccionada', {
        method: 'POST',
        body: opcion
    }).then(res => location.assign("/Tablero/Tablero"))
}

function vsJugadorXtreme() {
    let opcion = new FormData()
    opcion.append("opcion", "vsJugadorXtreme");
    fetch('/MenuPrincipal/opcionSeleccionada', {
        method: 'POST',
        body: opcion
    }).then(res => location.assign("/Tablero/Tablero"))
}

function cerrarSesion() {
    fetch('/Tablero/CerrarSesion', {
        method: 'POST',
    }).then(res => location.assign('/Loging/Loging'))
}