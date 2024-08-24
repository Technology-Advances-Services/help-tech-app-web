const url = 'https://localhost:44310/';

const room = document.getElementById("iptChatRoomId").value;
const webSocket = new WebSocket('wss://localhost:44315/chat?room=' + room);
const parameters = new FormData();

let messages = [];
let contentHtml = '';

window.addEventListener("load", function () {

    loadProfile();
    loadChatsByChatRoom();
});

document.getElementById("btnSendMessage").addEventListener("click", function () {

    parameters = new FormData();

    parameters.append("ChatRoomId", document.getElementById("iptChatRoomId").value);
    parameters.append("Sender", document.getElementById("iptRole").value);
    parameters.append("Message", document.getElementById("iptMessage").value);

    const resource = url + 'communications/send-message';

    fetch(resource, {

        method: 'POST',
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

    sendMessage();

    return false;
});

webSocket.onopen = function (event) {

    console.log("Connected to WebSocket server");
};

webSocket.onmessage = function (event) {

    const message = JSON.parse(event.data);

    if (message.room === room) {

        displayMessage(message);
    }
};

function loadProfile() {

    if (document.getElementById("iptRole").value === 'TECNICO') {

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

            contentHtml =
                `<div class="img_cont">
                    <img src="${data.ProfileUrl}" class="rounded-circle user_img">
                    <span class="online_icon"></span>
                </div>
                <div class="user_info">
                    <span>${data.Firstname}</span>
                </div>`;
        })
        .catch(() => {

            window.open(url + "home/error", "_self");
        })
    }
    else if (document.getElementById("iptRole").value === 'CONSUMIDOR') {

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

            contentHtml = `
                <div class="img_cont">
                    <img src="${data.ProfileUrl}" class="rounded-circle user_img">
                    <span class="online_icon"></span>
                </div>
                <div class="user_info">
                    <span>${data.Firstname}</span>
                </div>`;
        })
        .catch(() => {

            window.open(url + "home/error", "_self");
        })
    }

    document.getElementById("dvProfile").innerHTML = contentHtml;
}
function loadChatsByChatRoom() {

    const resource = url + 'communications/chats-by-chat-room';

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

        contentHtml = '';

        let date;

        if (document.getElementById("iptRole").value === 'TECNICO') {

            for (const item of data) {

                date = new Date(item.ShippingDate);

                if (!isNullOrEmpty(item.TechnicalId)) {

                    contentHtml +=
                        `<div class="d-flex justify-content-end mb-4">
                            <div class="msg_cotainer_send">
                                ${item.Message}
                                <span class="msg_time_send">${getFormattedDate(date)}</span>
                            </div>
                            <div class="img_cont_msg">
                                <img src="${item.ProfileUrlTechnical}" class="rounded-circle user_img">
                            </div>
                        </div>`;

                    document.getElementById("iptProfileUrlTechnical").value = item.ProfileUrlTechnical;
                }
                else if (!isNullOrEmpty(item.ConsumerId)) {

                    contentHtml +=
                        `<div class="d-flex justify-content-start mb-4">
                            <div class="img_cont_msg">
                                <img src="${item.ProfileUrlConsumer}" class="rounded-circle user_img">
                            </div>
                            <div class="msg_cotainer">
                                ${item.Message}
                                <span class="msg_time">${getFormattedDate(date)}</span>
                            </div>
                        </div>`;

                    document.getElementById("iptProfileUrlConsumer").value = item.ProfileUrlConsumer;
                }
            }
        }
        else if (document.getElementById("iptRole").value === 'CONSUMIDOR') {

            for (const item of data) {

                date = new Date(item.ShippingDate);

                if (!isNullOrEmpty(item.TechnicalId)) {

                    contentHtml +=
                        `<div class="d-flex justify-content-start mb-4">
                            <div class="img_cont_msg">
                                <img src="${item.ProfileUrlTechnical}" class="rounded-circle user_img">
                            </div>
                            <div class="msg_cotainer">
                                ${item.Message}
                                <span class="msg_time">${getFormattedDate(date)}</span>
                            </div>
                        </div>`;

                    document.getElementById("iptProfileUrlTechnical").value = item.ProfileUrlTechnical;
                }
                else if (!isNullOrEmpty(item.ConsumerId)) {

                    contentHtml +=
                        `<div class="d-flex justify-content-end mb-4">
                            <div class="msg_cotainer_send">
                                ${item.Message}
                                <span class="msg_time_send">${getFormattedDate(date)}</span>
                            </div>
                            <div class="img_cont_msg">
                                <img src="${item.ProfileUrlConsumer}" class="rounded-circle user_img">
                            </div>
                        </div>`;

                    document.getElementById("iptProfileUrlConsumer").value = item.ProfileUrlConsumer;
                }
            }
        }

        document.getElementById("dvChat").innerHTML = contentHtml;
    })
    .catch(() => {

        window.open(url + "home/error", "_self");
    })
}
function sendMessage() {

    const role = document.getElementById("iptRole");
    const message = document.getElementById("txaMessage");

    const user = role.value;
    const message = message.value;

    if (message.trim() !== "") {

        const messageObject = {

            room: room,
            user: user,
            text: message
        };

        webSocket.send(JSON.stringify(messageObject));

        displayMessage(messageObject);

        message.value = '';
    }
}
function displayMessage(message) {

    let date = new Date();

    if (document.getElementById("iptRole").value === 'TECNICO') {

        if (message.user === 'TECNICO') {

            contentHtml +=
                '<div class="d-flex justify-content-end mb-4">' +
                    '<div class="msg_cotainer_send">' +
                        message.text +
                        '<span class="msg_time_send">' + getFormattedDate(date) + '</span>' +
                    '</div>' +
                    '<div class="img_cont_msg">' +
                        '<img src="' + document.getElementById("iptProfileUrlTechnical").value + '" class="rounded-circle user_img">' +
                    '</div>' +
                '</div>';
        }
        else if (message.user == 'CONSUMIDOR') {

            contentHtml +=
                '<div class="d-flex justify-content-start mb-4">' +
                    '<div class="img_cont_msg">' +
                        '<img src="' + document.getElementById("iptProfileUrlConsumer").value + '" class="rounded-circle user_img">' +
                    '</div>' +
                    '<div class="msg_cotainer">' +
                        message.text +
                        '<span class="msg_time">' + getFormattedDate(date) + '</span>' +
                    '</div>' +
                '</div>';
        }
    }
    else if (document.getElementById("iptRole").value === 'CONSUMIDOR') {

        if (message.user == 'TECNICO') {

            contentHtml +=
                '<div class="d-flex justify-content-start mb-4">' +
                    '<div class="img_cont_msg">' +
                        '<img src="' + document.getElementById("iptProfileUrlTechnical").value + '" class="rounded-circle user_img">' +
                    '</div>' +
                    '<div class="msg_cotainer">' +
                        message.text +
                        '<span class="msg_time">' + getFormattedDate(date) + '</span>' +
                    '</div>' +
                '</div>';
        }
        else if (message.user == 'CONSUMIDOR') {

            contentHtml +=
                '<div class="d-flex justify-content-end mb-4">' +
                    '<div class="msg_cotainer_send">' +
                        message.text +
                        '<span class="msg_time_send">' + getFormattedDate(date) + '</span>' +
                    '</div>' +
                    '<div class="img_cont_msg">' +
                        '<img src="' + document.getElementById("iptProfileUrlConsumer").value + '" class="rounded-circle user_img">' +
                    '</div>' +
                '</div>';
        }
    }

    document.getElementById("dvChat").innerHTML = contentHtml;
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
function isNullOrEmpty(str) {
    return !str || str.trim() === '';
}