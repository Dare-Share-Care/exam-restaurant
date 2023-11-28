﻿namespace Restaurant.Grpc.Models;

public class MenuItemViewModel
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
}