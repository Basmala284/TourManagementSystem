using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TourManagementSystem.Data;
using TourManagementSystem.DTOs.Message;
using TourManagementSystem.Hubs;
using TourManagementSystem.Models.Entities;

namespace TourManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly AppDbContext dbContext;
        private readonly IHubContext<ChatHub> chatHubContext;

        public MessageController(AppDbContext dbContext, IHubContext<ChatHub> chatHubContext)
        {
            this.dbContext = dbContext;
            this.chatHubContext = chatHubContext;
        }

        [Authorize]                    
        [HttpPost("send")]
        public async Task<IActionResult> SendMessage(SendMessageDto sendMessageDto)
        {
            // Validate sender and receiver IDs
            var senderId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            if (senderId != sendMessageDto.SenderId)
            {
                return Unauthorized(new { message = "You are not authorized to send this message." });
            }

            var message = new Message
            {
                Text = sendMessageDto.Text,
                CreatedAt = DateTime.UtcNow,
                SenderId = senderId,
                ReceiverId = sendMessageDto.ReceiverId
            };

            dbContext.Messages.Add(message);
            await dbContext.SaveChangesAsync();

            // Notify the receiver in real-time via SignalR
            await chatHubContext.Clients.User(sendMessageDto.ReceiverId.ToString()).SendAsync("ReceiveMessage", new
            {
                message.Id,
                message.Text,
                message.CreatedAt,
                SenderId = message.SenderId,
                ReceiverId = message.ReceiverId
            });

            return Ok(new { message = "Message sent successfully." });
        }

        [Authorize]              
        [HttpGet("history/{receiverId}")]
        public async Task<IActionResult> GetChatHistory(int receiverId)
        {
            var senderId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var messages = await dbContext.Messages
                .Where(m => (m.SenderId == senderId && m.ReceiverId == receiverId) ||
                            (m.SenderId == receiverId && m.ReceiverId == senderId))
                .OrderBy(m => m.CreatedAt)
                .ToListAsync();

            return Ok(messages);
        }
    }
}