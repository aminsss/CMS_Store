using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using MyEshop.Classes;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using Koshop.DomainClasses;
using Koshop.ViewModels;
using Koshop.ServiceLayer.Contracts;

namespace Koshop.web.Controllers
{
   
    public class AccountController : Controller
    {
        private IUserService _userService;
        public AccountController(IUserService userService)
        {
            _userService = userService;
        }
        // GET: User
        public ActionResult Register()
        {
            return View();
        }

        private long LongRandom(long min, long max, Random rand)
        {
            long result = rand.Next((Int32)(min >> 32), (Int32)(max >> 32));
            result = (result << 32);
            result = result | (long)rand.Next((Int32)min, (Int32)max);
            return result;
        }

        private string googlecaptcha(string captcha)
        {
            string urlToPost = "https://www.google.com/recaptcha/api/siteverify";
            string secretKey = "6LcBpoAUAAAAAPyMfJgBGZxryHywqxRxwbZvKYXu"; // change this
            string gRecaptchaResponse;
            if (captcha == null)
                gRecaptchaResponse = Request.Form["g-recaptcha-response"];
            else
                gRecaptchaResponse = captcha;

            var postData = "secret=" + secretKey + "&response=" + gRecaptchaResponse;

            // send post data
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlToPost);
            request.Method = "POST";
            request.ContentLength = postData.Length;
            request.ContentType = "application/x-www-form-urlencoded";

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(postData);
            }

