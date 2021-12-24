using Decorus.DataAccess;
using Decorus.DataAccess.Repository.IRepository;
using Decorus.Models;
using Decorus.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DecorusWeb.Controllers;
[Area("Admin")]
public class ProductController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IWebHostEnvironment _hostEnvironment;

    public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
    {
        _unitOfWork = unitOfWork;
        _hostEnvironment = hostEnvironment;
    }

    public IActionResult Index()
    {
        return View();
    }

    //GET
    public IActionResult Upsert(int? id)
    {
        // NEW VERSION
        ProductVM productVM = new()
        {
            Product = new(),
            CategoryList = _unitOfWork.Category.GetAll().Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString(),
            }),
            CoverTypeList = _unitOfWork.CoverType.GetAll().Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString(),
            })
        };
        // OLD VERSION
        /*
        Product product = new();
        IEnumerable<SelectListItem> CategoryList = _unitOfWork.Category.GetAll().Select(
            u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            }
        );
        IEnumerable<SelectListItem> CoverTypeList = _unitOfWork.CoverType.GetAll().Select(
            u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            }
        );*/
        if (id == null || id == 0)
        {
            // OLD VERSION
            // Create product. Product does not exist.
            // ViewBag Data returning to front-end View
            //ViewBag.CategoryList = CategoryList;
            // ViewData returning to front-end View
            // ViewData["CoverTypeList"] = CoverTypeList;
            return View(productVM);
        }
        else
        {
            // Product exists. Load it in.
            productVM.Product = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == id);
            return View(productVM);
        }

    }

    //POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Upsert(ProductVM obj, IFormFile? file)
    {
        if (ModelState.IsValid)
        {
            string wwwRootPath = _hostEnvironment.WebRootPath;
            if (file != null)
            {
                string fileName = Guid.NewGuid().ToString();
                var uploads = Path.Combine(wwwRootPath, @"images\products");
                var extension = Path.GetExtension(file.FileName);

                if (obj.Product.ImageUrl != null)
                {
                    var oldImagePath = Path.Combine(wwwRootPath, obj.Product.ImageUrl.TrimStart('\\'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                {
                    file.CopyTo(fileStreams);
                }
                obj.Product.ImageUrl = @"\images\products\" + fileName + extension;
            }
            if (obj.Product.Id == 0)
            {
                _unitOfWork.Product.Add(obj.Product);
                TempData["success"] = "Product created succesfully";
            }
            else
            {
                _unitOfWork.Product.Update(obj.Product);
                TempData["success"] = "Product updated succesfully";
            }
            _unitOfWork.Save();
            return RedirectToAction("Index", "Product");
        }
        return View(obj);
    }

    //GET
    public IActionResult Delete(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }
        var CoverFromDb = _unitOfWork.CoverType.GetFirstOrDefault(u => u.Id == id);

        if (CoverFromDb == null)
        {
            return NotFound();
        }

        return View(CoverFromDb);
    }

    //POST
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeletePOST(int? id)
    {
        var obj = _unitOfWork.CoverType.GetFirstOrDefault(u => u.Id == id);
        if (obj == null)
        {
            return NotFound();
        }

        _unitOfWork.CoverType.Remove(obj);
        _unitOfWork.Save();
        TempData["success"] = "CoverType deleted succesfully";
        return RedirectToAction("Index");
    }

    #region API CALLS
    [HttpGet]
    public IActionResult GetAll()
    {
        var productList = _unitOfWork.Product.GetAll(includeProperties: "Category,CoverType");
        return Json(new { data = productList });
    }
    #endregion
}
