﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace HotelFuen31.APIs.Models;

public partial class BusTracker
{
    public int Id { get; set; }

    public int BusId { get; set; }

    public int Latitude { get; set; }

    public int Longtitude { get; set; }

    public string Status { get; set; }

    public DateTime LastUpdate { get; set; }
}