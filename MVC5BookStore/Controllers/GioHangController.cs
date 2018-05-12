using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC5BookStore.Models;

namespace MVC5BookStore.Controllers
{
    public class GioHangController : Controller
    {
        dbQLBansachDataContext data = new dbQLBansachDataContext();
        // GET: GioHang
        public List<Giohang> Laygiohang()
        {
            List<Giohang> fstGiohang = Session["Giohang"] as List<Giohang>;
            if (fstGiohang == null)
            {
                fstGiohang = new List<Giohang>();
                Session["Giohang"] = fstGiohang;
            }
            return fstGiohang;
        }

        public ActionResult Themgiohang(int iMasach, string strURL)
        {
            List<Giohang> fstGiohang = Laygiohang();
            Giohang sanpham = fstGiohang.Find(n => n.iMasach == iMasach);
            if (sanpham == null)
            {
                sanpham = new Giohang(iMasach);
                fstGiohang.Add(sanpham);
                return Redirect(strURL);
            }
            else
            {
                sanpham.iSoLuong++;
                return Redirect(strURL);
            }

        }
        public int TongSoLuong()
        {
            int iTongSoLuong = 0;
            List<Giohang> fstGiohang = Session["Giohang"] as List<Giohang>;
            if(fstGiohang!=null)
            {
                iTongSoLuong = fstGiohang.Sum(n => n.iSoLuong);
            }
            return iTongSoLuong;
        }
        public double TongTien()
        {
            double iTongTien = 0;
            List<Giohang> fstGiohang = Session["Giohang"] as List<Giohang>;
            if(fstGiohang !=null)
            {
                iTongTien = fstGiohang.Sum(n => n.dThanhTien);
            }
            return iTongTien;
        }
        public ActionResult Giohang()
        {
            List<Giohang> fstGiohang = Laygiohang();
            if (fstGiohang.Count == 0)
            {
                return RedirectToAction("Index", "Bookstore");

            }
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            return View(fstGiohang);
        }
        public ActionResult GiohangPartial()
        {
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            return PartialView();
        }
        public ActionResult Xoagiohang(int iMaSp)
        {
            List<Giohang> fstgiohang = Laygiohang();
            Giohang sanpham = fstgiohang.SingleOrDefault(n => n.iMasach == iMaSp);
            if(sanpham!=null)
            {
                fstgiohang.RemoveAll(n => n.iMasach == iMaSp);
                return RedirectToAction("Giohang");
            }
            if(fstgiohang.Count == 0)
            {
                return RedirectToAction("Index", "BookStore");
            }
            return RedirectToAction("Giohang");
        }
        public ActionResult Capnhatgiohang(int iMaSp, FormCollection f)
        {
            List<Giohang> fstgiohang = Laygiohang();
            Giohang sanpham = fstgiohang.SingleOrDefault(n => n.iMasach == iMaSp);
            if(sanpham != null)
            {
                sanpham.iSoLuong = int.Parse(f["txtSoluong"].ToString());
            }
            return RedirectToAction("Giohang");
        }
        public ActionResult Xoatatcagiohang()
        {
            List<Giohang> fstgiohang = Laygiohang();
            fstgiohang.Clear();
            return RedirectToAction("Index", "BookStore");

        }
        [HttpGet]
        public ActionResult DatHang()
        {
            if(Session["Taikhoan"] == null || Session["Taikhoan"].ToString() == "")
            {
                return RedirectToAction("Dangnhap", "nguoidung");
            }
            if (Session["Giohang"] == null)
            {
                return RedirectToAction("Index", "BookStore");
            }
            List<Giohang> fstgiohang = Laygiohang();
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            return View(fstgiohang);
        }
        public ActionResult Dathang(FormCollection collection)
        {
            DONDATHANG ddh = new DONDATHANG();
            KHACHHANG kh = (KHACHHANG)Session["Taikhoan"];
            List<Giohang> gh = Laygiohang();
            ddh.MaKH = kh.MaKH;
            ddh.Tennguoinhan = kh.HoTenKH;
            //ddh.Diachinhan = kh.DiachiKH;
            ddh.Dienthoainhan = kh.DienthoaiKH;
            ddh.Trigia = (decimal)TongTien();
            ddh.NgayDH = DateTime.Now;
            var ngaygiao = String.Format("{0:MM/DD/YYYY}", collection["ngaygiao"]);
            ddh.Ngaygiaohang = DateTime.Parse(ngaygiao);
            ddh.HTGiaohang = false;
            ddh.HTThanhtoan = false;
            ddh.Dagiao = false;
            data.DONDATHANGs.InsertOnSubmit(ddh);
            data.SubmitChanges();
            foreach (var item in gh)
            {
                CTDATHANG ctdh = new CTDATHANG();
                ctdh.SoDH = ddh.SoDH;
                ctdh.Masach = item.iMasach;
                ctdh.Soluong = item.iSoLuong;
                ctdh.Dongia = (decimal)item.dDongia;
                data.CTDATHANGs.InsertOnSubmit(ctdh);
            }
            data.SubmitChanges();
            Session["giohang"] = null;
            return RedirectToAction("Xacnhandonhang", "Giohang");
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Xacnhandonhang()
        {
            return View();
        }
    }
}