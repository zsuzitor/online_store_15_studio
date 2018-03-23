using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using online_store.Models;
using System.IO;

//
using static online_store.Models.Functions_project;
using static online_store.Models.DataBase;
using Microsoft.AspNet.Identity.Owin;
//

//var check_id = System.Web.HttpContext.Current.User.Identity.GetUserId();
//[Authorize(Roles="admin")] [Authorize]


namespace online_store.Controllers
{
    public class HomeController : Controller
    {
        //TODO TEST // не нужно ,просто попробовать
        [AllowAnonymous]
        public ActionResult Test_slider()
        {
            var res = db.Images.Take(10);
            ViewBag.Count_id_slider = res.Count();
            return View(res);
        }

        //главная страница
        [AllowAnonymous]
        public ActionResult Index()
        {
            ViewBag.main_list_slider = new List<Image_link>() { new Image_link() { Path_image="/Content/images/index/index_slider_1.jpg",Action=" ",Controller="Home" },
                new Image_link() { Path_image ="/Content/images/index/index_slider_2.jpg", Action =" ", Controller = "Home" },
                new Image_link() { Path_image ="/Content/images/index/index_slider_3.jpg", Action =" ", Controller = "Home" },
                new Image_link() { Path_image ="/Content/images/index/index_slider_4.jpg", Action =" ", Controller = "Home" },
                new Image_link() { Path_image ="/Content/images/index/index_slider_5.jpg", Action =" ", Controller = "Home" },
                new Image_link() { Path_image ="/Content/images/index/index_slider_6.jpg", Action =" ", Controller = "Home" },
                new Image_link() { Path_image ="/Content/images/index/index_slider_7.jpg", Action =" ", Controller = "Home" } };
            
                ViewBag.count_id_slider_main_index = 7;
            ////////////////////////////////
            var check_id = System.Web.HttpContext.Current.User.Identity.GetUserId<int>();
            bool Show_available_o = false;
            if (check_id != null)
                Show_available_o = db.Users.First(x1 => x1.Id == check_id).Show_available_object;
            ViewBag.Object_for_slider_1 = Search(text_rearch:null, count_return:10, Show_available_object: Show_available_o);
            ViewBag.count_obg_slider_1 = ViewBag.Object_for_slider_1.Count;

            //
            ViewBag.Object_for_slider_2 = Search(text_rearch: null, count_return: 10, Show_available_object: Show_available_o);
            ViewBag.count_obg_slider_2 = ViewBag.Object_for_slider_1.Count;

            return View();
        }
        //отображение формы для отправки и загрузки уже кнопки с поисков
        [AllowAnonymous]
        public ActionResult Menu_search_form()
        {
            // TODO реализовать text_rearch
            //дляя каждого типа своя менюшка
            //ViewBag.text_rearch = text_rearch;
            return PartialView();
        }
        //кнопка с поиском
        [AllowAnonymous]
        public JsonResult Menu_search(string text_rearch = null)
        {
            //TODO реализовать text_rearch
            //дляя каждого типа своя менюшка
            //сформировать меню и вернуть html(и все что нужно) строкой
            //
            string res = "123server_request";
            return Json(res, JsonRequestBehavior.AllowGet);
            //return res;
        }
        //отображение списка объектов дозагржается через ajax
        [AllowAnonymous]
        public ActionResult List_objects(string text_rearch = null,int count_object_from_one_load=10, int count_object_on_page=0)
        {
            var check_id = System.Web.HttpContext.Current.User.Identity.GetUserId<int>();
            bool Show_available_o = false;
            if (check_id != null)
                Show_available_o = db.Users.First(x1 => x1.Id == check_id).Show_available_object;
            List<Object_os_for_view> res = Search(text_rearch, count_skip: count_object_on_page, count_return: count_object_from_one_load, Show_available_object: Show_available_o);
            ViewBag.text_rearch = text_rearch;
            ViewBag.Count_in_list = res.Count;
            return PartialView(res);
        }
        //страница объектов
        [AllowAnonymous]
        public ActionResult List_objects_type(string text_rearch=null,int page=1)
        {
            ViewBag.text_rearch = text_rearch;
            ViewBag.Take_object = 30;//30
            ViewBag.Page = page;
            return View();
        }
        //страница 1 объекта(товара)
        [AllowAnonymous]
        public ActionResult Object_view(int id)
        {
            var check_id = System.Web.HttpContext.Current.User.Identity.GetUserId<int>();
            ViewBag.Person_id = check_id;
            ViewBag.Count_comment_from_one_load = 10;
           var not_res = db.Objects.FirstOrDefault(x1 => x1.Id == id);
            if (not_res == null)
            {
                return RedirectToAction("Not_found_page","Home",new { });
            }
            Object_os_for_view res = new Object_os_for_view(not_res);
            var img = db.Images.Where(x1 => x1.Something_id == id.ToString() && x1.What_something == "Object").ToList();
            res.Images = img;
            ViewBag.count_obg_slider_2 = res.Images.Count;
            var com_person = db.Comments.FirstOrDefault(x1 => x1.Object_id == id &&x1.Person_id== check_id && !string.IsNullOrEmpty(x1.Text));
            //var com_person = com.FirstOrDefault(x1 => x1.Person_id == check_id);
            //TODO определить админ ли зашел и если да передавать true
            if (check_id != null)
            {
                var roles = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>().GetRoles(check_id);
                var check = roles.FirstOrDefault(x1 => x1 == "admin");
                if (check != null)
                    ViewBag.admin = true;
            }
            //TODO убрать
            ViewBag.admin = true;
            
            
            
            if (com_person == null)
                ViewBag.Can_commented = -1;
            else
            {
                ViewBag.Can_commented = com_person.Id;
                
            }
            

            return View(res);
        }
        //добавляет и изменяет коммент
        [HttpPost]
        [Authorize]
        public ActionResult Edit_comment(int id_object, string text, int mark,string from)
        {
            Work_with_comment(id_object, text, mark);
            if(from== "Object_view")
            return RedirectToAction ("Object_view","Home",new {id= id_object });
            else
                return RedirectToAction("Personal_record","Home",new { });
        }
        //добавление комментария
        [Authorize]
        [HttpPost]
        public ActionResult Add_comment(int id_object, string text, int mark)
        {
            Work_with_comment(id_object, text, mark);            
            return RedirectToAction("Object_view", "Home", new { id = id_object });
        }
        //отображение оценки объекта
        [AllowAnonymous]
        public ActionResult Add_mark_for_object(int id)
        {
            
            var check_id = System.Web.HttpContext.Current.User.Identity.GetUserId<int>();
            ViewBag.Id = id;
           
            ViewBag.Mark_pers = 0;
            
            var marks = db.Comments.Where(x1 => x1.Object_id == id && x1.Mark != null).ToList();
            int mark = 0;
            if (marks.Count > 0)         
                mark = (int)(((double)marks.Sum(x1 => x1.Mark)) / marks.Count + 0.5);
            
            ViewBag.Mark = mark;

            if (check_id != null)//не комментить условие, должно быть так
            {
                var mrk = marks.FirstOrDefault(x1 => x1.Person_id == check_id);
                if (mrk != null)
                    ViewBag.Mark_pers = mrk.Mark;
            }

            return PartialView();
        }
        //добавление и изменение оценки объекту
        [Authorize]
        public ActionResult Change_mark_for_object(int id, int num)
        {
            //num,num_block_for_list для работы со списками объектов
            var check_id = System.Web.HttpContext.Current.User.Identity.GetUserId<int>();
            var marks = db.Comments.FirstOrDefault(x1 => x1.Object_id == id && x1.Person_id == check_id);
            if (marks == null)
            {
                db.Comments.Add(new Comment() { Object_id = id, Person_id = check_id, Mark = num });
                db.SaveChanges();
            }
            else
            {
                marks.Mark = num;
                db.SaveChanges();
            }
            return RedirectToAction("Add_mark_for_object", "Home", new { id = id });
        }
        //(страница)отображение одной покупки
        [AllowAnonymous]
        public ActionResult Purchase_view(int id)
        {
            //TODO проверять если ли доступ
            var not_res = db.Purchases.FirstOrDefault(x1=>x1.Id==id);
            var obj = db.Purchases_connect.Where(x1=>x1.Purchase_id==id).Join(db.Objects,x1=>x1.Object_id,x2=>x2.Id,(x1,x2)=>x2).ToList();
            var obj_v = new List<Object_os_for_view>();
            foreach(var i in obj)
            {
                var img = db.Images.FirstOrDefault(x1=>x1.What_something=="Object"&&x1.Something_id==i.Id.ToString());
                var tmp = new Object_os_for_view(i);
                if(img!=null)
                tmp.Images.Add(img);
                obj_v.Add(tmp);
            }
            Purchase_view res = new Purchase_view(not_res);
            res.Objects = obj_v;

            return View(res);
        }
        //страница если совершено "плохое" действие, произошла ошибка и тд
        [AllowAnonymous]
        public ActionResult Not_found_page()
        {

            return View();
        }
        //страница редактировая учетки пользователя
        [Authorize]
        public ActionResult Edit_personal_record()
        {
            var check_id = System.Web.HttpContext.Current.User.Identity.GetUserId<int>();
            var res = db.Users.FirstOrDefault(x1=>x1.Id==check_id);

            return View(res);
        }
        //сохранение изменений учетки пользователя
        [Authorize]
        [HttpPost]
        public ActionResult Edit_personal_record(ApplicationUser a)
        {
            var check_id = System.Web.HttpContext.Current.User.Identity.GetUserId<int>();
            var pers = db.Users.FirstOrDefault(x1=>x1.Id==check_id);
            if (pers != null)
            {
                pers.Eq(a);
                db.SaveChanges();
            }

            return RedirectToAction("Personal_record","Home");
        }
        //страница пользователя
        [AllowAnonymous]
        public ActionResult Personal_record(int id)
        {
            var check_id = System.Web.HttpContext.Current.User.Identity.GetUserId<int>();
            //id = string.IsNullOrEmpty(id) ? check_id : id;//hz mb ostavit tak   
            
            //if (string.IsNullOrEmpty(id))
                
                //return RedirectToAction("Login", "Account", new { });
            ViewBag.Person_id = id;
            var not_res = db.Users.First(x1 => x1.Id == id);
            var res = new Person(not_res);
            ViewBag.count_comment_from_one_load = 5;
            ViewBag.count_purchase_from_one_load = 10;
            if (check_id == id || !res.Db.Private_record)//TODO еще убрать в res некоторые свойства
            {
                if (check_id == id || !res.Db.Private_basket)
                {
                    ViewBag.Baskets = db.Baskets.Where(x1 => x1.Person_id == id).ToList();
                    ViewBag.Baskets.Reverse();
                }
                if (check_id == id || !res.Db.Private_follow)
                {
                    ViewBag.Follow = db.Follow_objects.Where(x1 => x1.Person_id == id).ToList();
                    ViewBag.Follow.Reverse();
                }
                if (check_id == id || (!res.Db.Private_comments&&!res.Db.Private_record))
                {
                    var com = db.Comments.FirstOrDefault(x1 => x1.Person_id == id);
                    if (com != null)
                        ViewBag.have_comments = true;
                }
                if (check_id == id || !res.Db.Private_purchase)
                {
                    var prc = db.Purchases.FirstOrDefault(x1 => x1.Person_id == id);
                    if (prc != null)
                        ViewBag.have_purchase = true;
                }
            }

               


            return View(res);
        }
        //"короткое" отображение покупки
        [AllowAnonymous]
        public ActionResult Purchase_short(string id)
        {


            return PartialView();
        }

