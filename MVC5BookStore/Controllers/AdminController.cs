using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;
using PagedList.Mvc;
using System.Web.Mvc;
using MVC5BookStore.Models;
using System.IO;
namespace MVC5BookStore.Controllers
{
    public class AdminController : Controller
    {
        dbQLBansachDataContext data = new dbQLBansachDataContext();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        private List<SACH> Laysachmoi(int count)
        {
            return data.SACHes.OrderByDescending(a => a.Ngaycapnhat).Take(count).ToList();
        }
        public ActionResult Sach(int ? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 15;
            var sachmoi = Laysachmoi(15);
            return View(data.SACHes.ToList().OrderBy(n => n.Masach).ToPagedList(pageNumber, pageSize));
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            var tendn = collection["username"];
            var matkhau = collection["password"];
            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = "Phải nhập tên đăng nhập";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Phải nhập mật khẩu";
            }
            else
            {
                //Gán giá trị cho đối tượng được tạo mới

                Admin ad = data.Admins.SingleOrDefault(n => n.UserAdmin == tendn && n.PassAdmin == matkhau);
                if (ad != null)
                {
                    Session["taikhoanadmin"] = ad;
                    return RedirectToAction("Index", "Admin");
                }
                else
                    ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không đúng";
            }
            return View();
        }
        [HttpGet]
        public ActionResult Themmoisach()
        {
            ViewBag.MaCD = new SelectList(data.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "Tenchude");
            ViewBag.MaNXB = new SelectList(data.NHAXUATBANs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB");
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Themmoisach(SACH sach, HttpPostedFileBase fileUpload)
        {
            ViewBag.MaCD = new SelectList(data.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChude");
            ViewBag.MaNXB = new SelectList(data.NHAXUATBANs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB");

            if (fileUpload == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh bìa ";
                return View();
            }
            //Them vao CSDL
            else
            {
                if (ModelState.IsValid)
                {
                    //Luu ten file
                    var fileName = Path.GetFileName(fileUpload.FileName);
                    //Luu duong dan file
                    var path = Path.Combine(Server.MapPath("~/Images"), fileName);
                    //Kiem tra hinh anh ton tai chua ?
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                    }
                    else
                    {
                        // Luu hinh anh vao duong dan
                        fileUpload.SaveAs(path);
                    }
                    sach.Hinhminhhoa = fileName;
                    //Luu vao CSDL
                    data.SACHes.InsertOnSubmit(sach);
                    data.SubmitChanges();

                }
                return RedirectToAction("Sach");
            }
        }
        public ActionResult Chitietsach(int id)
        {
            SACH sach = data.SACHes.SingleOrDefault(n => n.Masach == id);
            ViewBag.Masach = sach.Masach;
            if(sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sach);
        }
        public ActionResult Xoasach(int id)
        {
            SACH sach = data.SACHes.SingleOrDefault(n => n.Masach == id);
            ViewBag.Masach = sach.Masach;
            if(sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sach);
        }
        [HttpPost, ActionName ("Xoasach")]
        public ActionResult Xacnhanxoa(int id)
        {
            SACH sach = data.SACHes.SingleOrDefault(n => n.Masach == id);
            ViewBag.Masach = sach.Masach;
            if(sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            data.SACHes.DeleteOnSubmit(sach);
            data.SubmitChanges();
            return RedirectToAction("sach");
        }
        [HttpGet]
        public ActionResult Suasach(int id)
        {
            SACH sach = data.SACHes.SingleOrDefault(n => n.Masach == id);
            if(sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.MaCD = new SelectList(data.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "Tenchude", sach.MaCD);
            ViewBag.MaNXB = new SelectList(data.NHAXUATBANs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB", sach.MaNXB);
            return View(sach);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Suasach(SACH sach, HttpPostedFileBase fileUpload)
        {
            ViewBag.Masach = sach.Masach;
            ViewBag.MaCD = new SelectList(data.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "Tenchude");
            ViewBag.MaNXB = new SelectList(data.NHAXUATBANs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB");
            SACH s = data.SACHes.ToList().Find(n => n.Masach == sach.Masach);
            if (ModelState.IsValid)
            {
                if (fileUpload != null)
                {
                    //Luu ten file
                    var fileName = Path.GetFileName(fileUpload.FileName);
                    //Luu duong dan file
                    var path = Path.Combine(Server.MapPath("~/Images"), fileName);
                    //Kiem tra hinh anh ton tai chua ?
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                    }
                    else
                    {
                        // Luu hinh anh vao duong dan
                        fileUpload.SaveAs(path);
                        sach.Hinhminhhoa = fileName;
                    }
                }
                s.Masach = sach.Masach;
                s.Tensach = sach.Tensach;
                s.Donvitinh = sach.Donvitinh;
                s.Dongia = sach.Dongia;
                s.Mota = sach.Mota;
                s.MaCD = sach.MaCD;
                s.MaNXB = sach.MaNXB;
                s.Ngaycapnhat = sach.Ngaycapnhat;
                s.Soluongban = sach.Soluongban;
                s.solanxem = sach.solanxem;
                s.moi = sach.moi;
                data.SubmitChanges();
            }
            return RedirectToAction("Sach");
        }
    }
}