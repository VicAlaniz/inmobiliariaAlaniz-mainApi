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
   
    public class InmueblesController : Controller
    {
        RepositorioInmueble repo;
        RepositorioPropietario repoProp;

        public InmueblesController() {
            repo = new RepositorioInmueble();
            repoProp = new RepositorioPropietario();
        }
      
        public ActionResult Index()
        {
            try{
                IList<Inmueble> inmueble = repo.ObtenerTodos();
                 /*if (TempData.ContainsKey("Id"))
                ViewBag.Id = TempData["Id"];
                if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];*/
                return View(inmueble);
            }
            catch (Exception ex) {
                throw;
            }
        }

        [Authorize]
        public ActionResult Details(int id)
        {
            try {
                var inmueble = repo.ObtenerPorId(id);
                return View(inmueble);
            }
            catch (Exception ex) {
                throw;
            }
        }

        [Authorize]
        public ActionResult Create()
        {
            try {
                ViewBag.Propietarios = repoProp.ObtenerTodos();
                return View();
            }
            catch (Exception ex) {
                throw;
            }
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create(Inmueble inmueble)
        {
           try
            {
                // TODO: Add insert logic here
                //if (ModelState.IsValid) {
                    repo.Alta(inmueble);
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
                var inmueble = repo.ObtenerPorId(id);
                ViewBag.Propietarios = repoProp.ObtenerTodos();
              
                return View(inmueble);
            }
            catch (Exception ex) {
                throw;
            }
            
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit(int id, Inmueble i)
        {
            Inmueble inm = null;

            try
            {
                // TODO: Add update logic here
                inm = repo.ObtenerPorId(id);
                inm.Direccion = i.Direccion;
                inm.Uso = i.Uso;
                inm.Tipo = i.Tipo;
                inm.CantAmbientes = i.CantAmbientes;
                inm.Coordenadas = i.Coordenadas;
                inm.Precio = i.Precio;
                inm.PropietarioId = i.PropietarioId;
                repo.Modificacion(inm);
                
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
                var inmueble = repo.ObtenerPorId(id);
                
                return View(inmueble);
            }
            catch (Exception ex) {
                throw;
            }
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id, Inmueble inmueble)
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