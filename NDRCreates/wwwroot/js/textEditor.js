//textEditor.js
function editoroption1() {
    document.getElementById("textarea1")
        .style.fontWeight = "bold";
}

function editoroption2() {
    document.getElementById("textarea1")
        .style.fontStyle = "italic";
}

function editoroptionr3() {
    document.getElementById("textarea1")
        .style.textAlign = "left";
}

function editoroption4() {
    document.getElementById("textarea1")
        .style.textAlign = "center";
}

function editoroption5() {
    document.getElementById("textarea1")
        .style.textAlign = "right";
}

function editoroption6() {
    document.getElementById("textarea1")
        .style.textTransform = "uppercase";
}

function editoroption7() {
    document.getElementById("textarea1")
        .style.textTransform = "lowercase";
}

function editoroption8() {
    document.getElementById("textarea1")
        .style.textTransform = "capitalize";
}

function editoroption9() {
    document.getElementById("textarea1")
        .style.fontWeight = "normal";
    document.getElementById("textarea1")
        .style.textAlign = "left";
    document.getElementById("textarea1")
        .style.fontStyle = "normal";
    document.getElementById("textarea1")
        .style.textTransform = "capitalize";
    document.getElementById("textarea1")
        .value = "";
}