using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using HotelManagement.Data;
using HotelManagement.Models;

namespace HotelManagement.Pages_Setores
{
    public class CreateModel : PageModel
    {
        private readonly HotelManagement.Data.HotelContext _context;

        public CreateModel(HotelManagement.Data.HotelContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Setor Setor { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Setor.Add(Setor);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
