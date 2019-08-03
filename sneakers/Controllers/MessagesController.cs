using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize]
        // GET: This method will return a list of conversations
        public async Task<IActionResult> ListConversations()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    
                    cmd.CommandText = @"SELECT DISTINCT m.SneakerId, m.SenderId
                                        FROM Message m
                                        INNER JOIN Sneaker s ON s.SneakerId = m.SneakerId
                                        WHERE m.ReceiverId = @theUserId
                                        AND s.UserId = @theUserId;";

                    // parameters
                    var currentUser = await GetCurrentUserAsync();
                    cmd.Parameters.Add(new SqlParameter("@theUserId", currentUser.Id));

                    SqlDataReader reader = cmd.ExecuteReader();
                    List<ConversationsViewModel> Conversations = new List<ConversationsViewModel>();
                    

                    while (reader.Read())
                    {
                        ConversationsViewModel convo = new ConversationsViewModel
                        {
                            SneakerId = reader.GetInt32(reader.GetOrdinal("SneakerId")),
                            UserId = reader.GetString(reader.GetOrdinal("SenderId")),
                            Sneaker = _context.Sneaker.Find(reader.GetInt32(reader.GetOrdinal("SneakerId"))),
                            OtherUserId = reader.GetString(reader.GetOrdinal("SenderId")),
                            OtherUser = _context.Users.Find(reader.GetString(reader.GetOrdinal("SenderId")))
                        };
                        Conversations.Add(convo);
                    }
                    reader.Close();

                    
                    cmd.CommandText = @"SELECT DISTINCT m.SneakerId, m.ReceiverId
                                        FROM Message m
                                        INNER JOIN Sneaker s ON s.SneakerId = m.SneakerId
                                        WHERE m.SenderId = @curr
                                        AND s.UserId != @curr;";

                    cmd.Parameters.Add(new SqlParameter("@curr", currentUser.Id));
                    SqlDataReader reader1 = cmd.ExecuteReader();

                    while (reader1.Read())
                    {
                        ConversationsViewModel convo = new ConversationsViewModel
                        {
                            SneakerId = reader1.GetInt32(reader1.GetOrdinal("SneakerId")),
                            UserId = currentUser.Id,
                            Sneaker = _context.Sneaker.Find(reader1.GetInt32(reader1.GetOrdinal("SneakerId"))),
                            OtherUserId = reader1.GetString(reader1.GetOrdinal("ReceiverId")),
                            OtherUser = _context.Users.Find(reader1.GetString(reader1.GetOrdinal("ReceiverId")))
                        };
                        Conversations.Add(convo);
                        //ViewBag.OtherUserId = reader1.GetString(reader1.GetOrdinal("ReceiverId"));
                        //var otherPerson = _context.Users.Find(reader1.GetString(reader1.GetOrdinal("ReceiverId")));
                        //ViewBag.OtherUserName = otherPerson.FirstName;
                        
                    }
                    reader1.Close();
                    
                    return View(Conversations);
                }
            }
        }

        [Authorize]
        // GET: All this does is get the chatMessages for one conversation 
        public async Task<IActionResult> Chat(int sneakerId, string otherUserId)
        {
            // userId is the buyer, sneakerId is for the sneaker for sale
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT MessageId, Msg, Date,
                                               SneakerId, SenderId, ReceiverId
                                        FROM Message
                                        WHERE SneakerId = @sneakerId
                                        AND (SenderId = @userId
                                                AND ReceiverId = @otherUserId)
                                        ORDER BY Date;";

                    var currentUser = await GetCurrentUserAsync();
                    //var curruserId = currentUser.Id;
                    var sneakerInQuestion = _context.Sneaker
                        .Include(u => u.User)
                        .FirstOrDefault(s => s.SneakerId == sneakerId);

                    ViewBag.SneakerOwnerId = sneakerInQuestion.UserId;
                    ViewBag.SneakerOwner = sneakerInQuestion.User.FirstName;
                    ViewBag.currUserName = currentUser.FirstName;

                    cmd.Parameters.Add(new SqlParameter("@userId", currentUser.Id));
                    cmd.Parameters.Add(new SqlParameter("@otherUserId", otherUserId));
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
                            ReceiverId = reader.GetString(reader.GetOrdinal("ReceiverId"))
                        };
                        messages.Add(message);
                    }
                    reader.Close();

                    cmd.CommandText = @"SELECT MessageId, Msg, Date,
                                               SneakerId, SenderId, ReceiverId
                                        FROM Message
                                        WHERE SneakerId = @theSneakerId
                                        AND (SenderId = @theOtherUserId
                                                AND ReceiverId = @currUserId)
                                        ORDER BY Date;";


                    cmd.Parameters.Add(new SqlParameter("@currUserId", currentUser.Id));
                    cmd.Parameters.Add(new SqlParameter("@theOtherUserId", otherUserId));
                    cmd.Parameters.Add(new SqlParameter("@theSneakerId", sneakerId));

                    SqlDataReader reader1 = await cmd.ExecuteReaderAsync();
                    while (reader1.Read())
                    {
                        Message message = new Message
                        {
                            MessageId = reader1.GetInt32(reader1.GetOrdinal("MessageId")),
                            Msg = reader1.GetString(reader1.GetOrdinal("Msg")),
                            Date = reader1.GetDateTime(reader1.GetOrdinal("Date")),
                            SneakerId = reader1.GetInt32(reader1.GetOrdinal("SneakerId")),
                            SenderId = reader1.GetString(reader1.GetOrdinal("SenderId")),
                            ReceiverId = reader1.GetString(reader1.GetOrdinal("ReceiverId"))
                        };
                        messages.Add(message);
                    }
                    reader1.Close();

                    var sneaker = _context.Sneaker.Find(sneakerId);
                    ViewBag.User = currentUser.FirstName;

                    var viewModel = new ChatMessagesViewModel
                    {
                        ChatMessages = messages.OrderBy(m => m.Date).ToList(),
                        SneakerId = sneaker.SneakerId
                    };

                    return View(viewModel);
                }
            }
        }

        public IActionResult CreateMessage(int sneakerId, string MsgChat)
        {
            var viewModel = new ChatMessagesViewModel
            {
                SneakerId = sneakerId,
                MsgChat = MsgChat
            };
            return View(viewModel);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateMessage(ChatMessagesViewModel viewModel)
        {
            // find the sender from the sneaker id that was passed in
            Sneaker sneaker = await _context.Sneaker
                .Include(u => u.User)
                .FirstOrDefaultAsync(u => u.SneakerId == viewModel.SneakerId);
            
            if(viewModel.ChatMessages == null) // some reason the list of chat messages is never there
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        //cmd.CommandText = @"SELECT MessageId, Msg, Date,
                        //                       SneakerId, SenderId, ReceiverId
                        //                FROM Message
                        //                WHERE SneakerId = @sneakerId
                        //                AND (SenderId = @userId
                        //                        OR ReceiverId = @userId)
                        //                ORDER BY Date;";
                        cmd.CommandText = @"SELECT MessageId, Msg, Date,
                                               SneakerId, SenderId, ReceiverId
                                        FROM Message
                                        WHERE SneakerId = @sneakerId
                                        AND (SenderId = @userId
                                                AND ReceiverId = @otherUserId)
                                        ORDER BY Date;";
                        var currUser = await GetCurrentUserAsync();

                        cmd.Parameters.Add(new SqlParameter("@userId", currUser.Id));
                        cmd.Parameters.Add(new SqlParameter("@otherUserId", viewModel.OtherUserId));
                        cmd.Parameters.Add(new SqlParameter("@sneakerId", viewModel.SneakerId));

                        SqlDataReader reader = await cmd.ExecuteReaderAsync();
                        List<Message> theMsgs = new List<Message>();

                        while (reader.Read())
                        {
                            Message message = new Message
                            {
                                MessageId = reader.GetInt32(reader.GetOrdinal("MessageId")),
                                Msg = reader.GetString(reader.GetOrdinal("Msg")),
                                Date = reader.GetDateTime(reader.GetOrdinal("Date")),
                                SneakerId = reader.GetInt32(reader.GetOrdinal("SneakerId")),
                                SenderId = reader.GetString(reader.GetOrdinal("SenderId")),
                                ReceiverId = reader.GetString(reader.GetOrdinal("ReceiverId"))
                            };
                            theMsgs.Add(message);
                        }
                        reader.Close();
                        ViewBag.User = currUser.FirstName;

                        cmd.CommandText = @"SELECT MessageId, Msg, Date,
                                               SneakerId, SenderId, ReceiverId
                                        FROM Message
                                        WHERE SneakerId = @theSneakerId
                                        AND (SenderId = @theOtherUserId
                                                AND ReceiverId = @theUserId)
                                        ORDER BY Date;";

                        cmd.Parameters.Add(new SqlParameter("@theUserId", currUser.Id));
                        cmd.Parameters.Add(new SqlParameter("theOtherUserId", viewModel.OtherUserId));
                        cmd.Parameters.Add(new SqlParameter("@theSneakerId", viewModel.SneakerId));
                        SqlDataReader reader1 = await cmd.ExecuteReaderAsync();
                        while (reader1.Read())
                        {
                            Message message = new Message
                            {
                                MessageId = reader1.GetInt32(reader1.GetOrdinal("MessageId")),
                                Msg = reader1.GetString(reader1.GetOrdinal("Msg")),
                                Date = reader1.GetDateTime(reader1.GetOrdinal("Date")),
                                SneakerId = reader1.GetInt32(reader1.GetOrdinal("SneakerId")),
                                SenderId = reader1.GetString(reader1.GetOrdinal("SenderId")),
                                ReceiverId = reader1.GetString(reader1.GetOrdinal("ReceiverId"))
                            };
                            theMsgs.Add(message);
                        }
                        viewModel.ChatMessages = theMsgs.OrderBy(m => m.Date).ToList();

                    }
                }
            }// end of the IF statement
            var sneakerOwner = sneaker.User;
            var currentUser = await GetCurrentUserAsync();

            var messages = viewModel.ChatMessages; // get all the messages 
            int messagesLength = 0; // set the length to 0 so it's not null 
            

            // if the messesages length is not null, then get the count of the messages 
            // and save it to the variable
            if(messages != null)
            {
                int count = 0;
                foreach(var msg in messages)
                {
                    count++;
                }
                messagesLength = count;
            }

            // if the messageLength is not 0, it holds msgs 
            // then grab the last message, no reason, could get first
            // this if statement is to get the receiver's id
            if (messagesLength != 0) 
            {                        
                Message lastMessage = messages.Last();
                if (lastMessage.SenderId != currentUser.Id)
                { // if the current user wasn't the sender of the last message
                  // that was sent, that means the other user of the viewmodel
                  //  is the sender so grab that id
                    viewModel.OtherUserId = lastMessage.SenderId;
                }
                else
                {
                    // else the other user was the receiver
                    viewModel.OtherUserId = lastMessage.ReceiverId;
                }


                // now create the message with the correct sender & receiver
                Message message = new Message()
                {
                    Date = DateTime.Now,
                    Msg = viewModel.Message.Msg,
                    SenderId = currentUser.Id,
                    ReceiverId = viewModel.OtherUserId,
                    Sneaker = sneaker,
                    SneakerId = sneaker.SneakerId
                };

                // add that message to the db
                _context.Add(message);
                await _context.SaveChangesAsync();
            }
            else
            {
                // else this is the beginning of a new chat message
                Message message = new Message()
                {
                    Date = DateTime.Now,
                    Msg = viewModel.Message.Msg,
                    SenderId = currentUser.Id,
                    ReceiverId = sneaker.UserId,
                    Sneaker = sneaker,
                    SneakerId = sneaker.SneakerId
                };
                _context.Add(message);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Chat), new { sneakerId = sneaker.SneakerId, otherUserId = viewModel.OtherUserId});
        }

        [Authorize]
        public async Task<IActionResult> DeleteChatMessage(int msgId, int sneakerId, string otherUserId)
        {
            var message = await _context.Message
                .FindAsync(msgId);

            var sneaker = await _context.Sneaker.FindAsync(sneakerId);

            _context.Message.Remove(message);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Chat), new {sneakerId = sneaker.SneakerId, otherUserId });
        }

        private bool MessageExists(int id)
        {
            return _context.Message.Any(e => e.MessageId == id);
        }
    }
}
