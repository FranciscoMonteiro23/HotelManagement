using System;
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
        private readonly HotelContext _context;

        public EditModel(HotelContext context)
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

            var reserva = await _context.Reserva
                .Include(r => r.Cliente)
                .Include(r => r.Quarto)
                .FirstOrDefaultAsync(m => m.ReservaID == id);

            if (reserva == null)
            {
                return NotFound();
            }

            Reserva = reserva;
            PopulateDropdowns();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Remover validação de navegação
            ModelState.Remove("Reserva.Quarto");
            ModelState.Remove("Reserva.Cliente");

            if (!ModelState.IsValid)
            {
                // LOG de erros
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"❌ Erro de validação: {error.ErrorMessage}");
                }

                PopulateDropdowns();
                return Page();
            }

            try
            {
                // VALIDAÇÃO: Datas
                if (Reserva.DataCheckOut <= Reserva.DataCheckIn)
                {
                    ModelState.AddModelError("Reserva.DataCheckOut",
                        "⚠️ A data de check-out deve ser posterior à data de check-in!");
                    PopulateDropdowns();
                    return Page();
                }

                // Verificar se quarto existe
                var quarto = await _context.Quarto.FindAsync(Reserva.QuartoID);
                if (quarto == null)
                {
                    ModelState.AddModelError("Reserva.QuartoID", "⚠️ Quarto não encontrado!");
                    PopulateDropdowns();
                    return Page();
                }

                // Verificar disponibilidade (excluindo a própria reserva)
                var quartoOcupado = await _context.Reserva
                    .Where(r => r.QuartoID == Reserva.QuartoID &&
                               r.ReservaID != Reserva.ReservaID &&
                               r.Status != StatusReserva.Cancelada)
                    .AnyAsync(r =>
                        (Reserva.DataCheckIn >= r.DataCheckIn && Reserva.DataCheckIn < r.DataCheckOut) ||
                        (Reserva.DataCheckOut > r.DataCheckIn && Reserva.DataCheckOut <= r.DataCheckOut) ||
                        (Reserva.DataCheckIn <= r.DataCheckIn && Reserva.DataCheckOut >= r.DataCheckOut));

                if (quartoOcupado)
                {
                    ModelState.AddModelError("Reserva.QuartoID",
                        $"⚠️ O Quarto {Reserva.QuartoID} não está disponível nas datas selecionadas!");
                    PopulateDropdowns();
                    return Page();
                }

                // Recalcular valor total
                var numeroNoites = (Reserva.DataCheckOut - Reserva.DataCheckIn).Days;
                Reserva.ValorTotal = numeroNoites * quarto.PrecoPorNoite;

                // Atualizar
                _context.Attach(Reserva).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = $"✅ Reserva {Reserva.ReservaID} atualizada com sucesso!";
                return RedirectToPage("./Index");
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
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro: {ex.Message}");
                ModelState.AddModelError(string.Empty, $"❌ Erro ao atualizar: {ex.Message}");
                PopulateDropdowns();
                return Page();
            }
        }

        private bool ReservaExists(int id)
        {
            return _context.Reserva.Any(e => e.ReservaID == id);
        }

        private void PopulateDropdowns()
        {
            ViewData["ClienteID"] = new SelectList(
                _context.Cliente.OrderBy(c => c.Nome),
                "ClienteID",
                "NomeCompleto",
                Reserva?.ClienteID
            );

            ViewData["QuartoID"] = new SelectList(
                _context.Quarto
                    .OrderBy(q => q.QuartoID)
                    .Select(q => new
                    {
                        q.QuartoID,
                        Display = $"Quarto {q.QuartoID} - {q.TipoQuarto} (€{q.PrecoPorNoite}/noite)"
                    }),
                "QuartoID",
                "Display",
                Reserva?.QuartoID
            );
        }
    }
}