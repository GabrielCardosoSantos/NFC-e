using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TesteNFCe
{
    class Program
    {

        static async void Search()
        {
            string url = "https://www.sefaz.rs.gov.br/NFCE/NFCE-COM.aspx?p=43191187397865002165652070001264771004360527|2|1|1|3B9666B4B19EDA74307BAE1F059795CA425F036A";
            string url1 = "https://www.sefaz.rs.gov.br/ASP/AAE_ROOT/NFE/SAT-WEB-NFE-NFC_QRCODE_1.asp?p=43191187397865002165652070001264771004360527%7C2%7C1%7C1%7C3B9666B4B19EDA74307BAE1F059795CA425F036A";
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var response = await client.GetAsync(url1);

                    if (response.IsSuccessStatusCode)
                    {
                        string json = await response.Content.ReadAsStringAsync();
                    }
                    else
                    {

                    }
                }
            }
            catch (Exception)
            {
                return;
            }
        }

        static void Main(string[] args)
        {
            Search();
            Console.ReadKey();
        }
    }
}
