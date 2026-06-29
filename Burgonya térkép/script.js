/*Fields*/
const elements = [
    {
        time: 2,
        type: 'water',
        shape: [[1, 1, 1],
        [0, 0, 0],
        [0, 0, 0]],
        rotation: 0,
        mirrored: false
    },
    {
        time: 2,
        type: 'town',
        shape: [[1, 1, 1],
        [0, 0, 0],
        [0, 0, 0]],
        rotation: 0,
        mirrored: false
    },
    {
        time: 1,
        type: 'forest',
        shape: [[1, 1, 0],
        [0, 1, 1],
        [0, 0, 0]],
        rotation: 0,
        mirrored: false
    },
    {
        time: 2,
        type: 'farm',
        shape: [[1, 1, 1],
        [0, 0, 1],
        [0, 0, 0]],
        rotation: 0,
        mirrored: false
    },
    {
        time: 2,
        type: 'forest',
        shape: [[1, 1, 1],
        [0, 0, 1],
        [0, 0, 0]],
        rotation: 0,
        mirrored: false
    },
    {
        time: 2,
        type: 'town',
        shape: [[1, 1, 1],
        [0, 1, 0],
        [0, 0, 0]],
        rotation: 0,
        mirrored: false
    },
    {
        time: 2,
        type: 'farm',
        shape: [[1, 1, 1],
        [0, 1, 0],
        [0, 0, 0]],
        rotation: 0,
        mirrored: false
    },
    {
        time: 1,
        type: 'town',
        shape: [[1, 1, 0],
        [1, 0, 0],
        [0, 0, 0]],
        rotation: 0,
        mirrored: false
    },
    {
        time: 1,
        type: 'town',
        shape: [[1, 1, 1],
        [1, 1, 0],
        [0, 0, 0]],
        rotation: 0,
        mirrored: false
    },
    {
        time: 1,
        type: 'farm',
        shape: [[1, 1, 0],
        [0, 1, 1],
        [0, 0, 0]],
        rotation: 0,
        mirrored: false
    },
    {
        time: 1,
        type: 'farm',
        shape: [[0, 1, 0],
        [1, 1, 1],
        [0, 1, 0]],
        rotation: 0,
        mirrored: false
    },
    {
        time: 2,
        type: 'water',
        shape: [[1, 1, 1],
        [1, 0, 0],
        [1, 0, 0]],
        rotation: 0,
        mirrored: false
    },
    {
        time: 2,
        type: 'water',
        shape: [[1, 0, 0],
        [1, 1, 1],
        [1, 0, 0]],
        rotation: 0,
        mirrored: false
    },
    {
        time: 2,
        type: 'forest',
        shape: [[1, 1, 0],
        [0, 1, 1],
        [0, 0, 1]],
        rotation: 0,
        mirrored: false
    },
    {
        time: 2,
        type: 'forest',
        shape: [[1, 1, 0],
        [0, 1, 1],
        [0, 0, 0]],
        rotation: 0,
        mirrored: false
    },
    {
        time: 2,
        type: 'water',
        shape: [[1, 1, 0],
        [1, 1, 0],
        [0, 0, 0]],
        rotation: 0,
        mirrored: false
    },
]
let seasons = [
    {
        season: "Tavasz",
        score: 0
    },
    {
        season: "Nyár",
        score: 0
    },
    {
        season: "Ősz",
        score: 0
    },
    {
        season: "Tél",
        score: 0
    }
];
const missions = [
    {
        "title": "Az erdő széle",
        "description": "A térképed szélével szomszédos erdőmezőidért egy-egy pontot kapsz.",
        "path": "Images/Path/Az_erdő_széle.png"
    },
    {
        "title": "Álmos-völgy",
        "description": "Minden olyan sorért, amelyben három erdőmező van, négy-négy pontot kapsz.",
        "path": "Images/Path/Álmos-völgy.png"
    },
    {
        "title": "Krumpliöntözés",
        "description": "A farmmezőiddel szomszédos vízmezőidért két-két pontot kapsz.",
        "path": "Images/Path/Krumpliöntözés.png"
    },
    {
        "title": "Határvidék",
        "description": "Minden teli sorért vagy oszlopért 6-6 pontot kapsz.",
        "path": "Images/Path/Határvidék.png"
    },
    {
        "title": "Fasor",
        "description": "A leghosszabb, függőlegesen megszakítás nélkül egybefüggő erdőmezők mindegyikéért kettő-kettő pontot kapsz. Két azonos hosszúságú esetén csak az egyikért.",
        "path": "Images/Path/Fasor.png"
    },
    {
        "title": "Gazdag város",
        "description": "A legalább három különböző tereptípussal szomszédos falurégióidért három-három pontot kapsz.",
        "path": "Images/Path/Gazdag_város.png"
    },
    {
        "title": "Öntözőcsatorna",
        "description": "Minden olyan oszlopodért, amelyben a farm illetve a vízmezők száma megegyezik, négy-négy pontot kapsz. Mindkét tereptípusból legalább egy-egy mezőnek lennie kell az oszlopban ahhoz, hogy pontot kaphass érte.",
        "path": "Images/Path/Öntözőcsatorna.png"
    },
    {
        "title": "Mágusok völgye",
        "description": "A hegymezőiddel szomszédos vízmezőidért három-három pontot kapsz.",
        "path": "Images/Path/Mágusok_völgye.png"
    },
    {
        "title": "Üres telek",
        "description": "A városmezőiddel szomszédos üres mezőkért 2-2 pontot kapsz.",
        "path": "Images/Path/Üres_telek.png"
    },
    {
        "title": "Sorház",
        "description": "A leghosszabb, vízszintesen megszakítás nélkül egybefüggő falumezők mindegyikéért kettő-kettő pontot kapsz.",
        "path": "Images/Path/Sorház.png"
    },
    {
        "title": "Páratlan silók",
        "description": "Minden páratlan sorszámú teli oszlopodért 10-10 pontot kapsz.",
        "path": "Images/Path/Páratlan_silók.png"
    },
    {
        "title": "Gazdag vidék",
        "description": "Minden legalább öt különböző tereptípust tartalmazó sorért négy-négy pontot kapsz.",
        "path": "Images/Path/Gazdag_vidék.png"
    }
]
let choosenmission = [];
/*Choose Mission*/
function ChooseMission() {
    for (var j = 0; choosenmission.length != 4;) {
        let mission = missions[Math.floor(Math.random() * missions.length)];
        let bool = true
        for (var i = 0; i < choosenmission.length && bool; i++) {
            if (choosenmission[i] == mission) {
                bool = false;
            }
        }
        if (bool) {
            choosenmission[j] = mission;
            j++;
        }
    }
}
/*Generate Matrix*/
let gamefield = [];
let fieldstructore = [];
let minigamefield = [];
let questgamefield = [];
let rows = 11;
let columns = 11;
var x;
var y;
var element;
let nextfield;
let type;
let timecost = 0;
let rotation;
let mirrored;
var tablecontanier;
var time = 28;
NextField();



