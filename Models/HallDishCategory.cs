﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace HotelFuen31.APIs.Models;

public partial class HallDishCategory
{
    public int Id { get; set; }

    public string Category { get; set; }

    public virtual ICollection<HallMenu> HallMenus { get; set; } = new List<HallMenu>();
}