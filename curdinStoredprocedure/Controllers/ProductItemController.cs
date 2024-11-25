using curdinStoredprocedure.DataAccessLayer;
using curdinStoredprocedure.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace curdinStoredprocedure.Controllers
{
    public class ProductItemController : Controller
    {
        
        private readonly ProductItemsData _productItemData;


        public ProductItemController(ProductItemsData productItemsData)
        {
            _productItemData = productItemsData;
        }
        public IActionResult Index()
        {
            var productItems = _productItemData.GetAllProductItems();
            return PartialView("Index",productItems);
        }

        [HttpGet("api/productitems")]
        public IActionResult GetProductItems()
        {
            var productItems = _productItemData.GetAllProductItems();

            // Return the data as JSON
            return Ok(productItems);
        }


        [HttpGet]
        public IActionResult Create(int CategoryId)
        {
            var categories = _productItemData.GetAllProductCategories();

            ViewBag.Categories = new SelectList(categories, "CategoryId", "CategoryName", CategoryId);

            var model = new ProductItems
            {
                CategoryId = CategoryId
            };

            return View(model);
        }



        [HttpPost]
        public IActionResult Create(ProductItems model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    int newProductId = _productItemData.InsertProductItem(model);
                    TempData["Message"] = $"New Product ID: {newProductId} created successfully.";
                    return RedirectToAction("Index", "ProductCategory");
                }
                catch (Exception ex)
                {
                    ViewBag.Error = $"An error occurred: {ex.Message}";
                }
            }

            var categories = _productItemData.GetAllProductCategories();
            ViewBag.Categories = categories.Select(c => new SelectListItem
            {
                Value = c.CategoryId.ToString(),
                Text = c.CategoryName
            }).ToList();

            return View(model);
        }



        [HttpGet]
        public IActionResult Edit(int id)
        {
            var productItem = _productItemData.GetProductItemById(id);
            if (productItem != null)
            {
                return View(productItem);
            }
            return NotFound();
        }

        [HttpPost]
        public IActionResult Edit(ProductItems model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _productItemData.UpdateProductItem(model);
                    ViewBag.Message = "Product updated successfully";
                    return RedirectToAction("Index", "ProductCategory");

                }
                catch (Exception ex)
                {
                    ViewBag.Error = $"An error occurred: {ex.Message}";
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var productItem = _productItemData.GetProductItemById(id);
            if (productItem != null)
            {
                return View(productItem);
            }
            return NotFound();
        }

        
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                _productItemData.DeleteProductItem(id);
                ViewBag.Message = "Product deleted successfully";
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"An error occurred: {ex.Message}";
            }
            return RedirectToAction("Index","ProductCategory");
        }

        public IActionResult GetProductByCategory(int categoryId)
        {
              ViewData["SelectedCategoryId"] = categoryId;
            var productItems = _productItemData.GetProductItemsByCategory(categoryId);
            if (productItems == null || !productItems.Any())
            {
                return Json(new { success = false, message = "No products found for this category." });
            }
            return PartialView("_GetProductByCategory", productItems);
        }
    }
}
