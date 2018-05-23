var requestMethod = {
    GET: "GET",
    POST: "POST",
    DELETE: "DELETE",
    PUT: "PUT"
};

function ajaxHelper(url, data, method) {
    $('#loadingIndicator').modal({
        keyboard: false,
        backdrop: "static"
    });
    var promise = new Promise(function (resolve, reject) {
        window.setTimeout(function () {
            $.ajax({
                url: url,
                data: data,
                type: method,
                async: false,
                error: function (jqXHR, textStatus, errorThrown) {
                    if (jqXHR.status == 400)
                        alert(jqXHR.responseText);
                    else 
                        alert("Error code: " + jqXHR.status + "; Message: error occured while processing your request.");

                    $('#loadingIndicator').modal("hide");
                    return;
                },
                beforeSend: function () {
                },
                success: function (data) {
                    $('#loadingIndicator').modal("hide");
                    resolve(data);
                }
            });
        }, 100);
    });
    return promise;
}
