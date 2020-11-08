function ajustarConfsXtreme() {
    let confs = new FormData()
    confs.append("anchoTablero", document.getElementById("anchoTablero").options[document.getElementById("anchoTablero").selectedIndex].text)
    confs.append("altoTablero", document.getElementById("altoTablero").options[document.getElementById("altoTablero").selectedIndex].text)
    confs.append("modalidad", document.getElementById("modalidad").options[document.getElementById("modalidad").selectedIndex].text)
    confs.append("apertura", document.getElementById("apertura").options[document.getElementById("apertura").selectedIndex].text)

    confs.append("color1_p1", document.getElementById("color1_p1").options[document.getElementById("color1_p1").selectedIndex].text)
    confs.append("color2_p1", document.getElementById("color2_p1").options[document.getElementById("color2_p1").selectedIndex].text)
    confs.append("color3_p1", document.getElementById("color3_p1").options[document.getElementById("color3_p1").selectedIndex].text)
    confs.append("color4_p1", document.getElementById("color4_p1").options[document.getElementById("color4_p1").selectedIndex].text)
    confs.append("color5_p1", document.getElementById("color5_p1").options[document.getElementById("color5_p1").selectedIndex].text)

    confs.append("color1_p2", document.getElementById("color1_p2").options[document.getElementById("color1_p2").selectedIndex].text)
    confs.append("color2_p2", document.getElementById("color2_p2").options[document.getElementById("color2_p2").selectedIndex].text)
    confs.append("color3_p2", document.getElementById("color3_p2").options[document.getElementById("color3_p2").selectedIndex].text)
    confs.append("color4_p2", document.getElementById("color4_p2").options[document.getElementById("color4_p2").selectedIndex].text)
    confs.append("color5_p2", document.getElementById("color5_p2").options[document.getElementById("color5_p2").selectedIndex].text)

    fetch('/MenuXtreme/AjustarConfsXtreme', {
        method: 'POST',
        body: confs
    }).then(respuesta => respuesta.json())
    .then(
        function (respuesta) {
            if (respuesta) {
                location.assign("/Tablero/Tablero")
            } else {
                alert("Los jugadores deben de tener entre 1 a 5 colores sin reptreir entre los suyos y los del adversaio")
                location.assign("/MenuXtreme/MenuXtreme")
            }
        }
        
    )

}

function regresar() {
    location.assign("/MenuPrincipal/MenuPrincipal")
}

function irPerfil() {
    location.assign("/Perfil/Perfil")
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
      .then(respuesta => location.assign("/Tablero/Tablero"));
}