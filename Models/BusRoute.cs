﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace HotelFuen31.APIs.Models;

public partial class BusRoute
{
    public int Id { get; set; }

    public string StarterStop { get; set; }

    public string StarterLongtitude { get; set; }

    public string StarterLatitude { get; set; }

    public string DestinationStop { get; set; }

    public string DestinationLongtitude { get; set; }

    public string DestinationLatitude { get; set; }

    public DateTime DepartureTime { get; set; }

    public DateTime ArrivalTime { get; set; }
}