var imageInput = document.getElementById('imageInput');
var userImage = document.getElementById('userImage');

imageInput.addEventListener('change', function (event) {
    var selectedFile = event.target.files[0];
    var reader = new FileReader();

    reader.onload = function (event) {
        userImage.src = event.target.result;
    };

    reader.readAsDataURL(selectedFile);
});