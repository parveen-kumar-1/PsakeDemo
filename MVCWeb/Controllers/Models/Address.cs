namespace MVCWeb.Controllers
{
    public class Address
    {
        public int AddressId { get; set; }
        public string AddressLine1 { get; set; }
        public string City { get; set; }
        public States State { get; set; }
    }

    public enum States
    {
        Northamptonshire,
        Warwickshire,
        Ayrshire
    }
}
