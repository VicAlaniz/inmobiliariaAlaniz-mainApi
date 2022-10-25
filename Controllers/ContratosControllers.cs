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
    
    public class ContratosController : Controller
    {
        RepositorioContrato repo;
        RepositorioInquilino repositorioInquilino;
        RepositorioInmueble repositorioInmueble;

        public ContratosController() {
            repo = new RepositorioContrato();
            repositorioInquilino = new RepositorioInquilino();
            repositorioInmueble = new RepositorioInmueble();
        }
      
        public ActionResult Index()
        {
            try{
                var contrato = repo.ObtenerTodos();
                 /*if (TempData.ContainsKey("Id"))
                ViewBag.Id = TempData["Id"];
                if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];*/
                return View(contrato);
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
        public ActionResult Create()
        {
            try {
                ViewBag.Inquilinos = repositorioInquilino.ObtenerTodos();
                ViewBag.Inmuebles = repositorioInmueble.ObtenerTodos(); 
                return View();
            }
            catch (Exception ex) {
                throw;
            }
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create(Contrato contrato)
        {
           try
            {
                // TODO: Add insert logic here
                //if (ModelState.IsValid) {
                    repo.Alta(contrato);
                    //TempData["Id"] = inmueble.Id;
                    return RedirectToAction(nameof(Index));
                /*}
                else {
                    ViewBag.Propietarios = repoProp.ObtenerTodos();
                    return View(inmueble);
                }*/
            }
            catch
            {
                throw;
            }
        }

        [Authorize]
        public ActionResult Edit(int id)
        {
            try {
                var contrato = repo.ObtenerPorId(id);
                ViewBag.Inquilinos = repositorioInquilino.ObtenerTodos();
                ViewBag.Inmuebles = repositorioInmueble.ObtenerTodos();
               
                return View(contrato);
            }
            catch (Exception ex) {
                throw;
            }
            
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit(int id, Contrato contrato)
        {
            Contrato contrato1 = null;

            try
            {
                // TODO: Add update logic here
                contrato1 = repo.ObtenerPorId(id);
                contrato1.IdInquilino = contrato.IdInquilino;
                contrato1.IdInmueble = contrato.IdInmueble;
                contrato1.FechaInicio = contrato.FechaInicio;
                contrato1.FechaFin = contrato.FechaFin;
             
                repo.Modificacion(contrato1);
                
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
                var contrato = repo.ObtenerPorId(id);
                
                return View(contrato);
            }
            catch (Exception ex) {
                throw;
            }
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id, Contrato contrato)
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