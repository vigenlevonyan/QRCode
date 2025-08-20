using System.Collections.Generic;

namespace QShirt.Application.Contracts.Infrastructure.Payments.Models;

public class CreateSessionModel
{
    /// <summary>
    /// Total price
    /// </summary>
    public double TotalPrice { get; set; }

    /// <summary>
    /// Currency
    /// </summary>
    public string Currency { get; set; }

    /// <summary>
    /// Url for redirect after payment success
    /// </summary>
    public string SuccessUrl { get; set; }

    /// <summary>
    /// Url for redirect, after cancel payment
    /// </summary>
    public string CancelUrl { get; set; }

    public List<Item> Items { get; set; }
}

public class Item
{

    /// <summary>
    /// quantity
    /// </summary>
    public long Quantity { get; set; }

    /// <summary>
    /// Offer price
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Product name
    /// </summary>
    public string ProductName { get; set; }

    /// <summary>
    /// Image url
    /// </summary>
    public string ImageUrl { get; set; }
}
