using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SalesManagementSystem.Application.Dtos.CustomerDto;

public class CustomerRespone
{
    public ulong Id { get; set; }
    public ulong? CustomerTypeId { get; set; }

    public string CustomerCode { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public string? TaxCode { get; set; }

    public string? ShippingAddress { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }

    public DateOnly? LastPurchaseDate { get; set; }

    public string? ProductsSummary { get; set; }

    public string? LatestProductName { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public CustomerTypeDto? CustomerType { get; set; }
}