        //отображение списка покупок дозагржается через ajax
        [AllowAnonymous]
        public ActionResult Load_purchase_list(int id, int count_purchase_on_page = 0, int count_purchase_from_one_load = 20)
        {
            ViewBag.Person_id = id;
            var res = new List<Purchase>();
            var prc = db.Purchases.Where(x1 => x1.Person_id == id); //tolist hz  
            prc.Reverse();
            res = prc.OrderBy(x1 => x1.Id).Skip(count_purchase_on_page).Take(count_purchase_from_one_load).ToList();
            ViewBag.Count_in_list = res.Count;
            return PartialView(res);

        }
        //отображение списка комментов дозагржается через ajax
        [AllowAnonymous]
        public ActionResult Load_comment_for_personal_record(int id ,int count_comment_on_page= 0, int count_comment_from_one_load = 20)
        {
            var check_id = System.Web.HttpContext.Current.User.Identity.GetUserId<int>();
            var res = new List<Comment_view>();
            //var check_id = System.Web.HttpContext.Current.User.Identity.GetUserId();
            ViewBag.Person_id = id;
            ViewBag.Person_name =db.Users.First(x1=>x1.Id==id).Name;
            List<Comment> com = null;
            com = db.Comments.Where(x1 => x1.Person_id == id && !string.IsNullOrEmpty(x1.Text)).
                OrderBy(x1 => x1.Id).Skip(count_comment_on_page).Take(count_comment_from_one_load).ToList();

            /*
            if (check_id == id)
            {
                 com = db.Comments.Where(x1 => x1.Person_id == id&&!string.IsNullOrEmpty(x1.Text)).OrderBy(x1 => x1.Id).
                 Skip(count_comment_on_page).Take(count_comment_from_one_load).ToList();

            }
            else
            {
                 com = db.Comments.Where(x1 => x1.Person_id == id && !string.IsNullOrEmpty(x1.Text)).
               Join(db.Users, x_1 => x_1.Person_id, x_2 => x_2.Id, (x_1, x_2) => new { com = x_1, pers = x_2 }).
               Where(x1 => !x1.pers.Private_comments && x1.pers.Private_record).Select(x1 => x1.com).OrderBy(x1 => x1.Id)
               .Skip(count_comment_on_page).Take(count_comment_from_one_load).ToList();
            }
           */


            foreach (var i in com)
            {

                var tmp = new Comment_view(i);
                //var obj = db.Objects.FirstOrDefault(x1 => x1.Id == i.Object_id);
               

                var img = db.Images.FirstOrDefault(x1 => x1.What_something == "Object" && x1.Something_id == i.Object_id.ToString());
                if (img != null)
                    tmp.Image_object = img.Image;
                res.Add(tmp);


            }
            ViewBag.Count_in_list = res.Count;
            if(count_comment_on_page==0&& ViewBag.Count_in_list == 0)
            {
                ViewBag.No_comments = true;
            }
            res.Reverse();
            return PartialView(res);
        }



