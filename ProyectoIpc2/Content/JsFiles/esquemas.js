let nEquipos = 16;
let cordenadas16 = [
    [10, 10],
    [10, 60],
    [10, 110],
    [10, 160],
    [10, 210],
    [10, 260],
    [10, 310],
    [10, 360],
    [150, 30],
    [150, 130],
    [150, 230],
    [150, 330],
    [290, 80],
    [290, 270],
    [420, 180],
    [540, 180],
    [670, 80],
    [680, 270],
    [810, 30],
    [810, 130],
    [810, 230],
    [810, 330],
    [950, 10],
    [950, 60],
    [950, 110],
    [950, 160],
    [950, 210],
    [950, 260],
    [950, 310],
    [950, 360]
]

let cordenadas8 = [
    [150, 30],
    [150, 130],
    [150, 230],
    [150, 330],
    [290, 80],
    [290, 270],
    [420, 180],
    [540, 180],
    [670, 80],
    [680, 270],
    [810, 30],
    [810, 130],
    [810, 230],
    [810, 330]
]

let cordenadas4 = [
    [290, 80],
    [290, 270],
    [420, 180],
    [540, 180],
    [670, 80],
    [680, 270]
]

function colocar4Equipos(equipos) {
    let contenedor = document.getElementById("esquema")
    contenedor.style.backgroundImage = "url('../Content/Imgs/esquema4.jpg')"
    contenedor.innerHTML = ""
    for (let i = 0; i < 6; i++) {
        let bloque = document.createElement("div")
        bloque.classList.add("bloque")
        bloque.setAttribute("id", i)
        bloque.style.marginLeft = cordenadas4[i][0].toString() + "px"
        bloque.style.marginTop = cordenadas4[i][1].toString() + "px"
        if (i >= 0 && i <= 1) {
            bloque.innerHTML = equipos[i]["nombreEquipo"]
        } else if (i >= 4 && i <= 5) {
            bloque.innerHTML = equipos[i % 4 + 2]["nombreEquipo"]
        }
        contenedor.appendChild(bloque)
        contenedor.appendChild(bloque)
    }
}

function colocar8Equipos(equipos) {
    let contenedor = document.getElementById("esquema")
    contenedor.style.backgroundImage = "url('../Content/Imgs/esquema8.jpg')"
    contenedor.innerHTML = ""
    for (let i = 0; i < 14; i++) {
        let bloque = document.createElement("div")
        bloque.classList.add("bloque")
        bloque.setAttribute("id", i)
        bloque.style.marginLeft = cordenadas8[i][0].toString() + "px"
        bloque.style.marginTop = cordenadas8[i][1].toString() + "px"
        if (i >= 0 && i <= 3) {
            bloque.innerHTML = equipos[i]["nombreEquipo"]
        } else if (i >= 10 && i <= 13) {
            bloque.innerHTML = equipos[i % 8 + 2]["nombreEquipo"]
        }
        contenedor.appendChild(bloque)
    }
}

function colocar16Equipos(equipos) {
    let contenedor = document.getElementById("esquema")
    contenedor.style.backgroundImage = "url('../Content/Imgs/esquema16.jpg')"
    contenedor.innerHTML = ""
    for (let i = 0; i < 30; i++) {
        let bloque = document.createElement("div")
        bloque.classList.add("bloque")
        bloque.setAttribute("id", i)
        bloque.style.marginLeft = cordenadas16[i][0].toString() + "px"
        bloque.style.marginTop = cordenadas16[i][1].toString() + "px"
        if (i >= 0 && i <= 7) {
            bloque.innerHTML = equipos[i]["nombreEquipo"]
        } else if (i >= 22 && i <= 29) {
            bloque.innerHTML = equipos[i % 16 + 2]["nombreEquipo"]
        }
        contenedor.appendChild(bloque)
    }
}

function colocarEquipos(equipos) {
    if (equipos.length == 4) {
        colocar4Equipos(equipos)
    } else if (equipos.length == 8) {
        colocar8Equipos(equipos)
    } else {
        colocar16Equipos(equipos)
    }
}
