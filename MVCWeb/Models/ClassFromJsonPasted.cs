/// <summary>
/// #############################################
/// Class created by pasting JSON (as per below)
/// #############################################
/// - copy json to clipboard
/// In VS create new class 
/// Empty it out fully 
/// Select Edit - Paste Special - Paste Json as Classes (only appears if Json is in clipboard)
/// </summary>
public class Rootobject
{
    public Customer Customer { get; set; }
}

public class Customer
{
    public string CustomerID { get; set; }
    public string CompanyName { get; set; }
    public string ContactName { get; set; }
    public string ContactTitle { get; set; }
    public string Phone { get; set; }
    public Fulladdress FullAddress { get; set; }
}

public class Fulladdress
{
    public string Address { get; set; }
    public string City { get; set; }
    public string Region { get; set; }
    public string PostalCode { get; set; }
    public string Country { get; set; }
}