        //partial для отображения, добавления,удаления follow объекта
        //NET//action:(all||follow||basket)
        [Authorize]
        public ActionResult Object_follow(int id, bool? click)
        {
            var check_id = System.Web.HttpContext.Current.User.Identity.GetUserId<int>();
            ViewBag.Id = id;
            ViewBag.Follow = false;
            //ViewBag.action = action;


            //if (!string.IsNullOrEmpty(check_id))
            {
                var foll = db.Follow_objects.FirstOrDefault(x1 => x1.Object_id == id && x1.Person_id == check_id);
                if (foll != null)
                {
                    ViewBag.Follow = true;
                }
                if (click == true)
                {
                    if (ViewBag.Follow == true)
                    {
                        db.Follow_objects.Remove(foll);
                    }
                    else
                    {
                        db.Follow_objects.Add(new Follow_object() { Object_id = id, Person_id = check_id });
                    }
                    db.SaveChanges();
                    ViewBag.Follow = !ViewBag.Follow;
                }
            }

            return PartialView();
        }
        //partial для отображения, добавления,изменения оценки(пользователем) объекта
        [AllowAnonymous]
        public ActionResult Mark_for_comment( int comment_id, bool?click ,int mark=0)
        {
            var check_id = System.Web.HttpContext.Current.User.Identity.GetUserId<int>();
            ViewBag.Person_id = check_id;
            ViewBag.Comment_id = comment_id;
            if (click ==true&& check_id!=null)
            {
                if (mark > 0)
                {
                    var mark_db = db.Mark_for_comment.FirstOrDefault(x1 => x1.Comment_id == comment_id && x1.Person_id == check_id);
                    if (mark_db == null)
                    {
                        mark_db = new Mark_for_comment() { Mark=mark, Person_id=check_id, Comment_id= comment_id };
                        db.Mark_for_comment.Add(mark_db);
                        db.SaveChanges();
                    }
                    else
                    {
                        if (mark_db.Mark == mark)
                        {
                            db.Mark_for_comment.Remove(mark_db);
                            db.SaveChanges();
                        }
                        else
                        {
                            mark_db.Mark = mark;
                        }
                       
                    }
                }

            }
            {
                var mark_f_c = db.Mark_for_comment.Where(x1 => x1.Comment_id == comment_id).ToList();
                ViewBag.Count_good_mark = mark_f_c.Where(x1 =>  x1.Mark == 1).Count();//x1.Comment_id == comment_id &&
                ViewBag.Count_bad_mark = mark_f_c.Where(x1 => x1.Comment_id == comment_id && x1.Mark == 3).Count();//x1.Comment_id == comment_id &&
                ViewBag.Count_funny_mark = mark_f_c.Where(x1 => x1.Comment_id == comment_id && x1.Mark == 2).Count();//x1.Comment_id == comment_id &&
                var person_mark = mark_f_c.FirstOrDefault(x1 =>  x1.Person_id == check_id);//x1.Comment_id == comment_id &&
                ViewBag.person_mark = person_mark == null ? "" : person_mark.Mark.ToString();
            }
            
           

            return PartialView();
        }
        
