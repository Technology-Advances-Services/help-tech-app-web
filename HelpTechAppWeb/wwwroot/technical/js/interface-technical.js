const url = 'https://localhost:44310/';

let jobs = [];

window.addEventListener("load", function () {

    loadInformationTechnical();

    loadJobsByTechnical().then(data => {

        jobs = data;

        fillTableJobs(jobs);
    });
});

setInterval(refreshInterfaceData, 60000);

document.getElementById("btnInterfaceTechnical").addEventListener("click", function () {

    window.location.href = url + 'technical/interface-technical';

    return false;
});

document.getElementById("btnStatisticalReport").addEventListener("click", function () {

    window.open(url + 'technical/statistical-report', '_blank');

    return false;
});

document.getElementById("btnReportReviews").addEventListener("click", function () {

    window.open(url + 'technical/report-reviews', '_blank');

    return false;
});

document.getElementById("btnJobsResponses").addEventListener("click", function () {

    window.open('technical/jobs-responses', '_blank');

    return false;
});

document.getElementById("btnLogout").addEventListener("click", function () {

    window.location.href = url + 'access/logout';

    return false;
});


document.getElementById("dvSearchDate").addEventListener("click", function (event) {

    if (event.target && event.target.id === "btnSearchPendingsJobs") {

        const workDate = document.getElementById("iptWorkDate").value;
        const filteredJobs = jobs.filter(j => j.WorkDate === workDate && j.JobState === "PENDIENTE");

        fillTableJobs(filteredJobs);
    }
});

document.getElementById("tblPendingsJobs").addEventListener("click", function (event) {

    if (event.target && event.target.id === "btnDetailJob") {

        const row = event.target.closest("tr");
        const id = row.querySelector("#tId").textContent.trim();

        const filteredJobs = jobs.find(j => j.Id === id);

        document.getElementById('pAddress').textContent = filteredJobs.Address;
        document.getElementById('pDescription').textContent = filteredJobs.Description;
    }
});

document.getElementById("tblPendingsJobs").addEventListener("click", function (event) {

    if (event.target && event.target.id === "btnChat") {

        const row = event.target.closest("tr");
        const consumerId = row.querySelector("#tConsumerId").textContent.trim();

        window.open(url + `communications/messaging?personId=${consumerId}&role=TECNICO`, "_blank");

        event.preventDefault();
    }
});

document.getElementById("uChatsMembers").addEventListener("click", function (event) {

    if (event.target && event.target.id === "btnChat") {

        const listItem = event.target.closest("li");
        const consumerId = listItem.querySelector("#spnConsumerId").textContent.trim();

        window.open(url + `communications/messaging?personId=${consumerId}&role=TECNICO`, "_blank");

        event.preventDefault();
    }
});

