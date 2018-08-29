function isValid2(msg,selector) {
    var isOk = true;
    $(selector).each(function (i, v) {
        $(this).removeClass("is-invalid file-error error");
        if ($(this).val() == "") {
            if ($(this).attr('type') == "file")
                $(this).addClass("file-error");
            else
                $(this).addClass("is-invalid");
            isOk = false;
        }
    });

    if (!isOk) {
        $.toast({
            heading: 'System',
            //text: 'Please key in all required field(s).',
            text: msg,
            showHideTransition: 'slide',
            position: 'top-right',
            icon: 'error'
        });
    }
    return isOk;
}

function isValid(msg) {
    var isOk = true;
    $(".required").each(function (i, v) {
        $(this).removeClass("is-invalid file-error error");
        if ($(this).val() == "") {
            if ($(this).attr('type') == "file")
                $(this).addClass("file-error");
            else
                $(this).addClass("is-invalid");
            isOk = false;
        }
    });

    if (!isOk) {
        $.toast({
            heading: 'System',
            //text: 'Please key in all required field(s).',
            text: msg,
            showHideTransition: 'slide',
            position: 'top-right',
            icon: 'error'
        });
    }
    return isOk;
}

// Read a page's GET URL variables and return them as an associative array.
function getUrlVars(url) {
    var vars = [], hash;
    var hashes = url.slice(url.indexOf('?') + 1).split('&');
    for (var i = 0; i < hashes.length; i++) {
        hash = hashes[i].split('=');
        vars.push(hash[0]);
        vars[hash[0]] = hash[1];
    }
    return vars;
}