using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelManagement.Models
{
    public class Quarto
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "Número do Quarto")]
        public int QuartoID { get; set; }

        [Required(ErrorMessage = "O tipo de quarto é obrigatório")]
        [StringLength(50)]
        [Display(Name = "Tipo de Quarto")]
        public string TipoQuarto { get; set; }

        [Required]
        [Display(Name = "Capacidade")]
        [Range(1, 10, ErrorMessage = "A capacidade deve ser entre 1 e 10")]
        public int Capacidade { get; set; }

        [Required]
        [Display(Name = "Preço por Noite")]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0.01, 10000.00, ErrorMessage = "O preço deve ser maior que 0")]
        [DataType(DataType.Currency)]
        public decimal PrecoPorNoite { get; set; }

        [Required]
        [Display(Name = "Status")]
        public StatusQuarto Status { get; set; }

        [StringLength(200)]
        [Display(Name = "Descrição")]
        public string Descricao { get; set; }

        // Propriedade de navegação
        public ICollection<Reserva> Reservas { get; set; }
    }

    public enum StatusQuarto
    {
        [Display(Name = "Disponível")]
        Disponivel = 0,

        [Display(Name = "Ocupado")]
        Ocupado = 1,

        [Display(Name = "Manutenção")]
        Manutencao = 2,

        [Display(Name = "Limpeza")]
        Limpeza = 3
    }
}