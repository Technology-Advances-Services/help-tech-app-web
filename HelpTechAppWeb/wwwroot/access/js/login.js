const url = 'https://localhost:44310/';

document.getElementById("iptLogin").disabled = false;
document.getElementById("iptLogin").value = "Ingresar";

document.getElementById("frmCredentials").addEventListener("submit", function (event) {

    event.preventDefault();

    const loginButton = document.getElementById("iptLogin");

    loginButton.disabled = true;
    loginButton.value = "Iniciando sesión...";

    const credentials = new FormData();

    credentials.append("Username", document.getElementById("iptUsername").value);
    credentials.append("Password", document.getElementById("iptPassword").value);
    credentials.append("Role", document.getElementById("sltRole").value);

    login(credentials);
});

function login(credentials) {

    const resource = url + 'access/login';

    fetch(resource, {

        method: "POST",
        body: credentials,
        cache: "no-cache"
    })
    .then(response => {

        if (!response.ok) {

            throw new Error("Network response was not ok.");
        }
    })
    .catch(() => {

        window.open(url + "home/error", "_self");
    });
}