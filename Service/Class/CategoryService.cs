using ITI_Libraly_Api.Contexts;
using ITI_Libraly_Api.Models;
using ITI_Libraly_Api.MyModels;
using ITI_Libraly_Api.Service.Interface;
using ITI_Libraly_Api.Utilities;
using Microsoft.EntityFrameworkCore;


namespace ITI_Libraly_Api.Service.Class
{
    public class CategoryService : ICategoryService
    {
        ITI_LibraySystemContext db;
        

        public CategoryService(ITI_LibraySystemContext db)
        {
            this.db = db;
           
        }
        public async Task<Data> GetCategoryInfo(int CateId)
        {
            Data model = new Data();
            try
            {
                var list = db.TblItiCategory.Where(w => w.CateId == CateId).Select(s => new
                {
                    s.CateId,
                    s.CateNameKh,
                    s.CateNameEn,
                }).SingleOrDefault();
                model.data = list;
                model.success = true;
            }
            catch (Exception ex)
            {
                model.success = false;
                model.message = ex.Message;
            }
            return model;
        }

        public async Task<Data> GetCategoryList(/*int pageSkip = 0, int pageSize = 10,*/ bool isSearch = false, string? CateNameKh = null, string? CateNameEn = null)
        {
            Data model = new Data();
            try
            {

                //CZKEMClass axCZKEM1 = new CZKEMClass();
                //bool bIsConnected = false;
                ////the serial number of the device.After connecting the device ,this value will be changed.
                //int iMachineNumber = 1;
                //int idwErrorCode = 0;
                //// The string value identifies the IP the device.
                //string sIP = "172.19.17.55";// Floor 10
                ////The Int value identifies the Port the device.
                //string sPort = "4370";
                //bIsConnected = axCZKEM1.Connect_Net(sIP, Convert.ToInt32(sPort));
                //if (bIsConnected == true)
                //{
                //    iMachineNumber = 1;//In fact,when you are using the tcp/ip communication,this parameter will be ignored,that is any integer will all right.Here we use 1.
                //    //axCZKEM1.RegEvent(iMachineNumber, 65535);//Here you can register the realtime events that you want to be triggered(the parameters 65535 means registering all)
                //    if (axCZKEM1.RegEvent(iMachineNumber, 65535))
                //    {
                //       axCZKEM1.OnAttTransactionEx += new zkemkeeper._IZKEMEvents_OnAttTransactionExEventHandler(axCZKEM1_OnAttTransactionEx);
                //    }
                //    Console.WriteLine(bIsConnected);
                //}
                //else
                //{
                //    axCZKEM1.GetLastError(ref idwErrorCode);
                //    Console.WriteLine("Unable to connect the device,ErrorCode=" + idwErrorCode.ToString(), "Error");
                //}
                ////var runAt = DateTime.Today + new TimeSpan(14, 58, 20);


                ////if (runAt <= DateTime.Now)
                ////{
                ////    Result();
                ////}
                ////else
                ////{
                ////    var delay = runAt - DateTime.Now;

                ////   await System.Threading.Tasks.Task.Delay(delay).ContinueWith( e => Result());
                ////}
                ////void Result()
                ////{

                ////    var list = db.TblItiCategory.Select(s => new
                ////    {
                ////        s.CateId,
                ////        s.CateNameKh,
                ////        s.CateNameEn,
                ////    }).Skip(pageSkip).Take(pageSize).ToList();
                ////    model.data = list;
                ////    model.success = true;
                ////    model.setTotalRecord = db.TblItiCategory.Count();

                ////}
                ////Data model = new Data();
                ////CZKEMClass axCZKEM1 = new CZKEMClass();
                //////the boolean value identifies whether the device is connected
                ////bool bIsConnected = false;
                //////the serial number of the device.After connecting the device ,this value will be changed.
                ////int iMachineNumber = 1;
                ////int idwErrorCode = 0;
                ////// The string value identifies the IP the device.
                ////string sIP = "172.17.17.8";
                ////// The Int value identifies the Port the device.
                ////string sPort = "4370";
                ////bIsConnected = axCZKEM1.Connect_Net(sIP, Convert.ToInt32(sPort));
                ////if (bIsConnected == true)
                ////{
                ////    iMachineNumber = 1;//In fact,when you are using the tcp/ip communication,this parameter will be ignored,that is any integer will all right.Here we use 1.
                ////    axCZKEM1.RegEvent(iMachineNumber, 65535);//Here you can register the realtime events that you want to be triggered(the parameters 65535 means registering all)
                ////    Console.WriteLine("true");
                ////    model.message = "true";
                ////}
                ////else
                ////{
                ////    axCZKEM1.GetLastError(ref idwErrorCode);
                ////    Console.WriteLine("Unable to connect the device,ErrorCode=" + idwErrorCode.ToString(), "Error");
                ////    model.message = "false";
                ////}

                if (isSearch == false)
                {
                    var list = db.TblItiCategory.Select(s => new
                    {
                        s.CateId,
                        s.CateNameKh,
                        s.CateNameEn,
                    })/*.Skip(pageSkip).Take(pageSize)*/.ToList();
                    model.data = list;
                    model.success = true;
                    model.setTotalRecord = db.TblItiCategory.Count();
                }
                else
                {
                    var search = db.TblItiCategory.AsEnumerable().Where(w => (CateNameKh == null || w.CateNameKh.ToLower().Contains(CateNameKh.ToLower()))
                    && (CateNameEn == null || w.CateNameEn.ToLower().Contains(CateNameEn.ToLower()))).Select(s => new
                    {
                        s.CateId,
                        s.CateNameKh,
                        s.CateNameEn,
                    })/*.Skip(pageSkip).Take(pageSize)*/.ToList();
                    model.data = search;
                    model.success = true;
                    model.setTotalRecord = db.TblItiCategory.AsEnumerable().
                        Where(w => (CateNameKh == null || w.CateNameKh.ToLower().Contains(CateNameKh.ToLower()))
                        && (CateNameEn == null || w.CateNameEn.ToLower().Contains(CateNameEn.ToLower()))).Count();
                }
            }
             catch (Exception ex)
            {
                model.success = false;
                model.message = ex.ToString();
            }
            return model;
        }
        public async Task<Data> PostCategoryInfo(CategoryModel cate)
        {
            Data model = new Data();
            try
            {
                if(cate.CateId == 0)
                {
                    var check = db.TblItiCategory.AsEnumerable().Where(w => w.CateNameKh==cate.CateNameKh).ToList();
                    if(check.Count == 0)
                    {
                        TblItiCategory addcat = new TblItiCategory();
                        addcat.CateNameKh = cate.CateNameKh;
                        addcat.CateNameEn = cate.CateNameEn;
                        db.TblItiCategory.Add(addcat);
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
                    var updateCat = db.TblItiCategory.Where(w => w.CateId == cate.CateId).SingleOrDefault();
                    if(updateCat != null)
                    {
                        updateCat.CateNameKh = cate.CateNameKh;
                        updateCat.CateNameEn = cate.CateNameEn;
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
        //private void axCZKEM1_OnVerify(int iUserID)
        //{
        //    Console.WriteLine("RTEvent OnVerify Has been Triggered,Verifying...");
        //    if (iUserID != -1)
        //    {
        //        Console.WriteLine("Verified OK,the UserID is " + iUserID.ToString());
        //    }
        //    else
        //    {
        //        Console.WriteLine("Verified Failed... ");
        //    }
        //}
    }
}
