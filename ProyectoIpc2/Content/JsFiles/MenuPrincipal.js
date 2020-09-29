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
