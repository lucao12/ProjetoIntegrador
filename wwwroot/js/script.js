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

function validateConfirmPassword() {
    const password = document.getElementById('password').value;
    const confirmPassword = document.getElementById('confirmPassword').value;

    if (password === confirmPassword && (password != "" && confirmPassword != "")) {
        document.getElementById('passwordMatch').className = 'valid';
        return true;
    } else {
        document.getElementById('passwordMatch').className = 'invalid';
        return false;
    }
}

function verificaCampos() {
    const nome = document.getElementById("name").value
    const email = document.getElementById("email").value

    if (nome == "" || email == "") {
        return false
    } else {
        return true
    }
}

document.getElementById("name").addEventListener("input", function () {
    const password = document.getElementById("password").value;
    const isPasswordValid = validatePassword(password);
    const isConfirmPasswordValid = validateConfirmPassword();

    let simounao = verificaCampos()
    if (simounao == true) {
        document.getElementById("registerBtn").disabled = !(isPasswordValid && isConfirmPasswordValid);
    } else {
        document.getElementById("registerBtn").disabled = true;
    }
});

document.getElementById("email").addEventListener("input", function () {
    const password = document.getElementById("password").value;
    const isPasswordValid = validatePassword(password);
    const isConfirmPasswordValid = validateConfirmPassword();

    let simounao = verificaCampos()
    if (simounao == true) {
        document.getElementById("registerBtn").disabled = !(isPasswordValid && isConfirmPasswordValid);
    } else {
        document.getElementById("registerBtn").disabled = true;
    }
});


document.getElementById("password").addEventListener("input", function () {
    const password = document.getElementById("password").value;
    const isPasswordValid = validatePassword(password);
    const isConfirmPasswordValid = validateConfirmPassword();

    let simounao = verificaCampos()
    if (simounao == true) {
    document.getElementById("registerBtn").disabled = !(isPasswordValid && isConfirmPasswordValid);
    } else {
        document.getElementById("registerBtn").disabled = true;
    }
});

document.getElementById("confirmPassword").addEventListener("input", function () {
    const isPasswordValid = validatePassword(document.getElementById("password").value);
    const isConfirmPasswordValid = validateConfirmPassword();

    let simounao = verificaCampos()
    if (simounao == true) {
    document.getElementById("registerBtn").disabled = !(isPasswordValid && isConfirmPasswordValid);
    }
});

document.getElementById("btnVeja").addEventListener("click", function () {
    togglePasswordVisibility("password", "togglePassword");
});

document.getElementById("btnVeja2").addEventListener("click", function () {
    togglePasswordVisibility("confirmPassword", "toggleConfirmPassword");
});

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
        headers: { "Content-Type": "application/json" },
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
                headers: { "Content-Type": "application/json" },
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