            [Authorize]
        public ActionResult Delete_basket_havent()
        {
            var check_id = System.Web.HttpContext.Current.User.Identity.GetUserId<int>();
            var res = db.Baskets.Where(x1 => x1.Person_id == check_id).ToList();
            for(var i=0;i< res.Count; ++i)
            {
                var tmp_id = res[i].Object_id;
                var obj = db.Objects.First(x1 => x1.Id == tmp_id);
                if (obj.Remainder < 1)
                {
                    db.Baskets.Remove(res[i]);
                }

            }
            db.SaveChanges();
            return RedirectToAction("Basket_page", "Home",new { });
        }

        //метод активации купонов
        [Authorize]
        public ActionResult Coupons_activated(string name)
        {
            var check_id = System.Web.HttpContext.Current.User.Identity.GetUserId<int>();
            var coupon = db.Discount_type.FirstOrDefault(x1 => x1.Name == name);
            
            if (coupon != null)
            {
                var cp=coupon.Create_coupon(check_id);
                db.Discount_coupon.Add(cp);
                db.SaveChanges();
                ViewBag.message = "Купон активирован";
            }
            else
            {
                ViewBag.message = "Купон не активирован";
            }
            return RedirectToAction("Coupons_page", "Home",new { });
            //return View();
        }


        //страница отображения/активации купонов
        [Authorize]
        public ActionResult Coupons_page()
        {
            var check_id = System.Web.HttpContext.Current.User.Identity.GetUserId<int>();
            var coupons = db.Discount_coupon.Where(x1 => x1.User_id == check_id).ToList();
            ViewBag.coupons_left = coupons.Where(x1 => x1.Spent == false).ToList();
            ViewBag.coupons_spent = coupons.Where(x1 => x1.Spent == true).ToList();
            return View();
        }
        [Authorize]
        public ActionResult Notification_object(int id,bool click=false)
        {
            var check_id = System.Web.HttpContext.Current.User.Identity.GetUserId<int>();
            ViewBag.id = id;
            var not=db.Object_notification.FirstOrDefault(x1 => x1.User_id == check_id && x1.Object_id == id);
            if (not == null)
            {
                if (click)
                {
                    db.Object_notification.Add(new Object_notification() { User_id = check_id, Object_id = id });
                    db.SaveChanges();
                    ViewBag.In_notification = true;
                }
                else
                    ViewBag.In_notification = false;
            }
            else
            {
                if (click)
                {
                    db.Object_notification.Remove(not);
                    db.SaveChanges();
                    ViewBag.In_notification = false;
                }
                else
                    ViewBag.In_notification = true;


            }

            //ViewBag.In_notification =true;
            return PartialView();
        }
        
