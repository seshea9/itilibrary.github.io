﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace ITI_Libraly_Api.Models
{
    public partial class TblItiLocation
    {
        public TblItiLocation()
        {
            TblItiBook = new HashSet<TblItiBook>();
        }

        public string LocId { get; set; }
        public string LocLabel { get; set; }
        public bool? LocActive { get; set; }

        public virtual ICollection<TblItiBook> TblItiBook { get; set; }
    }
}