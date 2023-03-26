
// We'll do this one with 'vanilla JS'

// Assumes select list has options: all, new, approved, rejected

document.addEventListener('DOMContentLoaded', initializePage, false);

function initializePage() {
    const commentSelector = document.getElementById('comment-select');
    commentSelector.addEventListener('change', (event) => {
        const selected = event.target.value;
        if (selected) {
            getAndDisplayCommentsByType(selected);
        }
    });
}

async function getAndDisplayCommentsByType(value) {
    console.log('Need to fetch comments that are: ' + value);
    const request = new Request('/api/comments?' + new URLSearchParams({ status: value }), {
        method: 'GET',
        headers: new Headers({
            'Accept': 'application/json'
        })
    });
    const response = await fetch(request);
    if(response.ok) {
        const jsonData = await response.json();
        console.log(jsonData);
        buildCommentTable(jsonData);
    }
    else {
        console.log(response.status, response.statusText);
    }
}

function buildCommentTable(data) {
    let columns = [{ label: "Approve/Reject", key: "", valuefn: (j, key) => (`<button data-id="${data[j].id}" data-command="approve" type="button" class="${data[j].advisorRating == 1 ? "btn-success" : ""} rateButton btn btn-sm"><i class="bi bi-emoji-smile"></i></button><button data-id="${data[j].id}" data-command="reject" type="button" class="${data[j].advisorRating == -1 ? "btn-danger" : ""} rateButton btn btn-sm"><i class="bi bi-emoji-frown-fill"></i></button>`) },
                   { label: "Question",       key: "question",  valuefn: (j, key) => (data[j][key]) },
                   { label: "Comment",        key: "comment",   valuefn: (j, key) => (data[j][key]) }];

    let table = document.getElementById('comment-table');
    genTable(data.length, columns, table, true);
    // register click callbacks to approve or reject
    let rateButtons = document.querySelectorAll('.rateButton');
    console.log(rateButtons);
    rateButtons.forEach(b => b.addEventListener('click', (event) => {
        console.log(event.target);
        let id = event.currentTarget.getAttribute('data-id');
        let cmd = event.currentTarget.getAttribute('data-command');
        console.log(`id: ${id} command: ${cmd}`);
    }));
}

/*
Here's what we need for input:
The data for the table

    let data = [{comment:"Blue",id:1,question:"What is your favorite color?"},
				{comment:"Red",id:2,question:"What is your favorite color?"},
                {comment:"Yes",id:3,question:"What is today's date?"}];

And a "column descriptor", that has the label for each column, the key from the data above to access for that column,
and a function that is used to put in that cell
    let columns = [{label: "Approve/Reject", key: "",         valuefn: (rowIndex,key) => (`<button id="b{rowIndex}">...</button>`)},
                   {label: "Question",       key: "question", valuefn: (rowIndex,key) => (data[rowIndex][key]) },
                   {label: "Comment",        key: "comment",  valuefn: (rowIndex,key) => (data[rowIndex][key]) }];
*/
function genTable(rowCount, columnDescriptor, table, head = true, empty=true) {
    if (empty) {
        while (table.hasChildNodes()) {
            table.removeChild(table.firstChild);    // or use deleteRow(i)
        }
    }
    if (head) {
        const thead = document.createElement('thead');
        let tr = document.createElement('tr');
        columnDescriptor.forEach(d => {
            let th = document.createElement('th');
            th.innerHTML = d.label;
            th.setAttribute('scope', 'col');
            tr.appendChild(th);
        });
        thead.appendChild(tr);
        table.appendChild(thead);
    }

    const tbody = document.createElement('tbody');
    for (let row = 0; row < rowCount; row++) {
        tr = document.createElement('tr');
        for (let col = 0; col < columnDescriptor.length; ++col) {
            let td = document.createElement('td');
            let colEntry = columnDescriptor[col];
            td.innerHTML = colEntry.valuefn(row, colEntry.key);
            tr.appendChild(td);
        }
        tbody.appendChild(tr);
    }
    table.appendChild(tbody);
}
