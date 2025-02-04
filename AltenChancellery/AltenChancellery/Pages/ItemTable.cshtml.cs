using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceLayer.DTOs;
using ServiceLayer.Services.Implementations;
using ServiceLayer.Services.Interfaces;

namespace AltenChancellery.Pages
{
    public class ItemTableModel : PageModel
    {
        private readonly ICategoryService _categoryService;
        private readonly IItemService _itemService;

        public IList<CategoryDTO> Categories { get; set; }
        public IList<ItemDTO> Items { get; set; }

        public ItemTableModel(ICategoryService categoryService, IItemService itemService)
        {
            _categoryService = categoryService;
            _itemService = itemService;

            Categories = new List<CategoryDTO>();
            Items = new List<ItemDTO>();
        }

        public async Task OnGet()
        {
            var categories = await _categoryService.GetAll();
            var items = await _itemService.GetAll();

            Categories = categories.Data;
            Items = items.Data;
        }

        /* INSERT VALUES (TODO: to delete when not needed)
         insert into Item(Name, Description, Availability, MinimumAvailability, CategoryId) 
        values ('Quaderni', null, 10, 2, 1);

         insert into Item(Name, Description, Availability, MinimumAvailability, CategoryId) 
        values ('Cerotti', null, 30, 10, 2);
         
         insert into Item(Name, Description, Availability, MinimumAvailability, CategoryId) 
        values ('Estintore', null, 3, 2, 3);

         insert into Item(Name, Description, Availability, MinimumAvailability, CategoryId) 
        values ('Boccioni', null, 20, 2, 4);

         insert into Item(Name, Description, Availability, MinimumAvailability, CategoryId) 
        values ('Risme', null, 5, 1, 1);

         insert into Item(Name, Description, Availability, MinimumAvailability, CategoryId) 
        values ('Penne', null, 30, 5, 1);

         insert into Item(Name, Description, Availability, MinimumAvailability, CategoryId) 
        values ('Medikit', null, 3, 1, 2);
         */
    }
}
