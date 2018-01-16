﻿using System;
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
        public string User_name { get; set; }
        public Comment_view()
        {
            Db = null;
            Image_user = null;
            User_name = null;
        }
        public Comment_view(Comment a)
        {
            Db = a;
            Image_user = null;
            User_name = null;
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
    public class Purchase_connect
    {
        public int Id { get; set; }
        public string Purchase_id { get; set; }
        public int Object_id { get; set; }
        public int Price { get; set; }
        public Purchase_connect()
        {
            Id = 0;
            Purchase_id = null;
            Object_id = 0;
            Price = 0;
           
        }
    }

    public class Connect_basket
    {
        public int Id { get; set; }
        public int Object_id { get; set; }
        //public double Price { get; set; }
        public string Person_id { get; set; }
        public Connect_basket()
        {
            Id = 0;
            Object_id = 0;
            Person_id = "";
            // Price = 0;

        }
    }
    public class Purchase
    {
        public int Id { get; set; }
        
        public string Person_id { get; set; }
        public int Price { get; set; }
        public Purchase()
        {
            Id = 0;
            Price = 0;
             Person_id = null;

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
        public List<Comment> Comments;
        public List<Object_os_for_view> Baskets;
        public List<Object_os_for_view> Follow;


        public Person()
        {
            Db = null;
            Images = new List<Connect_image>();
            Comments = new List<Comment>();
            Baskets = new List<Object_os_for_view>();
            Follow = new List<Object_os_for_view>();
        }
        public Person(ApplicationUser a)
        {
            Db = a;
            Images = new List<Connect_image>();
            Comments = new List<Comment>();
            Baskets = new List<Object_os_for_view>();
            Follow = new List<Object_os_for_view>();
        }
    }

}