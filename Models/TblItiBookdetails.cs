﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace ITI_Libraly_Api.Models
{
    public partial class TblItiBookdetails
    {
        public TblItiBookdetails()
        {
            TblBorrowDetails = new HashSet<TblBorrowDetails>();
            TblItiReadDetails = new HashSet<TblItiReadDetails>();
        }

        public string BookDelId { get; set; }
        public int? BookId { get; set; }
        public string BookDelLabel { get; set; }
        public int? StatusId { get; set; }
        public int? ImpId { get; set; }

        public virtual TblItiBook Book { get; set; }
        public virtual TblItiImport Imp { get; set; }
        public virtual TblItiStatus Status { get; set; }
        public virtual ICollection<TblBorrowDetails> TblBorrowDetails { get; set; }
        public virtual ICollection<TblItiReadDetails> TblItiReadDetails { get; set; }
    }
}