using Decorus.DataAccess;
using Decorus.DataAccess.Repository.IRepository;
using Decorus.Models;
using Microsoft.AspNetCore.Mvc;

namespace DecorusWeb.Controllers
{
    public class CoverController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CoverController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Cover> objCoverList = _unitOfWork.Cover.GetAll();
            return View(objCoverList);
        }

        //GET
        public IActionResult Create()
        {
            return View();
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Cover obj)
        {
            if (obj.Name == obj.Id.ToString())
            {
                ModelState.AddModelError("Name", "DisplayOrder !== Name");
            }
            if (ModelState.IsValid)
            {
                _unitOfWork.Cover.Add(obj);
                _unitOfWork.Save();
                TempData["success"] = "Cover created succesfully";
                return RedirectToAction("Index", "Cover");
            }
            return View(obj);
        }

        //GET
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            //var CoverFromDb = _db.Categories.Find(id);
            var CoverFromDb = _unitOfWork.Cover.GetFirstOrDefault(u => u.Id == id);
            //var CoverFromDbSingle = _db.Categories.SingleOrDefault(u => u.Id == id);

            if (CoverFromDb == null)
            {
                return NotFound();
            }

            return View(CoverFromDb);
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Cover obj)
        {
            if (obj.Name == obj.Id.ToString())
            {
                ModelState.AddModelError("Name", "DisplayOrder !== Name");
            }
            if (ModelState.IsValid)
            {
                _unitOfWork.Cover.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "Cover edit succesfully";
                return RedirectToAction("Index", "Cover");
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
            var CoverFromDb = _unitOfWork.Cover.GetFirstOrDefault(u => u.Id == id);

            if (CoverFromDb == null)
            {
                return NotFound();
            }

            return View(CoverFromDb);
        }

        //POST
        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePOST(int? id)
        {
            var obj = _unitOfWork.Cover.GetFirstOrDefault(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }

            _unitOfWork.Cover.Remove(obj);
            _unitOfWork.Save();
            TempData["success"] = "Cover deleted succesfully";
            return RedirectToAction("Index");
        }
    }
}
