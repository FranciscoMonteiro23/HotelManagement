namespace HotelManagement.Models
{
    public class AtribuicaoSetor
    {
        public int FuncionarioID { get; set; }
        public int SetorID { get; set; }

        // Propriedades de navegação
        public Funcionario Funcionario { get; set; }
        public Setor Setor { get; set; }
    }
}