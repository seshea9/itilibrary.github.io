using System;

namespace ITI_Libraly_Api.Utilities
{
    public class Upload
    {
        private readonly IWebHostEnvironment environment;

        public Upload(IWebHostEnvironment environment)
        {
            this.environment = environment;
        }
       public string UploadImage(string ImagePath, string ImageName)
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
