using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using ChatSystem.Hubs;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace ChatSystem.Controllers
{
    [ApiController]
    [Route("api/chat")]
    public class ChatController : ControllerBase
    {
        private readonly IHubContext<ChatHub> _chatHub;

        // ✅ FIXED: Correct connection string format
        private readonly string connectionString = "Server=VISHALSILVERI28;Database=ChatDB;Trusted_Connection=True;TrustServerCertificate=True";

        public ChatController(IHubContext<ChatHub> chatHub)
        {
            _chatHub = chatHub;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendMessage([FromBody] ChatMessage message)
        {
            try
            {
                // ✅ Validate request
                if (message == null || string.IsNullOrEmpty(message.Sender) || string.IsNullOrEmpty(message.Content))
                    return BadRequest(new { Error = "Invalid message data" });

                // ✅ Store message in database
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    await conn.OpenAsync();
                    string query = "INSERT INTO Messages (SenderId, ReceiverId, Message, Timestamp) VALUES (@Sender, @Receiver, @Message, GETDATE())";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Sender", message.Sender);
                        cmd.Parameters.AddWithValue("@Receiver", string.IsNullOrEmpty(message.Receiver) ? DBNull.Value : message.Receiver);
                        cmd.Parameters.AddWithValue("@Message", message.Content);
                        await cmd.ExecuteNonQueryAsync();
                    }
                }

                // ✅ Send the message in real-time
                await _chatHub.Clients.All.SendAsync("ReceiveMessage", message.Sender, message.Receiver, message.Content);
                return Ok(new { Status = "Message Sent" });
            }
            catch (SqlException sqlEx)
            {
                return StatusCode(500, new { Error = "Database error", Details = sqlEx.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "Internal server error", Details = ex.Message });
            }
        }
    }

    // ✅ ChatMessage Model
    public class ChatMessage
    {
        public int Id { get; set; }  // Primary Key
        public required string Sender { get; set; }  // Required
        public string? Receiver { get; set; } // Nullable for group chats
        public required string Content { get; set; } // Required message content
        public DateTime Timestamp { get; set; } = DateTime.UtcNow; // Auto-set timestamp
    }
}
