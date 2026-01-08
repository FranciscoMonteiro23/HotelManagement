using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HotelManagement.Data;
using HotelManagement.Models;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace HotelManagement.Pages
{
    public class IndexModel : PageModel
    {
        private readonly HotelContext _context;

        public IndexModel(HotelContext context)
        {
            _context = context;
        }

        // Propriedades usadas na página
        public int TotalReservas { get; set; }
        public int TotalQuartos { get; set; }
        public int TotalClientes { get; set; }
        public int TotalFuncionarios { get; set; }
        public List<Reserva> UltimasReservas { get; set; }

        public async Task OnGetAsync()
        {
            // Contadores
            TotalReservas = await _context.Reserva.CountAsync();
            TotalQuartos = await _context.Quarto.CountAsync();
            TotalClientes = await _context.Cliente.CountAsync();
            TotalFuncionarios = await _context.Funcionario.CountAsync();

            // Últimas 10 reservas
            UltimasReservas = await _context.Reserva
                .Include(r => r.Cliente)
                .Include(r => r.Quarto)
                .OrderByDescending(r => r.ReservaID)
                .Take(10)
                .ToListAsync();
        }
    }
}
