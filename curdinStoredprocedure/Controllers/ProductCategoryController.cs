using curdinStoredprocedure.DataAccessLayer;
using curdinStoredprocedure.Models;
using Microsoft.AspNetCore.Mvc;

namespace ProductCategoryApp.Controllers
{
    public class ProductCategoryController : Controller
    {
        private readonly ProductCategoryData _productCategoryData;

        // Constructor injection
        public ProductCategoryController(ProductCategoryData productCategoryData)
        {
            _productCategoryData = productCategoryData;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var categories = _productCategoryData.GetAllProductCategories();
            return View(categories);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Productcategory model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    int newCategoryId = _productCategoryData.InsertProductCategory(model.CategoryName, model.Description);
                    ViewBag.Message = $"New Category ID: {newCategoryId}";
                }
                catch (Exception ex)
                {
                    ViewBag.Error = $"An error occurred: {ex.Message}";
                }
            }
            return View("Index");
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var category = _productCategoryData.GetProductCategoryById(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost]
        public IActionResult Edit(Productcategory model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _productCategoryData.UpdateProductCategory(model.CategoryId, model.CategoryName, model.Description);
                    ViewBag.Message = "Category updated successfully!";
                }
                catch (Exception ex)
                {
                    ViewBag.Error = $"An error occurred: {ex.Message}";
                }
            }
            return View(model);
        }

        // Delete product category
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var category = _productCategoryData.GetProductCategoryById(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                _productCategoryData.DeleteProductCategory(id);
                ViewBag.Message = "Category deleted successfully!";
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"An error occurred: {ex.Message}";
            }
            return RedirectToAction("Index");
        }
    }
}

