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
            // Inicializar reserva com valores padrão
            Reserva = new Reserva
            {
                DataCheckIn = DateTime.Today,
                DataCheckOut = DateTime.Today.AddDays(1),
                NumeroHospedes = 1,
                Status = StatusReserva.Pendente,
                DataReserva = DateTime.Now
            };

            PopulateDropdowns();
            return Page();
        }

        [BindProperty]
        public Reserva Reserva { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            // Remover validação de campos de navegação
            ModelState.Remove("Reserva.Quarto");
            ModelState.Remove("Reserva.Cliente");

            if (!ModelState.IsValid)
            {
                // LOG de erros para debug
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"❌ Erro de validação: {error.ErrorMessage}");
                }

                PopulateDropdowns();
                return Page();
            }

            try
            {
                // VALIDAÇÃO 1: Datas
                if (Reserva.DataCheckOut <= Reserva.DataCheckIn)
                {
                    ModelState.AddModelError("Reserva.DataCheckOut", 
                        "⚠️ A data de check-out deve ser posterior à data de check-in!");
                    PopulateDropdowns();
                    return Page();
                }

                // VALIDAÇÃO 2: Data check-in no passado
                if (Reserva.DataCheckIn.Date < DateTime.Today)
                {
                    ModelState.AddModelError("Reserva.DataCheckIn", 
                        "⚠️ A data de check-in não pode ser no passado!");
                    PopulateDropdowns();
                    return Page();
                }

                // VALIDAÇÃO 3: Verificar se o quarto existe
                var quarto = await _context.Quarto.FindAsync(Reserva.QuartoID);
                if (quarto == null)
                {
                    ModelState.AddModelError("Reserva.QuartoID", 
                        "⚠️ Quarto não encontrado!");
                    PopulateDropdowns();
                    return Page();
                }

                // VALIDAÇÃO 4: Verificar se o cliente existe
                var cliente = await _context.Cliente.FindAsync(Reserva.ClienteID);
                if (cliente == null)
                {
                    ModelState.AddModelError("Reserva.ClienteID", 
                        "⚠️ Cliente não encontrado!");
                    PopulateDropdowns();
                    return Page();
                }

                // VALIDAÇÃO 5: Verificar disponibilidade do quarto
                var quartoOcupado = await _context.Reserva
                    .Where(r => r.QuartoID == Reserva.QuartoID &&
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

                // VALIDAÇÃO 6: Número de hóspedes vs capacidade do quarto
                if (Reserva.NumeroHospedes > quarto.Capacidade)
                {
                    ModelState.AddModelError("Reserva.NumeroHospedes", 
                        $"⚠️ O quarto comporta no máximo {quarto.Capacidade} pessoa(s)!");
                    PopulateDropdowns();
                    return Page();
                }

                // CÁLCULO AUTOMÁTICO: Valor Total
                var numeroNoites = (Reserva.DataCheckOut - Reserva.DataCheckIn).Days;
                Reserva.ValorTotal = numeroNoites * quarto.PrecoPorNoite;

                // Definir data da reserva
                Reserva.DataReserva = DateTime.Now;

                // Guardar na base de dados
                _context.Reserva.Add(Reserva);
                var resultado = await _context.SaveChangesAsync();

                if (resultado > 0)
                {
                    TempData["SuccessMessage"] = 
                        $"✅ Reserva criada com sucesso! Quarto {Reserva.QuartoID} para {cliente.NomeCompleto} ({numeroNoites} noite(s) - €{Reserva.ValorTotal:F2})";
                    
                    Console.WriteLine($"✅ Reserva {Reserva.ReservaID} guardada com sucesso!");
                    
                    return RedirectToPage("./Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, 
                        "❌ Erro ao guardar a reserva. Tente novamente.");
                    PopulateDropdowns();
                    return Page();
                }
            }
            catch (DbUpdateException dbEx)
            {
                var mensagemErro = dbEx.InnerException?.Message ?? dbEx.Message;
                Console.WriteLine($"❌ Erro BD: {mensagemErro}");
                
                ModelState.AddModelError(string.Empty, 
                    $"❌ Erro ao guardar na base de dados: {mensagemErro}");
                PopulateDropdowns();
                return Page();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro inesperado: {ex.Message}");
                
                ModelState.AddModelError(string.Empty, 
                    $"❌ Erro inesperado: {ex.Message}");
                PopulateDropdowns();
                return Page();
            }
        }

        private void PopulateDropdowns()
        {
            // Dropdown de Clientes
            ViewData["ClienteID"] = new SelectList(
                _context.Cliente.OrderBy(c => c.Nome), 
                "ClienteID", 
                "NomeCompleto"
            );

            // Dropdown de Quartos (apenas disponíveis)
            ViewData["QuartoID"] = new SelectList(
                _context.Quarto
                    .Where(q => q.Status == StatusQuarto.Disponivel)
                    .OrderBy(q => q.QuartoID)
                    .Select(q => new
                    {
                        q.QuartoID,
                        Display = $"Quarto {q.QuartoID} - {q.TipoQuarto} (€{q.PrecoPorNoite}/noite)"
                    }), 
                "QuartoID", 
                "Display"
            );
        }
    }
}