                //страница отображения корзины
                [Authorize]
        public ActionResult Basket_page()
        {
            var check_id = System.Web.HttpContext.Current.User.Identity.GetUserId<int>();
            var res = db.Baskets.Where(x1 => x1.Person_id == check_id).ToList();//.Join(db.Objects,x1=>x1.Object_id,x2=>x2.Id,(x1,x2)=>x2);     //ToList() hz
            var summ_1 = res.Join(db.Objects, x1 => x1.Object_id, x2 => x2.Id, (x1, x2) => x2).ToList();
            bool error = false;
            if (summ_1.Where(x1 => x1.Remainder < 1).Count()>0)
            {
                error = true;
               
            }
            summ_1 = summ_1.Where(x1 => x1.Remainder > 0).ToList();
            foreach(var i in res)
            {
                var tmp_obj=summ_1.First(x1 => x1.Id == i.Object_id);
                if (tmp_obj.Remainder < i.Count_obj)
                {
                    error = true;
                    //summ_1.Remove(tmp_obj);
                }

            }


            if(error)
                ViewBag.Havent_message = "В вашей корзине присутствуют предметы, которых на данный момент нет в наличии, перед подтверждением покупки, их необходимо удалить";

            ViewBag.All_price = summ_1.Sum(x1 =>
            {
                var bsk=res.First(x2 => x2.Object_id == x1.Id).Count_obj;
                if (bsk >= x1.Remainder)
                {
                    return bsk * x1.Price;
                }
                return x1.Remainder * x1.Price;
            });//x1.Price
            ViewBag.All_price_small = summ_1.Sum(x1 =>
            {
            var tmp = x1.Price;
            var count = res.First(x2 => x2.Object_id == x1.Id).Count_obj;
            count = count >= x1.Remainder ? count :x1.Remainder;
            tmp *= count* (1 - x1.Discount);
        //((int)(x1.Price * res.First(x2 => x2.Object_id == x1.Id).Count_obj * (1 - x1.Discount))));
            return (int)tmp;
            });
           
            ViewBag.obj_list_id = res.Select(x1 => x1.Object_id);
            ViewBag.coupons = db.Discount_coupon.Where(x1 => x1.User_id == check_id&&x1.Spent==false).ToList();

            return View();
        }
        //TODO
        //действия на сервере которые происходят после нажатия пользователем "купить все" в его корзине
        [Authorize]
        public ActionResult Buy_basket()
        {
            bool error = false;
            var check_id = System.Web.HttpContext.Current.User.Identity.GetUserId<int>();
            var bsk = db.Baskets.Where(x1 => x1.Person_id == check_id).ToList();
            var bsk_obj = bsk.Join(db.Objects,x1=>x1.Object_id,x2=>x2.Id,(x1,x2)=>x2).ToList();
            int price = 0;
            foreach(var i in bsk)
            {
                var obj = bsk_obj.First(x1 => x1.Id == i.Object_id);
                if (obj.Remainder < i.Count_obj)
                {
                    error = true;
                    break;
                }
                price += (int)(obj.Price* (1 - obj.Discount) * i.Count_obj);


            }
            if (!error)
            {
                var prc = new Purchase() { Person_id = check_id, Price = price };
                db.Purchases.Add(prc);
                db.SaveChanges();

                foreach (var i in bsk)
                {
                    //TODO
                    var obj = bsk_obj.First(x1 => x1.Id == i.Object_id);
                    var tmp = new Purchase_connect() {Count_object=i.Count_obj, Purchase_id = prc.Id, Object_id = i.Object_id, Price = ((int)(obj.Price * (1 - obj.Discount)*i.Count_obj)) };
                    db.Purchases_connect.Add(tmp);
                    db.SaveChanges();

                    
                    obj.Count_buy += i.Count_obj;
                    obj.Remainder -= i.Count_obj;
                    db.SaveChanges();

                }
                db.Baskets.RemoveRange(db.Baskets.Where(x1 => x1.Person_id == check_id));
                db.SaveChanges();

               
            }
            else
            {
                ViewBag.Error_message = "в корзине присутствуют объекты которых нет в наличии, их необходимо удалить";
            }

            
            return View();
        }
        //частичное отображение 1 объекта для корзины
        [Authorize]
        //[ChildActionOnly]
        public ActionResult Basket_one_object_partial(int id)
        {
            var check_id = System.Web.HttpContext.Current.User.Identity.GetUserId<int>();
            var imgs = db.Images.Where(x1 => x1.What_something == "Object" && x1.Something_id == id.ToString()).ToList();
            var obj = db.Objects.First(x1 => x1.Id == id);
            var count = db.Baskets.First(x1=>x1.Person_id== check_id&&x1.Object_id==id);
            var res = new Object_os_for_view(obj) { Images = imgs,Count= count.Count_obj };

            ViewBag.Have = true;
            if (res.Db.Remainder < 1 || res.Count > res.Db.Remainder)
                ViewBag.Have = false;

            return PartialView(res);
        }
       //частичное отображение объекта который зафоловил человек  --(personal_record)
        [Authorize]
        public ActionResult Follow_one_object_partial(int id)
        {
            var imgs = db.Images.Where(x1 => x1.What_something == "Object" && x1.Something_id == id.ToString()).ToList();
            var obj = db.Objects.First(x1 => x1.Id == id);
            var res = new Object_os_for_view(obj) { Images = imgs };
            return PartialView(res);
        }
        //для объектов без параметров(цвета размера и тдтд и отображения в списках)
        //partial для отображения, добавления,удаления  объекта в\из корзины
        [Authorize]
        public ActionResult Object_add_basket(int id, bool? click)
        {
            ViewBag.Id = id;
            ViewBag.Count_obj = 0;
            var obj = db.Objects.FirstOrDefault(x1=>x1.Id==id);
            if (obj != null)
            {
                ViewBag.Count_obj = obj.Remainder;//
            }
            if (ViewBag.Count_obj != 0)
            {
                var check_id = System.Web.HttpContext.Current.User.Identity.GetUserId<int>();
                ViewBag.InBasket = false;
                //if (!string.IsNullOrEmpty(check_id))
                //{
                    var bask = db.Baskets.FirstOrDefault(x1 => x1.Object_id == id && x1.Person_id == check_id);
                    if (bask != null)
                    {
                        ViewBag.InBasket = true;
                    }
                    if (click == true)
                    {
                        if (ViewBag.InBasket == true)
                        {
                            db.Baskets.Remove(bask);
                        }
                        else
                        {
                            db.Baskets.Add(new Connect_basket() { Object_id = id, Person_id = check_id });
                        }
                        db.SaveChanges();
                        ViewBag.InBasket = !ViewBag.InBasket;
                    }
                //}
            }
            


            return PartialView();
        }
        //для отображения блока добавления в корзину на странице объекта(с параметрами)
        [Authorize]
        public ActionResult Object_add_basket_form_partial(int id)
        {
            ViewBag.object_id = id;

            return PartialView();
        }
        //параметры с которым добавлять объект(цвет размер и тдтд)
        [HttpPost]
        [Authorize]
        public ActionResult Object_add_basket_form(int id,int count=1)
        {
            var res = "";
            if (count <= 0)
                count = 1;


                int Count_obj = 0;
                var obj = db.Objects.FirstOrDefault(x1 => x1.Id == id);
                if (obj != null)
                {
                    Count_obj = obj.Remainder;//
                }
                if (Count_obj >= count)
                {
                    var check_id = System.Web.HttpContext.Current.User.Identity.GetUserId<int>();


                    var bask = db.Baskets.FirstOrDefault(x1 => x1.Object_id == id && x1.Person_id == check_id);
                    if (bask != null)
                    {
                        bask.Count_obj += count;
                    }
                    else
                    {
                        db.Baskets.Add(new Connect_basket() { Object_id = id, Count_obj = count, Person_id = check_id });
                    }
                    db.SaveChanges();
                    res = "Успешно добавлено";


                }
                else
                {
                    res = "данного объекта в таком колличестве нет";
                    //TODO сообщение что столько объектов нет и передать колличество
                }
            
           /* else
            {
                res = "Выберите отличное от 0 значение";
            }*/
            return Redirect(Url.Action("Partial_message", "Home", new { message = res }));
        }
        //удаление объекта из корзины
        [Authorize]
        public ActionResult Delete_object_from_basket(int id,int count_delete = -1)//-1 удалить все объекты  таким id
        {
            var check_id = System.Web.HttpContext.Current.User.Identity.GetUserId<int>();
            var res = "";
            if (Functions_project.Delete_object_from_basket(id,check_id, count_delete))
            {
                //ViewBag.Message = "Удалено";
                res = "Удалено";
            }
            else
                res = "Ошибка";
            var count_rem = db.Baskets.FirstOrDefault(x1=>x1.Person_id==check_id&&x1.Object_id==id);
            
            if(count_rem==null)
                return Redirect(Url.Action("Partial_message", "Home", new { message = res }));
            else

                return Redirect(Url.Action("Basket_one_object_partial", "Home", new { id = id }));
            //ViewBag.Message = "Ошибка";

        }
        //удаление объекта из follow
        [Authorize]
        public ActionResult Delete_object_from_follow(int id)
        {
            var check_id = System.Web.HttpContext.Current.User.Identity.GetUserId<int>();
            var res = "";
            if(Functions_project.Delete_object_from_follow(id,check_id))
                res = "Удалено";
            else
                res = "Ошибка";
            return Redirect(Url.Action("Partial_message", "Home", new { message = res }));

        }
        

