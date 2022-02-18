using FinalEmarketingProjectMVC.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace FinalEmarketingProjectMVC.Controllers
{
    public class HomeController : Controller
    {


        dbFinalEmarketingEntities1 db = new dbFinalEmarketingEntities1();

        #region Index
        public ActionResult Index()
        {
            var data = db.tbl_Categories.ToList();
            return View(data);
        }
        #endregion

        #region Ads
        public ActionResult Ads(int? id)
        {
            int num = 5;
            Session["count"] = num;
            var data = db.tbl_Products.Where(x => x.pro_fk_cat == id && x.pro_status == 1).OrderByDescending(x => x.pro_id).Take(num).ToList();
            ViewBag.ilist = db.tbl_Images.ToList();
            var cat = db.tbl_Categories.Where(model => model.cat_id == id).FirstOrDefault();
            ViewBag.clist = cat;
            return View(data);
        }


        [HttpPost]
        public ActionResult Ads(int? id, string search)
        {
            if (string.IsNullOrEmpty(search) == true)
            {


                int num = 5 + Convert.ToInt32(Session["count"]);
                var data = db.tbl_Products.Where(x => x.pro_fk_cat == id && x.pro_status == 1).OrderByDescending(x => x.pro_id).Take(num).ToList();
                ViewBag.ilist = db.tbl_Images.ToList();
                var cat = db.tbl_Categories.Where(model => model.cat_id == id).FirstOrDefault();
                ViewBag.clist = cat;

                Session["count"] = num;
                return PartialView("_AdsPartialView", data);
            }
            else
            {


                var p = db.tbl_Products.Where(x => x.pro_name.Contains(search) && x.pro_fk_cat == id).ToList();
                if (p.Count() > 0)
                {
                var data = db.tbl_Products.Where(x => x.pro_fk_cat == id).OrderByDescending(x => x.pro_id).ToList();
                ViewBag.ilist = db.tbl_Images.ToList();
                var cat = db.tbl_Categories.Where(model => model.cat_id == id).FirstOrDefault();

                ViewBag.clist = cat;


                return PartialView("_ShowAdsBySearchPartialView", p);


                }
                else
                {
                    return PartialView("_NoRowsFound");
                }
            }
           
        }
        #endregion

        #region SignUp
        public ActionResult SignUp()
        {
            List<string> gender = new List<string>()
            {
                "Male",
                "Female"
            };

            List<string> country = new List<string>()
            {
                "Pakistan",
                "India",
                "Bangladesh",
                "Srilanka"
            };
            ViewBag.gender = gender;
            ViewBag.country = country;

            return View();
        }

        [HttpPost]
        public ActionResult SignUp(tbl_Users u)
        {
            if (u.ImgFile != null)
            {
                string filename = Path.GetFileNameWithoutExtension(u.ImgFile.FileName);
                string extension = Path.GetExtension(u.ImgFile.FileName);

                HttpPostedFileBase PostedFile = u.ImgFile;
                int length = PostedFile.ContentLength;

                if (extension.ToLower() == ".jpg" || extension.ToLower() == ".png" || extension.ToLower() == ".jpeg")
                {
                    if (length <= 1000000)
                    {
                        filename = filename + extension;

                        u.u_image = "~/images/" + filename;

                        filename = Path.Combine(Server.MapPath("~/images/"), filename);

                        u.ImgFile.SaveAs(filename);

                        db.tbl_Users.Add(u);

                        int a = db.SaveChanges();

                        if (a > 0)
                        {
                            TempData["AddUserMessage"] = "<script>alert('SigUp Sucessfull')</script>";
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            ModelState.Clear();
                            ViewBag.AddUserMessage = "<script>alert('Not SignUp')</script>";
                        }

                    }
                    else
                    {
                        ViewBag.sizemessage = "<script>alert('Image Size Must Be In 1 MB')</script>";
                    }
                }
                else
                {
                    ViewBag.extensionmessage = "<script>alert('Image Not Supported')</script>";
                }

            }
            else
            {
                if (ModelState.IsValid == true)
                {
                    tbl_Users user = new tbl_Users();
                    user.u_username = u.u_username;
                    user.u_email = u.u_email;
                    user.u_password = u.u_password;
                    user.u_gender = u.u_gender;
                    user.u_phone = u.u_phone;
                    user.u_country = u.u_country;
                    user.u_image = "~/images/user.png";
                    db.tbl_Users.Add(user);
                    int a = db.SaveChanges();
                    if (a > 0)
                    {
                        TempData["AddUserMessage"] = "<script>alert('SigUp Sucessfull')</script>";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.Clear();
                        ViewBag.AddUserMessage = "<script>alert('Not SignUp')</script>";
                    }
                }
            }

            return View();
        }
        #endregion

        #region Login
        public ActionResult Login()
        {

            return View();

        }

        [HttpPost]
        public ActionResult Login(tbl_Users u)
        {
            if (ModelState.IsValid == true)
            {
                var data = db.tbl_Users.Where(model => model.u_username == u.u_username && model.u_password == u.u_password).FirstOrDefault();
                if (data != null)
                {
                    Session["u_id"] = data.u_id.ToString();
                    Session["u_username"] = data.u_username.ToString();
                    var name = Session["u_username"].ToString();
                    var mubeen = db.tbl_Users.Where(model => model.u_username == name).FirstOrDefault();
                    Session["ImgPath"] = mubeen.u_image.ToString();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.message = "<script>alert('Invalid username or password')</script>";
                    ModelState.Clear();
                    return View();
                }
            }
            else
            {
                ViewBag.message2 = "<script>alert('Please fill both fields')</script>";
                return View();
            }
        }
        #endregion

        #region Signout
        public ActionResult Signout()
        {
            Session.RemoveAll();
            Session.Abandon();
            return RedirectToAction("Index");
        }
        #endregion

        #region CreateAd
        public ActionResult CreateAd()
        {
            var cat = db.tbl_Categories.ToList();
            ViewBag.catList = new SelectList(cat, "cat_id", "cat_name");
            return View();
        }

        [HttpPost]
        public ActionResult CreateAd(tbl_Products pro, HttpPostedFileBase ImgFile)
        {
            var cat = db.tbl_Categories.ToList();
            ViewBag.catList = new SelectList(cat, "cat_id", "cat_name");


            tbl_Images imgx = new tbl_Images();
            imgx.i_image = ImgFile.FileName;
            imgx.p_id = pro.pro_id;

            var path = Path.Combine(Server.MapPath("~/images/"), ImgFile.FileName);
            ImgFile.SaveAs(path);

            tbl_Products p = new tbl_Products();
            p.pro_name = pro.pro_name;
            p.pro_price = pro.pro_price;
            p.pro_description = pro.pro_description;
            p.pro_status = pro.pro_status;
            p.pro_add_date = Convert.ToDateTime(DateTime.Now.ToLongDateString());
            p.pro_fk_cat = pro.pro_fk_cat;
            p.pro_fk_user = int.Parse(Session["u_id"].ToString());

            db.tbl_Images.Add(imgx);

            db.tbl_Products.Add(p);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        #endregion

        #region ViewAd
        public ActionResult ViewAd(int? id)
        {
            ViewAd vad = new ViewAd();
            tbl_Products p = db.tbl_Products.Where(x => x.pro_id == id).FirstOrDefault();
            vad.pro_id = p.pro_id;
            vad.pro_name = p.pro_name;
            vad.pro_id = p.pro_id;
            vad.pro_price = p.pro_price;
            vad.pro_description = p.pro_description;
            vad.pro_add_date = p.pro_add_date;
            tbl_Images i = db.tbl_Images.Where(x => x.p_id == id).FirstOrDefault();
            vad.i_image = i.i_image;
            tbl_Categories c = db.tbl_Categories.Where(x => x.cat_id == p.pro_fk_cat).FirstOrDefault();
            vad.cat_name = c.cat_name;
            tbl_Users u = db.tbl_Users.Where(x => x.u_id == p.pro_fk_user).FirstOrDefault();
            vad.u_username = u.u_username;
            vad.u_phone = u.u_phone;
            vad.u_image = u.u_image;
            vad.pro_fk_user = u.u_id;
            return View(vad);
        }
        #endregion

        #region DeleteAd
        public ActionResult DeleteAd(int? id)
        {
            var a = db.tbl_Products.Find(id);
            a.pro_status = 0;
            tbl_Products p = new tbl_Products();
            p.pro_status = a.pro_status;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        #endregion


    }
}

