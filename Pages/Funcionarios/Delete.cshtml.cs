using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HotelManagement.Data;
using HotelManagement.Models;

namespace HotelManagement.Pages_Funcionarios
{
    public class DeleteModel : PageModel
    {
        private readonly HotelManagement.Data.HotelContext _context;

        public DeleteModel(HotelManagement.Data.HotelContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Funcionario Funcionario { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var funcionario = await _context.Funcionario.FirstOrDefaultAsync(m => m.FuncionarioID == id);

            if (funcionario is not null)
            {
                Funcionario = funcionario;

                return Page();
            }

            return NotFound();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var funcionario = await _context.Funcionario.FindAsync(id);
            if (funcionario != null)
            {
                Funcionario = funcionario;
                _context.Funcionario.Remove(Funcionario);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
