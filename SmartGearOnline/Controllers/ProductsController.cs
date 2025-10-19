using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SmartGearOnline.Models;
using SmartGearOnline.Services;
using Microsoft.AspNetCore.Authorization;

namespace SmartGearOnline.Controllers
{
    [Route("products")]
    public class ProductsController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IProductRepository productRepository, ICategoryRepository categoryRepository, ILogger<ProductsController> logger)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _logger = logger;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
                var products = await _productRepository.GetAllAsync();
                return View(products);
        }

        [HttpGet("details/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var product = await _productRepository.GetByIdAsync(id.Value);
            if (product == null) return NotFound();

            return View(product);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("create")]
        public async Task<IActionResult> Create()
        {
            var categories = await _categoryRepository.GetAllAsync();

            ViewBag.Categories = categories.Select(c => new SelectListItem
            {
                Value = c.CategoryId.ToString(),
                Text = c.CategoryName
            }).ToList();

            return View();
        }

        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Price,CategoryId,ShippingCost")] Product product)
        {
            if (ModelState.IsValid)
            {
                var success = await _productRepository.AddAsync(product);
                if (success) return RedirectToAction(nameof(Index));
            }

            var categories = await _categoryRepository.GetAllAsync();
            ViewBag.Categories = categories.Select(c => new SelectListItem
            {
                Value = c.CategoryId.ToString(),
                Text = c.CategoryName
            }).ToList();

            return View(product);
        }

        [HttpGet("edit/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var product = await _productRepository.GetByIdAsync(id.Value);
            if (product == null) return NotFound();

            return View(product);
        }

        [HttpPost("edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,Price,CategoryId,ShippingCost")] Product product)
        {
            if (id != product.Id) return NotFound();

            if (ModelState.IsValid)
            {
                var updated = await _productRepository.UpdateAsync(product);
                if (updated) return RedirectToAction(nameof(Index));
            }

            return View(product);
        }

        [HttpGet("delete/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var product = await _productRepository.GetByIdAsync(id.Value);
            if (product == null) return NotFound();

            return View(product);
        }

        [HttpPost("delete/{id}"), ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _productRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
