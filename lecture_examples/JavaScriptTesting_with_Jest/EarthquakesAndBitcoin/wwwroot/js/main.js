import { validateEarthquakeData, validateBitcoinData } from './validation.js';

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


const alertTriangleSVG = `<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="currentColor" class="bi bi-exclamation-triangle-fill flex-shrink-0 me-2" viewBox="0 0 16 16" role="img" aria-label="Warning:">
    <path d="M8.982 1.566a1.13 1.13 0 0 0-1.96 0L.165 13.233c-.457.778.091 1.767.98 1.767h13.713c.889 0 1.438-.99.98-1.767L8.982 1.566zM8 5c.535 0 .954.462.9.995l-.35 3.507a.552.552 0 0 1-1.1 0L7.1 5.995A.905.905 0 0 1 8 5zm.002 6a1 1 0 1 1 0 2 1 1 0 0 1 0-2z"/>
  </svg>`;


// Bootstrap example code for dynamic alert, in a function to make it reusable
const alert = (message, type, element) => {
    const wrapper = document.createElement('div');
    wrapper.innerHTML = [
        `<div class="alert alert-${type} alert-dismissible d-flex align-items-center" role="alert">`,
        `${alertTriangleSVG}`,
        `   <div>${message}</div>`,
        '   <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>',
        '</div>'
    ].join('');

    element.append(wrapper);
}

// for a timer
var startTime, endTime;

// show both sets of results and the elapsed time
function showResults(data) {
    // Store the data in the window function so we can look at it easily in the console
    window.gdata = data;
    // stop the clock
    endTime = new Date();
    let dt = endTime - startTime; // this is in ms
    // add results to the DOM

    // Validate data before display
    if (validateEarthquakeData(data.earthquakes)) {
        displayEarthquakes(data.earthquakes);
        $('.results').removeAttr('hidden');
        $('#dt').text(dt);
    }
    else {
        const el = document.getElementById('error-alert');
        alert('Something seems to have gone wrong with retrieving earthquake data.  Please try again.', 'danger', el);
    }

    if (validateBitcoinData(data.bitcoinPrices)) {
        displayBitcoinPricesCryptoWatch(data.bitcoinPrices);
        $('.results').removeAttr('hidden');
        $('#dt').text(dt);
    }
    else {
        const el = document.getElementById('error-alert');
        alert('Something seems to have gone wrong with retrieving bitcoin data.  Please try again.', 'danger', el);
    }
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