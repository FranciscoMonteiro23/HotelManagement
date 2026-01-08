
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HotelManagement.Data;
using HotelManagement.Models;

namespace HotelManagement.Pages.Clientes
{
    public class IndexModel : PageModel
    {
        private readonly HotelContext _context;

        public IndexModel(HotelContext context)
        {
            _context = context;
        }

        public string OrdenacaoNome { get; set; }
        public string OrdenacaoData { get; set; }
        public string FiltroCorrente { get; set; }
        public IList<Cliente> Clientes { get; set; }

        public async Task OnGetAsync(string ordenacao, string filtroTexto)
        {
            // Definir parâmetros de ordenação
            OrdenacaoNome = string.IsNullOrEmpty(ordenacao) ? "nome_desc" : "";
            OrdenacaoData = ordenacao == "Data" ? "data_desc" : "Data";
            FiltroCorrente = filtroTexto;

            // Query base
            IQueryable<Cliente> clienteIQ = from c in _context.Cliente select c;

            // Aplicar filtro
            if (!string.IsNullOrEmpty(filtroTexto))
            {
                clienteIQ = clienteIQ.Where(c => c.Apelido.Contains(filtroTexto)
                                               || c.Nome.Contains(filtroTexto)
                                               || c.Email.Contains(filtroTexto));
            }

            // Aplicar ordenação
            switch (ordenacao)
            {
                case "nome_desc":
                    clienteIQ = clienteIQ.OrderByDescending(c => c.Nome);
                    break;
                case "Data":
                    clienteIQ = clienteIQ.OrderBy(c => c.DataCadastro);
                    break;
                case "data_desc":
                    clienteIQ = clienteIQ.OrderByDescending(c => c.DataCadastro);
                    break;
                default:
                    clienteIQ = clienteIQ.OrderBy(c => c.Nome);
                    break;
            }

            Clientes = await clienteIQ.AsNoTracking().ToListAsync();
        }
    }
}