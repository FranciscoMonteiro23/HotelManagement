using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelManagement.Models
{
    public class Setor
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int SetorID { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Nome do Setor")]
        public string NomeSetor { get; set; }

        [StringLength(200)]
        [Display(Name = "Descrição")]
        public string Descricao { get; set; }

        [Display(Name = "Número de Funcionários")]
        public int NumeroFuncionarios { get; set; }

        // Propriedade de navegação
        public ICollection<AtribuicaoSetor> AtribuicaoSetores { get; set; }
    }
}