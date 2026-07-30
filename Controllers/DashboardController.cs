using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductDesk.Data;
using ProductDesk.Dto;
using ProductDesk.Models;

namespace ProductDesk.Controllers
{
    [Authorize]
    public class DashboardController(AppDbContext _context) : Controller
    {
        public IActionResult Index()
        {
            var productList = _context.Products.Select(Product => new ProductDto
            {
                Id = Product.Id,
                Name = Product.Name,
                Description = Product.Description,
                Cost = Product.Cost,
            }).ToList();

            return View(productList);
        }

        public IActionResult AddProductPage() {
            return View();
        }

        public async Task<IActionResult> CreateProduct(ProductDto productDto)
        {
            if (productDto == null)
            {
                ViewBag.Message = "All fields (Name, Description and Cost) are required in order to Add a Product!";
                return View("AddProductPage");
            }

            if (productDto.Name == null || productDto.Description == null)
            {
                ViewBag.Message = "Please check whether all the required fields (Name, Description and Cost) are filled!";
                return View("AddProductPage");
            }

            var product = new Product
            {
                Name = productDto.Name,
                Description = productDto.Description,
                Cost = productDto.Cost,
            };

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
