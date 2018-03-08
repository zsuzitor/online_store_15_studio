using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace online_store.Models
{


    public class Discount_coupon//конкретный купон
    {
        public int Id { get; set; }
        public string User_id { get; set; }
        public int Discount_id { get; set; }
        public bool Spent { get; set; }

        public Discount_coupon()
        {
            Id = 0;
            User_id = null;
            Discount_id = 0;
            Spent = false;
        }

        }


        public class Discount//что то типо купонов
    {
        public int Id { get; set; }
        public string Name { get; set; }//доп настройка под tag и тд          //Follow_mail будет известно пользователю и он будет получать купон по ней
        public string Tag { get; set; }//скидка например только на обувь или производителя
        public int? Count_left { get; set; }//null==бесконечное колличество купонов

        public Double Discount_ { get; set; }
        public DateTime Date { get; set; }

        
        public Discount()
        {
            Id = 0;
            Name = "";
            Tag = null;
            Count_left = null;

            Date = DateTime.Now;
            Discount_ = 0;

        }
        //только создание без проверок
        public Discount_coupon Create_coupon(string user_id)
        {
            Discount_coupon res = null;
            if(Count_left<1)
                return res;
            
                if (Count_left != null)
                    this.Count_left--;
                res = new Discount_coupon() { User_id= user_id, Discount_id=this.Id };
            


            return res;
        }
    }
    public class Object_notification
    {
        public int Id { get; set; }
        
        public string User_id { get; set; }
        public int Object_id { get; set; }



        public Object_notification()
        {
            Id = 0;
            Object_id = 0;
            User_id = null;
            

        }
    }
    public class Follow_email
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string User_id { get; set; }
        public bool Dispatch { get; set; }//нужно ли отправлять письма
        public DateTime Date { get; set; }
        

        public Follow_email()
        {
            Id = 0;
            Email = null;
            Name = null;
            User_id = null;
            Dispatch = true;
            Date = DateTime.Now;
            
        }
    }
    public class Application_phone
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Complete { get; set; }
        public int Phone { get; set; }
        public DateTime Date { get; set; }
        public DateTime Date_comfortable { get; set; }
        public DateTime? Date_complete { get; set; }
        public string Message { get; set; }

        public Application_phone()
        {
            Id = 0;
            Complete = false;
            Name = "";
            Phone = -1;
            Date = DateTime.Now;
            Date_comfortable = DateTime.Now;
            Date_complete = null;
            Message = "";
        }
    }
    //отзывы
    public class Comment
    {
        public int Id { get; set; }
        public int Object_id { get; set; }
        public string Person_id { get; set; }
        public string Text { get; set; }
        private int? mark { get; set; }
        public int? Mark
        {
            get { return mark; }
            set
            {
                if (value == null)
                {
                    mark = null;
                    return;
                }

                if (value < 0)
                    mark = 0;
                else
                {
                    if (value > 5)
                        mark = 5;
                    else
                        mark = value;
                }

            }
        }


        public Comment()
        {
            Id = 0;
            Object_id = 0;
            Person_id = null;
            Text = null;
            mark = null;
        }

    }
    public class Comment_view
    {
        public Comment Db { get; set; }
        public byte[] Image_user { get; set; }
        public byte[] Image_object { get; set; }
        public string User_name { get; set; }
        public DateTime Date_Time { get; set; }
        //public List<Mark_for_comment> Mark_comment;
        //public int Count_good_mark { get; set; }
        //public int Count_bad_mark { get; set; }
        //public int Count_funny_mark { get; set; }
        public Comment_view()
        {
            Db = null;
            Image_user = null;
            Image_object = null;
            User_name = null;
            Date_Time = DateTime.Now;
           // Count_good_mark = 0;
            //Count_bad_mark = 0;
            //Count_funny_mark = 0;
        }
        public Comment_view(Comment a)
        {
            Db = a;
            Image_user = null;
            Image_object = null;
            User_name = null;
            Date_Time = DateTime.Now;
        }
    }

    //
    public class Section_main_header_link
    {
        public int Id { get; set; }
        public int Section_Id { get; set; }
        public string Line_name { get; set; }
        public string Line_link_action { get; set; }
        public string Line_link_controller { get; set; }
        public string Params { get; set; }
        public Section_main_header_link()
        {
            Line_name = null;
            Section_Id = 0;
            Line_link_action = null;
            Line_link_controller = null;
        }
    }
    public class Section_main_header
    {
        public int Id { get; set; }
        public string Name { get; set; }
      
    

        public Section_main_header()
        {
            Name = null;
            Id = 0;

        }
    }
    public class Section_main_header_view
    {
        public Section_main_header Section { get; set; }
        public List<Section_main_header_link> Link { get; set; }

        public Section_main_header_view()
        {
            Section = null;
            Link = null;

        }
    }

    //
    public class Connect_image
    {
        public int Id { get; set; }
        public string Something_id { get; set; }
        public string What_something { get; set; }//Person Object
        public byte[] Image { get; set; }
        public Connect_image()
        {
            Id = 0;
            Something_id = null;
            What_something = null;
            Image = null;
        }
    }
 

    public class Connect_basket
    {
        public int Id { get; set; }
        public int Object_id { get; set; }
        public int Count_obj { get; set; }
        //public double Price { get; set; }
        public string Person_id { get; set; }
        public Connect_basket()
        {
            Count_obj = 1;
            Id = 0;
            Object_id = 0;
            Person_id = "";
            // Price = 0;

        }
    }
    public class Purchase_connect
    {
        public int Id { get; set; }
        public int Purchase_id { get; set; }
        public int Object_id { get; set; }
        public int Price { get; set; }
        public Purchase_connect()
        {
            Id = 0;
            Purchase_id = 0;
            Object_id = 0;
            Price = 0;

        }
    }
    public class Purchase
    {
        public int Id { get; set; }
        public DateTime Date_Time { get; set; }
        public string Person_id { get; set; }
        public int Price { get; set; }
        public Purchase()
        {
            Id = 0;
            Price = 0;
             Person_id = null;
            Date_Time = DateTime.Now;

        }
        
    }
    public class Mark_for_comment
    {
       
      //TODO мб и не нужно
        public  enum Mark_en { Good = 1, Funny = 2,  Bad= 3};
        //
        public int Id { get; set; }
        public int Mark { get; set; }
        public string Person_id { get; set; }
       
        public int Comment_id { get; set; }
        public Mark_for_comment()
        {
            Id = 0;
            Mark = 0;
            Person_id = null;
           
            Comment_id = 0;
           
        }

    }
    public class Purchase_view
    {
        public Purchase Db { get; set; }
        public List<Object_os_for_view> Objects;

        public Purchase_view()
        {
            Db = null;
            Objects = new List<Object_os_for_view>();
        }
        public Purchase_view(Purchase a)
        {
            Db = a;
            Objects = new List<Object_os_for_view>();
        }
    }
    public class Follow_object
    {
        public int Id { get; set; }
        public int Object_id { get; set; }
        public string Person_id { get; set; }

        public Follow_object()
        {
            Id = 0;
            Object_id = 0;
            Person_id = "";

        }

    }
    public class Person
    {
        public ApplicationUser Db { get; set; }
        public List<Connect_image> Images;
        public List<Comment_view> Comments;
        public List<Object_os_for_view> Baskets;
        public List<Object_os_for_view> Follow;
        public List<Purchase> Purchases;


        public Person()
        {
            Db = null;
            Images = new List<Connect_image>();
            Comments = new List<Comment_view>();
            Baskets = new List<Object_os_for_view>();
            Follow = new List<Object_os_for_view>();
            Purchases = new List<Purchase>();
        }
        public Person(ApplicationUser a)
        {
            Db = a;
            Images = new List<Connect_image>();
            Comments = new List<Comment_view>();
            Baskets = new List<Object_os_for_view>();
            Follow = new List<Object_os_for_view>();
            Purchases = new List<Purchase>();
        }
    }
    public class Image_link
    {
        public string Path_image { get; set; }
        public byte[] Byte_image { get; set; }
        public string Action { get; set; }
        public string Controller { get; set; }
    }

    }