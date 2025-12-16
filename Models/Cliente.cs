using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HotelManagement.Models
{
    public class Cliente
    {
        public int ClienteID { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório")]
        [StringLength(50, ErrorMessage = "O nome não pode exceder 50 caracteres")]
        [Display(Name = "Nome")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O apelido é obrigatório")]
        [StringLength(50, ErrorMessage = "O apelido não pode exceder 50 caracteres")]
        [Display(Name = "Apelido")]
        public string Apelido { get; set; }

        [Required(ErrorMessage = "O documento é obrigatório")]
        [StringLength(20)]
        [Display(Name = "Documento/NIF")]
        public string Documento { get; set; }

        [Phone]
        [Display(Name = "Telefone")]
        public string Telefone { get; set; }

        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Data de Cadastro")]
        public DateTime DataCadastro { get; set; }

        [Display(Name = "Nome Completo")]
        public string NomeCompleto
        {
            get { return Nome + " " + Apelido; }
        }

        // Propriedade de navegação
        public ICollection<Reserva> Reservas { get; set; }
    }
}