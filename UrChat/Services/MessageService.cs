using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UrChat.Data;
using UrChat.Data.Models;
using UrChat.Data.UnitOfWork;
using UrChat.Extensions.Pagination;
using UrChat.Forms;
using UrChat.Services.Shared;
using UrChat.ViewModels;

namespace UrChat.Services
{
    public class MessageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly TranslationService _translationService;

        public MessageService(IUnitOfWork unitOfWork, TranslationService translationService)
        {
            _unitOfWork = unitOfWork;
            _translationService = translationService;
        }


        public async Task<ServiceOperationResult<MessageViewModel>> SendMessageAsync(long userId, PostMessageForm form)
        {
            if (! await _unitOfWork.Users.AnyAsync(u => u.Id == userId))
            {
                return ServiceOperationResult.NotFound<MessageViewModel>("User not found.");
            }
            
            var translated = _translationService.TranslateMessageToUr(form.Message);

            if (! translated.IsSuccessful)
            {
                return ServiceOperationResult.UnprocessableEntity<MessageViewModel>("Failed to translate message to UR.");
            }

            var message = new Message
            {
                Content = translated.Data,
                SenderId = userId
            };

            await _unitOfWork.Messages.AddAsync(message);
            
            await _unitOfWork.SaveAsync();

            var sender = await _unitOfWork.Users.GetAsQueryable().SingleAsync(u => u.Id == userId);
            
            return ServiceOperationResult.Ok(new MessageViewModel
            {
                Id = message.Id,
                Message = message.Content,
                Sender = sender.Username,
                SenderId = message.SenderId,
                SentDate = message.CreatedAt
            });
        }

        public async Task<ServiceOperationResult<PaginationViewModel<MessageViewModel>>> GetMessagesAsync(
            PaginationParams paginationParams
        )
        {
            var messages = _unitOfWork.Messages
                .GetAsQueryable()
                .Include(m => m.Sender)
                .OrderByDescending(m => m.CreatedAt);

            var result = await messages.PaginateAsync(
                paginationParams, 
                m => new MessageViewModel
                {
                    Id = m.Id,
                    Message = m.Content,
                    Sender = m.Sender.Username,
                    SenderId = m.SenderId,
                    SentDate = m.CreatedAt
                });
            
            return ServiceOperationResult.Ok(result);
        }
    }
}