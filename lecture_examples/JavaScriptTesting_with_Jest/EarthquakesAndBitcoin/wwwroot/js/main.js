// These match the controller action methods
const RUN_TYPE = {
    '1': 'Home/GetDataSynchronous',
    '2': 'Home/GetDataAsynchronous',
    '3': 'Home/GetDataAsynchronousParallel'
};

// Setup hiding and showing the loading animation
$(function() {
    $('#loadingAnimation').hide();
});

$(document).ajaxStart(function() {
    $('.results').hide();
    $('#loadingAnimation').show();
});
$(document).ajaxStop(function() {
    $('#loadingAnimation').hide();
    $('.results').show();
});

// for a timer
var startTime, endTime;

// show both sets of results and the elapsed time
function showResults(data) {
    gdata = data;
    // stop the clock
    endTime = new Date();
    let dt = endTime - startTime; // this is in ms
    // add results to the DOM

    displayEarthquakes(data.earthquakes);
    displayBitcoinPricesCryptoWatch(data.bitcoinPrices);

    $('#dt').text(dt);
    $('.results').removeAttr('hidden');

    console.log(dt);
    console.log(data);
}

// Upon an error, just print it to the console
function handleError(xhr, ajaxOptions, thrownError) {
    console.log('ajax error: ' + xhr.status);
}

// Register a click callback on the submit button to get things going:
//   fetch the data via AJAX and then show the results
$('#submit').click(function(event) {
    event.preventDefault();

    // Which radio button is selected determines the endpoint used
    var rbval = $('input[name=runType]:checked').val();
    var selectedMethod = RUN_TYPE[rbval];

    // start the clock
    startTime = new Date();

    // Version 1: Using jQuery

    $.ajax({
        type: 'GET',
        url: selectedMethod,
        //data: queryData,      // only if we needed to send something
        success: showResults,
        error: handleError
    });

    // Version 2: Using built-in Fetch API.
    const request = new Request(selectedMethod, {
        method: 'GET',
        //body: someDataWeMightNeedToSend,
        headers: new Headers({
            'Accept': 'application/json'
        })
    });
    /*    fetch(request) // returns a Promise that resolves to a Request object
            .then((response) => { // then() invokes the callback when the promise is resolved
                if (response.ok)
                    return response.json();
                else
                    throw Error(response.statusText);
            })
            .then((data) => {
                showResults(data);
            })
            .catch((error) => { // catch() is used for the rejected state of the promise
                console.log(error);
            })
    */
    // Version 3: Using the built-in Fetch API but with explicit async/await syntax
    /*
        const response = await fetch(request);
        if (response.ok) {
            const jsonData = await response.json();
            showResults(jsonData);
        } else {
            console.log(response.status, response.statusText);
        }
    */
});



// Display the earthquake data in a table
function displayEarthquakes(data) {
    $("#theQuakes").empty();
    console.log(data);
    for (let i = 0; i < data.length; ++i) {
        $("#theQuakes").append($("<tr><th>" + data[i]["magnitude"] + "</th><td>" + new Date(data[i]["eTime"]) + "</td><td>" + data[i]["location"] + "</td></tr>"));
    }
}

// Display bitcoin prices in a plot
function displayBitcoinPricesCryptoWatch(data) {
    console.log(data);
    let xData = data.map(a => new Date(a.closeTime * 1000));
    var close = {
        x: xData,
        y: data.map(a => a.closePrice),
        mode: 'lines',
        type: 'scatter',
        name: 'Closing price'
    };
    var low = {
        x: xData,
        y: data.map(a => a.lowPrice),
        mode: 'lines',
        type: 'scatter',
        name: 'Low Price'
    };
    var high = {
        x: xData,
        y: data.map(a => a.highPrice),
        mode: 'lines',
        type: 'scatter',
        name: 'High Price'
    };
    var plotData = [low,high,close];
    var layout = {
        title: {
            text: 'Bitcoin Price (historical)',
            font: {
                family: 'Courier New, monospace',
                size: 24
            },
            xref: 'paper',
            x: 0.5,
        },
        xaxis: {
            title: {
                text: 'Date',
                font: {
                    family: 'Courier New, monospace',
                    size: 18,
                    color: '#7f7f7f'
                }
            },
        },
        yaxis: {
            title: {
                text: 'Price ($ USD)',
                font: {
                    family: 'Courier New, monospace',
                    size: 18,
                    color: '#7f7f7f'
                }
            }
        }
    };
    Plotly.newPlot('bitcoinPlot', plotData, layout);
    $('#coindeskDisclaimer').text(data.disclaimer);
}

// Display bitcoin prices in a plot
function displayBitcoinPricesCoinDesk(data) {
    console.log(data);
    var trace = {
        x: data.days,
        y: data.closingPrices,
        mode: 'lines',
        type: 'scatter'
    };
    var plotData = [trace];
    var layout = {
        title: {
            text: 'Bitcoin Price (historical)',
            font: {
                family: 'Courier New, monospace',
                size: 24
            },
            xref: 'paper',
            x: 0.5,
        },
        xaxis: {
            title: {
                text: 'Date',
                font: {
                    family: 'Courier New, monospace',
                    size: 18,
                    color: '#7f7f7f'
                }
            },
        },
        yaxis: {
            title: {
                text: 'Price ($ USD)',
                font: {
                    family: 'Courier New, monospace',
                    size: 18,
                    color: '#7f7f7f'
                }
            }
        }
    };
    Plotly.newPlot('bitcoinPlot', plotData, layout);
    $('#coindeskDisclaimer').text(data.disclaimer);
}