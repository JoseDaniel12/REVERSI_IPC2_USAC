function salir() {
    fetch('/Tablero/Salir', {
        method: 'POST',
    }).then(respuesta => location.assign("/MenuPrincipal/MenuPrincipal"));
}

function desempatar() {
    let jugadorEquipo1 = document.getElementById("jugadorEquipo1").options[document.getElementById("jugadorEquipo1").selectedIndex].text
    let jugadorEquipo2 = document.getElementById("jugadorEquipo2").options[document.getElementById("jugadorEquipo2").selectedIndex].text
    let form = new FormData();
    form.append("jugadorEquipo1", jugadorEquipo1)
    form.append("jugadorEquipo2", jugadorEquipo2)
    fetch('/Empate/Desempatar', {
        method: 'POST',
        body: form
    }).then(respuesta => location.assign("/Tablero/Tablero"))
}