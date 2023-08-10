using Aspose.Imaging.Extensions;
using BarcodeLib;
using BarcodeStandard;
using ITI_Libraly_Api.Contexts;
using ITI_Libraly_Api.Controllers;
using ITI_Libraly_Api.Models;
using ITI_Libraly_Api.MyModels;
using ITI_Libraly_Api.Service.Interface;
using ITI_Libraly_Api.Utilities;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using WebHrApi.Utilities;

namespace ITI_Libraly_Api.Service.Class
{
    public class BookService : IBookedService
    {
        ITI_LibraySystemContext db;
        private readonly IWebHostEnvironment environment;

        public BookService(ITI_LibraySystemContext db, IWebHostEnvironment environment)
        {
            this.db = db;
            this.environment = environment;

        }
        public async Task<Data> GetBookInfo(int BookId)
        {
            Data model = new Data();
            try
            {
                var List = db.TblItiBook.Where(w => w.BookId == BookId).Select(s => new
                {
                    s.BookId,
                    s.BookNameKh,
                    s.BookNameEn,
                    BookYear = Convert.ToDateTime(s.BookYear).ToString("dd-MM-yyyy"),
                    //.BookYear,
                    s.CateId,
                    CateNameKh = s.CateId == null ? "" : db.TblItiCategory.Where(w => w.CateId == s.CateId).FirstOrDefault().CateNameKh,
                    s.BookAuthor,
                    s.BookDescription,
                    FilePath= new ReadPath(environment).GetImage(s.FilePath),
                    s.LocId,
                    Loc = s.LocId == null ? "" : db.TblItiLocation.Where(l => l.LocId == s.LocId).FirstOrDefault().LocLabel,
                    //bookDatails = db.TblItiBookdetails.Where(w => w.BookId == s.BookId).Select(sd => new
                    //{
                    //    sd.BookDelId,
                    //    sd.BookDelLabel,
                    //    BookDelLabelPrint = sd.ImpId == null ? null : "Loc:" + db.TblItiLocation.Where(w => w.LocId == s.LocId).FirstOrDefault().LocLabel + "/Imp:" + Convert.ToDateTime(db.TblItiImport.Where(w => w.ImpId == sd.ImpId).FirstOrDefault().ImpDate).ToString("dd-MM-yyyy") + "/No:" + sd.BookDelLabel,
                    //    sd.StatusId,
                    //    Status = sd.StatusId == null ? "" : db.TblItiStatus.Where(w => w.StatusId == sd.StatusId).FirstOrDefault().StatusName,
                    //    BookBarcode = GenerateId.GenerateBacode(sd.BookDelId)
                    //}).ToList(),
                }).SingleOrDefault();
                model.data = List;
                model.success = true;
            }
            catch (Exception ex)
            {
                model.success = false;
                model.message = ex.Message;
            }
            return model;
        }

