using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelManagement.Data;
using HotelManagement.Models;

namespace HotelManagement.Pages.Funcionarios
{
    public class EditModel : FuncionarioSetorPageModel
    {
        private readonly HotelContext _context;

        public EditModel(HotelContext context)
        {
            _context = context;
        }

        [BindProperty]
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

            PovoarAtribuicaoSetorData(_context, Funcionario);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id, string[] setoresSelecionados)
        {
            if (id == null)
            {
                return NotFound();
            }

            var funcionarioToUpdate = await _context.Funcionario
                .Include(f => f.AtribuicaoSetores)
                    .ThenInclude(a => a.Setor)
                .FirstOrDefaultAsync(f => f.FuncionarioID == id);

            if (funcionarioToUpdate == null)
            {
                return NotFound();
            }

            if (await TryUpdateModelAsync<Funcionario>(
                funcionarioToUpdate,
                "Funcionario",
                f => f.Nome,
                f => f.Apelido,
                f => f.Documento,
                f => f.Telefone,
                f => f.Email,
                f => f.Cargo,
                f => f.DataContratacao))
            {
                UpdateFuncionarioSetores(_context, setoresSelecionados, funcionarioToUpdate);
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }

            UpdateFuncionarioSetores(_context, setoresSelecionados, funcionarioToUpdate);
            PovoarAtribuicaoSetorData(_context, funcionarioToUpdate);
            return Page();
        }
    }
}