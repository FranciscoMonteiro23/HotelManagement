using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HotelManagement.Data;
using HotelManagement.Models;

namespace HotelManagement.Pages.Setores
{
    public class DetailsModel : PageModel
    {
        private readonly HotelContext _context;

        public DetailsModel(HotelContext context)
        {
            _context = context;
        }

        public Setor Setor { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Setor = await _context.Setor
                .Include(s => s.AtribuicaoSetores)
                    .ThenInclude(a => a.Funcionario)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.SetorID == id);

            if (Setor == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}