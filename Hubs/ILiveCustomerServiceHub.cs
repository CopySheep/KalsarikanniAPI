namespace HotelFuen31.APIs.Hubs
{
    public interface ILiveCustomerServiceHub
    {
        Task JsonDataTransfer(dynamic message);
        Task StringDataTransfer(string message);
    }
}
