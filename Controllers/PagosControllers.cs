using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using InmobiliariaAlaniz.Models;
using Microsoft.AspNetCore.Authorization;

namespace InmobiliariaAlaniz.Controllers
{
    
    public class PagosController : Controller
    {
        RepositorioPago repo;
        RepositorioContrato repositorioContrato;
  
        
        public PagosController() {
            repo = new RepositorioPago();
            repositorioContrato = new RepositorioContrato();
          
        }
        
        public ActionResult Index()
        {
            try{
                var pago = repo.ObtenerTodos();
                 /*if (TempData.ContainsKey("Id"))
                ViewBag.Id = TempData["Id"];
                if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];*/
                return View(pago);
            }
            catch (Exception ex) {
                throw;
            }
        }
        [Authorize]
        public ActionResult Details(int id)
        {
            try {
                var contrato = repo.ObtenerPorId(id);
                return View(contrato);
            }
            catch (Exception ex) {
                throw;
            }
        }

        [Authorize]
        public ActionResult Create(int id)
        {
            try {
                
                ViewBag.Contratos = repositorioContrato.ObtenerTodos();
                return View();
            }
            catch (Exception ex) {
                throw;
            }
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create(int id, Pago pago)
        {
           try
            {
                 repo.Alta(pago);
                 TempData["mensaje"] = "Pago guardado correctamente";
                 return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        [Authorize]
        public ActionResult Edit(int id)
        {
            try {
                var pago = repo.ObtenerPorId(id);
                ViewBag.Contratos = repositorioContrato.ObtenerTodos();

          
                return View(pago);
            }
            catch (Exception ex) {
                throw;
            }
            
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Pago pago)
        {
            Pago p = null;

            try
            {
                // TODO: Add update logic here
                p = repo.ObtenerPorId(id);
                p.FechaPago = pago.FechaPago;
                p.Importe = pago.Importe;
                p.IdContrato = pago.IdContrato;             
                repo.Modificacion(p);
                
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id)
        {
             try {
                var pago = repo.ObtenerPorId(id);
               
                return View(pago);
            }
            catch (Exception ex) {
                throw;
            }
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id, Pago pago)
        {
           try
            {
                // TODO: Add delete logic here
                repo.Baja(id);
                TempData["Mensaje"] = "Eliminado correctamente";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}