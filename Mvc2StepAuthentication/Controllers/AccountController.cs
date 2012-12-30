using System;
using System.Configuration;
using System.Web.Mvc;
using System.Web.Security;
using GVoiceSharp;
using Mvc2StepAuthentication.Models;

namespace Mvc2StepAuthentication.Controllers
{
    public class AccountController : Controller
    {
        [AllowAnonymous]
        public ActionResult Index(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;

            return View();
        }

        [HttpPost, AllowAnonymous]
        public ActionResult Index(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (FormsAuthentication.Authenticate(model.UserName, model.Password))
                {
                    var token = new Random().Next(100000, 1000000);

                    TempData["UserName"] = model.UserName;
                    TempData["TwoStepToken"] = token.ToString();

                    using (var conn = GVoiceConnectionFactory.Create())
                    {
                        conn.Login(ConfigurationManager.AppSettings["GVoiceUser"], ConfigurationManager.AppSettings["GVoicePassword"]);
                        conn.SendSms(ConfigurationManager.AppSettings["SMSNumber"], "Auth Token: " + token);
                    }
                    return RedirectToAction("SecondAuth", new { ReturnUrl = returnUrl });
                }
                ModelState.AddModelError("", "The user name or password provided is incorrect.");
            }
            ViewBag.ReturnUrl = returnUrl;

            return View(model);
        }

        [AllowAnonymous]
        public ActionResult SecondAuth(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            
            return View();
        }

        [HttpPost, AllowAnonymous]
        public ActionResult SecondAuth(SecondAuthModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (TempData["TwoStepToken"] as string == model.Token)
                {
                    FormsAuthentication.SetAuthCookie(TempData["UserName"] as string, false);

                    if (Url.IsLocalUrl(returnUrl))
                        return Redirect(returnUrl);
                    
                    return RedirectToAction("Index", "Home");
                }
                return Redirect("/Account/?returnUrl=" + returnUrl);
            }
            ViewBag.ReturnUrl = returnUrl;

            return View();
        }
    }
}
