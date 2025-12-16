using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HotelManagement.Data;
using HotelManagement.Models;

namespace HotelManagement.Pages.Setores
{
    public class IndexModel : PageModel
    {
        private readonly HotelContext _context;

        public IndexModel(HotelContext context)
        {
            _context = context;
        }

        public IList<Setor> Setores { get; set; }

        public async Task OnGetAsync()
        {
            Setores = await _context.Setor
                .Include(s => s.AtribuicaoSetores)
                    .ThenInclude(a => a.Funcionario)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}