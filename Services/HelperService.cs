using System.Security.Cryptography;
using System.Text;


namespace ServiceMonitoramentoWeb.Services
{
    public class HelperService
    {
        public enum ViewLevel
        {
            Admin, // CAN SEE RECORDS
            Coord, // CAN ONLY SEE RECORDS FROM THE SAME COORD
            Division, // CAN ONLY SEE THE RECORDS OF THE DIVISION
            User // CAN ONLY SEE RECORDS THAT ARE INVOLVED WITH
        }

        public string Encripta(string texto)
        {
            String retorno = "";
            String stexto = texto;
            if (stexto == "")
            {
                return stexto;
            }
            while (true)
            {
                String letra = stexto.Substring(0, 1);
                int nnumero = char.ConvertToUtf32(letra, 0);
                nnumero += 166;
                string snumero = nnumero.ToString();
                if (snumero.Length < 3)
                {
                    snumero = "0" + snumero;
                }
                if (snumero.Length < 3)
                {
                    snumero = "0" + snumero;
                }
                retorno += snumero;
                stexto = stexto.Substring(1);
                if (stexto == "")
                {
                    break;
                }
            }

            return retorno;
        }

        public string HashMd5(string text)
        {
            using MD5 md5Hash = MD5.Create();
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(text));
            StringBuilder sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }

        public string GetOsCode(string osCode, DateTime registerDate)
        {

            string formattedOs = osCode;

            if (osCode.Length == 1)
            {
                formattedOs = "00000" + osCode.ToString();
            }

            if (osCode.Length == 2)
            {
                formattedOs = "0000" + osCode;
            }

            if (osCode.Length == 3)
            {
                formattedOs = "000" + osCode;
            }

            if (osCode.Length == 4)
            {
                formattedOs = "00" + osCode;
            }

            if (osCode.Length == 5)
            {
                formattedOs = "0" + osCode;
            }

            formattedOs = formattedOs + "/" + registerDate.Year.ToString();

            return formattedOs;
        }

        public string GetTermCode(string termCode, string termType)
        {

            string formattedTerm = termCode;

            if (termCode.Length == 1)
            {
                formattedTerm = "00000" + termCode.ToString();
            }

            if (termCode.Length == 2)
            {
                formattedTerm = "0000" + termCode;
            }

            if (termCode.Length == 3)
            {
                formattedTerm = "000" + termCode;
            }

            if (termCode.Length == 4)
            {
                formattedTerm = "00" + termCode;
            }

            if (termCode.Length == 5)
            {
                formattedTerm = "0" + termCode;
            }

            formattedTerm = formattedTerm + "/" + DateTime.Now.Year.ToString();

            if (termType == "Termo de Visita")
            {
                formattedTerm = formattedTerm + "-TV";
            }

            if (termType == "Termo de Intimação")
            {
                formattedTerm = formattedTerm + "-TI";
            }

            if (termType == "Auto de Infração")
            {
                formattedTerm = formattedTerm + "-AI";
            }

            if (termType == "Auto de Multa")
            {
                formattedTerm = formattedTerm + "-AM";
            }

            return formattedTerm;
        }

        public ViewLevel GetViewLevel(int profileId)
        {
            if (profileId == 1 || profileId == 5 || profileId == 7 || profileId == 8 || profileId == 9 || profileId == 4)
            {
                return ViewLevel.Admin;
            }

            if (profileId == 2)
            {
                return ViewLevel.Coord;
            }

            if (profileId == 3 || profileId == 14)
            {
                return ViewLevel.Division;
            }

            return ViewLevel.User;
        }

        public string GetUsernameFromEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentException("Email cannot be null or empty", nameof(email));
            }

            int atIndex = email.IndexOf('@');
            if (atIndex > 0)
            {
                return email.Substring(0, atIndex);
            }
            else
            {
                return email;
            }
        }
    }


}