        public async Task<Data> GetBookList(int pageSkip = 0, int pageSize = 10, bool isSearch = false, string? BookNameKh = null, int? CateId = 0, string? LocId = null, string? BookDelId = null, int BookId =0)
        {
            Data model = new Data();
            try
            {
                if (isSearch == false)
                {
                    var List = db.TblItiBook.Where(w=>BookId == 0 || w.BookId == BookId).Select(s => new
                    {
                        s.BookId,
                        s.BookNameKh,
                        s.BookNameEn,
                        BookYear = Convert.ToDateTime(s.BookYear).ToString("dd-MM-yyyy"),
                        s.BookQty,
                        s.CateId,
                        CateNameKh = s.CateId == null ? "" : db.TblItiCategory.Where(w => w.CateId == s.CateId).FirstOrDefault().CateNameKh,
                        //BookPhoto = GetImage(s.BookPhoto),
                        s.BookAuthor,
                        s.BookDescription,
                        FilePath = new ReadPath(environment).GetImage(s.FilePath),
                        s.LocId,
                        LocLabel = s.LocId == null ? "" : db.TblItiLocation.Where(l => l.LocId == s.LocId).FirstOrDefault().LocLabel,
                        bookDatails = db.TblItiBookdetails.Where(w => w.BookId == s.BookId
                        && (BookDelId == null || w.BookDelId == BookDelId)).Select(sd => new
                        {
                            sd.BookDelId,
                            BookNameKh = sd.BookId == null ? "" : db.TblItiBook.Where(w => w.BookId == s.BookId).FirstOrDefault().BookNameKh,
                            sd.ImpId,
                            ImpDate = sd.ImpId == null ? null : Convert.ToDateTime(db.TblItiImport.Where(w => w.ImpId == sd.ImpId).FirstOrDefault().ImpDate).ToString("dd-MM-yyyy"),
                            LocLabel = s.LocId == null ? "" : db.TblItiLocation.Where(l => l.LocId == s.LocId).FirstOrDefault().LocLabel,
                            sd.BookDelLabel,
                            BookDelLabelPrint = sd.ImpId == null ? null : "Loc:" + db.TblItiLocation.Where(w => w.LocId == s.LocId).FirstOrDefault().LocLabel + "/Imp:" + Convert.ToDateTime(db.TblItiImport.Where(w => w.ImpId == sd.ImpId).FirstOrDefault().ImpDate).ToString("dd-MM-yyyy") + "/No:" + sd.BookDelLabel,
                            sd.StatusId,
                            StatusName = sd.StatusId == null ? "" : db.TblItiStatus.Where(w => w.StatusId == sd.StatusId).FirstOrDefault().StatusName,
                            BookBarcode = GenerateId.GenerateBacode(sd.BookDelId,Color.Black,Color.White)
                        }).OrderBy(o => o.ImpId).Skip(pageSkip).Take(pageSize).ToList(),
                    })/*.Skip(pageSkip).Take(pageSize)*/.ToList();
                    model.data = List;
                    model.success = true;
                    model.total_qty = (int)db.TblItiBook.Where(w=>BookId == 0 || w.BookId == BookId).Sum(s => s.BookQty);
                    model.setTotalRecord = db.TblItiBook.Where(w => BookId == 0 || w.BookId == BookId).Count();
                    model.total_record_detail = new List<DetailRecord>();
                    foreach (var i in List)
                    {
                        DetailRecord record = new DetailRecord();
                        record.id = i.BookId;
                        record.total_details_record = db.TblItiBookdetails.Where(w => w.BookId == i.BookId).Count();
                        model.total_record_detail.Add(record);
                    }
                }
                else
                {
                    // check
                    var check = db.TblItiBookdetails.Where(w => w.BookDelId == BookDelId).FirstOrDefault();
                    if (check == null)
                    {
                        check = new TblItiBookdetails();
                    }
                    var List = db.TblItiBook.AsEnumerable().Where(w => (BookNameKh == null || w.BookNameKh.ToLower().Contains(BookNameKh.ToLower()) || w.BookNameEn.ToLower().Contains(BookNameKh.ToLower()))
                    && (CateId == 0 || w.CateId.Equals(CateId))
                    && (LocId == null || w.LocId.Equals(LocId))
                    && (BookDelId == null || w.BookId == check.BookId)
                    && (BookId == 0 || w.BookId == BookId)).Select(s => new
                    {
                        s.BookId,
                        s.BookNameKh,
                        s.BookNameEn,
                        BookYear = Convert.ToDateTime(s.BookYear).ToString("dd-MM-yyyy"),
                        s.BookQty,
                        s.CateId,
                        CateNameKh = s.CateId == null ? "" : db.TblItiCategory.Where(w => w.CateId == s.CateId).FirstOrDefault().CateNameKh,
                        //BookPhoto = GetImage(s.BookPhoto),
                        s.BookAuthor,
                        s.BookDescription,
                        FilePath = new ReadPath(environment).GetImage(s.FilePath),
                        s.LocId,
                        LocLabel = s.LocId == null ? "" : db.TblItiLocation.Where(l => l.LocId == s.LocId).FirstOrDefault().LocLabel,
                        bookDatails = db.TblItiBookdetails.Where(w => w.BookId == s.BookId
                        && (BookDelId == null || w.BookDelId == BookDelId)).Select(sd => new
                        {
                            sd.BookDelId,
                            BookNameKh = sd.BookId == null ? "" : db.TblItiBook.Where(w => w.BookId == s.BookId).FirstOrDefault().BookNameKh,
                            sd.ImpId,
                            ImpDate = sd.ImpId == null ? null : Convert.ToDateTime(db.TblItiImport.Where(w => w.ImpId == sd.ImpId).FirstOrDefault().ImpDate).ToString("dd-MM-yyyy"),
                            LocLabel = s.LocId == null ? "" : db.TblItiLocation.Where(l => l.LocId == s.LocId).FirstOrDefault().LocLabel,
                            sd.BookDelLabel,
                            BookDelLabelPrint = sd.ImpId == null ? null : "Loc:" + db.TblItiLocation.Where(w => w.LocId == s.LocId).FirstOrDefault().LocLabel + "/Imp:" + Convert.ToDateTime(db.TblItiImport.Where(w => w.ImpId == sd.ImpId).FirstOrDefault().ImpDate).ToString("dd-MM-yyyy") + "/No:" + sd.BookDelLabel,
                            sd.StatusId,
                            StatusName = sd.StatusId == null ? "" : db.TblItiStatus.Where(w => w.StatusId == sd.StatusId).FirstOrDefault().StatusName,
                            BookBarcode = GenerateId.GenerateBacode(sd.BookDelId,Color.Black,Color.White)
                        }).OrderBy(o => o.ImpId).Skip(pageSkip).Take(pageSize).ToList(),
                    })/*.Skip(pageSkip).Take(pageSize)*/.ToList();
                    model.data = List;
                    model.success = true;
                    model.total_qty = (int)db.TblItiBook.AsEnumerable().Where(w => (BookNameKh == null || w.BookNameKh.ToLower().Contains(BookNameKh.ToLower()) || w.BookNameEn.ToLower().Contains(BookNameKh.ToLower()))
                    && (CateId == 0 || w.CateId.Equals(CateId))
                    && (LocId == null || w.LocId.Equals(LocId))
                    && (BookDelId == null || w.BookId == check.BookId)
                    && (BookId == 0 || w.BookId == BookId)).Sum(s => s.BookQty);
                    model.setTotalRecord = List.Count();
                    model.total_record_detail = new List<DetailRecord>();
                    foreach (var i in List)
                    {
                        DetailRecord record = new DetailRecord();
                        record.id = i.BookId;
                        record.total_details_record = db.TblItiBookdetails.Where(w => w.BookId == i.BookId).Count();
                        model.total_record_detail.Add(record);
                    }

                }
            }
            catch (Exception ex)
            {
                model.success = false;
                model.message = ex.Message;
            }
            return model;
        }

