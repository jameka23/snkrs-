using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using sneakers.Data;
using sneakers.Models;
using sneakers.Models.MessagesViewModels;

namespace sneakers.Controllers
{
    public class MessagesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _config;

        public SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }
        public MessagesController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager, IConfiguration config)
        {
            _context = context;
            _userManager = userManager;
            _config = config;
        }

        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        // GET: Messages
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Message.Include(m => m.Sender).Include(m => m.Sneaker);
            return View(await applicationDbContext.ToListAsync());
        }


        // GET: This method will return a list of conversations
        public IActionResult ListConversations()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    // to get the user as a sender convo
                    cmd.CommandText = @"SELECT DISTINCT m.SneakerId, m.SenderId
                                        FROM Message m
                                        INNER JOIN Sneakers s ON s.SneakerId = m.SneakerId
                                        WHERE RecieverId = @theUserId
                                        AND S.UserId = @theUserId;";

                    // parameters
                    var currentUser = GetCurrentUserAsync();
                    cmd.Parameters.Add(new SqlParameter("@theUserId", currentUser.Id));

                    SqlDataReader reader = cmd.ExecuteReader();
                    List<ConversationsViewModel> Conversations = new List<ConversationsViewModel>();
                    

                    while (reader.Read())
                    {
                        ConversationsViewModel convo = new ConversationsViewModel
                        {
                            SneakerId = reader.GetInt32(reader.GetOrdinal("SneakerId")),
                            UserId = reader.GetString(reader.GetOrdinal("SenderId")),
                            Sneaker = _context.Sneaker.Find(reader.GetInt32(reader.GetOrdinal("SneakerId")))
                        };
                        Conversations.Add(convo);
                    }
                    reader.Close();

                    // to get the user as a receiver
                    cmd.CommandText = @"SELECT DISTINCT m.SneakerId, m.SenderId
                                        FROM Message m
                                        INNER JOIN Sneakers s ON s.SneakerId = m.SneakerId
                                        WHERE RecieverId = @theUserId
                                        AND NOT S.UserId = @theUserId;";

                    cmd.Parameters.Add(new SqlParameter("@theUserId", currentUser.Id));
                    SqlDataReader reader1 = cmd.ExecuteReader();

                    while (reader1.Read())
                    {
                        ConversationsViewModel convo = new ConversationsViewModel
                        {
                            SneakerId = reader1.GetInt32(reader1.GetOrdinal("SneakerId")),
                            UserId = reader1.GetString(reader1.GetOrdinal("SenderId")),
                            Sneaker = _context.Sneaker.Find(reader1.GetInt32(reader1.GetOrdinal("SneakerId")))
                        };
                        Conversations.Add(convo);
                    }
                    reader1.Close();
                    
                    return View(Conversations);
                }
            }
        }

        // GET: All this does is get the chatMessages for one conversation 
        public async Task<IActionResult> Chat(int sneakerId, string userId)
        {
            // userId is the buyer, sneakerId is for the sneaker for sale
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT MessageId, Msg, Date,
                                               SneakerId, SenderId, RecieverId
                                        FROM Message
                                        WHERE SneakerId = @sneakerId
                                        AND (SenderId = @userId
                                                OR RecieverId = @userId)
                                        ORDER BY Date;";

                    var currentUser = GetCurrentUserAsync();
                    cmd.Parameters.Add(new SqlParameter("@userId", userId));
                    cmd.Parameters.Add(new SqlParameter("@sneakerId", sneakerId));

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();
                    List<Message> messages = new List<Message>();

                    while (reader.Read())
                    {
                        Message message = new Message
                        {
                            MessageId = reader.GetInt32(reader.GetOrdinal("MessageId")),
                            Msg = reader.GetString(reader.GetOrdinal("Msg")),
                            Date = reader.GetDateTime(reader.GetOrdinal("Date")),
                            SneakerId = reader.GetInt32(reader.GetOrdinal("SneakerId")),
                            SenderId = reader.GetString(reader.GetOrdinal("SenderId")),
                            RecieverId = reader.GetString(reader.GetOrdinal("RecieverId"))
                        };
                        messages.Add(message);
                    }

                    reader.Close();
                    return View();
                }
            }
        }

        // This method will return the actual content(messages) in the conversation
        //public async Task<IActionResult> Chat()
        //{


        //    return View();
        //}


        // GET: Messages/Create
        public async Task<IActionResult> Create(int id)
        {
            // find the sender from the sneaker id that was passed in
            Sneaker sneaker = await _context.Sneaker.FindAsync(id);

            var currentUser = await GetCurrentUserAsync();

            var viewModel = new MessagesCreateViewModel
            {
                Sneaker = sneaker,
                SneakerOwnerId = sneaker.UserId,
                BuyerId = currentUser.Id
            };
            return View(viewModel);
        }

        // POST: Messages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MessagesCreateViewModel viewModel)
        {
            //ModelState.Remove("Message.Sneaker");
            //ModelState.Remove("Message.Sender");
            //ModelState.Remove("Message.Receiver");
            //ModelState.Remove("Message.SenderId");
            //ModelState.Remove("Message.ReceiverId");

            var currentUser = await GetCurrentUserAsync();
            
            if (ModelState.IsValid)
            {
                var message = viewModel.Message;
                message.SenderId = currentUser.Id;
                message.RecieverId = viewModel.SneakerOwnerId;

                _context.Add(message);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Chat));
            }
            
            return View(viewModel);
        }

        // GET: Messages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var message = await _context.Message
                .Include(m => m.Sender)
                .Include(m => m.Sneaker)
                .FirstOrDefaultAsync(m => m.MessageId == id);
            if (message == null)
            {
                return NotFound();
            }

            return View(message);
        }

        // GET: Messages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var message = await _context.Message.FindAsync(id);
            if (message == null)
            {
                return NotFound();
            }
            ViewData["SenderId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", message.SenderId);
            ViewData["SneakerId"] = new SelectList(_context.Sneaker, "SneakerId", "Description", message.SneakerId);
            return View(message);
        }

        // POST: Messages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MessageId,Msg,Date,SneakerId,SenderId,RecieverId")] Message message)
        {
            if (id != message.MessageId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(message);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MessageExists(message.MessageId))
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
            ViewData["SenderId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", message.SenderId);
            ViewData["SneakerId"] = new SelectList(_context.Sneaker, "SneakerId", "Description", message.SneakerId);
            return View(message);
        }

        // GET: Messages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var message = await _context.Message
                .Include(m => m.Sender)
                .Include(m => m.Sneaker)
                .FirstOrDefaultAsync(m => m.MessageId == id);
            if (message == null)
            {
                return NotFound();
            }

            return View(message);
        }

        // POST: Messages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var message = await _context.Message.FindAsync(id);
            _context.Message.Remove(message);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MessageExists(int id)
        {
            return _context.Message.Any(e => e.MessageId == id);
        }
    }
}
