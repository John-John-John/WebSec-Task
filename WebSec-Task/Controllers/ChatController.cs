 using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebSec_Task;
using WebSec_Task.Models;

namespace WebSec_Task.Controllers
{
    public class ChatController : Controller
    {
        private readonly SqlContext _context;

        private string[] _tags = new string[] { "<b>", "</b>", "<i>", "</i>" };
        



        public ChatController(SqlContext context)
        {
            _context = context;
        }

        // GET: Chat
        public async Task<IActionResult> Index()
        {
              return View(await _context.Messages.ToListAsync());
        }

        // GET: Chat/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Messages == null)
            {
                return NotFound();
            }

            var chatMessageEntity = await _context.Messages
                .FirstOrDefaultAsync(m => m.Id == id);
            if (chatMessageEntity == null)
            {
                return NotFound();
            }

            return View(chatMessageEntity);
        }

        // GET: Chat/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Chat/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Author,Content")] ChatMessageEntity chatMessageEntity)
        {
            if (ModelState.IsValid)
            {
                string encodedContent = HttpUtility.HtmlEncode(chatMessageEntity.Content);
                foreach(var tag in _tags) 
                {
                    var encodedTag = HttpUtility.HtmlEncode(tag);
                    encodedContent = encodedContent.Replace(encodedTag, tag);
                }

                chatMessageEntity.Content = encodedContent;



                _context.Add(chatMessageEntity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(chatMessageEntity);
        }

        // GET: Chat/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Messages == null)
            {
                return NotFound();
            }

            var chatMessageEntity = await _context.Messages.FindAsync(id);
            if (chatMessageEntity == null)
            {
                return NotFound();
            }
            return View(chatMessageEntity);
        }

        // POST: Chat/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Author,Content")] ChatMessageEntity chatMessageEntity)
        {
            if (id != chatMessageEntity.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(chatMessageEntity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChatMessageEntityExists(chatMessageEntity.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(chatMessageEntity);
        }

        // GET: Chat/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Messages == null)
            {
                return NotFound();
            }

            var chatMessageEntity = await _context.Messages
                .FirstOrDefaultAsync(m => m.Id == id);
            if (chatMessageEntity == null)
            {
                return NotFound();
            }

            return View(chatMessageEntity);
        }

        // POST: Chat/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Messages == null)
            {
                return Problem("Entity set 'SqlContext.Messages'  is null.");
            }
            var chatMessageEntity = await _context.Messages.FindAsync(id);
            if (chatMessageEntity != null)
            {
                _context.Messages.Remove(chatMessageEntity);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ChatMessageEntityExists(int id)
        {
          return _context.Messages.Any(e => e.Id == id);
        }
    }
}