        public async Task<Data> PostBookInfo(BookModel book)
        {
            Data model = new Data();
            var bookYear = ConvertDate.StringToDateTime(book.BookYear);
            try
            {
                if (book.BookId == 0)
                {
                    var check = db.TblItiBook.AsEnumerable().Where(w => w.BookNameKh.Equals(book.BookNameKh)
                    && w.BookAuthor == book.BookAuthor && w.BookYear.Equals(bookYear)).ToList();
                    if (check.Count == 0)
                    {
                        TblItiBook addBook = new TblItiBook();
                        addBook.BookNameKh = book.BookNameKh;
                        addBook.BookNameEn = book.BookNameEn;
                        addBook.BookYear = bookYear;
                        addBook.CateId = book.CateId;
                        addBook.LocId = book.LocId;
                        addBook.BookQty = 0;
                        addBook.BookAuthor = book.BookAuthor;
                        addBook.BookDescription = book.BookDescription;
                        addBook.FilePath = string.IsNullOrEmpty(book.FileName) == false ? UploadImage(book.FileName, book.BookNameKh + "_" + book.BookNameEn) : null;
                        db.TblItiBook.Add(addBook);
                        db.SaveChanges();
                        model.success = true;
                        model.message = MessageStatus.SaveOK;
                    }
                    else
                    {
                        model.success = false;
                        model.message = MessageStatus.UnSave;
                    }
                }
                else
                {
                    //var checkUp = (from b in db.TblItiBook
                    //               join bd in db.TblItiBookdetails on b.BookId equals bd.BookId
                    //               where bd.StatusId == 1
                    //               select new { b, bd }).ToList();
                    //foreach (var item in checkUp)
                    //{
                    var update = db.TblItiBook.Where(w => w.BookId == book.BookId).SingleOrDefault();
                    //var checkUp = db.TblItiBookdetails.Where(w => w.BookId == update.BookId).ToList();
                    //if (checkUp.Count == 0)
                    //{
                    if (update != null)
                    {
                        update.BookNameKh = book.BookNameKh;
                        update.BookNameEn = book.BookNameEn;
                        update.BookYear = bookYear;
                        update.CateId = book.CateId;
                        update.LocId = book.LocId;
                        update.BookAuthor = book.BookAuthor;
                        update.BookDescription = book.BookDescription;
                        update.FilePath = string.IsNullOrEmpty(book.FileName) == false ? UploadImage(book.FileName, book.BookNameKh + "_" + book.BookNameEn) : null;
                        db.SaveChanges();
                        model.success = true;
                        model.message = MessageStatus.UpdateOK;
                    }
                }
            }
            catch (Exception ex)
            {
                model.success = false;
                model.message = ex.Message;
            }
            return model;
        }

