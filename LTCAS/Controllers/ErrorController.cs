using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LTCAS.Controllers
{
    public class ErrorController : Controller
    {

        /// <summary>

        /// Indexes the specified error.

        /// </summary>

        /// <param name="error">The error.</param>

        /// <returns></returns>

        public ActionResult Index(string error)

        {

            ViewData["Title"] = "抱歉, 處理你的請求發生錯誤";

            ViewData["Description"] = error;

            return View();

        }

        /// <summary>

        /// Generics the error.

        /// </summary>

        /// <param name="error">The error.</param>

        /// <returns></returns>

        public ActionResult GenericError(string error)

        {

            ViewData["Title"] = "抱歉, 處理你的請求發生錯誤";

            ViewData["Description"] = error;

            return View();

        }

        /// <summary>

        /// Forbiddens the specified error.

        /// </summary>

        /// <param name="error">The error.</param>

        /// <returns></returns>

        public ActionResult Forbidden(string error)

        {

            return RedirectToAction("Index", "Home");

        }

        /// <summary>

        /// Pages the not found.

        /// </summary>

        /// <param name="error">The error.</param>

        /// <returns></returns>

        public ActionResult PageNotFound(string error)

        {

            ViewData["Title"] = "抱歉, 處理你的請求發生404錯誤";

            ViewData["Description"] = error;

            Response.StatusCode = 404;

            return View();

        }

        /// <summary>

        /// Internals the error.

        /// </summary>

        /// <param name="error">The error.</param>

        /// <returns></returns>

        public ActionResult InternalError(string error)

        {

            ViewData["Title"] = "抱歉, 處理你的請求發生500錯誤";

            ViewData["Description"] = error;

            Response.StatusCode = 500;

            return View();

        }

    }
}
