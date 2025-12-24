namespace BookstoreAPI.Models.Afip
{
    public class AfipTicketAcceso
    {
        public string Token { get; set; } = string.Empty;
        public string Sign { get; set; } = string.Empty;
        public DateTime ExpirationTime { get; set; }
        public DateTime GenerationTime { get; set; }

        public bool IsValid()
        {
            return DateTime.Now < ExpirationTime;
        }
    }
}