        //      public async Task SavePostImage(BookModel book)
        //      {
        //          //string imgPath = "";

        //          //string stringReplace = "";
        //          //var fileName =FileHelper.GetUniqueFileName(book.Image.FileName);
        //          /*switch (extension)
        //          {
        //              case ".jpeg":
        //                  stringReplace = "data:image/jpeg;base64,";
        //                  extension = ".jpeg";
        //                  break;
        //              case ".png":
        //                  stringReplace = "data:image/png;base64,";
        //                  extension = ".png";
        //                  break;
        //              default://should write cases for more images types
        //                  stringReplace = "data:image/jpg;base64,";
        //                  extension = ".jpg";
        //                  break;
        //          }
        //          
        //          using var dataStream = new MemoryStream();
        //          //await book.Image.CopyTo(dataStream);
        //          byte[] imageBytes = dataStream.ToArray();
        //          var risizeImageBytes = Resized.resizeImage(imageBytes);
        //          string base64String = Convert.ToBase64String(risizeImageBytes);
        //          ///var image = stringReplace + "" + base64String;
        //          book.BookPhoto = base64String;// to save the image as base64String 
        //          return;
        //}
        private string UploadImage(string ImagePath, string ImageName)
		{
			string imgPath = "";
			string extension = "";
			string stringReplace = "";
			if (!string.IsNullOrEmpty(ImagePath))
			{
				string[] strings = ImagePath.Split(',');
				switch (strings[0])
				{//check image's extension
					case "data:image/jpeg;base64":
						stringReplace = "data:image/jpeg;base64,";
						extension = ".jpeg";
						break;
					case "data:image/png;base64":
						stringReplace = "data:image/png;base64,";
						extension = ".png";
						break;
					default://should write cases for more images types
						stringReplace = "data:image/jpg;base64,";
						extension = ".jpg";
						break;
				}
				ImagePath = ImagePath.Replace(stringReplace, "");
				string path = environment.WebRootPath + "/images/";
				var imageBytes = Convert.FromBase64String(ImagePath);
				if (!Directory.Exists(path))
				{
					Directory.CreateDirectory(path);
				}
				imgPath = Path.Combine(path, ImageName + extension);
				System.IO.File.WriteAllBytes(imgPath, imageBytes);
			}
			return String.Concat(ImageName, extension);
		}
        
    }
}

