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
                
           

            ViewBag.Object_for_slider_1 = Search(text_rearch:null, count_return:10);
            ViewBag.count_obg_slider_1 = ViewBag.Object_for_slider_1.Count;

            return View();
        }
        //отображение списка объектов дозагржается через ajax
        [AllowAnonymous]
        public ActionResult List_objects(string text_rearch = null,int count_object_from_one_load=10, int count_object_on_page=0)
        {
            List<Object_os_for_view> res = Search(text_rearch, count_skip: count_object_on_page, count_return: count_object_from_one_load);
            ViewBag.text_rearch = text_rearch;
            ViewBag.Count_in_list = res.Count;
            return PartialView(res);
        }
        //страница объектов
        [AllowAnonymous]
        public ActionResult List_objects_type(string text_rearch=null)
        {
            ViewBag.text_rearch = text_rearch;
            ViewBag.Take_object = 30;//30
            return View();
        }
        //страница 1 объекта(товара)
        [AllowAnonymous]
        public ActionResult Object_view(int id)
        {
            var check_id = System.Web.HttpContext.Current.User.Identity.GetUserId();
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
            
            var check_id = System.Web.HttpContext.Current.User.Identity.GetUserId();
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
            var check_id = System.Web.HttpContext.Current.User.Identity.GetUserId();
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
            var check_id = System.Web.HttpContext.Current.User.Identity.GetUserId();
            var res = db.Users.FirstOrDefault(x1=>x1.Id==check_id);

            return View(res);
        }
        //сохранение изменений учетки пользователя
        [Authorize]
        [HttpPost]
        public ActionResult Edit_personal_record(ApplicationUser a)
        {
            var check_id = System.Web.HttpContext.Current.User.Identity.GetUserId();
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
        public ActionResult Personal_record(string id)
        {
           
            id = string.IsNullOrEmpty(id) ? System.Web.HttpContext.Current.User.Identity.GetUserId() : id;//hz mb ostavit tak   
            if (string.IsNullOrEmpty(id))
                return RedirectToAction("Not_found_page", "Home",new { });
            ViewBag.Person_id = id;
            var not_res = db.Users.First(x1 => x1.Id == id);
            var res = new Person(not_res);
            ViewBag.count_comment_from_one_load = 1;
            ViewBag.count_purchase_from_one_load = 10; 
            ViewBag.Baskets = db.Baskets.Where(x1 => x1.Person_id == id).ToList();
            ViewBag.Baskets.Reverse();

            ViewBag.Follow = db.Follow_objects.Where(x1 => x1.Person_id == id).ToList();
            ViewBag.Follow.Reverse();

            var com = db.Comments.FirstOrDefault(x1 => x1.Person_id == id);
            if (com != null)
                ViewBag.have_comments = true;
            var prc = db.Purchases.FirstOrDefault(x1 => x1.Person_id == id);
            if (prc != null)
                ViewBag.have_purchase = true;


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
        public ActionResult Load_purchase_list(string id, int count_purchase_on_page = 0, int count_purchase_from_one_load = 20)
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
        public ActionResult Load_comment_for_personal_record(string id ,int count_comment_on_page= 0, int count_comment_from_one_load = 20)
        {
            
            var res = new List<Comment_view>();
            //var check_id = System.Web.HttpContext.Current.User.Identity.GetUserId();
            ViewBag.Person_id = id;
            ViewBag.Person_name =db.Users.First(x1=>x1.Id==id).Name;
            var com = db.Comments.Where(x1 => !string.IsNullOrEmpty(x1.Text)).OrderBy(x1 => x1.Id).Skip(count_comment_on_page).Take(count_comment_from_one_load).ToList();




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
        [Authorize]
        public ActionResult Object_follow(int id, bool? click)
        {
            var check_id = System.Web.HttpContext.Current.User.Identity.GetUserId();
            ViewBag.Id = id;
            ViewBag.Follow = false;
            
            if (!string.IsNullOrEmpty(check_id))
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
            var check_id = System.Web.HttpContext.Current.User.Identity.GetUserId();
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
            var check_id = System.Web.HttpContext.Current.User.Identity.GetUserId();
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
            var check_id = System.Web.HttpContext.Current.User.Identity.GetUserId();
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
            var check_id = System.Web.HttpContext.Current.User.Identity.GetUserId();
            var coupons = db.Discount_coupon.Where(x1 => x1.User_id == check_id).ToList();
            ViewBag.coupons_left = coupons.Where(x1 => x1.Spent == false).ToList();
            ViewBag.coupons_spent = coupons.Where(x1 => x1.Spent == true).ToList();
            return View();
        }
        //страница отображения корзины
        [Authorize]
        public ActionResult Basket_page()
        {
            var check_id = System.Web.HttpContext.Current.User.Identity.GetUserId();
            var res = db.Baskets.Where(x1 => x1.Person_id == check_id).ToList();//.Join(db.Objects,x1=>x1.Object_id,x2=>x2.Id,(x1,x2)=>x2);     //ToList() hz
            var summ_1 = res.Join(db.Objects, x1 => x1.Object_id, x2 => x2.Id, (x1, x2) => x2).ToList();
            
            if (summ_1.Where(x1 => x1.Remainder < 1).Count()>0)
            {
                ViewBag.Havent_message = "В вашей корзине присутствуют предметы, которых на данный момент нет в наличии, перед подтверждением покупки, их необходимо удалить";
            }
            summ_1 = summ_1.Where(x1 => x1.Remainder > 0).ToList();
            ViewBag.All_price = summ_1.Sum(x1 => x1.Price);
            ViewBag.All_price_small = summ_1.Sum(x1 => ((int)(x1.Price * (1 - x1.Discount))));
           
            ViewBag.obj_list_id = res.Select(x1 => x1.Object_id);
            ViewBag.coupons = db.Discount_coupon.Where(x1 => x1.User_id == check_id&&x1.Spent==false).ToList();

            return View();
        }
        //TODO
        //действия на сервере которые происходят после нажатия пользователем "купить все" в его корзине
        [Authorize]
        public ActionResult Buy_basket()
        {
            var check_id = System.Web.HttpContext.Current.User.Identity.GetUserId();
            var bsk_obj = db.Baskets.Where(x1=>x1.Person_id==check_id).Join(db.Objects,x1=>x1.Object_id,x2=>x2.Id,(x1,x2)=>x2).ToList();
            if (bsk_obj.Count > 0)
            {
                var havent = bsk_obj.Where(x1 => x1.Remainder < 1);
                if (havent.Count() == 0)
                {
                    var prc = new Purchase() { Person_id = check_id, Price = bsk_obj.Sum(x1 => (((int)(x1.Price * (1 - x1.Discount))))) };//мб в цикле прибалять
                    db.Purchases.Add(prc);
                    db.SaveChanges();
                    foreach (var i in bsk_obj)
                    {
                        var tmp = new Purchase_connect() { Purchase_id = prc.Id, Object_id = i.Id, Price = ((int)(i.Price * (1 - i.Discount))) };
                        db.Purchases_connect.Add(tmp);
                        db.SaveChanges();

                        var obj = db.Objects.First(x1 => x1.Id == i.Id);
                        obj.Count_buy += 1;
                        obj.Remainder -= 1;
                        db.SaveChanges();

                    }
                    db.Baskets.RemoveRange(db.Baskets.Where(x1 => x1.Person_id == check_id));
                    db.SaveChanges();
                    //TODO у всех объектов сделать количество -1
                }
                else
                {
                    ViewBag.Error_message = "в корзине присутствуют объекты которых нет в наличии, их необходимо удалить";
                }

            }
            return View();
        }
        //частичное отображение 1 объекта для корзины
        [Authorize]
        [ChildActionOnly]
        public ActionResult Basket_one_object_partial(int id)
        {
            var check_id = System.Web.HttpContext.Current.User.Identity.GetUserId();
            var imgs = db.Images.Where(x1 => x1.What_something == "Object" && x1.Something_id == id.ToString()).ToList();
            var obj = db.Objects.First(x1 => x1.Id == id);
            var count = db.Baskets.First(x1=>x1.Person_id== check_id&&x1.Object_id==id);
            var res = new Object_os_for_view(obj) { Images = imgs,Count= count.Count_obj };
            return PartialView(res);
        }
       //часточное отображение объекта который зафоловил человек  --(personal_record)
        [Authorize]
        public ActionResult Follow_one_object_partial(int id)
        {
            var imgs = db.Images.Where(x1 => x1.What_something == "Object" && x1.Something_id == id.ToString()).ToList();
            var obj = db.Objects.First(x1 => x1.Id == id);
            var res = new Object_os_for_view(obj) { Images = imgs };
            return PartialView(res);
        }
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
                var check_id = System.Web.HttpContext.Current.User.Identity.GetUserId();
                ViewBag.InBasket = false;
                if (!string.IsNullOrEmpty(check_id))
                {
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
                }
            }
            


            return PartialView();
        }
        //удаление объекта из корзины
        [Authorize]
        public ActionResult Delete_object_from_basket(int id)
        {
            var check_id = System.Web.HttpContext.Current.User.Identity.GetUserId();
            var res = "";
            if (Functions_project.Delete_object_from_basket(id,check_id))
            {
                //ViewBag.Message = "Удалено";
                res = "Удалено";
            }
            else
                res = "Ошибка";
            //ViewBag.Message = "Ошибка";
            return Redirect(Url.Action("Partial_message", "Home",new { message=res }));
        }
        //удаление объекта из follow
        [Authorize]
        public ActionResult Delete_object_from_follow(int id)
        {
            var check_id = System.Web.HttpContext.Current.User.Identity.GetUserId();
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
            var check_id = System.Web.HttpContext.Current.User.Identity.GetUserId();
            ViewBag.Person_id = check_id;
            var com = db.Comments.FirstOrDefault(x1 => x1.Id == id);
            Comment_view res = null;
            if (com != null)
            {
                if (com.Person_id == check_id)
                {
                    db.Comments.Remove(com);
                    db.SaveChanges();
                    ViewBag.Message = "Удалено";
                }
                else
                {
                    //TODO хз может убрать(просто отображение пустого блока с сообщением) или доделать там оценки комментов
                    ViewBag.Message = "Удалить невозможно";
                    var user = db.Users.First(x1 => x1.Id == com.Person_id);
                    res = new Comment_view(com) { Image_user = user.Image, User_name = user.Name };
                }
            }

            return PartialView(res);
        }
        //отображение списка комментов дозагржается через ajax
        [AllowAnonymous]
        public ActionResult Load_comment_for_object_view(int object_id,int count_comment_on_page=0,int count_comment_from_one_load=20, int com_us_id=-1)
        {
            var res = new List<Comment_view>();
            var check_id = System.Web.HttpContext.Current.User.Identity.GetUserId();
            ViewBag.Person_id = check_id;
            var com = db.Comments.Where(x1 => x1.Object_id == object_id && !string.IsNullOrEmpty(x1.Text)).OrderBy(x1 => x1.Id)
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
            var check_id = System.Web.HttpContext.Current.User.Identity.GetUserId();
            var imgs = Get_photo_post(uploadImage);
            var pers = db.Users.FirstOrDefault(x1 => x1.Id == check_id);
            if (pers != null)
            {
                pers.Image = imgs[0];
                db.SaveChanges();
            }

            return RedirectToAction("Personal_record", "Home", new { id = check_id });
        }




        [HttpPost]
        public ActionResult Main_help_block(Application_phone a)
        {
            //var res = new Application_phone();
            db.Application_phone_comm.Add(a);
            db.SaveChanges();
            
            return Redirect(Url.Action("Index", "Home"));
        }


        //-----------------------------------
       
        public ActionResult Main_present_block_save(Follow_email a)
        {
            //TODO проверять есть ли в бд такая почта?
            var check_id = System.Web.HttpContext.Current.User.Identity.GetUserId();
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
        public ActionResult Main_present_block()
        {
            var check_id = System.Web.HttpContext.Current.User.Identity.GetUserId();
            if (string.IsNullOrEmpty(check_id))
            {
                ViewBag.check = null;
            }
            else
            {
                var t = db.Follow_email.FirstOrDefault(x1 => x1.User_id == check_id);
                ViewBag.check = false;
                if (t == null)
                    ViewBag.check = true;
            }
           
           
           
            return PartialView();
        }
        [ChildActionOnly]
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

           
            var sect = db.Section_in_main_header.ToList();
            if (sect == null || sect.Count == 0)
            {
                var tmp_woman = new Section_main_header() {Name= "Женщинам", line1_name = "Обувь",
                    line2_name = "Аксессуары",
                    line3_name = "Спорт",
                    line4_name = "Игрушки",
                    line5_name = "Красота",
                    line6_name = "Новинки",
                    line7_name = "Ювелирные украшения",
                    line8_name = "Premium",
                    line9_name = "Подарки",
                    line10_name = "Зима",
                    line1_link="",
                    line2_link = "",
                    line3_link = "",
                    line4_link = "",
                    line5_link = "",
                    line6_link = "",
                    line7_link = "",
                    line8_link = "",
                    line9_link = "",
                    line10_link = "",
                };
                db.Section_in_main_header.Add(tmp_woman);
                var tmp_man = new Section_main_header()
                {
                    Name = "Мужчинам",
                    line1_name = "Обувь",
                    line2_name = "Аксессуары",
                    line3_name = "Спорт",
                    line4_name = "Игрушки",
                    line5_name = "Красота",
                    line6_name = "Новинки",
                    line7_name = "Ювелирные украшения",
                    line8_name = "Premium",
                    line9_name = "Подарки",
                    line10_name = "Зима",
                    line1_link = "",
                    line2_link = "",
                    line3_link = "",
                    line4_link = "",
                    line5_link = "",
                    line6_link = "",
                    line7_link = "",
                    line8_link = "",
                    line9_link = "",
                    line10_link = "",
                };
                db.Section_in_main_header.Add(tmp_man);
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
                 sect = db.Section_in_main_header.ToList();
            }
            
            ViewBag.List_class_for_header = sect;
            }
            catch { }
            return PartialView();
        }

     

        
    }
}