using BookstoreAPI.Models.Afip;

namespace BookstoreAPI.Services.Afip
{
    public interface IAfipAuthService
    {
        Task<AfipTicketAcceso> GetTicketAccesoAsync();
    }
}
