using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HotelManagement.Data;
using HotelManagement.Models;

namespace HotelManagement.Pages.Funcionarios
{
    public class DetailsModel : PageModel
    {
        private readonly HotelContext _context;

        public DetailsModel(HotelContext context)
        {
            _context = context;
        }

        public Funcionario Funcionario { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Funcionario = await _context.Funcionario
                .Include(f => f.AtribuicaoSetores)
                    .ThenInclude(a => a.Setor)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.FuncionarioID == id);

            if (Funcionario == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}