using System.ComponentModel.DataAnnotations;
using SmartXGPT.Service.Models.Entities;

namespace SmartXGPT.Service.Models.DTOs
{
    public class EventDto
    {
        public EventOfferType offerType { get; set; }
        public int Amount { get; set; }
        public int AllowedDays { get; set; }
        public bool SendExpireNotification { get; set; }
        public string MessagesSendDate { get; set; }
        [Range(1,24)]
        public int MessagesSendHour { get; set; }
        public CustomerType customerType { get; set; }
        public string? MessagePassage { get; set; }
        public string creatorLastName { get; set; }
        public string CreatorPhoneNumber { get; set; }
        public string RestaurantName { get; set; }


    }
    
}
