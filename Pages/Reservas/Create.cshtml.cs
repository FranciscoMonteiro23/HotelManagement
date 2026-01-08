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
            PopulateDropdowns();
            return Page();
        }

        [BindProperty]
        public Reserva Reserva { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            // Remover validação de campos que não vêm do formulário
            ModelState.Remove("Reserva.Quarto");
            ModelState.Remove("Reserva.Cliente");
            ModelState.Remove("Reserva.DataReserva");

            if (!ModelState.IsValid)
            {
                // Debug: Ver quais erros existem
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"Erro de validação: {error.ErrorMessage}");
                }

                PopulateDropdowns();
                return Page();
            }

            // Validar datas
            if (Reserva.DataCheckOut <= Reserva.DataCheckIn)
            {
                ModelState.AddModelError("Reserva.DataCheckOut", "A data de check-out deve ser posterior à data de check-in");
                PopulateDropdowns();
                return Page();
            }

            // Verificar se o quarto está disponível nas datas selecionadas
            var quartoOcupado = await _context.Reserva
                .AnyAsync(r => r.QuartoID == Reserva.QuartoID &&
                              r.Status != StatusReserva.Cancelada &&
                              ((Reserva.DataCheckIn >= r.DataCheckIn && Reserva.DataCheckIn < r.DataCheckOut) ||
                               (Reserva.DataCheckOut > r.DataCheckIn && Reserva.DataCheckOut <= r.DataCheckOut) ||
                               (Reserva.DataCheckIn <= r.DataCheckIn && Reserva.DataCheckOut >= r.DataCheckOut)));

            if (quartoOcupado)
            {
                ModelState.AddModelError("Reserva.QuartoID", "Este quarto não está disponível nas datas selecionadas");
                PopulateDropdowns();
                return Page();
            }

            // Calcular valor total automaticamente se não foi preenchido
            if (Reserva.ValorTotal <= 0)
            {
                var quarto = await _context.Quarto.FindAsync(Reserva.QuartoID);
                var numeroNoites = (Reserva.DataCheckOut - Reserva.DataCheckIn).Days;
                Reserva.ValorTotal = numeroNoites * quarto.PrecoPorNoite;
            }

            // Definir data da reserva como agora
            Reserva.DataReserva = DateTime.Now;

            _context.Reserva.Add(Reserva);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

        private void PopulateDropdowns()
        {
            ViewData["ClienteID"] = new SelectList(_context.Cliente, "ClienteID", "NomeCompleto");
            ViewData["QuartoID"] = new SelectList(_context.Quarto.Select(q => new
            {
                q.QuartoID,
                Display = $"Quarto {q.QuartoID} - {q.TipoQuarto}"
            }), "QuartoID", "Display");

            ViewData["StatusReserva"] = new SelectList(Enum.GetValues(typeof(StatusReserva))
                .Cast<StatusReserva>()
                .Select(e => new SelectListItem
                {
                    Value = ((int)e).ToString(),
                    Text = e.ToString()
                }), "Value", "Text");
        }
    }
}