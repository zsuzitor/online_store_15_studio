using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System.IO;
//
using static online_store.Models.Functions_project;
using static online_store.Models.DataBase;
//
namespace online_store.Models
{
    public static class Functions_project
    {
        public static List<Object_os_for_view> Search(string text_rearch, int count_skip = 0, int count_return = 10, bool extends_src = false, bool Show_available_object = false)
        {
            //Show_available_object--покзывать объекты которых нет в наличии
            List<Object_os_for_view> res = new List<Object_os_for_view>();
            var lst = new List<Object_os>();
            if (string.IsNullOrEmpty(text_rearch))
                lst = db.Objects.OrderBy(x1 => x1.Id).Where(x1=>x1.Show_flag&&(Show_available_object?true:x1.Remainder>0)).Skip(count_skip).Take(count_return).ToList();
            else
            {
                //TODO реализовать
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
                    }).OrderBy(x1=>x1.Id).Skip(count_skip).Take(count_return).ToList();

                }
                else
                {
                    //TODO не работает
                    lst = db.Objects.Where(x1 => x1.Seacrh(text_rearch)).OrderBy(x1 => x1.Id).Skip(count_skip).Take(count_return).ToList();
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
        
            public static bool Delete_object_from_basket(int id_object, string id_user,int count=-1)
        {
            bool res = false;
            var obj = db.Baskets.FirstOrDefault(x1 => x1.Object_id == id_object && x1.Person_id == id_user);
            if (obj != null)
            {
                if(count==-1)
                {
                    db.Baskets.Remove(obj);
                }
                else
                {
                    if (count != 0)
                    {


                        if (obj.Count_obj <= count)
                        {
                            db.Baskets.Remove(obj);
                        }
                        else
                        {
                            obj.Count_obj -= count;
                        }
                    }
                }
               
               
                db.SaveChanges();
                res = true;
            }


            return res;
        }
        public static bool Delete_object_from_follow(int id_object, string id_user)
        {
            bool res = false;
            var obj = db.Follow_objects.FirstOrDefault(x1 => x1.Object_id == id_object && x1.Person_id == id_user);
            if (obj != null)
            {
                db.Follow_objects.Remove(obj);
                db.SaveChanges();
                res = true;
                
            }
         
            return res;
        }

        public static bool Work_with_comment(int id_object, string text, int mark)
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



        public static List<byte[]> Get_photo_post(HttpPostedFileBase[] uploadImage)
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
                    { }
                }
            }
            return res;
        }




    }
}