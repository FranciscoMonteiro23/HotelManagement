using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HotelManagement.Data;
using HotelManagement.Models;

namespace HotelManagement.Pages.Quartos
{
    public class IndexModel : PageModel
    {
        private readonly HotelContext _context;

        public IndexModel(HotelContext context)
        {
            _context = context;
        }

        public string OrdenacaoNumero { get; set; }
        public string OrdenacaoTipo { get; set; }
        public string OrdenacaoPreco { get; set; }
        public string FiltroCorrente { get; set; }
        public string StatusCorrente { get; set; }
        public string OrdenacaoCorrente { get; set; }
        public SelectList StatusList { get; set; }
        public PaginaList<Quarto> Quartos { get; set; }

        public async Task OnGetAsync(string ordenacao, string filtroCorrente,
                                     string filtroTexto, string statusFiltro, int? indexPagina)
        {
            // Definir parâmetros de ordenação
            OrdenacaoCorrente = ordenacao;
            OrdenacaoNumero = string.IsNullOrEmpty(ordenacao) ? "numero_desc" : "";
            OrdenacaoTipo = ordenacao == "Tipo" ? "tipo_desc" : "Tipo";
            OrdenacaoPreco = ordenacao == "Preco" ? "preco_desc" : "Preco";

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
            StatusList = new SelectList(Enum.GetValues(typeof(StatusQuarto)).Cast<StatusQuarto>());

            // Query base
            IQueryable<Quarto> quartoIQ = from q in _context.Quarto select q;

            // Aplicar filtro de texto
            if (!string.IsNullOrEmpty(filtroTexto))
            {
                quartoIQ = quartoIQ.Where(q => q.TipoQuarto.Contains(filtroTexto)
                                             || q.Descricao.Contains(filtroTexto)
                                             || q.QuartoID.ToString().Contains(filtroTexto));
            }

            // Aplicar filtro de status
            if (!string.IsNullOrEmpty(statusFiltro))
            {
                if (Enum.TryParse<StatusQuarto>(statusFiltro, out var status))
                {
                    quartoIQ = quartoIQ.Where(q => q.Status == status);
                }
            }

            // Aplicar ordenação
            switch (ordenacao)
            {
                case "numero_desc":
                    quartoIQ = quartoIQ.OrderByDescending(q => q.QuartoID);
                    break;
                case "Tipo":
                    quartoIQ = quartoIQ.OrderBy(q => q.TipoQuarto);
                    break;
                case "tipo_desc":
                    quartoIQ = quartoIQ.OrderByDescending(q => q.TipoQuarto);
                    break;
                case "Preco":
                    quartoIQ = quartoIQ.OrderBy(q => q.PrecoPorNoite);
                    break;
                case "preco_desc":
                    quartoIQ = quartoIQ.OrderByDescending(q => q.PrecoPorNoite);
                    break;
                default:
                    quartoIQ = quartoIQ.OrderBy(q => q.QuartoID);
                    break;
            }

            int tamanhoPagina = 6;
            Quartos = await PaginaList<Quarto>.CreateAsync(
                quartoIQ.AsNoTracking(), indexPagina ?? 1, tamanhoPagina);
        }
    }
}