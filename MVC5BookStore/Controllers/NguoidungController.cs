using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC5BookStore.Models;
namespace MVC5BookStore.Controllers
{
    public class NguoidungController : Controller
    {
        dbQLBansachDataContext data = new dbQLBansachDataContext();
        // GET: Nguoidung
        public ActionResult Index()
        {
            return View();
        }
        
        
        [HttpGet]
        public ActionResult DangKy()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangKy(FormCollection collection, KHACHHANG kh)
        {
            var hoten = collection["HoTenKH"];
            var tendn = collection["TenDN"];
            var matkhau = collection["Matkhau"];
            var diachi = collection["DiachiKH"];
            var dienthoai = collection["DienthoaiKH"];
            var email = collection["Email"];
            var gioitinh = collection["Gioitinh"];
            var ngaysinh = string.Format("{0:MM/dd/yyyy}", collection["Ngaysinh"]);
            if(string.IsNullOrEmpty(hoten))
            {
                ViewData["Loi1"] = "Họ tên không được trống";
            }
            if (string.IsNullOrEmpty(diachi))
            {
                ViewData["Loi2"] = "Địa chỉ không được trống";
            }
            if (string.IsNullOrEmpty(dienthoai))
            {
                ViewData["Loi3"] = "Điện thoại không được trống";
            }
            if (string.IsNullOrEmpty(tendn))
            {
                ViewData["Loi4"] = "Tên đăng nhập không được trống";
            }
            if (string.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi5"] = "Mật khẩu không được trống";
            }
            if (string.IsNullOrEmpty(ngaysinh))
            {
                ViewData["Loi6"] = "Ngày sinh không được trống";
            }
            if (string.IsNullOrEmpty(gioitinh))
            {
                ViewData["Loi7"] = "Giới tính không được trống";
            }
            if (string.IsNullOrEmpty(email))
            {
                ViewData["Loi8"] = "Email không được trống";
            }
            else
            {
                kh.HoTenKH = hoten;
                kh.Matkhau = matkhau;
                kh.Ngaysinh = DateTime.Parse(ngaysinh);
                kh.TenDN = tendn;
                kh.DiachiKH = diachi;
                kh.DienthoaiKH = dienthoai;
                kh.Email = email;
                kh.Gioitinh = bool.Parse(gioitinh);
                data.KHACHHANGs.InsertOnSubmit(kh);
                data.SubmitChanges();
                return RedirectToAction("Dangnhap");
            }
            return this.DangKy();
        }

        [HttpGet]
        public ActionResult DangNhap()
        {
            return View();
        }

        [HttpPost]
       
        public ActionResult Dangnhap(FormCollection collection)
        {
            var tendn = collection["TenDN"];
            var matkhau = collection["matkhau"];
            if(String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = "Phải nhập tên đăng nhập";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Phải nhập mật khẩu";
            }
            else
            {
                KHACHHANG kh = data.KHACHHANGs.SingleOrDefault(n => n.TenDN == tendn && n.Matkhau == matkhau);
                if (kh != null)
                {
                    Session["Taikhoan"] = kh;
                    return RedirectToAction("Index", "BookStore");
                }
                else
                    ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không đúng";
            }
            return View();
        }
    }
}