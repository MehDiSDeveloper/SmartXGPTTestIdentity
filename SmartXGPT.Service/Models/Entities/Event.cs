using System.ComponentModel.DataAnnotations;

namespace SmartXGPT.Service.Models.Entities
{
    public class Event
    {
        public Guid EventId { get; set; } = Guid.NewGuid();
        public int AllowedDays { get; set; }
        public string MessagesSendDate { get; set; }
        public bool SendExpireNotification { get; set; }
        public int Amount { get; set; }


        [Range(1,24)]
        public int MessagesSendHour { get; set; }

        public string? MessagePassage { get; set; }

        public string creatorLastName { get; set; }

        public string CreatorPhoneNumber { get; set; }
        public string RestaurantName { get; set; }
        public EventOfferType offerType { get; set; }
        public CustomerType customerType { get; set; }




    }
    public enum EventOfferType
    {
        Informing=0,
        GivingCredit=1,
        GivingOff=2
    }
    public enum CustomerType
    {
        LostCustomers = 0,
        ActiveCustomers = 1,
        AllCustomers = 2,
        ConsultNeededCustomers=3,
        Others=4
    }
}
