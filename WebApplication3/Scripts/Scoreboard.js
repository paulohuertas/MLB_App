function setInputDate(date) {
    let inputDate = document.getElementById("date-input");

    console.log(inputDate);
    if (date == "" || date == null || date == undefined) {
        if (inputDate != null) {
            return inputDate.nodeValue = new Date();
        }
    } 
    return inputDate.nodeValue = new Date(date);
}