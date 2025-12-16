using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HotelManagement.Data;
using HotelManagement.Models;

namespace HotelManagement.Pages_Clientes
{
    public class DeleteModel : PageModel
    {
        private readonly HotelManagement.Data.HotelContext _context;

        public DeleteModel(HotelManagement.Data.HotelContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Cliente Cliente { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Cliente.FirstOrDefaultAsync(m => m.ClienteID == id);

            if (cliente is not null)
            {
                Cliente = cliente;

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

            var cliente = await _context.Cliente.FindAsync(id);
            if (cliente != null)
            {
                Cliente = cliente;
                _context.Cliente.Remove(Cliente);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
