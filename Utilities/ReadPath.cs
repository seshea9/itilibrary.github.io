using Microsoft.AspNetCore.Hosting;

namespace ITI_Libraly_Api.Utilities
{
	public class ReadPath
	{
        private readonly IWebHostEnvironment environment;

        public ReadPath(IWebHostEnvironment environment)
		{
			this.environment = environment;

        }
			public string GetImage(string ImagePath)
		{
			
			string base64 = "";
			try
			{
				if (ImagePath != null)
				{
					string path = Path.Combine(environment.WebRootPath, "images/") + ImagePath.Trim();
					byte[] bytes = System.IO.File.ReadAllBytes(path);

					base64 = Convert.ToBase64String(bytes);
					var extension = ImagePath.Trim().Split(".");
					//base64 = "data:image/" + extension[1] + ";base64," + base64;

					
				}
			}
			catch (System.Exception ex)
			{
				base64 = ex.Message;
			}
			return base64;
		}
	}
}
