const signUp = document.querySelector('.sign-up');
const signIn = document.querySelector('.sign-in');


const btn1 = document.querySelector('.opposite-btn1');
const btn2 = document.querySelector('.opposite-btn2');


// Switches to 'Create Account'
btn1.addEventListener('click', () => {
    signUp.style.display = 'block';
    signIn.style.display = 'none';
});

// Switches to 'Sign In'
btn2.addEventListener('click', () => {
    signUp.style.display = 'none';
    signIn.style.display = 'block';
});