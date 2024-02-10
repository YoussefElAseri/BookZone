using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            List<Category> objCategoriesList = _unitOfWork.Category.GetAll().ToList();
            return View(objCategoriesList);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category obj)
        {
            /*if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "Category name cannot match Display order!");
            }*/
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Add(obj);
                _unitOfWork.Save();
                TempData["success"] = "Category created successfully!";
                return RedirectToAction("Index", "Category");
            }
            return View();
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Category categoryFromDb = _unitOfWork.Category.GetFirstOrDefault(u => u.CategoryId == id);
            /*Category categoryFromDb = _db.Categories.Find(id);
            Category categoryFromDb1 = _db.Categories.FirstOrDefault(u => u.CategoryId == id);
            Category categoryFromDb2 = _db.Categories.Where(u => u.CategoryId == id).FirstOrDefault();*/
            if (categoryFromDb == null)
            {
                return NotFound();
            }
            return View(categoryFromDb);
        }

        [HttpPost]
        public IActionResult Edit(Category obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "Category updated successfully!";
                return RedirectToAction("Index", "Category");
            }
            return View();
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category categoryFromDb = _unitOfWork.Category.GetFirstOrDefault(u => u.CategoryId == id);
            /*Category categoryFromDb = _db.Categories.Find(id);
            Category categoryFromDb1 = _db.Categories.FirstOrDefault(u => u.CategoryId == id);
            Category categoryFromDb2 = _db.Categories.Where(u => u.CategoryId == id).FirstOrDefault();*/
            if (categoryFromDb == null)
            {
                return NotFound();
            }
            return View(categoryFromDb);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Category obj = _unitOfWork.Category.GetFirstOrDefault(u => u.CategoryId == id);
            if (obj == null)
            {
                return NotFound();
            }

            _unitOfWork.Category.Remove(obj);
            _unitOfWork.Save();
            TempData["success"] = "Category deleted successfully!";
            return RedirectToAction("Index", "Category");

        }
    }
}
