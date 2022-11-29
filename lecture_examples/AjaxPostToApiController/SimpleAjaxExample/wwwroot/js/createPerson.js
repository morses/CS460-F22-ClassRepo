console.log("CREATE Person...");

function getPersonValues() {
	const age = document.getElementById("age-input");
	const name = document.getElementById("name-input");
	const anniversary = document.getElementById("anniversary-input");
	const createPersonForm = document.getElementById("create-person-form");
	if (!createPersonForm.checkValidity()) {
		console.log("One or more form values are invalid");
		return { status: false };
	}
	return {
		status: true,
		age: Number(age.value),
		name: name.value,
		anniversary: new Date(anniversary.value + "T00:00:00").toISOString()
	}
}

function afterAddPerson(data) {
	console.log("Successfully added person: " + data.status);
}

function errorOnAjax(data) {
	console.log("Error in AJAX call: " + data.status);
}

// Initial setup
$(function () {
	$("#create-person-form").submit(function (event) {
		// prevent automatic submission of the form from button press or typing enter
		event.preventDefault();
	});
	// Add callback to the add person button
	$("#add-person-button").click(function () {
		const values = getPersonValues();
		console.log(values);
		if (values.status) {
			$.ajax({
				method: "POST",
				url: "/api/person",
				dataType: "json",					// data type expected in response
				contentType: "application/json; charset=UTF-8",	// data type to send
				data: JSON.stringify(values),
				success: afterAddPerson,
				error: errorOnAjax
			});
		}
		else {
			// report validation issues
		}
	});

});