/*Generate Table*/{
    var body = document.getElementsByTagName("body")[0];

    //Seasons

    var spring = document.getElementById("Spring");
    sprin = SeasonCell(seasons[0], spring);
    var summer = document.getElementById("Summer");
    summer = SeasonCell(seasons[1], summer);
    var autmn = document.getElementById("Autmn");
    autmn = SeasonCell(seasons[2], autmn);
    var winter = document.getElementById("Winter");
    winter = SeasonCell(seasons[3], winter);
    var sumscore = document.getElementById("SumScore");
    sumscore.innerText = "Összesn: " + SumScore() + " pont";

    //Seasons

    //table
    var table = document.createElement("table");
    var tableBody = document.createElement("tbody");
    for (var i = 0; i < rows; i++) {
        var row = document.createElement("tr");
        gamefield[i] = [];
        fieldstructore[i] = [];
        for (var j = 0; j < columns; j++) {
            var cell = document.createElement("td");
            cell.addEventListener("click", function () {
                if (PlaceField(this)) {
                    NextField();
                    ColorMiniTable();
                    var timeleft = document.getElementById("TimeLeft");
                    var context = document.createElement("h2");
                    context.innerText = "Evszakból hátralévő idő: " + (time % 7) + "/7";
                    timeleft.innerText = context.innerText;
                }
            });
            cell.addEventListener("mouseover", function () {
                HoloField(this);
            });
            cell.addEventListener("mouseout", function () {
                OverField(this);
            });
            row.appendChild(cell);
            gamefield[i][j] = cell;
            fieldstructore[i][j] = 0;
        }
        tableBody.appendChild(row);
    }
    table.appendChild(tableBody);
    body.appendChild(table);
    tablecontanier = document.getElementById("TableContainer");
    tablecontanier.appendChild(table);
    CreateMap();
    SetField(1, 1, 1)
    SetField(3, 8, 1)
    SetField(5, 3, 1)
    SetField(8, 9, 1)
    SetField(9, 5, 1)
    ColorMap();
    //table

    //Quest
    var timeleft = document.getElementById("TimeLeft");
    timeleft.innerText = "Evszakból hátralévő idő: 7/7";
    ChooseMission();
    for (var i = 0; i < 4; i++) {
        switch (i) {
            case 0:
                var a = document.getElementById("A")
                a.innerHTML = "<img src=" + choosenmission[i].path + "></img><h3>" + choosenmission[i].title + "</h3><p>" + choosenmission[i].description + "</p><h3>A</h3>";
                /*
                var ascore = document.getElementById("AScore");
                ascore.innerText = "(" + WichMission(choosenmission[0]) + " pont)";
                */
                break;
            case 1:
                var b = document.getElementById("B")
                b.innerHTML = "<img src=" + choosenmission[i].path + "></img><h3>" + choosenmission[i].title + "</h3><p>" + choosenmission[i].description + "</p><h3>B</h3>";
                /*
                var bscore = document.getElementById("BScore");
                bscore = "(" + WichMission(choosenmission[1]) + " pont)";
                */
                break;
            case 2:
                var c = document.getElementById("C")
                c.innerHTML = "<img src=" + choosenmission[i].path + "></img><h3>" + choosenmission[i].title + "</h3><p>" + choosenmission[i].description + "</p><h3>C</h3>";
                /*
                 var cscore = document.getElementById("CScore");
                 cscore = "(" + WichMission(choosenmission[2]) + " pont)";
                 */
                break;
            case 3:
                var d = document.getElementById("D")
                d.innerHTML = "<img src=" + choosenmission[i].path + "></img><h3>" + choosenmission[i].title + "</h3><p>" + choosenmission[i].description + "</p><h3>D</h3>";
                /*
                var dscore = document.getElementById("DScore");
                dscore = "(" + WichMission(choosenmission[3]) + " pont)";
                 */
                break;

        }
    }
    /*
    var questtable = document.getElementById("QuestTable");
    var questtabletableBody = document.createElement("tbody");
    for (var i = 0; i < 2; i++) {
        questgamefield[i] = [];
        var questrow = document.createElement("tr");
        for (var j = 0; j < 2; j++) {
            var questcell = document.createElement("td");
            questrow.appendChild(questcell);
            questgamefield[i][j] = questcell;
            let con;
            switch (k) {
                case 0:
                    con = "A"
                    break;
                case 1:
                    con = "B"
                    break;
                case 2:
                    con = "C"
                    break;
                case 3:
                    con = "D"
                    break;
            }
            ChooseMission();
            questgamefield[i][j].innerHTML = "<img src=" + choosenmission[k].path + "></img><h3>" + choosenmission[k].title + "</h3><p>" + choosenmission[k].description + "</p><h3>" + con + "</h3>";
            k++;
        }
        questtabletableBody.appendChild(questrow);
    }
    questtable.appendChild(questtabletableBody);*/

    //Quest

    //Shape
    //Buttons
    var shapebuttons = document.getElementById("Buttons");
    var shapebuttonscontext = document.createElement("h2")
    shapebuttonscontext.innerText = "Lehelyezhető elem:";
    shapebuttons.appendChild(shapebuttonscontext);
    var buttonrotate = document.createElement("button")
    buttonrotate.innerText = "Forgatás";
    buttonrotate.addEventListener("click", function () {
        Rotate();
        ColorMiniTable();
    })
    shapebuttons.appendChild(buttonrotate);
    var br = document.createElement("br");
    shapebuttons.appendChild(br);
    var buttonmirror = document.createElement("button")
    buttonmirror.innerText = "Tükrözés";
    buttonmirror.addEventListener("click", function () {
        Mirror();
        ColorMiniTable()
    })
    shapebuttons.appendChild(buttonmirror);
    //Buttons

    //MiniTable
    var shapetable = document.getElementById("MiniTable");
    var shapetabletable = document.createElement("table");
    var shapetabletableBody = document.createElement("tbody");
    for (var i = 0; i < 3; i++) {
        minigamefield[i] = [];
        var minirow = document.createElement("tr");
        for (var j = 0; j < 3; j++) {
            var minicell = document.createElement("td");
            minirow.appendChild(minicell);
            minigamefield[i][j] = minicell;
        }
        shapetabletableBody.appendChild(minirow);
    }
    shapetabletable.appendChild(shapetabletableBody);
    shapetable.appendChild(shapetabletable);

    ColorMiniTable();
    //MiniTable
    //Shape

}
/*Season Cell*/
function SeasonCell(season, cell) {
    switch (season.season) {
        case "Tavasz":
            cell.innerHTML = season.season + ":<br>" + season.score + " pont";
            cell.style.backgroundColor = "lightgreen";
            cell.style.border = "5px solid green"
            break;
        case "Nyár":
            cell.innerHTML = season.season + ":<br>" + season.score + " pont";
            cell.style.backgroundColor = "#FFD580";
            cell.style.border = "5px solid orange"
            break;
        case "Ősz":
            cell.innerHTML = season.season + ":<br>" + season.score + " pont";
            cell.style.backgroundColor = "#C4A484";
            cell.style.border = "5px solid brown"
            break;
        case "Tél":
            cell.innerHTML = season.season + ":<br>" + season.score + " pont";
            cell.style.backgroundColor = "lightblue";
            cell.style.border = "5px solid blue"
            break;
    }
    return cell;
}
/*Sum Score*/
function SumScore() {
    let sum = 0;
    for (var i = 0; i < 4; i++) {
        sum += seasons[i].score;
    }
    return sum;
}
/*Create Map*/
function CreateMap() {
    for (var i = 0; i < rows; i++) {
        for (var j = 0; j < columns; j++) {
            fieldstructore[i][j] = 0;
        }
    }
}
/*SetField*/
function SetField(i, j, num) {
    fieldstructore[i][j] = num;
}
/*SetXY*/
function SetXY(cell) {
    for (var i = 0; i < rows; i++) {
        for (var j = 0; j < columns; j++) {
            if (gamefield[i][j] == cell) {
                x = i;
                y = j
            }
        }
    }
}
/*Color Map*/
function ColorMap() {
    for (var i = 0; i < rows; i++) {
        for (var j = 0; j < columns; j++) {
            switch (fieldstructore[i][j]) {
                case 0:
                    var cell = document.createElement("td")
                    cell = gamefield[i][j];
                    cell.style.backgroundImage = "url('Images/tile.png')"
                    break;
                case 1:
                    var cell = document.createElement("td")
                    cell = gamefield[i][j];
                    cell.style.backgroundImage = "url('Images/mountain.png')"
                    break;
                case 2:
                    var cell = document.createElement("td")
                    cell = gamefield[i][j];
                    cell.style.backgroundImage = "url('Images/forest.png')"
                    break;
                case 3:
                    var cell = document.createElement("td")
                    cell = gamefield[i][j];
                    cell.style.backgroundImage = "url('Images/town.png')"
                    break;
                case 4:
                    var cell = document.createElement("td")
                    cell = gamefield[i][j];
                    cell.style.backgroundImage = "url('Images/farm.png')"
                    break;
                case 5:
                    var cell = document.createElement("td")
                    cell = gamefield[i][j];
                    cell.style.backgroundImage = "url('Images/water.png')"
                    break;
            }
        }
    }
}
/*Next field*/
function NextField() {
    time -= timecost;
    if (time > 0) {
        element = elements[Math.floor(Math.random() * elements.length)];
        type = element.type;
        nextfield = element.shape;
        rotation = element.rotation;
        mirrored = element.mirrored;
        timecost = element.time;
        var wichseason = document.getElementById("WichS");
        var seasonnow = document.createElement("h2");
        seasonnow = WichSeason();
        wichseason.innerText = seasonnow.innerText;
    }
    else {
        console.log("Game Over");
        var wichseason = document.getElementById("WichS");
        var seasonnow = document.createElement("h2");
        seasonnow = WichSeason();
        var sumscore = document.getElementById("SumScore");
        sumscore.innerText = "Összesn: " + SumScore() + " pont";
    }

}
/*Holo field*/
function HoloField(cell) {
    SetXY(cell)
    if (Occupied()) {
        Place();
    }
}
function Occupied() {
    var placeabble = true;
    for (var i = -1; i < 2; i++) {
        for (var j = -1; j < 2; j++) {
            if (nextfield[1 + i][1 + j] == 1 && fieldstructore[x + i][y + j] != 0) {
                placeabble = false;
            }
        }
    }
    return placeabble;
}
function Place() {
    for (var i = -1; i < 2; i++) {
        for (var j = -1; j < 2; j++) {
            if (nextfield[1 + i][1 + j] == 1) {
                switch (type) {
                    case "forest":
                        gamefield[x + i][y + j].style.backgroundImage = "url('Images/forest.png')"
                        break;
                    case "farm":
                        gamefield[x + i][y + j].style.backgroundImage = "url('Images/farm.png')"
                        break;
                    case "water":
                        gamefield[x + i][y + j].style.backgroundImage = "url('Images/water.png')"
                        break;
                    case "town":
                        gamefield[x + i][y + j].style.backgroundImage = "url('Images/town.png')"
                        break;
                }
            }
        }
    }
}
/*Over Field*/
function OverField(cell) {
    SetXY(cell);
    for (var i = -1; i < 2; i++) {
        for (var j = -1; j < 2; j++) {
            if (fieldstructore[x + i][y + j] == 0) {
                gamefield[x + i][y + j].style.backgroundImage = "url('Images/tile.png')";
            }
        }
    }
    ColorMap()
}
/*Place Field */
function PlaceField(cell) {
    SetXY(cell)
    var happened = false;
    if (Occupied()) {
        for (var i = -1; i < 2; i++) {
            for (var j = -1; j < 2; j++) {
                if (nextfield[1 + i][1 + j] == 1) {
                    SetField(x + i, y + j, WichTile());
                    happened = true;
                }
            }
        }
    }
    ColorMap();
    return happened;
}
function WichTile() {
    switch (type) {
        case "forest":
            return 2;
        case "town":
            return 3;
        case "farm":
            return 4;
        case "water":
            return 5;
    }
}
/*Change rotation*/
function Rotate() {
    let tmptable = [];
    for (var i = 0; i < 3; i++) {
        tmptable[i] = []
        for (var j = 0; j < 3; j++) {
            tmptable[i][j] = nextfield[2 - j][i];
        }
    }
    if (rotation == 3) {
        rotation = 0;
    }
    else {
        rotation++;
    }
    nextfield = tmptable;
}
function Mirror() {
    let tmptable = [];
    console.log(nextfield);
    for (var i = 0; i < 3; i++) {
        tmptable[i] = []
        for (var j = 0; j < 3; j++) {
            tmptable[i][j] = nextfield[j][i];
        }
    }
    nextfield = tmptable;
    if (mirrored) {
        mirrored = false;
    }
    else {
        mirrored = true;
    }
}

