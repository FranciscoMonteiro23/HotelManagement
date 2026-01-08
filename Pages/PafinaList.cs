using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace HotelManagement
{
    public class PaginaList<T> : List<T>
    {
        public int PaginaIndex { get; private set; }
        public int TotalPaginas { get; private set; }

        public PaginaList(List<T> items, int contar, int indexPagina, int tamanhoPagina)
        {
            PaginaIndex = indexPagina;
            TotalPaginas = (int)Math.Ceiling(contar / (double)tamanhoPagina);
            this.AddRange(items);
        }

        public bool HaPaginasAntes
        {
            get
            {
                return (PaginaIndex > 1);
            }
        }

        public bool HaPaginasDepois
        {
            get
            {
                return (PaginaIndex < TotalPaginas);
            }
        }

        public static async Task<PaginaList<T>> CreateAsync(
            IQueryable<T> source, int indexPagina, int tamanhoPagina)
        {
            var contar = await source.CountAsync();
            var items = await source.Skip((indexPagina - 1) * tamanhoPagina)
                .Take(tamanhoPagina).ToListAsync();
            return new PaginaList<T>(items, contar, indexPagina, tamanhoPagina);
        }
    }
}
