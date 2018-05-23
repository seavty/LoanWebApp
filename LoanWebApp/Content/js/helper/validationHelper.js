function isValid() {
    var isOk = true;
    $(".required").each(function (i, v) {
        $(this).removeClass("is-invalid file-error error");
        if ($(this).val() == "") {
            if ($(this).attr('type') == "file")
                $(this).addClass("file-error")
            else
                $(this).addClass("is-invalid")

            isOk = false;
        }
    });
    return isOk;
}