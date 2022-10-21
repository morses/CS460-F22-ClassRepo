
console.log("hello from main.js");

// Once the DOM is ready, execute everything in this function to set up the UI
$(function () {
    // Prevent this form from being submitted normally by the browser (by hitting enter or clicking the button) and substitute our own behavior
    $("#numberForm").submit(function (event) {
        event.preventDefault();
    });

    $("#earthquakeForm").submit(function (event) {
        event.preventDefault();
    });

    // Populate the earthquake time range select list
    $.ajax({
        type: "GET",
        dataType: "json",
        url: "/api/earthquakes/timeranges",
        success: populateEarthquakesTimeRangeSelect,
        error: errorOnAjax
    });

    // Add a click handler on the button to request random numbers
    $("#numberButton").click(function () {
        let n = $("#numberCountInput").val();
        let address = "/api/numbers/list/?count=" + n;
        $.ajax({
            type: "GET",
            dataType: "json",
            url: address,
            success: displayNumbers,
            error: errorOnAjax
        });
    });

    // Add a click handler to request new earthquakes
    $("#earthquakeButton").click(function () {
        console.log("earthquake button pressed");
        let params = { timeRange: $("#timeRange").val() };
        let address = "/api/earthquakes/"+params.timeRange;
        $.ajax({
            type: "GET",
            dataType: "json",
            url: address,
            success: displayEarthquakes,
            error: errorOnAjax
        });
    });
});

// Callback functions that execute once the AJAX calls return

function errorOnAjax() {
    console.log("ERROR in ajax request");
    // take care of the error, maybe display a message to the user
    // ...
}

/*
Expects
[
    {text: "hour", value: 1},
    {text: "day", value: 2},
    {text: "week", value: 3},
    {text: "month", value: 4}
]
*/
function populateEarthquakesTimeRangeSelect(data) {
    // Use something other than a for loop, this one is a jQuery $.each() rather than a JS Array.foreach()
    $.each(data, function (index, item) {
        let options = `<option value="${item.value}">${item.text}</option>`;
        $("#timeRange").append($(options));
    });

    // Set the first one to be selected
    $("#timeRange").val(data[0].value);
}

/*
Expects:
    { message: "Random Numbers API", count: 10, max: 1000, domain: [Number], range: [Number]}
*/
function displayNumbers(data) {
    console.log(data);
    $("#numberMessage").text(data["message"]);
    $("#numberCount").text(data.num);
    $("#numbers").text(data["range"].join(", "));
    let trace = {
        x: data.domain,
        y: data.range,
        mode: 'lines',
        type: 'scatter'
    };
    let plotData = [trace];
    let layout = {};
    Plotly.newPlot('theplot', plotData, layout);
}


/*
Expects:
[
    {magnitude: 2.9, location: "60 km SSW of Whites City, New Mexico"},
    {magnitude: 1.1, location: "30 km SSE of Mina, Nevada"}
]
*/
function displayEarthquakes(data) {
    console.log("received: ",data);
    $("#earthquakeTable>tbody").empty();
    for (let i = 0; i < data.length; ++i) {
        let earthquakeTR =
            `<tr>
                <td>${data[i]["magnitude"]}</td>
                <td>${data[i]["location"]}</td>
            </tr>`;
        $("#earthquakeTable>tbody").append($(earthquakeTR));
        $("#earthquakeTable").show();
    }
}