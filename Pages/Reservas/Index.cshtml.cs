using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HotelManagement.Data;
using HotelManagement.Models;

namespace HotelManagement.Pages.Reservas
{
    public class IndexModel : PageModel
    {
        private readonly HotelContext _context;

        public IndexModel(HotelContext context)
        {
            _context = context;
        }

        public string OrdenacaoData { get; set; }
        public string OrdenacaoCliente { get; set; }
        public string OrdenacaoValor { get; set; }
        public string FiltroCorrente { get; set; }
        public string StatusCorrente { get; set; }
        public string OrdenacaoCorrente { get; set; }
        public SelectList StatusList { get; set; }
        public PaginaList<Reserva> Reservas { get; set; }

        public async Task OnGetAsync(string ordenacao, string filtroCorrente,
                                     string filtroTexto, string statusFiltro, int? indexPagina)
        {
            // Definir parâmetros de ordenação
            OrdenacaoCorrente = ordenacao;
            OrdenacaoData = string.IsNullOrEmpty(ordenacao) ? "data_desc" : "";
            OrdenacaoCliente = ordenacao == "Cliente" ? "cliente_desc" : "Cliente";
            OrdenacaoValor = ordenacao == "Valor" ? "valor_desc" : "Valor";

            if (filtroTexto != null)
            {
                indexPagina = 1;
            }
            else
            {
                filtroTexto = filtroCorrente;
            }

            FiltroCorrente = filtroTexto;
            StatusCorrente = statusFiltro;

            // Criar lista de status para dropdown
            StatusList = new SelectList(Enum.GetValues(typeof(StatusReserva)).Cast<StatusReserva>());

            // Query base com includes
            IQueryable<Reserva> reservaIQ = _context.Reserva
                .Include(r => r.Cliente)
                .Include(r => r.Quarto);

            // Aplicar filtro de texto
            if (!string.IsNullOrEmpty(filtroTexto))
            {
                reservaIQ = reservaIQ.Where(r => r.Cliente.Nome.Contains(filtroTexto)
                                               || r.Cliente.Apelido.Contains(filtroTexto)
                                               || r.Quarto.TipoQuarto.Contains(filtroTexto)
                                               || r.Quarto.QuartoID.ToString().Contains(filtroTexto));
            }

            // Aplicar filtro de status
            if (!string.IsNullOrEmpty(statusFiltro))
            {
                if (Enum.TryParse<StatusReserva>(statusFiltro, out var status))
                {
                    reservaIQ = reservaIQ.Where(r => r.Status == status);
                }
            }

            // Aplicar ordenação
            switch (ordenacao)
            {
                case "data_desc":
                    reservaIQ = reservaIQ.OrderByDescending(r => r.DataCheckIn);
                    break;
                case "Cliente":
                    reservaIQ = reservaIQ.OrderBy(r => r.Cliente.Nome);
                    break;
                case "cliente_desc":
                    reservaIQ = reservaIQ.OrderByDescending(r => r.Cliente.Nome);
                    break;
                case "Valor":
                    reservaIQ = reservaIQ.OrderBy(r => r.ValorTotal);
                    break;
                case "valor_desc":
                    reservaIQ = reservaIQ.OrderByDescending(r => r.ValorTotal);
                    break;
                default:
                    reservaIQ = reservaIQ.OrderBy(r => r.DataCheckIn);
                    break;
            }

            int tamanhoPagina = 8;
            Reservas = await PaginaList<Reserva>.CreateAsync(
                reservaIQ.AsNoTracking(), indexPagina ?? 1, tamanhoPagina);
        }
    }
}