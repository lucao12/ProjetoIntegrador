function togglePasswordVisibility(inputId, iconId) {
    const passwordField = document.getElementById(inputId);
    const icon = document.getElementById(iconId);

    if (passwordField.type === "password") {
        passwordField.type = "text";
        icon.classList.remove("fa-eye");
        icon.classList.add("fa-eye-slash");
    } else {
        passwordField.type = "password";
        icon.classList.remove("fa-eye-slash");
        icon.classList.add("fa-eye");
    }
}

function validatePassword(password) {
    const lowercase = /[a-z]/.test(password);
    const uppercase = /[A-Z]/.test(password);
    const number = /\d/.test(password);
    const specialChar = /[!@#$%^&*(),.?":{}|<>]/.test(password);
    const length = password.length >= 8;

    document.getElementById('lowercase').className = lowercase ? 'valid' : 'invalid';
    document.getElementById('uppercase').className = uppercase ? 'valid' : 'invalid';
    document.getElementById('numberReq').className = number ? 'valid' : 'invalid';
    document.getElementById('specialChar').className = specialChar ? 'valid' : 'invalid';
    document.getElementById('length').className = length ? 'valid' : 'invalid';

    return lowercase && uppercase && number && specialChar && length;
}

document.getElementById("password").addEventListener("input", function () {
    const password = document.getElementById("password").value;
    const isPasswordValid = validatePassword(password);
});