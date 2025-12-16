using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HotelManagement.Data;
using HotelManagement.Models;

namespace HotelManagement.Pages.Quartos
{
    public class EditModel : PageModel
    {
        private readonly HotelContext _context;

        public EditModel(HotelContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Quarto Quarto { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Quarto = await _context.Quarto.FirstOrDefaultAsync(m => m.QuartoID == id);

            if (Quarto == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Quarto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuartoExists(Quarto.QuartoID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool QuartoExists(int id)
        {
            return _context.Quarto.Any(e => e.QuartoID == id);
        }
    }
}