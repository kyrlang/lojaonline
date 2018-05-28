using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LojaOnline.DAO;
using LojaOnline.Models;

namespace LojaOnline.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            ProdutoDAO produto = new ProdutoDAO();
            IList<Produtos> prod = produto.Listar();
            IList<Produtos> newProd = new List<Produtos>();

            Random random = new Random();
            int numero = 0;

            for (int i = 0; i < 4; i++)
            {
                numero = random.Next(0, prod.Count);
                newProd.Add(prod[numero]);
            }

            ViewBag.Produtos = newProd;
            return View();

        }

        public ActionResult Contato()
        {
            return View();
        }

        public ActionResult Sobre()
        {
            return View();
        }
    }
}