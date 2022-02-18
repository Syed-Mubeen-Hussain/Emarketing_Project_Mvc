using FinalEmarketingProjectMVC.Models;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FinalEmarketingProjectMVC.Controllers
{

    public class AdminController : Controller
    {
        dbFinalEmarketingEntities1 db = new dbFinalEmarketingEntities1();

        // GET: Admin

        #region Index
        public ActionResult Index()
        {
            if (Session["a_id"] == null)
            {
                return RedirectToAction("Login");
            }
            else
            {
                ViewBag.pro = db.tbl_Products.Count();
                ViewBag.cat = db.tbl_Categories.Count();
                ViewBag.user = db.tbl_Users.Count();
                return View();

            }
        }

        #endregion

        #region Login
        public ActionResult Login()
        {
            if (Session["a_id"] == null)
            {
                return View();
            }
            else
            {

                return RedirectToAction("Index");

            }
        }

        [HttpPost]
        public ActionResult Login(tbl_Admins admin)
        {
            if (ModelState.IsValid == true)
            {
                var ad = db.tbl_Admins.Where(model => model.a_username == admin.a_username && model.a_password == admin.a_password).SingleOrDefault();
                if (ad != null)
                {
                    Session["a_id"] = admin.a_id.ToString();
                    Session["a_username"] = admin.a_username.ToString();
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
                return View();
            }
        }

        #endregion

        #region List
        public ActionResult CategoryList()
        {
            var data = db.tbl_Categories.ToList();
            ViewBag.pro = db.tbl_Products.ToList();
            return View(data);
        }

        public ActionResult ProductList()
        {
            var a = db.tbl_Products.ToList();
            ViewBag.cat = db.tbl_Categories.ToList();
            ViewBag.img = db.tbl_Images.ToList();
            ViewBag.user = db.tbl_Users.ToList();
            return View(a);
        }

        #endregion

        #region Edit 
        public ActionResult EditProduct(int? id)
        {
            var a = db.tbl_Products.Find(id);
            return View(a);
        }

        [HttpPost]
        public ActionResult EditProduct(tbl_Products p)
        {
            tbl_Products pro = new tbl_Products();
            pro.pro_id = p.pro_id;
            pro.pro_name = p.pro_name;
            pro.pro_price = p.pro_price;
            pro.pro_description = p.pro_description;
            pro.pro_fk_user = p.pro_fk_user;
            pro.pro_fk_cat = p.pro_fk_cat;
            pro.pro_status = p.pro_status;
            pro.pro_add_date = p.pro_add_date;

            db.Entry(pro).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        #endregion

        #region Delete

        public ActionResult DeleteProduct(int? id)
        {
            var data = db.tbl_Products.Where(x => x.pro_id == id).SingleOrDefault();
            data.pro_status = 0;
            tbl_Products p = new tbl_Products();
            p.pro_status = data.pro_status;
            db.SaveChanges();
            return RedirectToAction("ProductList");
        }


        #endregion

        #region AddCategory
        public ActionResult AddCategory()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddCategory(tbl_Categories cat)
        {
            string filename = Path.GetFileNameWithoutExtension(cat.ImgFile.FileName);
            string extension = Path.GetExtension(cat.ImgFile.FileName);

            HttpPostedFileBase PostedFile = cat.ImgFile;
            int length = PostedFile.ContentLength;

            if (extension.ToLower() == ".jpg" || extension.ToLower() == ".png" || extension.ToLower() == ".jpeg")
            {
                if (length <= 1000000)
                {
                    filename = filename + extension;

                    cat.cat_image = "~/images/" + filename;

                    filename = Path.Combine(Server.MapPath("~/images/"), filename);

                    cat.ImgFile.SaveAs(filename);

                    db.tbl_Categories.Add(cat);

                    int a = db.SaveChanges();

                    if (a > 0)
                    {
                        TempData["AddCategoryMessage"] = "<script>alert('Category Add')</script>";
                        return RedirectToAction("CategoryList");
                    }
                    else
                    {
                        ModelState.Clear();
                        ViewBag.AddCategoryMessage = "<script>alert('Category Not Add')</script>";
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




            return View();
        }

        #endregion

        #region Logout

        public ActionResult Logout()
        {
            Session.RemoveAll();
            Session.Abandon();
            return RedirectToAction("Login");
        }

        #endregion

    }
}
