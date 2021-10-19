var blazorDemo = {};

blazorDemo.setSessionStorage = function (key, data) {
    sessionStorage.setItem(key, data);
}

blazorDemo.getSessionStorage = function (key) {
    return sessionStorage.getItem(key);
}

blazorDemo.hideModal = function (element) {
    bootstrap.Modal.getInstance(element).hide();
}