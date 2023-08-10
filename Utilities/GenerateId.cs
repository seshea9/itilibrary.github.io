using BarcodeLib;
using ITI_Libraly_Api.Contexts;
using System;
using System.Drawing;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace WebHrApi.Utilities
{
    public class GenerateId
    {
        ITI_LibraySystemContext db;

        public GenerateId(ITI_LibraySystemContext db)
        {
            this.db = db;
        }
        public string AutoId(DateTime Date, int BookId)
        {
            string code = string.Empty;
            var emp = (from f in db.TblItiBookdetails
                       join i in db.TblItiImport on f.ImpId equals i.ImpId
                       where i.ImpDate == Date && f.BookId == BookId
                       orderby f.BookDelId descending select f).FirstOrDefault();

            if(emp != null) {
                string num = emp.BookDelLabel == null ? "0" : emp.BookDelLabel;
                 code = string.Format("{0:00000}", (Convert.ToInt32(num) + 1));

                //while ((from f in db.TblItiBookdetails where f.BookDelLabel.Equals(code) select f).Count() > 0)
                //{

                //    code = string.Format("{0:00000}", (Convert.ToInt32(code) + 1));

                //}
            }
            else
            {
                code = string.Format("{0:00000}", (Convert.ToInt32(0) + 1));
            }
            return code;
        }
        public static string GenerateBacode(string value, Color color, Color BackColor)
        {
            string str = "";
            Barcode barcode = new Barcode();
            Image image = barcode.Encode(BarcodeLib.TYPE.CODE39, value, color, BackColor, 150, 50);
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                var arr = ms.ToArray();
                str =Convert.ToBase64String(arr);
            }
            return str;
        }
        public string AutoIdBorrow()
        {
            string code = string.Empty;
            var emp = (from f in db.TblItiBorrow
                       //where f.EmpIsNoteOrder == false
                       orderby f.BorId descending
                       select f).FirstOrDefault();

            if (emp != null)
            {
                string num = emp.BorCode == null ? "0" : emp.BorCode.Substring(1,7);
                code = string.Format("B{0:0000000}", (Convert.ToInt32(num) + 1));

                while ((from f in db.TblItiBorrow where f.BorCode.Equals(code) select f).Count() > 0)
                {

                    code = string.Format("B{0:0000000}", (Convert.ToInt32(code) + 1));

                }
            }
            else
            {
                code = string.Format("B{0:0000000}", (Convert.ToInt32(0) + 1));
            }


            return code;
        }
        public string AutoIdStudent()
        {
            string code = string.Empty;
            var emp = (from f in db.TblItiStudent
                           //where f.EmpIsNoteOrder == false
                       orderby f.StuId descending
                       select f).FirstOrDefault();

            if (emp != null)
            {
                string num = emp.StuCode == null ? "0" : emp.StuCode.Substring(1, 7);
                code = string.Format("S{0:0000000}", (Convert.ToInt32(num) + 1));

                while ((from f in db.TblItiStudent where f.StuCode.Equals(code) select f).Count() > 0)
                {

                    code = string.Format("S{0:0000000}", (Convert.ToInt32(code) + 1));

                }
            }
            else
            {
                code = string.Format("S{0:0000000}", (Convert.ToInt32(0) + 1));
            }


            return code;
        }
    }
}
