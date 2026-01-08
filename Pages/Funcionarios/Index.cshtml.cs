using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HotelManagement.Data;
using HotelManagement.Models;
using HotelManagement.Models.HotelViewModels;

namespace HotelManagement.Pages.Funcionarios
{
    public class IndexModel : PageModel
    {
        private readonly HotelContext _context;

        public IndexModel(HotelContext context)
        {
            _context = context;
        }

        public FuncionarioIndexData Funcionario { get; set; }
        public int FuncionarioID { get; set; }
        public int SetorID { get; set; }

        public async Task OnGetAsync(int? id, int? setorID)
        {
            Funcionario = new FuncionarioIndexData();

            Funcionario.Funcionarios = await _context.Funcionario
                .Include(f => f.AtribuicaoSetores)
                    .ThenInclude(a => a.Setor)
                .AsNoTracking()
                .OrderBy(f => f.Nome)
                .ToListAsync();

            if (id != null)
            {
                FuncionarioID = id.Value;
                Funcionario funcionario = Funcionario.Funcionarios
                    .Where(f => f.FuncionarioID == id.Value).Single();
                Funcionario.Setores = funcionario.AtribuicaoSetores.Select(a => a.Setor);
            }

            if (setorID != null)
            {
                SetorID = setorID.Value;
            }
        }
    }
}