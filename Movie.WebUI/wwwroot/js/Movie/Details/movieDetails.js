document.addEventListener('DOMContentLoaded', () => {

    let ratingSubmitBtn = document.getElementById('ratingInputSubmitBtn');

    if (ratingSubmitBtn != null)
    {
        ratingSubmitBtn.addEventListener('click', (e) => {
            e.preventDefault();

            let url = ratingSubmitBtn.getAttribute('data-url');
            let ratingValue = $("#ratingInput").val();
            url = url + ratingValue;

            fetch(url,{ method: 'POST'})
                .then(data => changeButtonToLabel(true))
                .catch(error => changeButtonToLabel(false));
        });
    }
});

function changeButtonToLabel(isSuccess) {
    const newLabel = document.createElement("label");
    if (isSuccess) {
        newLabel.textContent = "Rating submitted successfully!";
        newLabel.className = "text-success";
    }
    else {
        newLabel.textContent = "Rating could not be submitted!";
        newLabel.className = "text-failure";
    }

    $("#userRatingInput").replaceWith(newLabel);
}