        //удаление комментария
        [Authorize]
        public ActionResult Delete_Comment(int id)
        {
            var check_id = System.Web.HttpContext.Current.User.Identity.GetUserId<int>();
            ViewBag.Person_id = check_id;
            ViewBag.comment_id = id;
            ViewBag.delete = false;
            var com = db.Comments.FirstOrDefault(x1 => x1.Id == id);
            
            if (com != null)
            {
                if (com.Person_id == check_id)
                {
                    db.Comments.Remove(com);
                    db.SaveChanges();
                    ViewBag.Message = "Удалено";
                    ViewBag.delete = true;
                }
                else
                {
                    var roles = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>().GetRoles(check_id);
                    var check = roles.FirstOrDefault(x1 => x1 == "employee");
                    if (check != null)
                    {
                        db.Comments.Remove(com);
                        db.SaveChanges();
                        ViewBag.Message = "Удалено";
                        ViewBag.delete = true;
                    }
                        
                        ViewBag.Message = "Неудача";
                   
                }
            }
           
            return PartialView();
        }
        //отображение списка комментов дозагржается через ajax
        [AllowAnonymous]
        public ActionResult Load_comment_for_object_view(int object_id,int count_comment_on_page=0,int count_comment_from_one_load=20, int com_us_id=-1)
        {
            var res = new List<Comment_view>();
            var check_id = System.Web.HttpContext.Current.User.Identity.GetUserId<int>();
            ViewBag.Person_id = check_id;
            var com = db.Comments.Where(x1 => x1.Object_id == object_id && !string.IsNullOrEmpty(x1.Text)).
                Join(db.Users, x_1=>x_1.Person_id,x_2=>x_2.Id,(x_1,x_2)=>new { com = x_1,pers=x_2 }).
                Where(x1=>!x1.pers.Private_comments&& x1.pers.Private_record).Select(x1=>x1.com).OrderBy(x1 => x1.Id)
                .Skip(count_comment_on_page).Take(count_comment_from_one_load).ToList();
           
            //TODO определить админ ли зашел и если да передавать true
           
           
            
            foreach (var i in com)
            {
                if (i.Person_id != check_id)
                {
                    var user = db.Users.FirstOrDefault(x1 => x1.Id == i.Person_id);
                    if (user != null)
                    {
                        var tmp = new Comment_view(i) { Image_user = user.Image, User_name = user.Name };
                        
                        res.Add(tmp);
                    }
                    
                }
            }
            if (com_us_id > 0&& count_comment_on_page==0)
            {
                var user = db.Users.First(x1 => x1.Id == check_id);
                var pers_com = db.Comments.FirstOrDefault(x1 => x1.Id == com_us_id);
                if (pers_com != null)
                {
                    var tmp = new Comment_view(pers_com) { Image_user = user.Image, User_name = user.Name };
                    
                    res.Add(tmp);
                }
               
            }
            res.Reverse();
            ViewBag.Count_in_list = res.Count;
            if (count_comment_on_page == 0 && ViewBag.Count_in_list == 0)
            {
                ViewBag.No_comments = true;
            }
            return PartialView(res);

        }



