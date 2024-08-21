const url = 'https://localhost:44310/';

document.getElementById("btnRegisterConsumer").disabled = false;
document.getElementById("btnRegisterConsumer").textContent = "Registrar";

window.addEventListener('load', function () {

    loadDepartments();
});

document.getElementById('sltDepartments').addEventListener("change", function () {

    const resource = url + 'locations/districts-by-department?departmentId=' + this.value;

    fetch(resource, {
        method: "GET",
        headers: {
            "Content-Type": "application/json; charset=utf-8"
        }
    })
    .then(response => {

        if (!response.ok) {

            throw new Error("Network response was not ok");
        }

        return response.json();
    })
    .then(data => {

        let contentHtml = '';

        for (const item of data) {

            contentHtml += `<option value="${item.id}">${item.name}</option>`;
        }

        document.getElementById('sltDistricts').innerHTML = contentHtml;
    })
    .catch(() => {

        window.open(url + "home/error", "_self");
    });
});

document.getElementById("frmConsumerData").addEventListener("submit", function (event) {

    event.preventDefault();

    const btnRegister = document.getElementById("btnRegisterConsumer");
    btnRegister.disabled = true;
    btnRegister.textContent = "Registrando información...";

    const parameters = new FormData();

    parameters.append("Id", document.getElementById("iptId").value);
    parameters.append("DistrictId", document.getElementById("sltDistricts").value);
    parameters.append("FirstName", document.getElementById("iptFirstName").value);
    parameters.append("LastName", document.getElementById("iptLastName").value);
    parameters.append("Age", document.getElementById("iptAge").value);
    parameters.append("Genre", document.getElementById("sltGenre").value);
    parameters.append("Phone", document.getElementById("iptPhone").value);
    parameters.append("Email", document.getElementById("iptEmail").value);
    parameters.append("Code", document.getElementById("iptCode").value);

    const profile = document.getElementById("iptProfile").files[0];

    if (profile) {
        parameters.append("profile", profile);
    }

    registerConsumer(parameters);
});

function loadDepartments() {

    const resource = url + 'locations/all-departments';

    fetch(resource, {

        method: "GET",
        headers: {
            "Content-Type": "application/json; charset=utf-8"
        }
    })
    .then(response => {

        if (!response.ok) {

            throw new Error("Network response was not ok");
        }

        return response.json();
    })
    .then(data => {

        let contentHtml = '';

        for (const item of data) {

            contentHtml += `<option value="${item.id}">${item.name}</option>`;
        }

        document.getElementById('sltDepartments').innerHTML = contentHtml;
    })
    .catch(() => {

        window.open(url + "home/error", "_self");
    });
}
function registerConsumer(parameters) {

    const resource = url + 'access/register-consumer';

    fetch(resource, {

        method: "POST",
        body: parameters,
        cache: "no-cache"
    })
    .then(response => {

        if (!response.ok) {

            throw new Error("Network response was not ok");
        }
    })
    .catch(() => {

        window.open(url + "home/error", "_self");
    });
}