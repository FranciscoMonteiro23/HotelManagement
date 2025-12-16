using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HotelManagement.Data;
using HotelManagement.Models;

namespace HotelManagement.Pages.Reservas
{
    public class CreateModel : PageModel
    {
        private readonly HotelContext _context;

        public CreateModel(HotelContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            // Criar listas para os dropdowns
            ViewData["ClienteID"] = new SelectList(_context.Cliente, "ClienteID", "NomeCompleto");
            ViewData["QuartoID"] = new SelectList(_context.Quarto, "QuartoID", "QuartoID");

            // IMPORTANTE: Adicionar lista de Status
            ViewData["StatusReserva"] = new SelectList(Enum.GetValues(typeof(StatusReserva))
                .Cast<StatusReserva>()
                .Select(e => new SelectListItem
                {
                    Value = ((int)e).ToString(),
                    Text = e.ToString()
                }), "Value", "Text");

            return Page();
        }

        [BindProperty]
        public Reserva Reserva { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                ViewData["ClienteID"] = new SelectList(_context.Cliente, "ClienteID", "NomeCompleto");
                ViewData["QuartoID"] = new SelectList(_context.Quarto, "QuartoID", "QuartoID");
                ViewData["StatusReserva"] = new SelectList(Enum.GetValues(typeof(StatusReserva))
                    .Cast<StatusReserva>()
                    .Select(e => new SelectListItem
                    {
                        Value = ((int)e).ToString(),
                        Text = e.ToString()
                    }), "Value", "Text");
                return Page();
            }

            // Definir data da reserva como agora
            Reserva.DataReserva = DateTime.Now;

            _context.Reserva.Add(Reserva);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}