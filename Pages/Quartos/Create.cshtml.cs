using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HotelManagement.Data;
using HotelManagement.Models;

namespace HotelManagement.Pages.Quartos
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
            // Inicializar com valores padrão
            Quarto = new Quarto
            {
                Status = StatusQuarto.Disponivel,
                Capacidade = 2,
                PrecoPorNoite = 50.00m
            };
            return Page();
        }

        [BindProperty]
        public Quarto Quarto { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            // VALIDAÇÃO CRÍTICA: Verificar se QuartoID foi preenchido
            if (Quarto.QuartoID <= 0)
            {
                ModelState.AddModelError("Quarto.QuartoID", 
                    "⚠️ Digite o número do quarto (ex: 101, 201, 301)");
            }

            // Verificar se ModelState é válido
            if (!ModelState.IsValid)
            {
                // LOG de erros para debug
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"❌ Erro de validação: {error.ErrorMessage}");
                }
                return Page();
            }

            try
            {
                // Verificar se já existe quarto com este número
                var quartoExistente = await _context.Quarto
                    .AsNoTracking()
                    .FirstOrDefaultAsync(q => q.QuartoID == Quarto.QuartoID);

                if (quartoExistente != null)
                {
                    ModelState.AddModelError("Quarto.QuartoID", 
                        $"⚠️ Já existe um quarto com o número {Quarto.QuartoID}!");
                    return Page();
                }

                // Adicionar e guardar
                _context.Quarto.Add(Quarto);
                var resultado = await _context.SaveChangesAsync();

                if (resultado > 0)
                {
                    TempData["SuccessMessage"] = 
                        $"✅ Quarto {Quarto.QuartoID} ({Quarto.TipoQuarto}) criado com sucesso!";
                    
                    Console.WriteLine($"✅ Quarto {Quarto.QuartoID} guardado na base de dados!");
                    
                    return RedirectToPage("./Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, 
                        "❌ Nenhum registo foi guardado. Verifique os dados.");
                    return Page();
                }
            }
            catch (DbUpdateException dbEx)
            {
                var mensagemErro = dbEx.InnerException?.Message ?? dbEx.Message;
                Console.WriteLine($"❌ Erro BD: {mensagemErro}");
                
                ModelState.AddModelError(string.Empty, 
                    $"❌ Erro ao guardar na base de dados: {mensagemErro}");
                return Page();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro inesperado: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                
                ModelState.AddModelError(string.Empty, 
                    $"❌ Erro inesperado: {ex.Message}");
                return Page();
            }
        }
    }
}