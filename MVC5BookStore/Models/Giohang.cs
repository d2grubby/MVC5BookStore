using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC5BookStore.Models;

namespace MVC5BookStore.Models
{
    public class Giohang
    {
        dbQLBansachDataContext data = new dbQLBansachDataContext();

        public int iMasach { get; set; }

        public string sTensach { set; get; }


        public string sHinhminhhoa { set; get; }

        public double dDongia { set; get; }
        public int iSoLuong { set; get; }
        public double dThanhTien
        {
            get { return iSoLuong * dDongia; }
        }
        public Giohang(int Masach)
        {
            iMasach = Masach;
            SACH sach = data.SACHes.Single(n => n.Masach == iMasach);
            sTensach = sach.Tensach;
            sHinhminhhoa = sach.Hinhminhhoa;
            dDongia = double.Parse(sach.Dongia.ToString());
            iSoLuong = 1;
        }
    }
}