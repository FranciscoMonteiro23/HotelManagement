using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HotelManagement.Models
{
    public class Cliente
    {
        [Key]
        public int ClienteID { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "O nome deve ter entre 2 e 50 caracteres")]
        [Display(Name = "Nome")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O apelido é obrigatório")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "O apelido deve ter entre 2 e 50 caracteres")]
        [Display(Name = "Apelido")]
        public string Apelido { get; set; } = string.Empty;

        [Required(ErrorMessage = "O documento/NIF é obrigatório")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "O documento deve ter entre 5 e 20 caracteres")]
        [Display(Name = "Documento/NIF")]
        public string Documento { get; set; } = string.Empty;

        [Required(ErrorMessage = "O telefone é obrigatório")]
        [Phone(ErrorMessage = "Formato de telefone inválido")]
        [StringLength(20, MinimumLength = 9, ErrorMessage = "O telefone deve ter entre 9 e 20 caracteres")]
        [Display(Name = "Telefone")]
        public string Telefone { get; set; } = string.Empty;

        [Required(ErrorMessage = "O email é obrigatório")]
        [EmailAddress(ErrorMessage = "Formato de email inválido")]
        [StringLength(100, ErrorMessage = "O email não pode exceder 100 caracteres")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [DataType(DataType.DateTime)]
        [Display(Name = "Data de Cadastro")]
        public DateTime DataCadastro { get; set; } = DateTime.Now;

        [Display(Name = "Nome Completo")]
        public string NomeCompleto
        {
            get { return $"{Nome} {Apelido}"; }
        }

        // Propriedade de navegação
        public ICollection<Reserva>? Reservas { get; set; }
    }
}