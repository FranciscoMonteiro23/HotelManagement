using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
            var funcionario = new Funcionario();
            funcionario.AtribuicaoSetores = new List<AtribuicaoSetor>();
            PovoarAtribuicaoSetorData(_context, funcionario);
            return Page();
        }

        [BindProperty]
        public Funcionario Funcionario { get; set; }

        public async Task<IActionResult> OnPostAsync(string[] setoresSelecionados)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var novoFuncionario = new Funcionario();

            if (setoresSelecionados != null)
            {
                novoFuncionario.AtribuicaoSetores = new List<AtribuicaoSetor>();
                foreach (var setor in setoresSelecionados)
                {
                    var setorAAdicionar = new AtribuicaoSetor
                    {
                        SetorID = int.Parse(setor)
                    };
                    novoFuncionario.AtribuicaoSetores.Add(setorAAdicionar);
                }
            }

            if (await TryUpdateModelAsync<Funcionario>(
                novoFuncionario,
                "Funcionario",
                f => f.Nome, f => f.Apelido,
                f => f.Documento, f => f.Telefone,
                f => f.Email, f => f.Cargo,
                f => f.DataContratacao))
            {
                _context.Funcionario.Add(novoFuncionario);
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }

            PovoarAtribuicaoSetorData(_context, novoFuncionario);
            return Page();
        }
    }
}