        //загрузка фотографии профиля человека
        [Authorize]
        [HttpPost]
        public ActionResult Add_new_main_image(HttpPostedFileBase[] uploadImage)
        {
            var check_id = System.Web.HttpContext.Current.User.Identity.GetUserId<int>();
            var imgs = Get_photo_post(uploadImage);
            var pers = db.Users.FirstOrDefault(x1 => x1.Id == check_id);
            if (pers != null)
            {
                pers.Image = imgs[0];
                db.SaveChanges();
            }

            return RedirectToAction("Personal_record", "Home", new { id = check_id });
        }




       


        //-----------------------------------
       
        public ActionResult Main_present_block_save(Follow_email a)
        {
            //TODO проверять есть ли в бд такая почта?
            var check_id = System.Web.HttpContext.Current.User.Identity.GetUserId<int>();
            var em = db.Follow_email.FirstOrDefault(x1=>x1.User_id==check_id||x1.Email==a.Email);
            if (em == null)
            {
                a.User_id = check_id;
                db.Follow_email.Add(a);
                db.SaveChanges();
                //
                var dis = db.Discount_type.FirstOrDefault(x1 => x1.Name == "Follow_mail");

                if (dis != null)
                {
                    db.Discount_coupon.Add(new Discount_coupon() { Discount_id = dis.Id, User_id = check_id });
                    db.SaveChanges();
                    ViewBag.massage_activated_coupon = "Купон выдан";
                }
                
            }
            else
            {
                ViewBag.massage_activated_coupon ="Скидка на данную учетную зпись/почту уже была выдана";
            }

            return RedirectToAction("Coupons_page", "Home",new{ });
            //return PartialView();
        }
        [ChildActionOnly]
        public ActionResult Admin_page_link()
        {
            var check_id = System.Web.HttpContext.Current.User.Identity.GetUserId<int>();

            ViewBag.admin = false;
            if (check_id != null)
            {
                var roles=HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>().GetRoles(check_id);
                var check=roles.FirstOrDefault(x1=>x1=="admin");
                if(check!=null)
                ViewBag.admin = true;
            }
            //TODO убрать строчку
            ViewBag.admin = true;
            //

            return PartialView();
        }


        [ChildActionOnly]
        public ActionResult Main_present_block()
        {
            var check_id = System.Web.HttpContext.Current.User.Identity.GetUserId<int>();
            //TODO --int
            //if (string.IsNullOrEmpty(check_id))
            //{
                //ViewBag.check = null;
            //}
            //else
           // {
                var t = db.Follow_email.FirstOrDefault(x1 => x1.User_id == check_id);
                ViewBag.check = false;
                if (t == null)
                    ViewBag.check = true;
            //}
           
           
           
            return PartialView();
        }

        [HttpPost]
        public ActionResult Main_help_block(Application_phone a)
        {
            //var res = new Application_phone();
            db.Application_phone_comm.Add(a);
            db.SaveChanges();
            //return PartialView();
            return RedirectToAction("Index", "Home", new { });
            return Redirect(Url.Action("Index", "Home",new { }));
        }

        //[ChildActionOnly]
        public ActionResult Main_help_block()
        {
            //var res = new Application_phone();

            return PartialView();
        }
        public ActionResult Partial_message(string message)
        {
            ViewBag.message = message;

            return PartialView();
        }

