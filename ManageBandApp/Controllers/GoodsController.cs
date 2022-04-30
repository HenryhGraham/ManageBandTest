using ManageBandApp.ControllerProcessors;
using ManageBandApp.Models;
using ManageBandApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;

namespace ManageBandApp.Controllers
{
    /// <summary>
    /// Контроллер перемещений номенклатуры
    /// </summary>
    public class GoodsController : Controller
    {
        /// <summary>
        /// Возвращает все перемещения номенклатуры
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Goods/GoodsMovings/{pageNum?}")]
        public IActionResult GoodsMovings(int? pageNum)
        {
            try
            {
                if (pageNum == null || pageNum <= 0)
                    pageNum = 1;

                using (DbAppContext context = new DbAppContext())
                {
                    int intPageNum = (int)pageNum;
                    int toSkip = (intPageNum - 1) * Consts.DefaultPageSize;
                    GoodsControllerProcessor processor = new GoodsControllerProcessor(context);
                    GoodsMovingViewModel model = processor.GetGoodsMovingViewModel(intPageNum, toSkip);

                    ViewBag.Title = "Перемещения номенклатуры";
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                return View("~/Views/Errors/DefaultErrorView.cshtml", ex.ToString());
            }
        }

        /// <summary>
        /// Удаление номенклатуры
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pageNum"></param>
        /// <returns></returns>
        public IActionResult Remove(Guid? id, int? pageNum)
        {
            try
            {
                if (id == null)
                    throw new ArgumentNullException(nameof(id));

                if (pageNum == null || pageNum <= 0)
                    pageNum = 1;

                using (DbAppContext context = new DbAppContext())
                {
                    GoodsControllerProcessor processor = new GoodsControllerProcessor(context);
                    processor.RemoveMoving((Guid)id);
                }

                return RedirectToAction("GoodsMovings", new { pageNum = pageNum });
            }
            catch (Exception ex)
            {
                return View("~/Views/Errors/DefaultErrorView.cshtml", ex.ToString());
            }

        }

        public IActionResult Index()
        {
            return RedirectToAction("GoodsMovings", new { pageNum = 1 });
        }

        /// <summary>
        /// Создание перемещения номенклатур
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult CreateMoving()
        {
            try
            {
                CreateMovingViewModel model = new CreateMovingViewModel();
                ViewBag.Title = "Создание перемещения номенклатуры";
                return View(model);
            }
            catch (Exception ex)
            {
                return View("~/Views/Errors/DefaultErrorView.cshtml", ex.ToString());
            }
        }

        /// <summary>
        /// Создание перемещения номенклатур
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult CreateMoving(CreateMovingViewModel model)
        {
            try
            {
                using (DbAppContext context = new DbAppContext())
                {
                    GoodsControllerProcessor processor = new GoodsControllerProcessor(context);
                    bool created = processor.CreateMoving(model);
                    if (created == true)
                    {
                        ViewBag.SuccessMessage = "Перемещение создано успешно";
                        model = new CreateMovingViewModel();
                    }
                    else
                        ViewBag.SuccessMessage = null;

                    return View(model);
                }
            }
            catch (Exception ex)
            {
                return View("~/Views/Errors/DefaultErrorView.cshtml", ex.ToString());
            }
        }
    }
}