function refreshInterfaceData() {

    let resource = url + 'technical/general-technical-statistic';

    fetch(resource, {

        method: 'GET',
        headers: {
            'Accept': 'application/json'
        }
    })
    .then(response => {

        if (!response.ok) {

            throw new Error('Network response was not ok.');
        }

        return response.json();
    })
    .then(data => {

        document.getElementById('spnTotalIncome').textContent = data.TotalIncome;
        document.getElementById('spnTotalConsumersServed').textContent = data.TotalConsumersServed;
        document.getElementById('spnTotalWorkTime').textContent = data.TotalWorkTime;
        document.getElementById('spnTotalPendingsJobs').textContent = data.TotalPendingsJobs;
    })
    .catch(() => {

        window.open(url + 'home/error', '_self');
    });

    resource = url + 'communications/chats-members-by-technical';

    fetch(resource, {

        method: 'GET',
        headers: {
            'Accept': 'application/json'
        }
    })
    .then(response => {

        if (!response.ok) {

            throw new Error('Network response was not ok.');
        }

        return response.json();
    })
    .then(data => {

        let contentHtml = '';

        for (const item of data) {

            contentHtml +=

                `<li>
                    <a id="btnChat" class="nav-link d-flex align-items-center">
                        <div class="me-4">
                            <div class="position-relative d-inline-block text-white">
                                <img alt="image Placeholder" src="${item.ProfileUrl}" class="avatar rounded-circle">
                                <span class="position-absolute bottom-2 end-2 transform translate-x-1/2 translate-y-1/2 border-2 border-solid border-current w-3 h-3 bg-success rounded-circle"></span>
                            </div>
                        </div>
                        <div>
                            <span class="d-block text-sm font-semibold">${item.Firstname}</span>
                            <span class="d-block text-xs text-muted font-regular">${item.Lastname}</span>
                            <span id="spnConsumerId" style="display:none;">${item.ConsumerId}</span>
                        </div>
                        <div class="ms-auto"> <i class="bi bi-chat"></i> </div>
                    </a>
                </li>`;
        }

        document.getElementById('uChatsMembers').innerHTML = contentHtml;
    })
    .catch(() => {

        window.open(url + 'home/error', '_self');
    });
}
function loadInformationTechnical() {

    const resource = url + 'techninal/information-technical';

    fetch(resource, {

        method: 'GET',
        headers: {
            'Content-Type': 'application/json; charset=utf-8'
        }
    })
    .then(response => {

        if (!response.ok) {

            throw new Error('Network response was not ok.');
        }

        return response.json();
    })
    .then(data => {

        document.getElementById('iProfilePhoto').src = data.ProfileUrl;
        document.getElementById('pMembership').textContent = data.Membership;
        document.getElementById('pEspecialty').textContent = data.Specialty;
        document.getElementById('pFirstname').textContent = data.Firstname;
        document.getElementById('pLastname').textContent = data.Lastname;
        document.getElementById('pAge').textContent = data.Age;
        document.getElementById('pEmail').textContent = data.Email;
        document.getElementById('pPhone').textContent = data.Phone;
    })
    .catch(error => {

        window.open(url + 'home/error', '_self');
    });
}
function loadJobsByTechnical() {

    const resource = url + 'attentions/jobs-by-technical';

    fetch(resource, {

        method: 'GET',
        headers: {
            'Accept': 'application/json'
        }
    })
    .then(response => {

        if (!response.ok) {

            throw new Error('Network response was not ok.');
        }

        return response.json();
    })
    .then(data => {

        if (data.length === 0) {

            return [];
        }

        return data;
    })
    .catch(() => {

        window.open(url + 'home/error', '_self');
    });
}
function fillTableJobs(jobs) {

    const tableMessage = document.getElementById('spnTableMessage');
    const pendingsJobs = document.getElementById('tbdPendingsJobs');

    if (jobs.lenght === 0) {

        tableMessage.textContent = "Usted no cuenta con citas pendientes.";

        return;
    }

    let contentHtml = '';

    for (const item of jobs) {

        const workDate = new Date(item.WorkDate);

        contentHtml +=
            
            `<tr>
                <td id="tId">${item.Id}</td>
                <td id="tConsumerId">${item.ConsumerId}</td>
                <td id="tFirstname">${item.Firstname}</td>
                <td id="tLastname">${item.Lastname}</td>
                <td id="tPhone">${item.Phone}</td>
                <td id="tWorkDate">${getFormattedDate(workDate)}</td>
                <td id="tWorkingTime">${item.WorkDate}</td>
                <td id="tLaborBudget">${item.LaborBudget}</td>
                <td id="tEquipmentBudget">${item.EquipmentBudget}</td>
                <td id="tAmountFinal">${item.AmountFinal}</td>
                <td id="tJobState">${item.JobState}</td>
                <td>
                    <button id="btnDetailJob" class="btn btn-sm btn-neutral" data-toggle="modal" data-target="#mdlJobDetail">Detalle</button>
                    <button id="btnChat" class="btn btn-sm btn-neutral">Mensaje</button>
                </td>
            </tr>`;
    }

    pendingsJobs.innerHTML = contentHtml;
    tableMessage.textContent = "Se está mostrando las citas pendientes durante la semana.";
}
function getFormattedDate(date) {

    let day = String(date.getDate()).padStart(2, '0');
    let mounth = String(date.getMonth() + 1).padStart(2, '0');
    let year = date.getFullYear();
    let hour = String(date.getHours()).padStart(2, '0');
    let minutes = String(date.getMinutes()).padStart(2, '0');
    let seconds = String(date.getSeconds()).padStart(2, '0');

    return `${day}/${mounth}/${year} ${hour}:${minutes}:${seconds}`;
}