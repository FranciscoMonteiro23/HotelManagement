using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HotelManagement.Data;
using HotelManagement.Models;

namespace HotelManagement.Pages_Reservas
{
    public class EditModel : PageModel
    {
        private readonly HotelManagement.Data.HotelContext _context;

        public EditModel(HotelManagement.Data.HotelContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Reserva Reserva { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reserva =  await _context.Reserva.FirstOrDefaultAsync(m => m.ReservaID == id);
            if (reserva == null)
            {
                return NotFound();
            }
            Reserva = reserva;
           ViewData["ClienteID"] = new SelectList(_context.Cliente, "ClienteID", "Apelido");
           ViewData["QuartoID"] = new SelectList(_context.Quarto, "QuartoID", "TipoQuarto");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Reserva).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReservaExists(Reserva.ReservaID))
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

        private bool ReservaExists(int id)
        {
            return _context.Reserva.Any(e => e.ReservaID == id);
        }
    }
}