            // receive the response now
            string result = string.Empty;
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                }
            }

            return result;
        }


        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Register(RegisterViewModel register)
        {
            if (register.Moblie == null || register.Email == null)
                ModelState.AddModelError("", "لطفا ایمیل و شماره تلفن را برای فعالسازی حسابتان وارد کنید!");

            string result = googlecaptcha(null);
            SendEmailSMS send = new SendEmailSMS();

            //// validate the response from Google reCaptcha
            var captChaesponse = JsonConvert.DeserializeObject<reCaptchaResponse>(result);
            if (!captChaesponse.Success)
            {
                ViewBag.Message = "لطفا ابتدا کادر تایید هویتی را علامت بزنید!";
                return View();
            }


            long activeCode = LongRandom(min: 123456, max: 654321, rand: new Random());

            
            if (ModelState.IsValid)
            {
                User user = new User()
                {
                    UserName = register.UserName.Trim(),
                    Name = register.FirstName + " " + register.LastName,
                    Password = register.Pass.Trim(),
                    moblie = register.Moblie.Trim(),
                    Email = register.Email.Trim(),
                    AddedDate = DateTime.Now,
                    Profile = "no-photo.jpg",
                    RoleId = 2,
                    IP = ServiceLayer.AddressIpClass.GetPublicIPAddress(),
                    ActiveCode = activeCode.ToString(),
                    activecodeDate = DateTime.Now,
                    ISActive = false
                };

                try
                {
                    string bodySms = "کد تایید شما : " + activeCode.ToString();
                    send.SendSMS(new[]{ register.Moblie }, bodySms);
                }
                catch { }
                try
                {
                    string bodyemail = PartialToStringClass.RenderPartialView("Email", "ActiveUser", register);
                    send.SendEmail(new[] { register.Email }, "کد تاییدیه حساب", bodyemail);
                }
                catch { }

                _userService.Add(user);
                ViewBag.status = "برای فعال کردن حسابتان کد تائیدیه را وارد کنید!";
                return View("RegisteredActivate",register);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "خطایی وجود دارد");
            }
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Login(LoginViewModel login, string returnUrl, string captcha)
        {

            //string result = googlecaptcha(null);

            ////// validate the response from Google reCaptcha
            //var captChaesponse = JsonConvert.DeserializeObject<reCaptchaResponse>(result);
            //if (!captChaesponse.Success)
            //{
            //    ViewBag.Message = "لطفا ابتدا کادر تایید هویتی را علامت بزنید!";
            //    return View();
            //}

            // go ahead and write code to validate mobile password against database
            var Qlogin = _userService.LogIn(login.UserName, login.password);

            if (Qlogin != null)
            {
                if (Qlogin.ISActive == true)
                {
                    FormsAuthentication.SetAuthCookie(login.UserName, login.Remember);
                    if (returnUrl != null)
                    {
                        return Redirect(returnUrl);
                    }
                    return Redirect("/");
                }
                else
                {
                    long activecode = LongRandom(min: 123456, max: 654321, rand: new Random());
                    Qlogin.ActiveCode = activecode.ToString();
                    Qlogin.activecodeDate = DateTime.Now;
                    _userService.Edit(Qlogin);
                    ///send SMS to number
                    SendEmailSMS send = new SendEmailSMS();
                    try
                    {
                        string bodySms = "کد تایید شما : " + activecode.ToString();
                        send.SendSMS(new[] { Qlogin.moblie }, bodySms);
                    }
                    catch { }
                    try
                    {
                        string bodyemail = PartialToStringClass.RenderPartialView("Email", "ActiveUser", Qlogin);
                        send.SendEmail(new[] { Qlogin.Email }, "کد تاییدیه حساب", bodyemail);
                    }
                    catch { }
                    RegisterViewModel register = new RegisterViewModel();
                    register.UserName = Qlogin.UserName;
                    ViewBag.status = "حساب شما غیر فعال است";
                    return View("RegisteredActivate", register);
                }
            }
            else
            {
                ModelState.AddModelError("UserName", "نام کاربری  یا رمز عبور اشتباه میباشد!");
            }
            return View();
        }


       

        [ValidateAntiForgeryToken]
        [HttpPost]
        public JsonResult Activating(string id, string activeCode)
        {
            DateTime till = DateTime.Now.AddDays(-1);
            var user = _userService.GetUserByIdentity(id);
            if (user != null)
            {
                if (user.ActiveCode == activeCode && user.activecodeDate > till)
                {
                    FormsAuthentication.SetAuthCookie(id, false);
                    user.ISActive = true;
                    string activecode = Guid.NewGuid().ToString().Replace("-", "");
                    user.ActiveCode = activecode;
                    _userService.Edit(user);
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public JsonResult SendAgain(string userName)
        {
            DateTime till = DateTime.Now.AddMinutes(-2);
            var user = _userService.GetByUserName(userName);
            SendEmailSMS send = new SendEmailSMS();

            if (user != null)
            {
                if (user.activecodeDate > till)
                {
                    return Json("Time", JsonRequestBehavior.AllowGet);
                }
                long activecode = LongRandom(min: 123456, max: 654321, rand: new Random());
                user.ActiveCode = activecode.ToString();
                user.activecodeDate = DateTime.Now;
                _userService.Edit(user);
                ///send SMS to number
                try
                {
                    string bodySms = "کد تایید شما : " + activecode.ToString();
                    send.SendSMS(new[] { user.moblie }, bodySms);
                }
                catch { }
                try
                {
                    string bodyemail = PartialToStringClass.RenderPartialView("Email", "ActiveUser", user);
                    send.SendEmail(new[] { user.Email }, "کد تاییدیه حساب", bodyemail);
                }
                catch { }
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }

        }

        [Authorize]
        public new ActionResult Profile()
        {
            if(User.Identity.IsAuthenticated)
            {
                var user = _userService.GetByUserName(User.Identity.Name);
                return View(user);
            }
            return View("LogIn");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public new ActionResult Profile( User user, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                var newUser = _userService.GetById(user.UserId);
                if(newUser != null)
                {
                    newUser.Name = user.Name;
                    newUser.UserName = user.UserName;
                    newUser.Email = user.Email;
                    newUser.moblie = user.moblie;
                    newUser.MeliID = user.MeliID;
                    newUser.BirthDate = user.BirthDate;
                    newUser.State = user.State;
                    newUser.Adress = user.Adress;
                }
                user.ActiveCode = Guid.NewGuid().ToString().Replace("-", "");
                _userService.Edit(newUser);
                return View(newUser);
            }

            return View(user);
        }

        public ActionResult ForgotPassword()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult SendForgotPass(string userName)
        {
            var user = _userService.GetByUserName(userName);
            SendEmailSMS send = new SendEmailSMS();
            if (user != null)
            {
                DateTime till = DateTime.Now.AddMinutes(-2);
                if (user.activecodeDate > till)
                {
                    return Json("Time", JsonRequestBehavior.AllowGet);
                }
                string[] linkgenerate = Guid.NewGuid().ToString().Split('-');
                user.ActiveCode = linkgenerate[0];
                user.activecodeDate = DateTime.Now;
                _userService.Edit(user);
                // // send SMS to number
                try
                {
                    string bodySms = Request.Url.GetLeftPart(UriPartial.Authority) + "/Account/changePass/" + user.ActiveCode;
                    send.SendSMS(new[] { user.moblie }, bodySms);
                }
                catch { }
                try
                {
                    string bodyemail = PartialToStringClass.RenderPartialView("Email", "ForgotPass", user);
                    send.SendEmail(new[] { user.Email }, "بازیابی رمز عبور", bodyemail);
                }
                catch { }

                return Json("<div class=\"alert alert-success rtl\" role=\"alert\">لینک تغییر رمز عبور به موبایل و ایمیل شما ارسال شد</div>", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("<div class=\"alert alert-danger rtl\" role=\"alert\"> <strong>خطا </strong> کاربری یافت نشد</div>", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ChangePassword(string id)
        {
            DateTime yesrterday = DateTime.Now.AddDays(-1);
            var user = _userService.GetByActiveCode(id);
            if (user == null)
            {
                if (user.activecodeDate > yesrterday)
                {
                    ViewBag.id = user.ActiveCode;
                    ViewBag.status = "Ok";
                    return View();
                }
            }
            ViewBag.id = "None";
            ViewBag.status = "Nok";
            return View();
        }

        public JsonResult SendChangePass(string id, string pass, string repass)
        {
            DateTime yesrterday = DateTime.Now.AddDays(-1);
            var user = _userService.GetByActiveCode(id);
            if (user == null)
                return Json("<div class=\"alert alert-danger\" role=\"alert\"> <strong>خطا </strong>  کاربری یافت نشد و یا لینک منقضی شده است </div>", JsonRequestBehavior.AllowGet);
            else if(user.activecodeDate > yesrterday)
                return Json("<div class=\"alert alert-danger\" role=\"alert\"> <strong>خطا </strong>  کاربری یافت نشد و یا لینک منقضی شده است </div>", JsonRequestBehavior.AllowGet);
            else if (pass != repass)
                return Json("<div class=\"alert alert-danger\" role=\"alert\"> <strong>خطا </strong>  کلمه عبور با هم مغایرت دارند </div>", JsonRequestBehavior.AllowGet);
            else if (pass == user.Password)
                return Json("<div class=\"alert alert-danger\" role=\"alert\"> <strong>خطا </strong>  کلمه عبور فعلی قابل قبول نیست! </div>", JsonRequestBehavior.AllowGet);
            else
            {
                string activecode = Guid.NewGuid().ToString().Replace("-", "");
                user.ActiveCode = activecode;
                user.Password = pass;
                _userService.Edit(user);
                return Json("<div class=\"alert alert-success\" role=\"alert\"> تغییر کلمه عبور با موفقیت انجام شد</div>", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            return Redirect("/account/login");
        }

        public JsonResult UniqueUserEmail(string Email, string UserName)
        {
            if (_userService.UniqueUserEmail(Email, UserName))
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult UniqueUserMobile(string Moblie, string UserName)
        {
            if (_userService.UniqueUserMobile(Moblie, UserName))
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }

        //public JsonResult SendSmsRegister(string id)
        //{
        //    string mobile = id.Trim();
        //    var Qlogin = db.Users.FirstOrDefault(L => L.moblie == mobile);
        //    long activecode = LongRandom(min: 123456, max: 654321, rand: new Random());
        //    string bodySms = "";
        //    if (Qlogin == null)
        //    {
        //        string[] pass = Guid.NewGuid().ToString().Split('-');
        //        User user = new User()
        //        {
        //            moblie = mobile,
        //            AddedDate = DateTime.Now,
        //            Profile = "no-photo.png",
        //            RoleId = 2,
        //            ActiveCode = activecode.ToString(),
        //            activecodeDate = DateTime.Now,
        //            ISActive = false,
        //            Password = pass[0]

        //        };
        //        _userService.Add(user);
        //        bodySms = "کد تایید شما : " + activecode.ToString() + "\n\rبا تشکر از عضویت شما در koshop.ir ";
        //    }
        //    else
        //    {
        //        DateTime till = DateTime.Now.AddMinutes(-2);
        //        if (Qlogin.activecodeDate > till)
        //        {
        //            return Json("Time", JsonRequestBehavior.AllowGet);
        //        }
        //        Qlogin.ActiveCode = activecode.ToString();
        //        Qlogin.activecodeDate = DateTime.Now;
        //        db.SaveChanges();
        //        bodySms = "کد تایید شما : " + activecode.ToString();
        //    }
        //    try
        //    {
        //        SendEmailSMS.sendSMS(mobile, bodySms);
        //    }
        //    catch { }
        //    return Json(true, JsonRequestBehavior.AllowGet);

        //}


        //public ActionResult RegisteredActivate()
        //{
        //    RegisterViewModel register = new RegisterViewModel();
        //    ViewBag.status = "حساب شما غیر فعال است";
        //    ViewBag.mobilenumber = "09367642958";
        //    return View(register);
        //}


    }
}