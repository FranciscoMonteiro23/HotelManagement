using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelManagement.Models
{
    public class Reserva
    {
        public int ReservaID { get; set; }

        [Required]
        public int QuartoID { get; set; }

        [Required]
        public int ClienteID { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Data Check-in")]
        public DateTime DataCheckIn { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Data Check-out")]
        public DateTime DataCheckOut { get; set; }

        [Display(Name = "Número de Hóspedes")]
        [Range(1, 10)]
        public int NumeroHospedes { get; set; }

        [Display(Name = "Valor Total")]
        [Column(TypeName = "decimal(18,2)")]
        [DataType(DataType.Currency)]
        public decimal ValorTotal { get; set; }

        [Required]
        [Display(Name = "Status da Reserva")]
        public StatusReserva Status { get; set; }

        [StringLength(500)]
        [Display(Name = "Observações")]
        public string Observacoes { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Data da Reserva")]
        public DateTime DataReserva { get; set; }

        // Propriedades de navegação
        public Quarto Quarto { get; set; }
        public Cliente Cliente { get; set; }

        // Propriedade calculada
        [Display(Name = "Número de Noites")]
        public int NumeroNoites
        {
            get
            {
                if (DataCheckOut > DataCheckIn)
                    return (DataCheckOut - DataCheckIn).Days;
                return 0;
            }
        }
    }

    public enum StatusReserva
    {
        [Display(Name = "Pendente")]
        Pendente = 0,

        [Display(Name = "Confirmada")]
        Confirmada = 1,

        [Display(Name = "Check-in Realizado")]
        CheckInRealizado = 2,

        [Display(Name = "Check-out Realizado")]
        CheckOutRealizado = 3,

        [Display(Name = "Cancelada")]
        Cancelada = 4
    }
}