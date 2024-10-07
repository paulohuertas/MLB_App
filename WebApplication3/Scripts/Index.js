function GetGameList() {
    $.ajax({
        type: 'POST',
        url: 'Home/GetGameList',
        dataType: 'json',
        success: function (data) {
            console.log(data);
            $.each(data, function (i, item) {
                console.log(item);
                let logoHome = GetTeamLogos(item.Home);
                let logoAway = GetTeamLogos(item.Away);
                let homeRun = parseInt(item.HomeRun);
                let awayRun = parseInt(item.AwayRun);
                let winnerTeam = (homeRun > awayRun ? logoHome :logoAway);
                var rows =
                    "<tr id='" + item.GameId + "'>"
                    + "<td scope='row' class='text-center'>&#x26BE;</td>"
                    + "<td scope='row' class='text-center'>" + convertDate(item.GameDate) + "</td>"
                    + "<td scope='row' class='text-center'>" + item.GameType.replace("_", " ") + "</td>"
                    + "<td scope='row' class='text-center'>" + item.GameStatus + "</td>"
                    + "<td scope='row' class='text-center'>" + "<img src='" + winnerTeam + "'" + "style='width: 20px; height: 20px; margin: 10px;' /> " + "</td>"
                    + "<td scope='row' class='text-center'>" + item.Attendance + "</td>"
                    + "<td scope='row' class='text-center'>" + item.Venue + "</td>"
                    + "<td scope='row' class='text-center'>" + item.Home + "<img src='" + logoHome + "'" + "style='width: 20px; height: 20px; margin: 10px;' /> " + "</td>"
                    + "<td scope='row' class='text-center'>" + item.HomeRun + "X" + item.AwayRun + "</td>"
                    + "<td scope='row' class='text-center'>" + "<img src='" + logoAway + "'" + "style='width: 20px; height: 20px; margin: 10px;' /> " + item.Away + "</td>";

                $('#main-body').append(rows);
            });
        },
        error: function (ex) {
            console.log("Deu erro " + ex.responseText);
        }
    });
}

function GetTeamLogos(teamLogo) {
    var xhr = $.ajax({
        type: 'POST',
        url: 'Data/GetTeamLogo',
        data: { logo: teamLogo },
        dataType: 'json',
        async: false,
        success: function (myData) {
            console.log("Printing my data " + myData);

        },
        error: function (ex) {
            console.log("Deu erro " + ex.responseText);
        }
    });

    return JSON.parse(xhr.responseText);
}

function GetTeamLogos1(key) {
    let logoObject = {};
    logoObject["ARI"] = "https://www.mlbstatic.com/team-logos/team-cap-on-light/109.svg";
    logoObject["ATL"] = "https://www.mlbstatic.com/team-logos/team-cap-on-light/144.svg";
    logoObject["BAL"] = "https://www.mlbstatic.com/team-logos/team-cap-on-light/110.svg";
    logoObject["BOS"] = "https://www.mlbstatic.com/team-logos/team-cap-on-light/111.svg";
    logoObject["CHC"] = "https://www.mlbstatic.com/team-logos/team-cap-on-light/112.svg";
    logoObject["CHW"] = "https://www.mlbstatic.com/team-logos/team-cap-on-light/145.svg";
    logoObject["CIN"] = "https://www.mlbstatic.com/team-logos/team-cap-on-light/113.svg";
    logoObject["CLE"] = "https://www.mlbstatic.com/team-logos/team-cap-on-light/114.svg";
    logoObject["COL"] = "https://www.mlbstatic.com/team-logos/team-cap-on-light/115.svg";
    logoObject["DET"] = "https://www.mlbstatic.com/team-logos/team-cap-on-light/116.svg";
    logoObject["HOU"] = "https://www.mlbstatic.com/team-logos/team-cap-on-light/117.svg";
    logoObject["KC"] = "https://www.mlbstatic.com/team-logos/team-cap-on-light/118.svg";
    logoObject["LAA"] = "https://www.mlbstatic.com/team-logos/team-cap-on-light/108.svg";
    logoObject["LAD"] = "https://www.mlbstatic.com/team-logos/team-cap-on-light/119.svg";
    logoObject["MIA"] = "https://www.mlbstatic.com/team-logos/team-cap-on-light/146.svg";
    logoObject["MIL"] = "https://www.mlbstatic.com/team-logos/team-cap-on-light/158.svg";
    logoObject["MIN"] = "https://www.mlbstatic.com/team-logos/team-cap-on-light/142.svg";
    logoObject["NYM"] = "https://www.mlbstatic.com/team-logos/team-cap-on-light/121.svg";
    logoObject["NYY"] = "https://www.mlbstatic.com/team-logos/team-cap-on-light/147.svg";
    logoObject["OAK"] = "https://www.mlbstatic.com/team-logos/team-cap-on-light/133.svg";
    logoObject["PHI"] = "https://www.mlbstatic.com/team-logos/team-cap-on-light/143.svg";
    logoObject["PIT"] = "https://www.mlbstatic.com/team-logos/team-cap-on-light/134.svg";
    logoObject["SD"] = "https://www.mlbstatic.com/team-logos/team-cap-on-light/135.svg";
    logoObject["SF"] = "https://www.mlbstatic.com/team-logos/team-cap-on-light/137.svg";
    logoObject["SEA"] = "https://www.mlbstatic.com/team-logos/team-cap-on-light/136.svg";
    logoObject["STL"] = "https://www.mlbstatic.com/team-logos/team-cap-on-light/138.svg";
    logoObject["TB"] = "https://www.mlbstatic.com/team-logos/team-cap-on-light/139.svg";
    logoObject["TEX"] = "https://www.mlbstatic.com/team-logos/team-cap-on-light/140.svg";
    logoObject["TOR"] = "https://www.mlbstatic.com/team-logos/team-cap-on-light/141.svg";
    logoObject["WAS"] = "https://www.mlbstatic.com/team-logos/team-cap-on-light/120.svg";

    if (key != "" || key != null || key != undefined) {
        return logoObject[key];
    }
}

function convertDate(dateString) {
    if (dateString == undefined || dateString == null || dateString == "") return "";

    let year = dateString.substring(0, 4);
    let month = dateString.substring(4, 6);
    let day = dateString.substring(6, 8);

    let finalDate = `${day}/${month}/${year}`;

    return finalDate;
}

