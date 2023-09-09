const Chref = window.location.href;

function translator() {
    let code = INPUT.value;
    const url = `${Chref}translator?code=${code}`;

    $.ajax({
        url: url,
        method: "GET",
        success: function (data, status, xhr) {
            let translated = document.getElementById("translated");
            translated.innerHTML = data.resp;
        }
    })
}

const INPUT = document.getElementById("code-input");
INPUT.focus()

if (INPUT) {
    INPUT.addEventListener("keydown", function (keyData) {
        if (keyData.key === "Enter") {
            document.getElementById("translated-btn").click();
        }
    });
}
