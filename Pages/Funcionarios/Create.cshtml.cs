using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelManagement.Data;
using HotelManagement.Models;

namespace HotelManagement.Pages.Funcionarios
{
    public class CreateModel : FuncionarioSetorPageModel
    {
        private readonly HotelContext _context;

        public CreateModel(HotelContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            // Inicializar funcionário com valores padrão
            Funcionario = new Funcionario
            {
                DataContratacao = DateTime.Now,
                AtribuicaoSetores = new List<AtribuicaoSetor>()
            };

            PovoarAtribuicaoSetorData(_context, Funcionario);
            return Page();
        }

        [BindProperty]
        public Funcionario Funcionario { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync(string[] setoresSelecionados)
        {
            // Verificar ModelState
            if (!ModelState.IsValid)
            {
                // LOG de erros
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"❌ Erro de validação: {error.ErrorMessage}");
                }

                // Repovoar setores antes de retornar
                PovoarAtribuicaoSetorData(_context, Funcionario);
                return Page();
            }

            try
            {
                // Verificar duplicados
                var documentoExiste = await _context.Funcionario
                    .AsNoTracking()
                    .AnyAsync(f => f.Documento == Funcionario.Documento);

                if (documentoExiste)
                {
                    ModelState.AddModelError("Funcionario.Documento",
                        $"⚠️ Já existe um funcionário com o documento {Funcionario.Documento}!");
                    PovoarAtribuicaoSetorData(_context, Funcionario);
                    return Page();
                }

                var emailExiste = await _context.Funcionario
                    .AsNoTracking()
                    .AnyAsync(f => f.Email == Funcionario.Email);

                if (emailExiste)
                {
                    ModelState.AddModelError("Funcionario.Email",
                        $"⚠️ Já existe um funcionário com o email {Funcionario.Email}!");
                    PovoarAtribuicaoSetorData(_context, Funcionario);
                    return Page();
                }

                // Criar novo funcionário
                var novoFuncionario = new Funcionario
                {
                    Nome = Funcionario.Nome.Trim(),
                    Apelido = Funcionario.Apelido.Trim(),
                    Documento = Funcionario.Documento.Trim().ToUpper(),
                    Telefone = Funcionario.Telefone.Trim(),
                    Email = Funcionario.Email.Trim().ToLower(),
                    Cargo = Funcionario.Cargo,
                    DataContratacao = Funcionario.DataContratacao,
                    AtribuicaoSetores = new List<AtribuicaoSetor>()
                };

                // Adicionar setores selecionados
                if (setoresSelecionados != null && setoresSelecionados.Length > 0)
                {
                    foreach (var setorId in setoresSelecionados)
                    {
                        if (int.TryParse(setorId, out int id))
                        {
                            var setorAAdicionar = new AtribuicaoSetor
                            {
                                SetorID = id,
                                FuncionarioID = novoFuncionario.FuncionarioID
                            };
                            novoFuncionario.AtribuicaoSetores.Add(setorAAdicionar);
                        }
                    }
                }

                // Guardar na base de dados
                _context.Funcionario.Add(novoFuncionario);
                var resultado = await _context.SaveChangesAsync();

                if (resultado > 0)
                {
                    TempData["SuccessMessage"] =
                        $"✅ Funcionário {novoFuncionario.NomeCompleto} ({novoFuncionario.Cargo}) cadastrado com sucesso!";

                    Console.WriteLine($"✅ Funcionário {novoFuncionario.FuncionarioID} - {novoFuncionario.NomeCompleto} guardado!");

                    return RedirectToPage("./Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty,
                        "❌ Nenhum registo foi guardado. Verifique os dados.");
                    PovoarAtribuicaoSetorData(_context, Funcionario);
                    return Page();
                }
            }
            catch (DbUpdateException dbEx)
            {
                var mensagemErro = dbEx.InnerException?.Message ?? dbEx.Message;
                Console.WriteLine($"❌ Erro BD: {mensagemErro}");

                ModelState.AddModelError(string.Empty,
                    $"❌ Erro ao guardar na base de dados: {mensagemErro}");
                PovoarAtribuicaoSetorData(_context, Funcionario);
                return Page();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro inesperado: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");

                ModelState.AddModelError(string.Empty,
                    $"❌ Erro inesperado: {ex.Message}");
                PovoarAtribuicaoSetorData(_context, Funcionario);
                return Page();
            }
        }
    }
}