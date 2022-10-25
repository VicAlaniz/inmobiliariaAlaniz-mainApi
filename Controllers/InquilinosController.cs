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
    
    public class InquilinosController : Controller
    {
        RepositorioInquilino repo;

        public InquilinosController() {
            repo = new RepositorioInquilino();
        }
        // GET: Inquilinos
        public ActionResult Index()
        {
            try{
                IList<Inquilino> inquilino = repo.ObtenerTodos();
                return View(inquilino);
            }
            catch (Exception ex) {
                throw;
            }
        }

        // GET: Inquilinos/Details/5
        [Authorize]
        public ActionResult Details(int id)
        {
            try {
                var inquilino = repo.ObtenerPorId(id);
                return View(inquilino);
            }
            catch (Exception ex) {
                throw;
            }
        }

        // GET: Inquilinos/Create
        [Authorize]
        public ActionResult Create()
        {
            try {
            return View();
            }
            catch (Exception ex) {
                throw;
            }
        }

        // POST: Inquilinos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create(Inquilino inquilino)
        {
           try
            {
                // TODO: Add insert logic here
                if (ModelState.IsValid) {
                    repo.Alta(inquilino);
                    TempData["Id"] = inquilino.Id;
                    return RedirectToAction(nameof(Index));
                }
                else {
                    return View(inquilino);
                }
            }
            catch
            {
                throw;
            }
        }

        // GET: Inquilinos/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            try {
                var inquilino = repo.ObtenerPorId(id);
                return View(inquilino);
            }
            catch (Exception ex) {
                throw;
            }
            
        }

        // POST: Inquilinos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit(int id, Inquilino i)
        {
            Inquilino inc = null;

            try
            {
                // TODO: Add update logic here
                inc = repo.ObtenerPorId(id);
                inc.Nombre = i.Nombre;
                inc.Apellido = i.Apellido;
                inc.Dni = i.Dni;
                inc.Email = i.Email;
                inc.Telefono = i.Telefono;
                repo.Modificacion(inc);
                
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        // GET: Inquilinos/Delete/5
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id)
        {
             try {
                var inquilino = repo.ObtenerPorId(id);
                return View(inquilino);
            }
            catch (Exception ex) {
                throw;
            }
        }

        // POST: Inquilinos/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id, Inquilino inquilino)
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