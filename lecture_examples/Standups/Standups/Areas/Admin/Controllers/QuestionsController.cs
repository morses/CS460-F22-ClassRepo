using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Standups.DAL.Abstract;
using Standups.Data;
using Standups.Models;

namespace Standups.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    [Area("Admin")]
    public class QuestionsController : Controller
    {
        private readonly IQuestionRepository _questionRepository;

        public QuestionsController(IQuestionRepository questionRepository)
        {
            _questionRepository = questionRepository;
        }

        public IActionResult Index()
        {
            List<Supquestion> questions = _questionRepository.GetAll().OrderByDescending(q => q.Active).ToList();
            return View(questions);
        }

        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Supquestion q = _questionRepository.GetById((int)id);

            if (q == null)
            {
                return NotFound();
            }

            return View(q);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Question,Active")] Supquestion supquestion)
        {
            if (ModelState.IsValid)
            {
                supquestion.SubmissionDate = DateTime.UtcNow;
                _questionRepository.AddOrUpdate(supquestion);
                return RedirectToAction(nameof(Index));
            }
            return View(supquestion);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            int qid = (int)id;
            Supquestion question = _questionRepository.GetById(qid);

            if (question == null)
            {
                return NotFound();
            }
            return View(question);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,SubmissionDate,Question,Active")] Supquestion supquestion)
        {
            if (id != supquestion.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _questionRepository.AddOrUpdate(supquestion);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_questionRepository.Exists(supquestion.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        // TODO: handle this case
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(supquestion);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            int qid = (int)id;
            Supquestion question = _questionRepository.GetById(qid);

            if (question == null)
            {
                return NotFound();
            }

            return View(question);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            // NOT CURRENTLY IMPLEMENTED -- Need to handle comments already made for this question
            try
            {
                _questionRepository.DeleteById(id);
            }
            catch(Exception ex) 
            {
                // Delete failed because id didn't exist or failed at DB
                return BadRequest(ex.Message);
            }
            
            return RedirectToAction(nameof(Index));
        }
    }
}
