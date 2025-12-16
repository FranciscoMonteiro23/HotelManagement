using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HotelManagement.Data;
using HotelManagement.Models;

namespace HotelManagement.Pages_Quartos
{
    public class DeleteModel : PageModel
    {
        private readonly HotelManagement.Data.HotelContext _context;

        public DeleteModel(HotelManagement.Data.HotelContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Quarto Quarto { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quarto = await _context.Quarto.FirstOrDefaultAsync(m => m.QuartoID == id);

            if (quarto is not null)
            {
                Quarto = quarto;

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

            var quarto = await _context.Quarto.FindAsync(id);
            if (quarto != null)
            {
                Quarto = quarto;
                _context.Quarto.Remove(Quarto);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
