using EntityFrameworkProject.Data;
using EntityFrameworkProject.Models;
using EntityFrameworkProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFrameworkProject.Controllers
{
    public class BasketController : Controller
    {
        private readonly AppDbContext _context;
        public BasketController(AppDbContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Index()
        {
            List<BasketVM> basketItems = JsonConvert.DeserializeObject<List<BasketVM>>(Request.Cookies["basket"]);
            List<BasketDetailVM> basketDetail = new List<BasketDetailVM>();

            foreach (var item in basketItems)
            {
                Product product = await _context.Products
                    .Where(m => m.Id == item.Id && m.IsDeleted == false)
                    .Include(m => m.ProductImages).FirstOrDefaultAsync();

                BasketDetailVM newBasket = new BasketDetailVM
                {
                    Title = product.Title,
                    Image = product.ProductImages.Where(m=>m.IsMain).FirstOrDefault().Image,
                    Price = product.Price,
                    Count = item.Count,
                    Total = product.Price * item.Count
                };

                basketDetail.Add(newBasket);

            }

            return View(basketDetail);
        }
    }
}
