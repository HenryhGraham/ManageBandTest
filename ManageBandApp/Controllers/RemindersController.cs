using ManageBandApp.ControllerProcessors;
using ManageBandApp.Models;
using ManageBandApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;

namespace ManageBandApp.Controllers
{
    /// <summary>
    /// Контроллер остатков
    /// </summary>
    public class RemindersController : Controller
    {
        /// <summary>
        /// Просмотр остатков
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Index()
        {
            try
            {

                RemindersReportViewModel model = new RemindersReportViewModel();
                ViewBag.Title = "Просмотр остатков";
                return View(model);
            }
            catch (Exception ex)
            {
                return View("~/Views/Errors/DefaultErrorView.cshtml", ex.ToString());
            }
        }

        /// <summary>
        /// Просмотр остатков
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Index(RemindersReportViewModel model)
        {
            try
            {
                using (DbAppContext context = new DbAppContext())
                {
                    RemindersControllerProcessor processor = new RemindersControllerProcessor(context);
                    processor.SetReminders(model);
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
