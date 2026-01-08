using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelManagement.Models
{
    public class Quarto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "Número do Quarto")]
        [Required(ErrorMessage = "O número do quarto é obrigatório")]
        [Range(1, 9999, ErrorMessage = "Digite um número de quarto válido (1-9999)")]
        public int QuartoID { get; set; }

        [Required(ErrorMessage = "O tipo de quarto é obrigatório")]
        [StringLength(50, ErrorMessage = "O tipo de quarto não pode exceder 50 caracteres")]
        [Display(Name = "Tipo de Quarto")]
        public string TipoQuarto { get; set; } = string.Empty;

        [Required(ErrorMessage = "A capacidade é obrigatória")]
        [Display(Name = "Capacidade")]
        [Range(1, 10, ErrorMessage = "A capacidade deve ser entre 1 e 10 pessoas")]
        public int Capacidade { get; set; }

        [Required(ErrorMessage = "O preço é obrigatório")]
        [Display(Name = "Preço por Noite")]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0.01, 10000.00, ErrorMessage = "O preço deve estar entre €0.01 e €10,000.00")]
        [DataType(DataType.Currency)]
        public decimal PrecoPorNoite { get; set; }

        [Required(ErrorMessage = "O status é obrigatório")]
        [Display(Name = "Status")]
        public StatusQuarto Status { get; set; } = StatusQuarto.Disponivel;

        [StringLength(500, ErrorMessage = "A descrição não pode exceder 500 caracteres")]
        [Display(Name = "Descrição")]
        public string? Descricao { get; set; }

        // Propriedade de navegação
        public ICollection<Reserva>? Reservas { get; set; }
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