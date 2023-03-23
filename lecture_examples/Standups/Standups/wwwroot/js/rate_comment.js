function rateThanks(jsonData) {
    //console.log("rateThanks");
    //console.log(jsonData.status);
    $("#" + window.questionIDLastClicked).click();
}

// TODO: Need better error handling here
function rateError(jsonData) {
    console.log("rateError");
    console.log(jsonData.status);
}

// At page load, and after a rate has been made we need to repopulate the table that shows comments and ratings
function populateComments(jsonData) {
    //console.log(jsonData);
    $("#commentsTable tbody").empty();
    if (jsonData.length == 0) {
        $("<tr>").append(
            $("<td>").text("There are no comments yet for this question"),
            $("<td>").text("")
        ).appendTo("#commentsTable");
    }
    else {
        for (var i = 0; i < jsonData.length; i++) {
            var cmt = jsonData[i];
            var userRating = cmt.userRating;
            var downRating = userRating < 0 ? "btn-danger" : "btn-secondary";
            var upRating = userRating > 0 ? "btn-success" : "btn-secondary";
            var rateDownHtml = "<button id='" + "d" + cmt.id + "' type='button' class='rateButton btn " + downRating + " btn-sm'><i class='bi bi-hand-thumbs-down-fill'></i></button>";
            var rateUpHtml = "<button id='" + "u" + cmt.id + "' type='button' class='rateButton btn " + upRating + " btn-sm'><i class='bi bi-hand-thumbs-up-fill'></i></button>";
            var rateBarHtml = "<div class='btn-group' role='group' aria-label='Thumbs up or thumbs down'>" + rateUpHtml + rateDownHtml + "</div>";
            $("<tr>").append(
                $("<td>").html("<p>" + cmt.comment + "</p>" + "<div class='float-left'>" + rateBarHtml + "</div>")
            ).appendTo("#commentsTable");
        }
    }

    // Add click handlers to rate buttons (thumbs up or down) in the comments
    $(".rateButton").on("click", function (e) {
        var id = $(this).attr("id");        // we stored the comment id and the (d)own or (u)p command in the id of the button
        var action = id.charAt(0) == "d" ? "DOWN" : "UP";
        var commentID = id.substring(1);
        //console.log("button clicked" + action + " " + commentID);
        var formData = { CommentId: commentID, Action: action };    // all we need to send is the id of the comment and the action.  The user is obtained from the auth cookie.
        $.ajax({
            type: "POST",
            dataType: "json",
            data: formData,
            url: "/api/Ratings",
            success: rateThanks,
            error: rateError
        });
        e.preventDefault();
    });
}

$(document).ready(function () {
    // Add click handler to each button.  Makes an ajax call to get comments
    $("#questions").on("click", "button", null, function () {
        var qid = $(this).attr("id");
        window.questionIDLastClicked = qid;
        $("#questions button").removeClass("active");
        $(this).addClass("active");
        $.ajax({
            type: "GET",
            dataType: "json",
            url: "/api/Ratings/" + qid,
            success: populateComments
        });
    });
});