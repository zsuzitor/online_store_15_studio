using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using online_store.Models;
using System.IO;

namespace online_store.Controllers
{
    public class HomeController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        //var check_id = System.Web.HttpContext.Current.User.Identity.GetUserId();
        //[Authorize(Roles="admin")] [Authorize]


        public ActionResult Index()
        {
            //TODO работа с object
            try { 
            var objs = db.Objects.Take(10).ToList();
                 ViewBag.Object_for_slider_1 =new List<Object_os_for_view>();
            foreach (var i in objs)
            {
                var tmp = new Object_os_for_view(i);
                var img=db.Images.FirstOrDefault(x1=>x1.What_something=="Object"&&x1.Something_id==i.Id.ToString());
                if(img!=null)
                tmp.Images.Add(img);
                ViewBag.Object_for_slider_1.Add(tmp);
            }
                ViewBag.count_obg_slider_1 = ViewBag.Object_for_slider_1.Count;
            }
            catch { }
            
            return View();
        }
        public ActionResult List_objects(string text_rearch = null)
        {
            List<Object_os_for_view> res = Search(text_rearch);
            
            
            return PartialView(res);
        }
        public ActionResult List_objects_type(string text_rearch=null)
        {
            ViewBag.text_rearch = text_rearch;

            return View();
        }
        public ActionResult Object_view(int id)
        {
            var check_id = System.Web.HttpContext.Current.User.Identity.GetUserId();
            ViewBag.Person_id = check_id;
            var not_res = db.Objects.FirstOrDefault(x1 => x1.Id == id);
            Object_os_for_view res = new Object_os_for_view(not_res);
            var img = db.Images.Where(x1 => x1.Something_id == id.ToString() && x1.What_something == "Object");
            res.Images = img.ToList();
            var com = db.Comments.Where(x1 => x1.Object_id == id && !string.IsNullOrEmpty(x1.Text)).ToList();
            var com_person = com.FirstOrDefault(x1 => x1.Person_id == check_id);
            
            
            foreach (var i in com)
            {

                if (i.Person_id != check_id)
                {
                    var user = db.Users.First(x1 => x1.Id == i.Person_id);
                    var tmp = new Comment_view(i) { Image_user = user.Image, User_name = user.Name };

                    res.Comments.Add(tmp);
                }

            }
            if (com_person == null)
                ViewBag.Can_commented = true;
            else
            {
                //if(string.IsNullOrEmpty(com_person.Text))
                // ViewBag.Can_commented = true;
                // else
                ViewBag.Can_commented = false;
                //
                var user = db.Users.First(x1 => x1.Id == check_id);
                var tmp = new Comment_view(com_person) { Image_user = user.Image, User_name = user.Name };

                res.Comments.Add(tmp);
            }
            res.Comments.Reverse();


            return View(res);
        }
        //добавляет и изменяет коммент
        [HttpPost]
        public ActionResult Edit_comment(int id_object, string text, int mark,string from)
        {
            Work_with_comment(id_object, text, mark);
            if(from== "Object_view")
            return RedirectToAction ("Object_view","Home",new {id= id_object });
            else
                return RedirectToAction("Personal_record","Home",new { });
        }
        [HttpPost]
        public ActionResult Add_comment(int id_object, string text, int mark)
        {

            Work_with_comment(id_object, text, mark);

            return RedirectToAction("Object_view", "Home", new { id = id_object });

        }
        public ActionResult Add_mark_for_object(int id, string num = "")
        {
            //num для работы со списками объектов
            var check_id = System.Web.HttpContext.Current.User.Identity.GetUserId();
            ViewBag.Id = id;
            ViewBag.Num = num;
            ViewBag.Mark_pers = 0;
            if (check_id != null)//не комментить условие, должно быть так
            {
               
                var mrk = db.Comments.FirstOrDefault(x1 => x1.Person_id == check_id&&x1.Mark!=null);
                if(mrk!=null)
                    ViewBag.Mark_pers = mrk.Mark;
            }
            var marks = db.Comments.Where(x1 => x1.Object_id == id && x1.Mark != null).ToList();
            int mark = 0;
            if (marks.Count > 0)
            {
                mark = (int)(marks.Sum(x1 => x1.Mark) / marks.Count);
            }

            ViewBag.Mark = mark;
            return PartialView();
        }
        //[Authorize]
        public ActionResult Change_mark_for_object(int id, int num, string num_block_for_list = "")
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
                //var mm=db.Comments.FirstOrDefault(x1 => x1.Object_id == id  && x1.Person_id == check_id);
                //if (mm != null)
                //{
                marks.Mark = num;
                db.SaveChanges();
                //}
            }
            //white_star.png
            return RedirectToAction("Add_mark_for_object", "Home", new { id = id, num = num_block_for_list });
        }
        //[Authorize]
        public ActionResult Purchase_view(int id)
        {
            //TODO проверять если ли доступ
            var not_res = db.Purchases.FirstOrDefault(x1=>x1.Id==id);
            var obj = db.Purchases_connect.Where(x1=>x1.Purchase_id==id).Join(db.Objects,x1=>x1.Object_id,x2=>x2.Id,(x1,x2)=>x2).ToList();
            //TODO Object_os_for_view
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
        //[Authorize]
        public ActionResult Edit_personal_record()
        {
            var check_id = System.Web.HttpContext.Current.User.Identity.GetUserId();
            var res = db.Users.FirstOrDefault(x1=>x1.Id==check_id);



            return View(res);

        }
        //[Authorize]
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
        //[Authorize]
        public ActionResult Personal_record(string id)
        {
            id = string.IsNullOrEmpty(id) ? System.Web.HttpContext.Current.User.Identity.GetUserId() : id;
            ViewBag.Person_id = id;
            var not_res = db.Users.First(x1 => x1.Id == id);
            var res = new Person(not_res);
          


            //res.Images.AddRange(db.Images.Where(x1 => x1.What_something == "Person" && x1.Something_id == id).ToList());
            //
            ViewBag.Baskets = db.Baskets.Where(x1 => x1.Person_id == id).ToList();
            ViewBag.Baskets.Reverse();
            /* var bsk = db.Baskets.Where(x1 => x1.Person_id == id).Join(db.Objects, x1 => x1.Object_id, x2 => x2.Id, (x1, x2) => x2).ToList();
             foreach (var i in bsk)
             {
                 var tmp_img = db.Images.First(x1 => x1.What_something == "Object" && x1.Something_id == i.Id.ToString());

                 res.Baskets.Add(new Object_os_for_view(i) {
                     Images = new List<Connect_image> {
                tmp_img}
             });


             } */
            //

            ViewBag.Follow = db.Follow_objects.Where(x1 => x1.Person_id == id).ToList();
            ViewBag.Follow.Reverse();
            /*
           var foll = db.Follow_objects.Where(x1=>x1.Person_id==id).Join(db.Objects, x1 => x1.Object_id, x2 => x2.Id, (x1, x2) => x2).ToList();
           foreach (var i in foll)
           {
               var tmp_img = db.Images.First(x1 => x1.What_something == "Object" && x1.Something_id == i.Id.ToString());

               res.Baskets.Add(new Object_os_for_view(i)
               {
                   Images = new List<Connect_image>{
               tmp_img }
           });


           }
           */
            var com = db.Comments.Where(x1 => x1.Person_id == id  ).ToList();
            foreach (var i in com)
            {
                    res.Comments.Add(new Comment_view( i));
            }
            res.Comments.Reverse();
            var prc = db.Purchases.Where(x1=>x1.Person_id==id);
            res.Purchases.AddRange(prc);
            res.Purchases.Reverse();
            return View(res);
        }
        //[Authorize]
        public ActionResult Object_follow(int id, bool? click, string num_block_for_list = "")
        {

            var check_id = System.Web.HttpContext.Current.User.Identity.GetUserId();
            ViewBag.Id = id;
            ViewBag.Follow = false;
            ViewBag.Num = num_block_for_list;
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
        //[Authorize]
        public ActionResult Basket_page()
        {
            var check_id = System.Web.HttpContext.Current.User.Identity.GetUserId();
            var res = db.Baskets.Where(x1 => x1.Person_id == check_id);//.Join(db.Objects,x1=>x1.Object_id,x2=>x2.Id,(x1,x2)=>x2);
            //var res = new List<Object_os_for_view>();
            //var summ =res.Select(x1=>x1.Price).ToList();
            var summ_1 = res.Join(db.Objects, x1 => x1.Object_id, x2 => x2.Id, (x1, x2) => x2).ToList();

            ViewBag.All_price = summ_1.Sum(x1 => x1.Price);
            ViewBag.All_price_small = summ_1.Sum(x1 => ((int)(x1.Price * (1 - x1.Discount))));

            return View(res);
        }
        //TODO
        //[Authorize]
        public ActionResult Buy_basket()
        {
            var check_id = System.Web.HttpContext.Current.User.Identity.GetUserId();
            var bsk_obj = db.Baskets.Where(x1=>x1.Person_id==check_id).Join(db.Objects,x1=>x1.Object_id,x2=>x2.Id,(x1,x2)=>x2).ToList();
            if (bsk_obj.Count > 0)
            {
                var prc = new Purchase() { Person_id = check_id, Price = bsk_obj.Sum(x1 => (((int)(x1.Price * (1 - x1.Discount))))) };//мб в цикле прибалять
                db.Purchases.Add(prc);
                db.SaveChanges();
                foreach (var i in bsk_obj)
                {
                    var tmp = new Purchase_connect() { Purchase_id = prc.Id, Object_id = i.Id, Price = ((int)(i.Price * (1 - i.Discount))) };
                    db.Purchases_connect.Add(tmp);
                    db.SaveChanges();
                }
                db.Baskets.RemoveRange(db.Baskets.Where(x1 => x1.Person_id == check_id));
                db.SaveChanges();
                //TODO у всех объектов сделать количество -1
            }
            


            return View();
        }
        //[Authorize]
        public ActionResult Basket_one_object_partial(int id)
        {

            var imgs = db.Images.Where(x1 => x1.What_something == "Object" && x1.Something_id == id.ToString()).ToList();
            var obj = db.Objects.First(x1 => x1.Id == id);
            var res = new Object_os_for_view(obj) { Images = imgs };
            return PartialView(res);

        }
       
        //[Authorize]
        public ActionResult Follow_one_object_partial(int id)
        {

            var imgs = db.Images.Where(x1 => x1.What_something == "Object" && x1.Something_id == id.ToString()).ToList();
            var obj = db.Objects.First(x1 => x1.Id == id);
            var res = new Object_os_for_view(obj) { Images = imgs };
            return PartialView(res);

        }
        //[Authorize]
        public ActionResult Object_add_basket(int id, bool? click, string num_block_for_list = "")
        {
            ViewBag.Id = id;
            ViewBag.Num = num_block_for_list;
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



            return PartialView();
        }
        //[Authorize]
        public ActionResult Delete_object_from_basket(int id)
        {
            var check_id = System.Web.HttpContext.Current.User.Identity.GetUserId();
            var obj = db.Baskets.FirstOrDefault(x1 => x1.Object_id == id && x1.Person_id == check_id);
            if (obj != null)
            {
                db.Baskets.Remove(obj);
                db.SaveChanges();
                ViewBag.Message = "Удалено";
            }
            else
                ViewBag.Message = "Ошибка";
            return PartialView();

        }
        //[Authorize]
        public ActionResult Delete_object_from_follow(int id)
        {
            var check_id = System.Web.HttpContext.Current.User.Identity.GetUserId();
            var obj = db.Follow_objects.FirstOrDefault(x1 => x1.Object_id == id && x1.Person_id == check_id);
            if (obj != null)
            {
                db.Follow_objects.Remove(obj);
                db.SaveChanges();
                ViewBag.Message = "Удалено";
            }
            else
                ViewBag.Message = "Ошибка";
            return PartialView();

        }
        //[Authorize(Roles="admin")]
        public ActionResult Delete_object(int id)
        {
            db.Objects.Remove(db.Objects.First(x1 => x1.Id == id));
            db.Comments.RemoveRange(db.Comments.Where(x1 => x1.Object_id == id));

            return RedirectToAction("Index", "Home", new { });
        }
        //[Authorize(Roles="admin")]
        public ActionResult Add_object()
        {
            Object_os res = new Object_os();

            return View(res);
        }
        //[Authorize(Roles="admin")]
        [HttpPost]
        public ActionResult Add_object(Object_os a)
        {
            //проверки и тд

            db.Objects.Add(a);
            db.SaveChanges();
            ViewBag.Id = a.Id;
            return RedirectToAction("Work_with_images_object", new { id = a.Id });

            //return View();
        }
        //[Authorize(Roles="admin")]
        public ActionResult Work_with_images_object(int id)
        {
            ViewBag.Id = id;
            var imgs = db.Images.Where(x1 => x1.What_something == "Object" && x1.Something_id == id.ToString());
            ViewBag.Images = imgs.ToList();
            return View();
        }
        //[Authorize(Roles="admin")]
        public ActionResult Delete_img_block(int id)
        {
            db.Images.Remove(db.Images.First(x1 => x1.Id == id));
            db.SaveChanges();
            return PartialView();
        }



        //[Authorize]
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
                    ViewBag.Message = "Удалить невозможно";
                    var user = db.Users.First(x1 => x1.Id == com.Person_id);
                    res = new Comment_view(com) { Image_user = user.Image, User_name = user.Name };

                }
            }



            return PartialView(res);
        }




        //[Authorize(Roles="admin")]  админ объектам, юзерам юзеры
        [HttpPost]
        public ActionResult Add_new_image(HttpPostedFileBase[] uploadImage, string id,string from)
        {
            var imgs = Get_photo_post(uploadImage);
            foreach (var i in imgs)
            {
                db.Images.Add(new Connect_image() { Something_id = id, What_something = from, Image = i });
                db.SaveChanges();
            }


            return RedirectToAction("Object_view", "Home", new { id = id });
        }

        //[Authorize]
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















        //-----------------------------------
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

        //------------------------------------------FUNCTIONS----------------------------------------------------------------------------------

        public List<Object_os_for_view> Search(string text_rearch,int count_in_list=10, bool extends_src=false)
        {
            List<Object_os_for_view> res = new List<Object_os_for_view>();
            var lst = new List<Object_os>();
            if (string.IsNullOrEmpty(text_rearch))
                lst=db.Objects.Take(count_in_list).ToList();
            else
            {
                if (extends_src)
                {
                    //TODO скорее всего так нельзя
                    var list_words = text_rearch.Split(' ');

                    lst = db.Objects.AsEnumerable().Where(x1 => {//.AsEnumerable()
                        var ret = false;
                        foreach (var i in list_words)
                        {
                            ret = x1.Seacrh(i);
                            if (ret)
                                return ret;
                        }
                        return ret;
                    }).ToList();

                }
                else
                {
                    //TODO не работает
                     lst=db.Objects.Where(x1 => x1.Seacrh(text_rearch)).ToList();
                }

            }




            foreach (var i in lst)
            {
                var tmp = new Object_os_for_view(i);
                tmp.Images.AddRange(db.Images.Where(x1 => x1.Something_id == i.Id.ToString() && x1.What_something == "Object"));


                res.Add(tmp);
            }



            return res;
        }



        public bool Work_with_comment(int id_object, string text, int mark)
        {

            var check_id = System.Web.HttpContext.Current.User.Identity.GetUserId();
            var marks = db.Comments.FirstOrDefault(x1 => x1.Object_id == id_object && x1.Person_id == check_id);
            if (marks == null)
            {
                var new_comm = new Comment() { Object_id = id_object, Person_id = check_id, Text = text };
                if (mark > 0)

                    new_comm.Mark = mark;

                else
                    new_comm.Mark = null;
                db.Comments.Add(new_comm);
                db.SaveChanges();
            }
            else
            {
                if (!string.IsNullOrEmpty(text))
                {
                    marks.Text = text;
                    if (mark > 0)
                        marks.Mark = mark;
                    db.SaveChanges();
                }

            }




            return true;
        }







        public List<byte[]> Get_photo_post(HttpPostedFileBase[] uploadImage)
        {

            /* сохранение картинок как файл ...
              HttpPostedFileBase image = Request.Files["fileInput"];
            
            if (image != null && image.ContentLength > 0 && !string.IsNullOrEmpty(image.FileName))
            {
                string fileName = image.FileName;
                image.SaveAs(Path.Combine(Server.MapPath("Images"), fileName));
            }
             
             * */
            List<byte[]> res = new List<byte[]>();
            if (uploadImage != null)
            {

                foreach (var i in uploadImage)
                {
                    try
                    {
                        byte[] imageData = null;
                        // считываем переданный файл в массив байтов
                        using (var binaryReader = new BinaryReader(i.InputStream))
                        {
                            imageData = binaryReader.ReadBytes(i.ContentLength);
                        }
                        // установка массива байтов
                        res.Add(imageData);

                    }
                    catch
                    {

                    }



                }

            }


            return res;
        }
    }
}