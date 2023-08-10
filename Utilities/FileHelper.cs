using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ITI_Libraly_Api.Utilities
{
    public class FileHelper
    {
        public static string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            //var c = Path.GetExtension(fileName);
            //return c;
            return string.Concat(Path.GetFileNameWithoutExtension(fileName)
                , "-"
                ,Guid.NewGuid().ToString().AsSpan(0,4)
                ,Path.GetExtension(fileName));
        }
    }
}
