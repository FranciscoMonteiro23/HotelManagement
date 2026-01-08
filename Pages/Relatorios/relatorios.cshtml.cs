using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HotelManagement.Data;
using HotelManagement.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelManagement.Pages
{
    public class RelatoriosModel : PageModel
    {
        private readonly HotelContext _context;

        public RelatoriosModel(HotelContext context)
        {
            _context = context;
        }

        // Ocupação
        public int TotalQuartos { get; set; }
        public int QuartosOcupados { get; set; }
        public int QuartosDisponiveis { get; set; }
        public int TaxaOcupacao { get; set; }

        // Reservas por Status
        public int ReservasConfirmadas { get; set; }
        public int ReservasPendentes { get; set; }
        public int ReservasCheckIn { get; set; }
        public int TotalReservas { get; set; }

        public int PercentagemConfirmadas { get; set; }
        public int PercentagemPendentes { get; set; }
        public int PercentagemCheckIn { get; set; }

        // Receita
        public decimal ReceitaTotal { get; set; }
        public decimal ReceitaConfirmada { get; set; }
        public decimal ReceitaPendente { get; set; }
        public decimal TicketMedio { get; set; }

        // Top Clientes
        public List<Cliente> TopClientes { get; set; }

        public async Task OnGetAsync()
        {
            // OCUPAÇÃO
            TotalQuartos = await _context.Quarto.CountAsync();
            QuartosOcupados = await _context.Quarto
                .Where(q => q.Status == StatusQuarto.Ocupado)
                .CountAsync();
            QuartosDisponiveis = TotalQuartos - QuartosOcupados;
            TaxaOcupacao = TotalQuartos > 0 ? (QuartosOcupados * 100) / TotalQuartos : 0;

            // RESERVAS POR STATUS
            ReservasConfirmadas = await _context.Reserva
                .Where(r => r.Status == StatusReserva.Confirmada)
                .CountAsync();

            ReservasPendentes = await _context.Reserva
                .Where(r => r.Status == StatusReserva.Pendente)
                .CountAsync();

            ReservasCheckIn = await _context.Reserva
                .Where(r => r.Status == StatusReserva.CheckInRealizado)
                .CountAsync();

            TotalReservas = ReservasConfirmadas + ReservasPendentes + ReservasCheckIn;

            // Calcular percentagens
            if (TotalReservas > 0)
            {
                PercentagemConfirmadas = (ReservasConfirmadas * 100) / TotalReservas;
                PercentagemPendentes = (ReservasPendentes * 100) / TotalReservas;
                PercentagemCheckIn = (ReservasCheckIn * 100) / TotalReservas;
            }

            // RECEITA
            var reservas = await _context.Reserva.ToListAsync();
            ReceitaTotal = reservas.Sum(r => r.ValorTotal);

            ReceitaConfirmada = reservas
                .Where(r => r.Status == StatusReserva.Confirmada || r.Status == StatusReserva.CheckInRealizado)
                .Sum(r => r.ValorTotal);

            ReceitaPendente = reservas
                .Where(r => r.Status == StatusReserva.Pendente)
                .Sum(r => r.ValorTotal);

            TicketMedio = TotalReservas > 0 ? ReceitaTotal / TotalReservas : 0;

            // TOP CLIENTES
            TopClientes = await _context.Cliente
                .Include(c => c.Reservas)
                .OrderByDescending(c => c.Reservas.Count)
                .Take(5)
                .ToListAsync();
        }
    }
}
