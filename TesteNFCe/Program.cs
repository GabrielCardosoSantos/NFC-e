using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;
using NFCe;
using NFCe.Models;

namespace NFCe
{
    class Program
    {
        static public DateTime ProcessData(string str)
        {
            return Convert.ToDateTime(str.Remove(0, str.LastIndexOf("o: ") + 3)); ;
        }

        static public async Task<string> GetURLAsync(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var doc = new HtmlDocument();
                    doc.LoadHtml(await response.Content.ReadAsStringAsync());

                    var frame = doc.DocumentNode.SelectNodes("//iframe").First(n => n.Attributes["id"].Value == "iframeConteudo");
                    return frame.Attributes["src"].Value;
                }
                else
                {
                    return null;
                }
            }
        }

        static async Task<NotaFiscalModel> SearchItens(string url)
        {
            //string url = "https://www.sefaz.rs.gov.br/NFCE/NFCE-COM.aspx?p=43191187397865002165652070001264771004360527|2|1|1|3B9666B4B19EDA74307BAE1F059795CA425F036A";
            //string url1 = "https://www.sefaz.rs.gov.br/ASP/AAE_ROOT/NFE/SAT-WEB-NFE-NFC_QRCODE_1.asp?p=43191187397865002165652070001264771004360527%7C2%7C1%7C1%7C3B9666B4B19EDA74307BAE1F059795CA425F036A";
            //string url1 = "https://www.sefaz.rs.gov.br/ASP/AAE_ROOT/NFE/SAT-WEB-NFE-NFC_QRCODE_1.asp?p=43191175315333012115655020002257541048579178|2|1|1|9EEF25B0ED906720BFF9849A163F4F54CAB69F75";
            NotaFiscal notaFiscal = new NotaFiscal();
            Local local = new Local();
            Produto produto = new Produto();

            url = await GetURLAsync(url);

            NotaFiscalModel notaFiscalModel = new NotaFiscalModel(notaFiscal, local);
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var response = await client.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        var doc = new HtmlDocument();
                        doc.LoadHtml(await response.Content.ReadAsStringAsync());

                        var value = doc.DocumentNode.SelectNodes("//td").Where(n => n.HasClass("NFCCabecalho_SubTitulo")).ToList();
                        local.Nome = value[0].InnerText;
                        notaFiscal.ChaveAcesso = value[5].InnerText;
                        notaFiscal.DataEmissao = ProcessData(value[2].InnerText);

                        value = doc.DocumentNode.SelectNodes("//td").Where(n => n.HasClass("NFCCabecalho_SubTitulo1")).ToList();
                        local.Endereco = Regex.Replace(value[1].InnerText, @"\s+", " ");

                        value = doc.DocumentNode.SelectNodes("//td").Where(n => n.HasClass("NFCDetalhe_Item")).ToList();
                        value.RemoveRange(0, 5);

                        for (int i = 0; i < value.Count; i += 5)
                        {
                            if (value[i].InnerText.Contains("Valor"))
                            {
                                notaFiscal.ValorCompra = Convert.ToDecimal(value[i + 1].InnerText);
                                notaFiscal.ValorDesconto = Convert.ToDecimal(value[i + 3].InnerText);

                                break;
                            }
                            produto.Descricao = value[i + 1].InnerText;
                            produto.Qtd = Convert.ToDecimal(value[i + 2].InnerText);
                            produto.TipoUnidade = value[i + 3].InnerText;
                            produto.ValorUnidade = Convert.ToDecimal(value[i + 4].InnerText);
                            produto.ValorPago = Convert.ToDecimal(value[i + 5].InnerText);

                            notaFiscalModel.AddProduto(produto);
                            produto = new Produto();
                            i++;
                        }

                        return notaFiscalModel;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception)
            { 
                return null;
            }
        }

        static void Main(string[] args)
        {
            var x = SearchItens("https://www.sefaz.rs.gov.br/NFCE/NFCE-COM.aspx?p=43191187397865002165652070001264771004360527|2|1|1|3B9666B4B19EDA74307BAE1F059795CA425F036A").Result;
            x.ShowNota();
            Console.ReadKey();
        }
    }
}
