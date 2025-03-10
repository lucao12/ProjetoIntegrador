using System.ComponentModel.DataAnnotations;

namespace ProjetoIntegrador.ViewModel
{
    public class UsuarioCadastroViewModel
    {
        [Required(ErrorMessage = "Campo nome é de preenchimento obrigatório")]
        [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres")]
        [RegularExpression(@"^[a-zA-Z''-'\s]{1,100}$", ErrorMessage = "Nome contém caracteres inválidos")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "Campo email é de preenchimento obrigatório")]
        [EmailAddress(ErrorMessage = "Campo email deve ser um endereço de email válido")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Campo senha é de preenchimento obrigatório")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "A senha deve ter entre 8 e 100 caracteres")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\p{P}\p{S}])[A-Za-z\d\p{P}\p{S}]{8,}$", ErrorMessage = "A senha deve conter no mínimo 8 caracteres, com pelo menos uma letra maiúscula, uma letra minúscula, um número e um caractere especial")]
        public string Senha { get; set; }
        [Required(ErrorMessage = "Confirmação de senha é obrigatória")]
        [Compare("Senha", ErrorMessage = "As senhas não coincidem")]
        public string ConfirmaSenha { get; set; }

        public string? Telefone { get; set; }
    }
    public class UsuarioLoginViewModel
    {
        [Required(ErrorMessage = "Campo email é de preenchimento obrigatório")]
        [EmailAddress(ErrorMessage = "Campo email deve ser um endereço de email válido")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Campo senha é de preenchimento obrigatório")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\p{P}\p{S}])[A-Za-z\d\p{P}\p{S}]{8,}$", ErrorMessage = "A senha deve conter no mínimo 8 caracteres, com pelo menos uma letra maiúscula, uma letra minúscula, um número e um caractere especial")]
        public string Senha { get; set; }
    }
    public class UsuarioMudaSenhaViewModel
    {
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\p{P}\p{S}])[A-Za-z\d\p{P}\p{S}]{8,}$", ErrorMessage = "A senha deve conter no mínimo 8 caracteres, com pelo menos uma letra maiúscula, uma letra minúscula, um número e um caractere especial")]
        public string Senha { get; set; }
    }
    public class UsuarioUpdateViewModel
    {
        [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres")]
        [RegularExpression(@"^[a-zA-Z''-'\s]{1,100}$", ErrorMessage = "Nome contém caracteres inválidos")]
        public string? Nome { get; set; }
        [EmailAddress(ErrorMessage = "Campo email deve ser um endereço de email válido")]
        public string? Email { get; set; }
        public string? Sexo { get; set; }
        public int? Idade { get; set; }
        public double? Peso { get; set; }
        public double? Altura { get; set; }
        [Required]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\p{P}\p{S}])[A-Za-z\d\p{P}\p{S}]{8,}$", ErrorMessage = "A senha deve conter no mínimo 8 caracteres, com pelo menos uma letra maiúscula, uma letra minúscula, um número e um caractere especial")]
        public string SenhaAntiga { get; set; }
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\p{P}\p{S}])[A-Za-z\d\p{P}\p{S}]{8,}$", ErrorMessage = "A senha deve conter no mínimo 8 caracteres, com pelo menos uma letra maiúscula, uma letra minúscula, um número e um caractere especial")]
        public string? NovaSenha { get; set; }
        public string? Telefone { get; set; }
    }
    public class NutriSocailUpdateViewModel
    {
        public string? Instagram { get; set; }
    }

    public class UsuarioUpdateAdminViewModel
    {
        [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres")]
        [RegularExpression(@"^[a-zA-Z''-'\s]{1,100}$", ErrorMessage = "Nome contém caracteres inválidos")]
        public string? Nome { get; set; }
        [EmailAddress(ErrorMessage = "Campo email deve ser um endereço de email válido")]
        public string? Email { get; set; }
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\p{P}\p{S}])[A-Za-z\d\p{P}\p{S}]{8,}$", ErrorMessage = "A senha deve conter no mínimo 8 caracteres, com pelo menos uma letra maiúscula, uma letra minúscula, um número e um caractere especial")]
        public string? Senha { get; set; }

        public string? Role { get; set; }

        public string? Telefone { get; set; }
    }
    public class UsuarioCadastroAdminViewModel
    {
        [Required(ErrorMessage = "Campo nome é de preenchimento obrigatório")]
        [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres")]
        [RegularExpression(@"^[a-zA-Z''-'\s]{1,100}$", ErrorMessage = "Nome contém caracteres inválidos")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "Campo email é de preenchimento obrigatório")]
        [EmailAddress(ErrorMessage = "Campo email deve ser um endereço de email válido")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Campo senha é de preenchimento obrigatório")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\p{P}\p{S}])[A-Za-z\d\p{P}\p{S}]{8,}$", ErrorMessage = "A senha deve conter no mínimo 8 caracteres, com pelo menos uma letra maiúscula, uma letra minúscula, um número e um caractere especial")]
        public string Senha { get; set; }
        public string? Telefone { get; set; }
        public string Role { get; set; }
    }
}
