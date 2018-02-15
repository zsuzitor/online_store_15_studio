using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace online_store.Models
{
    public class Object_os
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Category { get; set; }
        public string Manufacturer { get; set; }
        public double Price { get; set; }
        public double Discount { get; set; }
        public string Color { get; set; }
        public string Composition { get; set; }//состав 
        public string Description { get; set; }
        public int Count_buy { get; set; }
        public int Remainder { get; set; }
        public bool Show_flag { get; set; }
        
        public Object_os()
        {
            Id = 0;
            Name = "";
            Type = "";
            Manufacturer = "";
            Color = "";
            Composition = "";
            Description = "";
            Count_buy = 0;
            Price = 0;
            Discount = 0;
            Remainder = 0;
            Show_flag = true;
            
        }
        public bool Eq(Object_os a)
        {
           
            Name = a.Name;
            Type = a.Type;
            Manufacturer = a.Manufacturer;
            Color = a.Color;
            Composition = a.Composition;
            Description = a.Description;
            Count_buy = a.Count_buy;
            Price = a.Price;
            Discount = a.Discount;
            Remainder = a.Remainder;
            Show_flag = a.Show_flag;
            
            return true;
        }
        public bool Seacrh(string str)
        {
            //TODO не работает
            if (Name.Contains(str))
                return true;
            if (Type.Contains(str))
                return true;
            if (Category.Contains(str))
                return true;
            if (Manufacturer.Contains(str))
                return true;
            if (Color.Contains(str))
                return true;
            if (Composition.Contains(str))
                return true;
            if (Description.Contains(str))
                return true;


            return false;
        }
    }

    public class Object_os_for_view
    {
        public Object_os Db;
        public int Count { get; set; }
        public List<Connect_image> Images;
        public List<Comment_view> Comments;
        public Object_os_for_view(Object_os a)
        {
            Db = a;
            Count = 0;
            Images = new List<Connect_image>();
            Comments = new List<Comment_view>();
        }
    }
}



