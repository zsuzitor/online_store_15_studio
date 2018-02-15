using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace online_store.Models
{
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
    public class Section_main_header
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string line1_name { get; set; }
        public string line1_link { get; set; }
        public string line2_name { get; set; }
        public string line2_link { get; set; }
        public string line3_name { get; set; }
        public string line3_link { get; set; }
        public string line4_name { get; set; }
        public string line4_link { get; set; }
        public string line5_name { get; set; }
        public string line5_link { get; set; }
        public string line6_name { get; set; }
        public string line6_link { get; set; }
        public string line7_name { get; set; }
        public string line7_link { get; set; }
        public string line8_name { get; set; }
        public string line8_link { get; set; }
        public string line9_name { get; set; }
        public string line9_link { get; set; }
        public string line10_name { get; set; }
        public string line10_link { get; set; }
    

        public Section_main_header()
        {
            Name = null;
            line1_name = null;
            line1_link = null;
            line2_name = null;
            line2_link = null;
            line3_name = null;
            line3_link = null;
            line4_name = null;
            line4_link = null;
            line5_name = null;
            line5_link = null;
            line6_name = null;
            line6_link = null;
            line7_name = null;
            line7_link = null;
            line8_name = null;
            line8_link = null;
            line9_name = null;
            line9_link = null;
            line10_name = null;
            line10_link = null;

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