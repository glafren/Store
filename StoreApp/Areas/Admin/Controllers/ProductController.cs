using Entities.Dtos;
using Entities.Models;
using Entities.RequestParameters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Services.Contracts;
using StoreApp.Models;

namespace StoreApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        private readonly IServiceManager _manager;

        public ProductController(IServiceManager manager)
        {
            _manager = manager;
        }

        public IActionResult Index([FromQuery] ProductRequestParameters p)
        {
            ViewData["Title"] = "Products";
            var products = _manager.ProductService.GetAllProductsWithDetails(p);
            var pagination = new Pagination()
            {
                CurrenPage = p.PageNumber,
                ItemsPerPage = p.PageSize,
                TotalItems = _manager.ProductService.GetAllProducts(false).Count()
            };
            return View(new ProductListViewModel()
            {
                Products = products,
                Pagination = pagination
            });
        }

        public IActionResult Create()
        {
            ViewData["Title"] = "Products Create";
            //ViewBag.Categories = _manager.CategoryService.GetAllCategories(false);
            ViewBag.Categories = GetCategoriesSelectList();  // seçilebilir nesne oluşturduk(veri alanı, text alanı ve seçili gelecek değer)
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] ProductDtoForInsertion productDto, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                //file operation
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", file.FileName); // a/b/c
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                productDto.ImageUrl = String.Concat("/images/", file.FileName);
                _manager.ProductService.CreateProduct(productDto);
                TempData["success"] =  $"{productDto.ProductName} has been created.";
                return RedirectToAction("Index");
            }
            return View();
        }
        private SelectList GetCategoriesSelectList()
        {
            return new SelectList(_manager.CategoryService.GetAllCategories(false), "CategoryId", "CategoryName", "1");
        }

        public IActionResult Update([FromRoute(Name = "id")] int id)
        {
            ViewBag.Categories = GetCategoriesSelectList();
            var model = _manager.ProductService.GetOneProductForUpdate(id, false);
            ViewData["Title"] = model?.ProductName;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update([FromForm] ProductDtoForUpdate productDto, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                //file operation
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", file.FileName); // a/b/c
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                productDto.ImageUrl = String.Concat("/images/", file.FileName);
                _manager.ProductService.UpdateOneProduct(productDto);
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet]
        public IActionResult Delete([FromRoute(Name = "id")] int id)
        {
            _manager.ProductService.DeleteOneProduct(id);
            TempData["danger"] =  "Product has been removed.";
            return RedirectToAction("Index");
        }
    }
}