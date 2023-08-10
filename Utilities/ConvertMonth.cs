namespace ITI_Libraly_Api.Utilities
{
    public class ConvertMonth
    {
        public static string Month(int month)
        {
            string m= "";
            switch(month)
            {
                case 1: m += "មករា";
                    break;
                case 2: m += "កុម្ភៈ";
                    break;
                case 3: m += "មីនា";
                    break;
                case 4: m += "មេសា";
                    break;
                case 5: m += "ឧសភា";
                    break;
                case 6:
                    m += "មិថុនា";
                    break;
                case 7:
                    m += "កក្កដា";
                    break;
                case 8:
                    m += "សីហា";
                    break;
                case 9:
                    m += "កញ្ញា";
                    break;
                case 10:
                    m += "តុលា";
                    break;
                case 11:
                    m += "វិច្ឆិកា";
                    break;
                case 12:
                    m += "ធ្នូ";
                    break;
                default: m += null;
                    break;
            }
            return m;
        }
    }
}
