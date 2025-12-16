using Microsoft.AspNetCore.Mvc.RazorPages;
using HotelManagement.Data;
using HotelManagement.Models;
using HotelManagement.Models.HotelViewModels;
using System.Collections.Generic;
using System.Linq;

namespace HotelManagement.Pages.Funcionarios
{
    public class FuncionarioSetorPageModel : PageModel
    {
        public List<AtribuicaoSetorData> AtribuicaoSetorDataList;

        public void PovoarAtribuicaoSetorData(HotelContext context, Funcionario funcionario)
        {
            var todosSetores = context.Setor.ToList();

            var funcionarioSetores = new HashSet<int>();
            if (funcionario.AtribuicaoSetores != null)
            {
                funcionarioSetores = new HashSet<int>(
                    funcionario.AtribuicaoSetores.Select(a => a.SetorID));
            }

            AtribuicaoSetorDataList = new List<AtribuicaoSetorData>();

            foreach (var setor in todosSetores)
            {
                AtribuicaoSetorDataList.Add(new AtribuicaoSetorData
                {
                    SetorID = setor.SetorID,
                    NomeSetor = setor.NomeSetor,
                    Atribuido = funcionarioSetores.Contains(setor.SetorID)
                });
            }
        }

        public void UpdateFuncionarioSetores(HotelContext context,
            string[] setoresSelecionados, Funcionario funcionarioToUpdate)
        {
            if (setoresSelecionados == null)
            {
                funcionarioToUpdate.AtribuicaoSetores = new List<AtribuicaoSetor>();
                return;
            }

            var setoresSelecionadosHS = new HashSet<string>(setoresSelecionados);

            if (funcionarioToUpdate.AtribuicaoSetores == null)
            {
                funcionarioToUpdate.AtribuicaoSetores = new List<AtribuicaoSetor>();
            }

            var funcionarioSetores = new HashSet<int>(
                funcionarioToUpdate.AtribuicaoSetores.Select(a => a.SetorID));

            foreach (var setor in context.Setor)
            {
                if (setoresSelecionadosHS.Contains(setor.SetorID.ToString()))
                {
                    if (!funcionarioSetores.Contains(setor.SetorID))
                    {
                        funcionarioToUpdate.AtribuicaoSetores.Add(
                            new AtribuicaoSetor
                            {
                                FuncionarioID = funcionarioToUpdate.FuncionarioID,
                                SetorID = setor.SetorID
                            });
                    }
                }
                else
                {
                    if (funcionarioSetores.Contains(setor.SetorID))
                    {
                        AtribuicaoSetor setorToRemove = funcionarioToUpdate
                            .AtribuicaoSetores
                            .FirstOrDefault(a => a.SetorID == setor.SetorID);

                        if (setorToRemove != null)
                        {
                            context.Remove(setorToRemove);
                        }
                    }
                }
            }
        }
    }
}