        [ChildActionOnly]
        public ActionResult Main_footer()
        {


            return PartialView();
        }
        [ChildActionOnly]
        public ActionResult Main_header()
        {
            /*
            ViewBag.List_class_for_header = new string[] { "Женщинам", "Мужчинам", "Детям", "Обувь", "Аксессуары", "Книги и диски", "Дом и дача", "Спорт",
            "Игрушки", "Красота", "Новинки", "Электроника", "Ювелирные украшения", "Premium", "Подарки", "Зима"
            };
        var block = new {
                Name = "Женщинам",line1= "Обувь",
                line2 = "Аксессуары",
                line3 = "Спорт",
                line4 = "Игрушки",
                line5 = "Красота",
                line6 = "Новинки",
                line7 = "Ювелирные украшения",
                 line8 = "Premium",
                line9 = "Подарки",
                line10 = "Зима"
            };
            var beatlesList =
                       (new[] { block }).ToList();


            ViewBag.List_class_for_header = beatlesList;*/
            try
            {

                var res = new List<Section_main_header_view>();
            var sect = db.Section_in_main_header.ToList();
            if (sect == null || sect.Count == 0)
            {
                var tmp_woman = new Section_main_header() {Name= "Женщинам"};
                db.Section_in_main_header.Add(tmp_woman);
                    db.SaveChanges();
                    db.Section_in_main_header_link.Add(new Section_main_header_link() { Section_Id= tmp_woman.Id, Line_name= "Аксессуары", Line_link_action="#", Line_link_controller="Home" });
                    db.Section_in_main_header_link.Add(new Section_main_header_link() { Section_Id = tmp_woman.Id, Line_name = "Спорт", Line_link_action = "#", Line_link_controller = "Home" });
                    db.Section_in_main_header_link.Add(new Section_main_header_link() { Section_Id = tmp_woman.Id, Line_name = "Игрушки", Line_link_action = "#", Line_link_controller = "Home" });
                    db.Section_in_main_header_link.Add(new Section_main_header_link() { Section_Id = tmp_woman.Id, Line_name = "Красота", Line_link_action = "#", Line_link_controller = "Home" });
                    db.Section_in_main_header_link.Add(new Section_main_header_link() { Section_Id = tmp_woman.Id, Line_name = "Новинки", Line_link_action = "#", Line_link_controller = "Home" });
                    db.Section_in_main_header_link.Add(new Section_main_header_link() { Section_Id = tmp_woman.Id, Line_name = "Ювелирные украшения", Line_link_action = "#", Line_link_controller = "Home" });
                    db.Section_in_main_header_link.Add(new Section_main_header_link() { Section_Id = tmp_woman.Id, Line_name = "Premium", Line_link_action = "#", Line_link_controller = "Home" });
                    db.Section_in_main_header_link.Add(new Section_main_header_link() { Section_Id = tmp_woman.Id, Line_name = "Подарки", Line_link_action = "#", Line_link_controller = "Home" });
                    db.Section_in_main_header_link.Add(new Section_main_header_link() { Section_Id = tmp_woman.Id, Line_name = "Зима", Line_link_action = "#", Line_link_controller = "Home" });
                    db.SaveChanges();
                    

                    //
                    var tmp_man = new Section_main_header(){Name = "Мужчинам"};
                db.Section_in_main_header.Add(tmp_man);

                    db.SaveChanges();
                    db.Section_in_main_header_link.Add(new Section_main_header_link() { Section_Id = tmp_man.Id, Line_name = "Аксессуары", Line_link_action = "#", Line_link_controller = "Home" });
                    db.Section_in_main_header_link.Add(new Section_main_header_link() { Section_Id = tmp_man.Id, Line_name = "Спорт", Line_link_action = "#", Line_link_controller = "Home" });
                    db.Section_in_main_header_link.Add(new Section_main_header_link() { Section_Id = tmp_man.Id, Line_name = "Игрушки", Line_link_action = "#", Line_link_controller = "Home" });
                    db.Section_in_main_header_link.Add(new Section_main_header_link() { Section_Id = tmp_man.Id, Line_name = "Красота", Line_link_action = "#", Line_link_controller = "Home" });
                    db.Section_in_main_header_link.Add(new Section_main_header_link() { Section_Id = tmp_man.Id, Line_name = "Новинки", Line_link_action = "#", Line_link_controller = "Home" });
                    db.Section_in_main_header_link.Add(new Section_main_header_link() { Section_Id = tmp_man.Id, Line_name = "Ювелирные украшения", Line_link_action = "#", Line_link_controller = "Home" });
                    db.Section_in_main_header_link.Add(new Section_main_header_link() { Section_Id = tmp_man.Id, Line_name = "Premium", Line_link_action = "#", Line_link_controller = "Home" });
                    db.Section_in_main_header_link.Add(new Section_main_header_link() { Section_Id = tmp_man.Id, Line_name = "Подарки", Line_link_action = "#", Line_link_controller = "Home" });
                    db.Section_in_main_header_link.Add(new Section_main_header_link() { Section_Id = tmp_man.Id, Line_name = "Зима", Line_link_action = "#", Line_link_controller = "Home" });
                    db.SaveChanges();
                    

                    var tmp_1 = new Section_main_header()
                {
                    Name = "Детям"
                };
                db.Section_in_main_header.Add(tmp_1);
                var tmp_2 = new Section_main_header()
                {
                    Name = "Обувь"
                };
                db.Section_in_main_header.Add(tmp_2);
                var tmp_3 = new Section_main_header()
                {
                    Name = "Аксессуары"
                };
                db.Section_in_main_header.Add(tmp_3);
                var tmp_4 = new Section_main_header()
                {
                    Name = "Книги и диски"
                };
                db.Section_in_main_header.Add(tmp_4);
                var tmp_5 = new Section_main_header()
                {
                    Name = "Дом и дача"
                };
                db.Section_in_main_header.Add(tmp_5);
                var tmp_6 = new Section_main_header()
                {
                    Name = "Спорт"
                };
                db.Section_in_main_header.Add(tmp_6);
                var tmp_7 = new Section_main_header()
                {
                    Name = "Игрушки"
                };
                db.Section_in_main_header.Add(tmp_7);
                var tmp_8 = new Section_main_header()
                {
                    Name = "Красота"
                };
                db.Section_in_main_header.Add(tmp_8);
                var tmp_9 = new Section_main_header()
                {
                    Name = "Новинки"
                };
                db.Section_in_main_header.Add(tmp_9);
                var tmp_10 = new Section_main_header()
                {
                    Name = "Электроника"
                };
                db.Section_in_main_header.Add(tmp_10);
                var tmp_11 = new Section_main_header()
                {
                    Name = "Ювелирные украшения"
                };
                db.Section_in_main_header.Add(tmp_11);
                var tmp_12 = new Section_main_header()
                {
                    Name = "Premium"
                };
                db.Section_in_main_header.Add(tmp_12);
                var tmp_13 = new Section_main_header()
                {
                    Name = "Подарки"
                };
                db.Section_in_main_header.Add(tmp_13);
                var tmp_14 = new Section_main_header()
                {
                    Name = "Зима"
                };
                db.Section_in_main_header.Add(tmp_14);




                db.SaveChanges();
                    
            }
                var names = db.Section_in_main_header.ToList();
                foreach(var i in names)
                res.Add(new Section_main_header_view() { Section = i, Link = db.Section_in_main_header_link.Where(x1 => x1.Section_Id == i.Id).ToList() });



                ViewBag.List_class_for_header = res;
            }
            catch { }
            return PartialView();
        }

     

        
    }
}