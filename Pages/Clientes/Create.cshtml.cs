using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HotelManagement.Data;
using HotelManagement.Models;

namespace HotelManagement.Pages.Clientes
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
            // Inicializar cliente com data atual
            Cliente = new Cliente
            {
                DataCadastro = DateTime.Now
            };
            return Page();
        }

        [BindProperty]
        public Cliente Cliente { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
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
                // Garantir que a data de cadastro é preenchida
                if (Cliente.DataCadastro == DateTime.MinValue)
                {
                    Cliente.DataCadastro = DateTime.Now;
                }

                // Verificar se já existe cliente com mesmo documento
                var clienteExistente = await _context.Cliente
                    .AsNoTracking()
                    .FirstOrDefaultAsync(c => c.Documento == Cliente.Documento);

                if (clienteExistente != null)
                {
                    ModelState.AddModelError("Cliente.Documento", 
                        $"⚠️ Já existe um cliente cadastrado com o documento {Cliente.Documento}!");
                    return Page();
                }

                // Verificar se já existe cliente com mesmo email
                var emailExistente = await _context.Cliente
                    .AsNoTracking()
                    .FirstOrDefaultAsync(c => c.Email == Cliente.Email);

                if (emailExistente != null)
                {
                    ModelState.AddModelError("Cliente.Email", 
                        $"⚠️ Já existe um cliente cadastrado com o email {Cliente.Email}!");
                    return Page();
                }

                // Normalizar dados antes de guardar
                Cliente.Nome = Cliente.Nome.Trim();
                Cliente.Apelido = Cliente.Apelido.Trim();
                Cliente.Documento = Cliente.Documento.Trim().ToUpper();
                Cliente.Email = Cliente.Email.Trim().ToLower();
                Cliente.Telefone = Cliente.Telefone.Trim();

                // Adicionar e guardar
                _context.Cliente.Add(Cliente);
                var resultado = await _context.SaveChangesAsync();

                if (resultado > 0)
                {
                    TempData["SuccessMessage"] = 
                        $"✅ Cliente {Cliente.NomeCompleto} cadastrado com sucesso!";
                    
                    Console.WriteLine($"✅ Cliente {Cliente.ClienteID} - {Cliente.NomeCompleto} guardado!");
                    
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