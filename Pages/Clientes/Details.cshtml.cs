using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HotelManagement.Data;
using HotelManagement.Models;

namespace HotelManagement.Pages.Clientes
{
    public class DetailsModel : PageModel
    {
        private readonly HotelContext _context;

        public DetailsModel(HotelContext context)
        {
            _context = context;
        }

        public Cliente Cliente { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Incluir reservas com quartos
            Cliente = await _context.Cliente
                .Include(c => c.Reservas)
                    .ThenInclude(r => r.Quarto)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ClienteID == id);

            if (Cliente == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}