﻿var precision = 2;

//-> toInt
function toInt(number) {
    return parseInt(number) ? parseInt(number) : 0;
}

//-> toFloat
function toFloat(number) {
    var num = parseFloat(number) ? parseFloat(number) : 0.0;
    return parseFloat(num).toFixed(precision);
}


//-> toFloatWithDollarCurrency
function toFloatWithDollarCurrency(number) {
    return toFloat(number) +  " USD";
}

//->Date Add

function calRepayDate(i) {
    if (isNaN(i) || i == "") {
        return "";
    }
    var d = new Date();
    var nd = new Date(d.getFullYear(), d.getMonth(), d.getDate() + parseInt(i));
    return nd.getDate() + "/" + (nd.getMonth() + 1) + "/" + nd.getFullYear();

}