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
    }).then(res => location.assign("/MenuXtreme/MenuXtreme"))
}

function vsJugadorXtreme() {
    let opcion = new FormData()
    opcion.append("opcion", "vsJugadorXtreme");
    fetch('/MenuPrincipal/opcionSeleccionada', {
        method: 'POST',
        body: opcion
    }).then(res => location.assign("/MenuXtreme/MenuXtreme"))
}

function campeonato() {
    let opcion = new FormData()
    opcion.append("opcion", "campeonato");
    fetch('/MenuPrincipal/opcionSeleccionada', {
        method: 'POST',
        body: opcion
    }).then(res => location.assign("/MenuCampeonato/MenuCampeonato"))
}

function irPerfil() {
    location.assign('/Perfil/Perfil')
}

function cerrarSesion() {
    fetch('/Tablero/CerrarSesion', {
        method: 'POST',
    }).then(res => location.assign('/Loging/Loging'))
}
