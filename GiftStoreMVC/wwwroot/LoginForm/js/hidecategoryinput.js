var option1 = document.getElementById('option2');
var option2 = document.getElementById('option3');
var option2 = document.getElementById('option4');
var category = document.getElementById('category');

function handleUserTypeChange() {
    if (!option1.checked) {
        category.style.display = 'none';
        category.disabled = true;
    } else {
        category.style.display = 'block';
        category.disabled = false;
    }
}
option1.addEventListener('change', handleUserTypeChange);
option2.addEventListener('change', handleUserTypeChange);
option3.addEventListener('change', handleUserTypeChange);