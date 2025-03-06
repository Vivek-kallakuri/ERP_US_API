using ChatSystem.Controllers;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatSystem.Hubs
{
    public class ChatHub : Hub
    {
        private readonly AppDbContext _dbContext; // 🔹 Database context

        public ChatHub(AppDbContext dbContext) // 🔹 Inject database context
        {
            _dbContext = dbContext;
        }

        // 🔹 Send message (Direct or Group Chat)
        public async Task SendMessage(string sender, string receiver, string message)
        {
            try
            {
                Console.WriteLine($"📩 [ChatHub] Sending Message: {sender} -> {receiver ?? "Group"} : {message}");

                // 🔹 Save message to database
                var chatMessage = new ChatMessage
                {
                    Sender = sender,
                    Receiver = receiver,
                    Content = message,
                    Timestamp = DateTime.UtcNow
                };

                _dbContext.ChatMessages.Add(chatMessage);
                await _dbContext.SaveChangesAsync(); // Save to database

                // 🔹 If it's a direct message
                if (!string.IsNullOrEmpty(receiver))
                {
                    await Clients.User(receiver).SendAsync("ReceiveMessage", sender, receiver, message);
                }
                else // 🔹 Broadcast to all clients for group chat
                {
                    await Clients.All.SendAsync("ReceiveMessage", sender, receiver, message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error in SendMessage: {ex.Message}");
                throw;
            }
        }

        // 🔹 Join a group
        public async Task JoinGroup(string groupName)
        {
            try
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
                await Clients.Group(groupName).SendAsync("ReceiveNotification", $"{Context.ConnectionId} has joined {groupName}");
                Console.WriteLine($"✅ {Context.ConnectionId} joined group {groupName}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error joining group {groupName}: {ex.Message}");
                throw;
            }
        }

        // 🔹 Send message to a group
        public async Task SendMessageToGroup(string groupName, string sender, string message)
        {
            try
            {
                Console.WriteLine($"📩 [ChatHub] Group Message Received - Group: {groupName}, Sender: {sender}, Message: {message}");

                var groupMessage = new ChatMessage
                {
                    Sender = sender,
                    Receiver = groupName, // Using group name as receiver
                    Content = message,
                    Timestamp = DateTime.UtcNow
                };

                _dbContext.ChatMessages.Add(groupMessage);
                await _dbContext.SaveChangesAsync(); // Save to database

                await Clients.Group(groupName).SendAsync("ReceiveGroupMessage", sender, message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error in SendMessageToGroup: {ex.Message}");
                throw;
            }
        }

        // 🔹 Leave a group
        public async Task LeaveGroup(string groupName)
        {
            try
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
                await Clients.Group(groupName).SendAsync("ReceiveNotification", $"{Context.ConnectionId} has left {groupName}");
                Console.WriteLine($"🚪 {Context.ConnectionId} left group {groupName}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error leaving group {groupName}: {ex.Message}");
                throw;
            }
        }
    }
}
