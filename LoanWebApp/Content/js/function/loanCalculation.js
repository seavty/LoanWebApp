//-> loanAmount
function loanAmount() {
    var amount = toFloat($("#amount").val());
    var payDay = toInt($("#payDay").val());
    var interestRate = 0;
    var interestAmount = 0;
    var total = 0;
    switch (payDay) {
        case 10:
            interestRate = 10;
            break;
        case 15:
            interestRate = 15;
            break;
        case 30:
            interestRate = 30;
            break;
        default:
            interestRate = 0;
    }
    interestRate = toFloat(interestRate);
    interestAmount = amount * interestRate / 100;

    //-- remember toFixed return string not number;
    total = parseFloat(amount) + parseFloat(interestAmount);
    $("#interestRate").val(toFloat(interestRate) + " %");
    $("#interestAmount").val(toFloatWithDollarCurrency(interestAmount));
    $("#loanAmount").val(toFloatWithDollarCurrency(total));
}