/*Color Mini Table*/
function ColorMiniTable() {
    var path = "";
    switch (type) {
        case "mountain":
            path = "url('Images/mountain.png')"
            break;
        case "water":
            path = "url('Images/water.png')"
            break;
        case "farm":
            path = "url('Images/farm.png')"
            break;
        case "town":
            path = "url('Images/town.png')"
            break;
        case "forest":
            path = "url('Images/forest.png')"
            break;
    }
    for (var i = 0; i < 3; i++) {
        for (var j = 0; j < 3; j++) {
            switch (nextfield[i][j]) {
                case 0:
                    var cell = document.createElement("td")
                    cell = minigamefield[i][j];
                    cell.style.backgroundImage = "url('Images/tile.png')"
                    break;
                case 1:
                    var cell = document.createElement("td")
                    cell = minigamefield[i][j];
                    cell.style.backgroundImage = path;
                    break;
            }
        }
    }
    var shapetimecost = document.getElementById("TimeCost");
    shapetimecost.innerText = timecost + " month";
}
/*Wich Season*/
function WichSeason() {
    var context = document.createElement("h2");
    context.innerText = "Jelenlegi évszak:"
    switch (Math.floor(time / 7)) {
        case 4:
            context.innerText += " Tavasz (AB)"
            break;
        case 3:
            context.innerText += " Tavasz (AB)"
            break;
        case 2:
            context.innerText += " Nyár (BC)"
            break;
        case 1:
            context.innerText += " Ősz (CD)"
            break;
        case 0:
            context.innerText += " Tél (DA)"
            break;
    }
    if (time == 21 || time == 20) {
        seasons[0].score += WichMission(choosenmission[0].title)
        seasons[0].score += WichMission(choosenmission[1].title)
        var springmini = document.getElementById("Spring");
        springmini = SeasonCell(seasons[0], springmini);
        /*
        var ascore = document.getElementById("AScore");
        ascore.innerText = "(" + WichMission(choosenmission[0]) + " pont)";
        var bscore = document.getElementById("BScore");
        bscore.innerText = "(" + WichMission(choosenmission[1]) + " pont)";
        */
    }
    if (time == 14 || time == 13) {
        seasons[1].score += WichMission(choosenmission[1].title)
        seasons[1].score += WichMission(choosenmission[2].title)
        var summermini = document.getElementById("Summer");
        summermini = SeasonCell(seasons[1], summermini);
        /*
        var bscore = document.getElementById("BScore");
        bscore.innerText = "(" + WichMission(choosenmission[1]) + " pont)";
        var cscore = document.getElementById("CScore");
        cscore.innerText = "(" + WichMission(choosenmission[2]) + " pont)";
        */
    }
    if (time == 7 || time == 6) {
        seasons[2].score += WichMission(choosenmission[2].title)
        seasons[2].score += WichMission(choosenmission[3].title)
        var autmnmini = document.getElementById("Autmn");
        autmnmini = SeasonCell(seasons[2], autmnmini);
        /*
        var cscore = document.getElementById("CScore");
        cscore.innerText = "(" + WichMission(choosenmission[2]) + " pont)";
        var dscore = document.getElementById("DScore");
        dscore.innerText = "(" + WichMission(choosenmission[3]) + " pont)";
        */
    }
    if (time == 0 || time == -1) {
        seasons[3].score += WichMission(choosenmission[3].title)
        seasons[3].score += WichMission(choosenmission[1].title)
        var wintermini = document.getElementById("Winter");
        wintermini = SeasonCell(seasons[3], wintermini);
        /*
        var dscore = document.getElementById("DScore");
        dscore.innerText = "(" + WichMission(choosenmission[3]) + " pont)";
        var ascore = document.getElementById("AScore");
        ascore.innerText = "(" + WichMission(choosenmission[1]) + " pont)";
        */
    }
    return context;
}

