// Function to toggle password visibility
function togglePasswordVisibility(inputId, iconId) {
    const passwordInput = document.getElementById(inputId);
    const toggleIcon = document.getElementById(iconId);
    if (passwordInput.type === "password") {
        passwordInput.type = "text";
        toggleIcon.classList.remove("fa-eye");
        toggleIcon.classList.add("fa-eye-slash");
    } else {
        passwordInput.type = "password";
        toggleIcon.classList.remove("fa-eye-slash");
        toggleIcon.classList.add("fa-eye");
    }
}

// Function to validate password and update tooltip
function validatePassword(password) {
    const passwordInput = document.getElementById('password');
    let errors = [];

    const lowercase = /[a-z]/.test(password);
    const uppercase = /[A-Z]/.test(password);
    const number = /\d/.test(password);
    const specialChar = /[!@#$%^&*(),.?":{}|<>]/.test(password);
    const length = password.length >= 8;

    if (!lowercase) {
        errors.push("Mínimo 1 letra minúscula");
    }
    if (!uppercase) {
        errors.push("Mínimo 1 letra maiúscula");
    }
    if (!number) {
        errors.push("Mínimo 1 número");
    }
    if (!specialChar) {
        errors.push("Mínimo 1 caractere especial");
    }
    if (!length) {
        errors.push("Mínimo 8 caracteres");
    }

    const tooltipInstance = bootstrap.Tooltip.getInstance(passwordInput);

    if (errors.length > 0) {
        const errorMessage = "A senha deve conter: " + errors.join(", ");
        passwordInput.setAttribute('data-bs-original-title', errorMessage);
        if (tooltipInstance) {
            tooltipInstance.hide(); // Hide before showing to ensure it updates
            tooltipInstance.dispose(); // Dispose to re-initialize with new title
        }
        new bootstrap.Tooltip(passwordInput, {
            title: errorMessage,
            trigger: 'manual', // We will control when it shows
            placement: 'top'
        }).show();
        passwordInput.classList.add('is-invalid'); // Add Bootstrap's invalid class for visual feedback
        passwordInput.classList.remove('is-valid');
    } else {
        if (tooltipInstance) {
            tooltipInstance.hide();
            tooltipInstance.dispose(); // Dispose the tooltip when no errors
        }
        passwordInput.classList.remove('is-invalid');
        passwordInput.classList.add('is-valid'); // Add Bootstrap's valid class
    }

    // Return true only if all conditions are met
    return lowercase && uppercase && number && specialChar && length;
}

// Function to validate confirm password
function validateConfirmPassword() {
    const password = document.getElementById('password').value;
    const confirmPasswordInput = document.getElementById('confirmPassword');
    const confirmPassword = confirmPasswordInput.value;

    if (password === confirmPassword && (password !== "" && confirmPassword !== "")) {
        confirmPasswordInput.classList.add('is-valid');
        confirmPasswordInput.classList.remove('is-invalid');
        return true;
    } else {
        confirmPasswordInput.classList.add('is-invalid');
        confirmPasswordInput.classList.remove('is-valid');
        return false;
    }
}

// Function to check if name and email fields are filled
function verificaCampos() {
    const nome = document.getElementById("name").value;
    const email = document.getElementById("email").value;

    return nome !== "" && email !== "";
}

// Event listeners for input fields to enable/disable register button
document.getElementById("name").addEventListener("input", function () {
    const isPasswordValid = validatePassword(document.getElementById("password").value);
    const isConfirmPasswordValid = validateConfirmPassword();
    document.getElementById("registerBtn").disabled = !(verificaCampos() && isPasswordValid && isConfirmPasswordValid);
});

document.getElementById("email").addEventListener("input", function () {
    const isPasswordValid = validatePassword(document.getElementById("password").value);
    const isConfirmPasswordValid = validateConfirmPassword();
    document.getElementById("registerBtn").disabled = !(verificaCampos() && isPasswordValid && isConfirmPasswordValid);
});

document.getElementById("password").addEventListener("input", function () {
    const password = document.getElementById("password").value;
    const isPasswordValid = validatePassword(password);
    const isConfirmPasswordValid = validateConfirmPassword();
    document.getElementById("registerBtn").disabled = !(verificaCampos() && isPasswordValid && isConfirmPasswordValid);
});

document.getElementById("confirmPassword").addEventListener("input", function () {
    const isPasswordValid = validatePassword(document.getElementById("password").value);
    const isConfirmPasswordValid = validateConfirmPassword();
    document.getElementById("registerBtn").disabled = !(verificaCampos() && isPasswordValid && isConfirmPasswordValid);
});

// Event listeners for password visibility toggles
document.getElementById("btnVeja").addEventListener("click", function () {
    togglePasswordVisibility("password", "togglePassword");
});

document.getElementById("btnVeja2").addEventListener("click", function () {
    togglePasswordVisibility("confirmPassword", "toggleConfirmPassword");
});

// Form submission handler
document.getElementById("authForm").addEventListener("submit", function (event) {
    event.preventDefault();

    let data = {
        nome: document.getElementById("name").value,
        telefone: document.getElementById("number").value,
        email: document.getElementById("email").value,
        senha: document.getElementById("password").value,
        confirmaSenha: document.getElementById("confirmPassword").value
    };

    fetch("/signup", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(data)
        })
        .then(async response => {
            const json = await response.json();

            if (!response.ok) {
                let errorMessage = "Erro ao cadastrar";

                if (json.error) {
                    errorMessage = json.error;
                } else if (json.errors) {
                    const firstErrorKey = Object.keys(json.errors)[0];
                    if (json.errors[firstErrorKey] && json.errors[firstErrorKey].length > 0) {
                        errorMessage = json.errors[firstErrorKey][0];
                    }
                }

                throw new Error(errorMessage);
            }

            return json;
        })
        .then(data => {
            console.log("Cadastro bem-sucedido:", data);

            let loginData = {
                email: document.getElementById("email").value,
                senha: document.getElementById("password").value
            };

            fetch("/login", {
                    method: "POST",
                    headers: {
                        "Content-Type": "application/json"
                    },
                    body: JSON.stringify(loginData)
                })
                .then(response => {
                    if (!response.ok) {
                        throw new Error("Erro ao realizar login");
                    }
                    return response.json();
                })
                .then(data => {
                    console.log("Login bem-sucedido:", data);
                    document.getElementById("authForm").reset();
                    if (data.token) {
                        localStorage.setItem("token", data.token);
                    }
                    window.location.href = "/Redirecionando";
                })
                .catch(error => {
                    console.error("Erro no login:", error);
                    alert("Falha ao realizar login!");
                });
        })
        .catch(error => {
            console.error("Erro:", error);
            const errorMessageElement = document.getElementById("error-message");
            errorMessageElement.innerText = error.message;
            errorMessageElement.style.display = "block";
        });
});