using NFCe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFCe.Models
{
    public class NotaFiscalModel
    {
        private NotaFiscal Nota { get; set; }
        private Local Local { get; set; }
        private List<Produto> Produtos { get; set; }

        public NotaFiscalModel(NotaFiscal nota, Local local)
        {
            Nota = nota;
            Local = local;
            Produtos = new List<Produto>();
        }

        public bool AddProduto(Produto produto)
        {
            try
            {
                Produtos.Add(produto);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public Produto GetProduto(string nome)
        {
            return Produtos.Find(n => n.Descricao == nome);
        }

        public void ShowNota()
        {
            Console.WriteLine("   ------------------");
            Console.WriteLine("   Id: " + Nota.Id);
            Console.WriteLine("   Id Lugar: " + Nota.IdLocal);
            Console.WriteLine("   Chave Acesso: " + Nota.ChaveAcesso);
            Console.WriteLine("   Data Emissao: " + Nota.DataEmissao);
            Console.WriteLine("   Valor Compra: " + Nota.ValorCompra);
            Console.WriteLine("   Valor Desconto: " + Nota.ValorDesconto);
            Console.WriteLine("   Lugar: " + Local.Nome);

            Console.WriteLine("   ------------------");
            foreach (Produto p in Produtos)
            {
                //Console.WriteLine("  " + p.Id);
                //Console.WriteLine("  " + p.IdLocal);
                //Console.WriteLine("  " + p.IdNota);
                Console.Write("  " + p.Descricao);
                Console.Write("  " + p.Qtd);
                Console.Write("  " + p.ValorPago);
                Console.WriteLine("  " + p.ValorUnidade);
            }

            Console.WriteLine("   ------------------");
        }
    }
}
