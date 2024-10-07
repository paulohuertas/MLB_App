let tbody = document.querySelector('tbody');
console.log(tbody);

selectPlayerDetails(tbody);

function selectPlayerDetails(bodyElement) {
    bodyElement.addEventListener('click', function (e) {
        let currentElement = "";
        if (e.target.nodeName == "IMG") {
            currentElement = e.target.parentElement.parentElement;
        }
        else {
            currentElement = e.target.parentElement;
        }
        let id = currentElement.id;
        if (!id.includes("new_")) {
            if (currentElement.id != null) {
                $.ajax({
                    type: 'POST',
                    url: 'GetPlayerById',
                    data: { 'id': id },
                    dataType: 'json',
                    success: function (data) {
                        var newlyCreatedId = "new_" + data.playerID;
                        var findIfDataExists = document.getElementById(newlyCreatedId);
                        var parent = document.getElementById(data.playerID);

                        if (findIfDataExists == null) {

                            var rows =
                                "<tr id='" + newlyCreatedId + "'>"
                                + "<td scope='row' class='text-center'>" + data.longName + "</td>"
                                + "<td scope='row' class='text-center btn-link'>" + data.espnLink + "</td>"
                                + "<td scope='row' class='text-center btn-link'>" + data.fantasyProsLink + "</td>"
                                + "<td scope='row' class='text-center  text-danger'>" + data.injury.description + "</td>"
                                + "</tr>";

                            //$(parent).append(rows).insertAfter(parent);
                            $(rows).show().insertAfter(currentElement);


                        }
                        else if (findIfDataExists.classList.contains('hidden')) {
                            findIfDataExists.classList.remove('hidden');
                        }
                    },
                    error: function (ex) {
                        console.log("Deu erro " + ex.responseText);
                    }
                });
            }
        }
    });

    bodyElement.addEventListener('click', (e) => {
        console.log(e);
        let clickedElement = e.target.parentElement.id;
        if (clickedElement.includes("new_")) {
            let element = $('#' + clickedElement);
            element.addClass('hidden');
        }
    });
}

