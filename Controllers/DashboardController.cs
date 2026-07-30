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

        public async Task<IActionResult> EditProductPage(int Id)
        {
            var product = await _context.Products.FindAsync(Id);

            if (product == null)
            {
                ViewBag.Message = $"Product with ID: {Id} doesn't exit!";
                return View("Index");
            }

            var productDto = new ProductDto
            {
                Name = product.Name,
                Description = product.Description,
                Cost = product.Cost,
            };

            return View(productDto);
        }

        public async Task<IActionResult> UpdateProduct(ProductDto productDto)
        {
            var product = await _context.Products.FindAsync(productDto.Id);

            if(product == null)
            {
                return NotFound();
            }

            product.Name = productDto.Name;
            product.Description = productDto.Description;
            product.Cost = productDto.Cost;

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
