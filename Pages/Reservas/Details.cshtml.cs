using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HotelManagement.Data;
using HotelManagement.Models;

namespace HotelManagement.Pages.Reservas
{
    public class DetailsModel : PageModel
    {
        private readonly HotelContext _context;

        public DetailsModel(HotelContext context)
        {
            _context = context;
        }

        public Reserva Reserva { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Incluir dados relacionados do Cliente e Quarto
            Reserva = await _context.Reserva
                .Include(r => r.Cliente)
                .Include(r => r.Quarto)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ReservaID == id);

            if (Reserva == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}