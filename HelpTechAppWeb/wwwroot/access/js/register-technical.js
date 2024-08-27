const url = 'https://localhost:44310/';

document.getElementById("btnRegisterTechnical").disabled = false;
document.getElementById("btnRegisterTechnical").textContent = "Registrar";

window.addEventListener('load', function () {

    loadSpecialties();
    loadDepartments();
});

document.getElementById('sltDepartments').addEventListener("change", function () {

    const resource = url + 'Locations/DistrictsByDepartment?departmentId=' + this.value;

    fetch(resource, {

        method: 'GET',
        headers: {

            'Content-Type': 'application/json; charset=utf-8'
        }
    })
    .then(response => {

        if (!response.ok) {

            throw new Error("Network response was not ok.");
        }

        return response.json();
    })
    .then(data => {

        let contentHtml = '';

        for (const item of data) {

            contentHtml += `<option value="${item.Id}">${item.Name}</option>`;
        }

        document.getElementById('sltDistricts').innerHTML = contentHtml;
    })
    .catch(() => {

        window.open(url + "Home/Error", "_self");
    });
});

document.getElementById("frmTechnicianData").addEventListener("submit", function (event) {

    event.preventDefault();

    const btnRegister = document.getElementById("btnRegisterTechnical");

    btnRegister.disabled = true;
    btnRegister.textContent = "Registrando información...";

    const parameters = new FormData();

    parameters.append("Id", document.getElementById("iptId").value);
    parameters.append("SpecialtyId", document.getElementById("sltSpecialties").value);
    parameters.append("DistrictId", document.getElementById("sltDistricts").value);
    parameters.append("Firstname", document.getElementById("iptFirstName").value);
    parameters.append("Lastname", document.getElementById("iptLastName").value);
    parameters.append("Age", document.getElementById("iptAge").value);
    parameters.append("Genre", document.getElementById("sltGenre").value);
    parameters.append("Phone", document.getElementById("iptPhone").value);
    parameters.append("Email", document.getElementById("iptEmail").value);
    parameters.append("Code", document.getElementById("iptCode").value);

    const profile = document.getElementById("iptProfile").files[0];

    if (profile) {

        parameters.append("profile", profile);
    }

    const criminalRecord = document.getElementById("iptCriminalRecord").files[0];

    if (criminalRecord) {

        parameters.append("criminalRecord", criminalRecord);
    }

    registerTechnical(parameters);
});

function loadSpecialties() {

    const resource = url + 'Specialties/AllSpecialties';

    fetch(resource, {

        method: 'GET',
        headers: {

            'Content-Type': 'application/json; charset=utf-8'
        }
    })
    .then(response => {

        if (!response.ok) {

            throw new Error("Network response was not ok.");
        }

        return response.json();
    })
    .then(data => {

        let contentHtml = '';

        for (const item of data) {

            contentHtml += `<option value="${item.Id}">${item.Name}</option>`;
        }

        document.getElementById('sltSpecialties').innerHTML = contentHtml;
    })
    .catch(() => {

        window.open(url + "Home/Error", "_self");
    });
}
function loadDepartments() {

    const resource = url + 'Locations/AllDepartments';

    fetch(resource, {

        method: 'GET',
        headers: {

            'Content-Type': 'application/json; charset=utf-8'
        }
    })
    .then(response => {

        if (!response.ok) {

            throw new Error("Network response was not ok.");
        }

        return response.json();
    })
    .then(data => {

        let contentHtml = '';

        for (const item of data) {

            contentHtml += `<option value="${item.Id}">${item.Name}</option>`;
        }

        document.getElementById('sltDepartments').innerHTML = contentHtml;
    })
    .catch(() => {

        window.open(url + "Home/Error", "_self");
    });
}
function registerTechnical(parameters) {

    const resource = url + 'Access/RegisterTechnical';

    fetch(resource, {

        method: 'POST',
        body: parameters,
        cache: "no-cache"
    })
    .then(response => {

        if (!response.ok) {

            throw new Error("Network response was not ok.");
        }
    })
    .catch(() => {

        window.open(url + "Home/Error", "_self");
    });
}