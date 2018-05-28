using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LojaOnline.Models
{
    public class ItensCarrinho
    {
        public int CarrinhoId;
        public int ProdutoId;
        public int UsuarioId;
        public int Quantidade;
        public string Produto;
        public double Valor;
    }
}