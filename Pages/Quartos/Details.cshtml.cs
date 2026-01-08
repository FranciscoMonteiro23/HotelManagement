using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HotelManagement.Data;
using HotelManagement.Models;

namespace HotelManagement.Pages.Quartos
{
    public class DetailsModel : PageModel
    {
        private readonly HotelContext _context;

        public DetailsModel(HotelContext context)
        {
            _context = context;
        }

        public Quarto Quarto { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Incluir reservas com clientes
            Quarto = await _context.Quarto
                .Include(q => q.Reservas)
                    .ThenInclude(r => r.Cliente)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.QuartoID == id);

            if (Quarto == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}