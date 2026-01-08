using System.Collections.Generic;

namespace HotelManagement.Models.HotelViewModels
{
    public class FuncionarioIndexData
    {
        public IEnumerable<Funcionario> Funcionarios { get; set; }
        public IEnumerable<Setor> Setores { get; set; }
    }
}