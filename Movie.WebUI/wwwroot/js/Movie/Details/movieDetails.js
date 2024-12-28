document.addEventListener('DOMContentLoaded', () => {

    let ratingSubmitBtn = document.getElementById('ratingInputSubmitBtn');

    if (ratingSubmitBtn != null)
    {
        ratingSubmitBtn.addEventListener('click', () => {
            let url = ratingSubmitBtn.getAttribute('data-url');
            let ratingValue = $("#ratingInput").val();
            url = url + ratingValue;

            fetch(url,
                {
                    method: 'POST'
                })
                .then(response => response.json())
                .then(data => console.log('Created post:', data))
                .catch(error => console.error('Error creating post:', error));
        });
    }
});