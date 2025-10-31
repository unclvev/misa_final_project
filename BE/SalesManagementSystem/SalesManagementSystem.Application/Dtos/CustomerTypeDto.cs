using System;
using System.Collections.Generic;

namespace SalesManagementSystem.Application.Dtos;

public partial class CustomerTypeDto
{
    public ulong Id { get; set; }

    public string TypeName { get; set; } = null!;

    public string? Description { get; set; }

}
