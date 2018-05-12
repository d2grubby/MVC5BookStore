using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC5BookStore.Models;
using PagedList;
using PagedList.Mvc;
namespace MVC5BookStore.Controllers
{
    
    public class BookStoreController : Controller
    {
        // GET: BookStore
        //
        // GET: /BookStore/
        //tao 1 doi tuong chua toan bo CSDL
        dbQLBansachDataContext data = new dbQLBansachDataContext();

        private List<SACH> Laysachmoi(int count)
        {
            return data.SACHes.OrderByDescending(a => a.Ngaycapnhat).Take(count).ToList();
        }
        public ActionResult Index(int ? page)
        {
            int pageSize = 5;
            int pageNum = (page ?? 1);
            //Lay 5 quyen moi nhat
            var sachmoi = Laysachmoi(15);
            return View(sachmoi.ToPagedList(pageNum, pageSize));
        }
        public ActionResult Chude()
        {
            //Lay 5 quyen moi nhat
            var chude = from cd in data.CHUDEs select cd;
            return PartialView(chude);
        }
        public ActionResult Nhaxuatban()
        {
            var nhaxuatban = from cd in data.NHAXUATBANs select cd;
            return PartialView(nhaxuatban);
        }
        public ActionResult SPTheoChuDe (int id)
        {
            var sach = from s in data.SACHes where s.MaCD == id select s;
            return View(sach);
        }
        public ActionResult SPTheoNXB(int id)
        {
            var sach = from s in data.SACHes where s.MaNXB == id select s;
            return View(sach);
        }
        public ActionResult Details(int id)
        {
            var sach = from s in data.SACHes
                       where s.Masach == id
                       select s;
            return View(sach.Single());
        }
    }
}