using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HotelManagement.Models
{
    public class Funcionario
    {
        public int FuncionarioID { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório")]
        [StringLength(50)]
        [Display(Name = "Nome")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O apelido é obrigatório")]
        [StringLength(50)]
        [Display(Name = "Apelido")]
        public string Apelido { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "Documento/NIF")]
        public string Documento { get; set; }

        [Phone]
        [Display(Name = "Telefone")]
        public string Telefone { get; set; }

        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Cargo")]
        public CargoFuncionario Cargo { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Data de Contratação")]
        public DateTime DataContratacao { get; set; }

        [Display(Name = "Nome Completo")]
        public string NomeCompleto
        {
            get { return Nome + " " + Apelido; }
        }

        // Propriedade de navegação
        public ICollection<AtribuicaoSetor> AtribuicaoSetores { get; set; }
    }

    public enum CargoFuncionario
    {
        [Display(Name = "Recepcionista")]
        Recepcionista = 0,

        [Display(Name = "Camareira")]
        Camareira = 1,

        [Display(Name = "Gerente")]
        Gerente = 2,

        [Display(Name = "Manutenção")]
        Manutencao = 3,

        [Display(Name = "Segurança")]
        Seguranca = 4,

        [Display(Name = "Chef")]
        Chef = 5
    }
}