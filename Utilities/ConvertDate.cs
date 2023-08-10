namespace ITI_Libraly_Api.Utilities
{
    public class ConvertDate
    {
        public static DateTime? StringToDateTime(string? date)
        {
            if(date == null)
            {
                return null;
            }
            var dob_spit = date.Split("-");
            var dob = new DateTime(Convert.ToInt32(dob_spit[2]), Convert.ToInt32(dob_spit[1]), Convert.ToInt32(dob_spit[0]));
            return dob;
        }
    }
}
