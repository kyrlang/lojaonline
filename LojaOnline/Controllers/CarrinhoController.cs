using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LojaOnline.DAO;
using LojaOnline.Models;

namespace LojaOnline.Controllers
{
    public class CarrinhoController : Controller
    {
        // GET: Carrinho
        public ActionResult Index()
        {
            if (Session["ItensPedido"] != null)
                return View();
            else
                return RedirectToAction("Index", "Home");
        }

        public ActionResult AdicionarItem(int produtoId)
        {
            IList<ItensCarrinho> itensCarrinho = new List<ItensCarrinho>();
            ProdutoDAO produto = new ProdutoDAO();
            Produtos prod = new Produtos();

            if (Session["ItensPedido"] != null)
            {
                bool controle = false;
                IList<ItensCarrinho> itensCarrinhoAux = (List<ItensCarrinho>)Session["ItensPedido"];
                foreach (var item in itensCarrinhoAux)
                {
                    if (produtoId == item.ProdutoId)
                    {
                        controle = true;
                        prod = produto.ListarProdutoId(produtoId);
                        itensCarrinho.Add(adicionarItem(item.Produto, produtoId, item.Quantidade + 1, item.UsuarioId, ((item.Quantidade + 1) * prod.Preco)));
                    }
                    else
                        itensCarrinho.Add(adicionarItem(item.Produto, item.ProdutoId, item.Quantidade, item.UsuarioId, item.Valor));
                }

                if (!controle)
                {
                    prod = produto.ListarProdutoId(produtoId);
                    itensCarrinho.Add(adicionarItem(prod.Nome, prod.Id, 1, 0, prod.Preco));
                }
            }
            else
            {
                prod = produto.ListarProdutoId(produtoId);
                itensCarrinho.Add(adicionarItem(prod.Nome, prod.Id, 1, 0, prod.Preco));
            }

            Session["ItensPedido"] = itensCarrinho;
            return View("Index", Session["ItensPedido"]);
        }

        public ActionResult RemoverItem(int produtoId)
        {
            IList<ItensCarrinho> itensCarrinho = new List<ItensCarrinho>();
            ProdutoDAO produto = new ProdutoDAO();
            Produtos prod = new Produtos();

            if (Session["ItensPedido"] != null)
            {
                IList<ItensCarrinho> itensCarrinhoAux = (List<ItensCarrinho>)Session["ItensPedido"];
                foreach (var item in itensCarrinhoAux)
                {
                    int quantidade = item.Quantidade - 1;

                    if (produtoId == item.ProdutoId)
                    {
                        if (quantidade == 0)
                            itensCarrinho.Remove(item);
                        else
                        {
                            prod = produto.ListarProdutoId(produtoId);
                            itensCarrinho.Add(adicionarItem(item.Produto, produtoId, quantidade, item.UsuarioId, (quantidade) * prod.Preco));
                        }
                    }
                    else
                        itensCarrinho.Add(item);

                }
            }

            if (itensCarrinho.Count == 0)
            {
                Session["ItensPedido"] = null;
                return RedirectToAction("Index", "Home");
            }
            else
                Session["ItensPedido"] = itensCarrinho;

            return View("Index", Session["ItensPedido"]);
        }

        public ActionResult Finalizar()
        {
            if (Session["usuarioLogado"] == null)
                return RedirectToAction("Index", "Home");
            else
                EnviarEmail();

            return View();
        }

        private ItensCarrinho adicionarItem(string produto, int produtoId, int quantidade, int usuarioId, double valor) {
            ItensCarrinho itensCarrinho = new ItensCarrinho();
            itensCarrinho.Produto = produto;
            itensCarrinho.ProdutoId = produtoId;
            itensCarrinho.Quantidade = quantidade;
            itensCarrinho.UsuarioId = usuarioId;
            itensCarrinho.Valor = valor;
            return itensCarrinho;
        }

        private void EnviarEmail() {

            IList<ItensCarrinho> itensCarrinhoAux = (List<ItensCarrinho>)Session["ItensPedido"];
            string html = string.Empty;
            html = @"<htmll>
                        <head></head>
                        <title></title>
                        <body>
                    <table class=""table table-hover"">
                        <thead>
                            <tr>
                                <th scope = ""col"">Produto</th>
                                <th scope = ""col"">Quantidade</th>
                                <th scope = ""col"">Valor</th>
                            </tr>
                        </thead>
                        <tbody>";

            string html2 = string.Empty;
            double valor = 0;
                

            foreach (var item in itensCarrinhoAux)
            {
                html2 = @"<tr>
                            <td>" + item.Produto + @"</td>" +
                           "<td> " + item.Quantidade + @" </td>" +
                           "<td> " + item.Valor.ToString("0:N") + @" </td>";

                valor = valor + item.Valor;
            }

            html = html + html2 + @"</tbody>
                                    </table>";
            html = @"<h2>O valor total do pedido é: R$ " + valor.ToString("0:N") + @"</h2>" +
                                                                                @"</body>
                                                                            </html>";

        }
    }
}