using System;
using System.Web.Mvc;
using NHibernate;
using POC.VisitTracker.Helpers;
using POC.VisitTracker.Models;
using System.Linq;

namespace POC.VisitTracker.Controllers
{
    public class VisitController : Controller
    {
        /// <summary>
        /// GET: /Visit/Index
        /// Lists all visits in a grid
        /// </summary>
        public ActionResult Index()
        {
            try
            {
                using (IStatelessSession session = NHibernateHelper.OpenStatelessSession())
                {
                    var visitsList = session.QueryOver<Visit>()
                        .OrderBy(x => x.VisitDate).Desc
                        .List()
                        .ToList();

                    var model = new VisitViewModel
                    {
                        VisitsList = visitsList,
                        TotalVisits = visitsList.Count
                    };

                    return View(model);
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error loading visits: " + ex.Message;
                return View(new VisitViewModel());
            }
        }

        /// <summary>
        /// GET: /Visit/Edit or /Visit/Edit/5
        /// Returns partial view for Add (empty form) or Edit (pre-filled)
        /// </summary>
        public ActionResult Edit(int? id)
        {
            Visit visitToEdit = new Visit();

            if (id.HasValue)
            {
                using (IStatelessSession session = NHibernateHelper.OpenStatelessSession())
                {
                    visitToEdit = session.Get<Visit>(id.Value);
                    if (visitToEdit == null)
                    {
                        TempData["ErrorMessage"] = "Visit not found.";
                        return RedirectToAction("Index");
                    }
                }
            }
            else
            {
                visitToEdit.VisitDate = DateTime.Now;
                visitToEdit.Status = "New";
            }

            return PartialView("_EditVisit", visitToEdit);
        }

        /// <summary>
        /// POST: /Visit/Edit
        /// Binding: Form data auto-maps to Visit model
        /// Saves to DB via NHibernate stateless session in transaction
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Visit model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return PartialView("_EditVisit", model);
                }

                using (IStatelessSession session = NHibernateHelper.OpenStatelessSession())
                {
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        try
                        {
                            if (model.Id == 0)
                            {
                                model.CreatedDate = DateTime.Now;
                                session.Insert(model);
                            }
                            else
                            {
                                session.Update(model);
                            }

                            transaction.Commit();
                            TempData["SuccessMessage"] = model.Id == 0
                                ? "Visit created successfully."
                                : "Visit updated successfully.";

                            return RedirectToAction("Index");
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            TempData["ErrorMessage"] = "Error saving visit: " + ex.Message;
                            return PartialView("_EditVisit", model);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Unexpected error: " + ex.Message;
                return PartialView("_EditVisit", model);
            }
        }

        /// <summary>
        /// GET: /Visit/Delete/5
        /// Deletes a visit by ID
        /// </summary>
        public ActionResult Delete(int id)
        {
            try
            {
                using (IStatelessSession session = NHibernateHelper.OpenStatelessSession())
                {
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        var visit = session.Get<Visit>(id);
                        if (visit == null)
                        {
                            TempData["ErrorMessage"] = "Visit not found.";
                            return RedirectToAction("Index");
                        }

                        session.Delete(visit);
                        transaction.Commit();

                        TempData["SuccessMessage"] = "Visit deleted successfully.";
                        return RedirectToAction("Index");
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error deleting visit: " + ex.Message;
                return RedirectToAction("Index");
            }
        }
    }
}
