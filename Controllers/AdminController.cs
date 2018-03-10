using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static online_store.Models.Functions_project;
using static online_store.Models.DataBase;
using online_store.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;

namespace online_store.Controllers
{

    //[Authorize(Roles="admin")]
    public class AdminController : Controller
    {
        public ActionResult Admin_page()
        {

            return View();
        }
        [HttpPost]
        public ActionResult Delete_object(int id1,int id2)
        {
            if (!string.IsNullOrEmpty(id1.ToString()) && id1 > 0&&id1==id2)
            {
                db.Objects.Remove(db.Objects.First(x1 => x1.Id == id1));
                db.Comments.RemoveRange(db.Comments.Where(x1 => x1.Object_id == id1));
                db.Images.RemoveRange(db.Images.Where(x1 => x1.What_something == "Object" && x1.Something_id == id1.ToString()));
                db.Baskets.RemoveRange(db.Baskets.Where(x1 => x1.Object_id == id1));
                db.Follow_objects.RemoveRange(db.Follow_objects.Where(x1 => x1.Object_id == id1));
                db.SaveChanges();
            }


            return RedirectToAction("Index", "Home", new { });
        }
        public ActionResult Delete_object(int id)
        {
            ViewBag.id = id;
            return PartialView();
            //return RedirectToAction("Index", "Home", new { });
        }
        [HttpGet]
        public ActionResult Add_object(int id = -1)
        {
            Object_os res = null;
            if (id < 0)//string.IsNullOrEmpty(id)
            {
                res = new Object_os();
            }
            else
            {
                res = db.Objects.FirstOrDefault(x1 => x1.Id == id);
            }

            return PartialView(res);
        }
        [HttpGet]
        public ActionResult Edit_object(int id = -1)
        {
            ViewBag.id = id;
            return View();
        }
       [HttpGet]
        public ActionResult Edit_count_current_object(int id)
        {
            ViewBag.count = -1;
            ViewBag.id = id;
            var obj = db.Objects.FirstOrDefault(x1=>x1.Id==id);
            if (obj != null)
            {
                ViewBag.count = obj.Remainder;
            }
            return PartialView();
        }

        [HttpPost]
        public ActionResult Edit_count_current_object(int id,int count)
        {
            var obj=db.Objects.FirstOrDefault(x1 => x1.Id == id);

            if (obj != null && count > 0)
            {
                obj.Remainder += count;
                db.SaveChanges();
            }
            return RedirectToAction("Edit_object", "Admin",new {id=id });
        }
        [HttpPost]
        public ActionResult Add_object(Object_os a)
        {
            bool new_ = true;
            //проверки и тд
            if (a.Price > 0 && a.Discount >= 0 && a.Discount < 1)
            {
                if (a.Id > 0)
                {
                    var check = db.Objects.FirstOrDefault(x1 => x1.Id == a.Id);
                    if (check != null)
                    {
                        new_ = false;
                        check.Eq(a);
                        db.SaveChanges();
                    }
                    else
                        new_ = true;
                }
                if (new_)
                {
                    db.Objects.Add(a);
                    db.SaveChanges();
                    ViewBag.Id = a.Id;
                }

                return RedirectToAction("Object_view", "Home", new { id = a.Id });
            }



            return RedirectToAction("Index", "Home", new { });
        }
        [HttpGet]
        public ActionResult Discount_all_coupon()
        {
            var res = db.Discount_type.ToList();


            return View(res);
        }


        [HttpPost]
        public ActionResult Discount_coupon_create(Discount a)
        {
            //TODO обработка купона
            db.Discount_type.Add(a);
            db.SaveChanges();

            return View();
        }
        [HttpGet]
        public ActionResult Discount_coupon_create()
        {
            var res=new Discount();

            return View(res);
        }
        public ActionResult Application_phone_list()
        {
            var app = db.Application_phone_comm.Where(x1=>x1.Complete==false).ToList();

            return View(app);
        }
        public ActionResult Work_with_images_object(int id)
        {
            ViewBag.Id = id;
            var imgs = db.Images.Where(x1 => x1.What_something == "Object" && x1.Something_id == id.ToString());
            ViewBag.Images = imgs.ToList();
            return PartialView();
        }
       
        public ActionResult Delete_img_block(int id)
        {
            db.Images.Remove(db.Images.First(x1 => x1.Id == id));
            db.SaveChanges();
            //ViewBag.Message = "Удалено";
            return Redirect(Url.Action("Partial_message", "Home", new { message = "Удалено" }));
            //return PartialView();
        }
        public ActionResult Delete_object_from_follow(int id_object,string id_user)
        {
            if (Functions_project.Delete_object_from_follow(id_object, id_user))
                ViewBag.Message = "Удалено";
            else
                ViewBag.Message = "Ошибка";
            return PartialView();

        }
        public ActionResult Delete_object_from_basket(int id_object, string id_user)
        {
            if (Functions_project.Delete_object_from_basket(id_object, id_user))
                ViewBag.Message = "Удалено";
            else
                ViewBag.Message = "Ошибка";
            return PartialView();

        }

        //[Authorize(Roles="admin")]  админ объектам, юзерам юзеры
        //TODO
        [HttpPost]
        public ActionResult Add_new_image(HttpPostedFileBase[] uploadImage, string id, string from)
        {
            var imgs = Get_photo_post(uploadImage);
            foreach (var i in imgs)
            {
                db.Images.Add(new Connect_image() { Something_id = id, What_something = from, Image = i });
                db.SaveChanges();
            }

            return RedirectToAction("Object_view", "Home", new { id = id });
        }


    }
}