/*Mission scores*/
function ForestBorder() {
    var score = 0;
    for (var i = 0; i < rows; i++) {
        for (var j = 0; j < columns; j++) {
            if (fieldstructore[i][j] == 2 && (i + 1 == 11 || i - 1 == -1 || j + 1 == 11 || j - 1 == -1)) {
                score++;
            }
        }
    }
    return score;
}
function ForestRow() {
    var score = 0;
    var rowforest = 0
    for (var i = 0; i < rows; i++) {
        for (var j = 0; j < columns; j++) {
            if (fieldstructore[i][j] == 2) {
                rowforest++;
            }
        }
        if (rowforest >= 3) {
            score += 4;
        }
        rowforest = 0;
    }
    return score;
}
function PotatoShower() {
    var score = 0;
    for (var i = 0; i < rows; i++) {
        for (var j = 0; j < columns; j++) {
            if (fieldstructore[i][j] == 5) {
                if (i + 1 != 11) {
                    if (fieldstructore[i + 1][j] == 4) {
                        score++
                    }
                }
                if (j + 1 != 11) {
                    if (fieldstructore[i][j + 1] == 4) {
                        score++
                    }
                }
                if (i - 1 != -1) {
                    if (fieldstructore[i - 1][j] == 4) {
                        score++
                    }
                }
                if (j - 1 != -1) {
                    if (fieldstructore[i][j - 1] == 4) {
                        score++
                    }
                }
            }
        }
    }
    return 2 * score;
}
function RowCol() {
    var score = 0;
    var boolrow = true;
    var boolcol = true;
    for (var i = 0; i < rows; i++) {
        for (var j = 0; j < columns; j++) {
            if (fieldstructore[i][j] == 0) {
                boolrow = false;
            }
            if (fieldstructore[j][i] == 0) {
                boolcol = false;
            }
        }
        if (boolrow) {
            score += 6;
        }
        if (boolcol) {
            score += 6;
        }
        boolrow = true;
        boolcol = true;

    }
    return score;
}
function LongestForest() {
    var rowforest = 0;
    var colforest = 0;
    max = 0;
    for (var i = 0; i < rows; i++) {
        for (var j = 0; j < columns; j++) {
            if (fieldstructore[i][j] == 2) {
                if (j - 1 == -1 || fieldstructore[i][j - 1] != 2) {
                    rowforest = 1;
                }
                else if (fieldstructore[i][j - 1] == 2) {
                    rowforest++;
                }

                if (j - 1 == -1 || fieldstructore[j - 1][i] != 2) {
                    colforest = 1;
                }
                else if (fieldstructore[j - 1][i] == 2) {
                    colforest++;
                }
            }
        }
        if (max < rowforest) {
            max = rowforest;
        }
        if (max < colforest) {
            max = colforest;
        }
        rowforest = 0;
        colforest = 0;
    }
    return max * 2;
}
function RichTown() {
    var score = 0;
    var tiles = [];
    for (var i = 0; i < rows; i++) {
        for (var j = 0; j < columns; j++) {
            if (fieldstructore[i][j] == 3) {
                if (i + 1 != 11) {
                    tiles = Contain(tiles, fieldstructore[i + 1][j]);
                }
                if (i - 1 != -1) {
                    tiles = Contain(tiles, fieldstructore[i - 1][j]);
                }
                if (j + 1 != 11) {
                    tiles = Contain(tiles, fieldstructore[i][j + 1]);
                }
                if (j - 1 != -1) {
                    tiles = Contain(tiles, fieldstructore[i][j - 1]);
                }
            }
            if (tiles.length >= 3) {
                score++;
            }
            tiles = [];
        }
    }
    return 3 * score;
}
function Contain(tiles, cell) {
    var bool = true
    for (var i = 0; i < tiles.length; i++) {
        if (tiles[i] == cell) {
            bool = false;
        }
    }
    if (bool && cell != 0) {
        tiles[tiles.length] = cell;
    }
    return tiles;
}
function ShowerTunnel() {
    var score = 0;
    var waters = 0;
    var farms = 0;
    for (var i = 0; i < rows; i++) {
        for (var j = 0; j < columns; j++) {
            if (fieldstructore[j][i] == 4) {
                farms++;
            }
            if (fieldstructore[j][i] == 5) {
                waters++;
            }
        }
        if (waters == farms && farms != 0 && waters != 0) {
            score++;
        }
        waters = 0;
        farms = 0;
    }
    return 4 * score;
}
function MountainWater() {
    var score = 0;
    for (var i = 0; i < rows; i++) {
        for (var j = 0; j < columns; j++) {
            if (fieldstructore[i][j] == 1) {
                if (fieldstructore[i + 1][j] == 5) {
                    score++;
                }
                if (fieldstructore[i - 1][j] == 5) {
                    score++;
                }
                if (fieldstructore[i][j + 1] == 5) {
                    score++;
                }
                if (fieldstructore[i][j - 1] == 5) {
                    score++;
                }
            }
        }
    }
    return 3 * score;
}
function LostTile() {
    var score = 0;
    var tiles = [];
    for (var i = 0; i < rows; i++) {
        for (var j = 0; j < columns; j++) {
            if (fieldstructore[i][j] == 3) {
                if (i + 1 != 11) {
                    if (fieldstructore[i + 1][j] == 0) {
                        score++;
                    }
                }
                if (i - 1 != -1) {
                    if (fieldstructore[i - 1][j] == 0) {
                        score++;
                    }
                }
                if (j + 1 != 11) {
                    if (fieldstructore[i][j + 1] == 0) {
                        score++;
                    }
                }
                if (j - 1 != -1) {
                    if (fieldstructore[i][j - 1] == 0) {
                        score++;
                    }
                }
            }
        }
    }
    return 2 * score;
}
function LongestTown() {
    var rowtown = 0;
    max = 0;
    for (var i = 0; i < rows; i++) {
        for (var j = 0; j < columns; j++) {
            if (fieldstructore[i][j] == 3) {
                if (j - 1 == -1 || fieldstructore[i][j - 1] != 3) {
                    rowtown = 1;
                }
                else if (fieldstructore[i][j - 1] == 3) {
                    rowtown++;
                }
            }
        }
        if (max < rowtown) {
            max = rowtown;
        }
        rowtown = 0;
    }
    return max * 2;
}
function OddCol() {
    var score = 0;
    var boolcol = true;
    for (var i = 0; i < columns; i++) {
        if (i % 2 == 0) {
            for (var j = 0; j < rows; j++) {
                if (fieldstructore[j][i] == 0) {
                    boolcol = false;
                }
            }
        }
        else {
            boolcol = false;
        }
        if (boolcol) {
            score++;
        }
        boolcol = true;
    }
    return score * 10;
}
function Everycol() {
    var score = 0;
    var tiles = [];
    var bool = true;
    for (var i = 0; i < rows; i++) {
        for (var j = 0; j < columns; j++) {
            if (fieldstructore[i][j] != 0) {
                for (var k = 0; k < tiles.length; k++) {
                    if (fieldstructore[i][j] == tiles[k]) {
                        bool = false;
                    }
                }
                if (bool) {
                    tiles[tiles.length] = fieldstructore[i][j];
                }
            }
            bool = true;
        }
        if (tiles.length == 5) {
            score++;
        }
        tiles = [];
    }
    return 4 * score;
}

/*Wich mission*/
function WichMission(mission) {
    var score = 0;
    switch (mission) {
        case "Az erdő széle":
            score = ForestBorder()
            break;
        case "Álmos-völgy":
            score = ForestRow()
            break;
        case "Krumpliöntözés":
            score = PotatoShower()
            break;
        case "Határvidék":
            score = RowCol()
            break;
        case "Fasor":
            score = LongestForest()
            break;
        case "Gazdag város":
            score = RichTown()
            break;
        case "Öntözőcsatorna":
            score = ShowerTunnel()
            break;
        case "Mágusok völgye":
            score = MountainWater()
            break;
        case "Üres telek":
            score = LostTile()
            break;
        case "Sorház":
            score = LongestTown()
            break;
        case "Páratlan silók":
            score = OddCol()
            break;
        case "Gazdag vidék":
            score = Everycol()
            break;
    }
    return score;
}
