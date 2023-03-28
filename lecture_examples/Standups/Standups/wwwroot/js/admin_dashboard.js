
// We'll do this one with 'vanilla JS'

// Assumes select list has options: all, new, approved, rejected

// TODO: add error handling to alert the user something has gone wrong

/* NOTE: This code potentially exposes a security vulnerability.  What is it, why and how can it be addressed? */

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

function updatePage() {
    const commentSelector = document.getElementById('comment-select');
    const value = commentSelector.options[commentSelector.selectedIndex].value;
    getAndDisplayCommentsByType(value);
}

async function getAndDisplayCommentsByType(value) {
    //console.log('Need to fetch comments that are: ' + value);
    const request = new Request('/api/comments?' + new URLSearchParams({ status: value }), {
        method: 'GET',
        headers: new Headers({
            'Accept': 'application/json'
        })
    });
    const response = await fetch(request);
    if(response.ok) {
        const jsonData = await response.json();
        //console.log(jsonData);
        buildCommentTable(jsonData);
    }
    else {
        console.log(response.status, response.statusText);
    }
}

async function processRateClick(id, command, questionId, submissionDate, comment) {
    let value = 0;
    const commands = { "approve": 1, "reject": -1 };
    if (command in commands) {
        value = commands[command];
    }
    const values = { Id: Number.parseInt(id), AdvisorRating: value,  QuestionId: Number.parseInt(questionId), SubmissionDate: submissionDate, Comment: comment};
    const request = new Request(`/api/comments/${id}`, {
        method: 'PATCH',
        headers: new Headers({
            'Accept': 'application/json',
            'Content-Type': 'application/json; charset=UTF-8'
        }),
        body: JSON.stringify(values)
    });
    const response = await fetch(request);
    if (response.ok) {
        //const jsonData = await response.json();
        //console.log(jsonData);
        updatePage();
    }
    else {
        console.log(response.status, response.statusText);
    }
}

const approveRejectButtonHTMLFn = (j, key, data) => `
<button data-id="${data[j].id}" 
        data-command="approve"
        data-comment="${data[j].comment}" 
        data-questionId="${data[j].questionId}" 
        data-submissionDate="${data[j].submissionDate}" 
        type="button" class="${data[j].advisorRating == 1 ? "btn-success" : ""} rateButton btn btn-sm">
   <i class="bi bi-emoji-smile"></i>
</button>
<button data-id="${data[j].id}" 
        data-command="reject"
        data-comment="${data[j].comment}" 
        data-questionId="${data[j].questionId}"
        data-submissionDate="${data[j].submissionDate}" 
        type="button" class="${data[j].advisorRating == -1 ? "btn-danger" : ""} rateButton btn btn-sm">
   <i class="bi bi-emoji-frown-fill"></i>
</button>
`;

const defaultValueFn = (rowIndex, key, data) => (data[rowIndex][key]);

const columnDescriptor =
    [{label: "Approve/Reject", key: "", valuefn: approveRejectButtonHTMLFn },
        { label: "Question", key: "question", valuefn: defaultValueFn },
        { label: "Comment", key: "comment", valuefn: defaultValueFn }];

function buildCommentTable(data) {
    let table = document.getElementById('comment-table');
    genTable(data, columnDescriptor, table, true);
    // register click callbacks to approve or reject
    let rateButtons = document.querySelectorAll('.rateButton');
    rateButtons.forEach(b => b.addEventListener('click', (event) => {
        let id = event.currentTarget.getAttribute('data-id');
        let cmd = event.currentTarget.getAttribute('data-command');
        let comment = event.currentTarget.getAttribute('data-comment');
        let questionId = event.currentTarget.getAttribute('data-questionId');
        let submissionDate = event.currentTarget.getAttribute('data-submissionDate');
        //console.log(`id: ${id} command: ${cmd} questionId: ${questionId} submissionDate: ${submissionDate} comment: ${comment}`);
        processRateClick(id, cmd, questionId, submissionDate, comment);
    }));
}

/*
For more, see: https://jsfiddle.net/morses/1azg0k5x/59/
*/
function genTable(data, columnDescriptor, table, head = true, empty=true) {
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
    for (let row = 0; row < data.length; row++) {
        tr = document.createElement('tr');
        for (let col = 0; col < columnDescriptor.length; ++col) {
            let td = document.createElement('td');
            let colEntry = columnDescriptor[col];
            td.innerHTML = colEntry.valuefn(row, colEntry.key, data);
            tr.appendChild(td);
        }
        tbody.appendChild(tr);
    }
    table.appendChild(tbody);
}
