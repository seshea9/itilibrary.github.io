﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace ITI_Libraly_Api.Models
{
    public partial class TblBorrowDetails
    {
        public int BorDelId { get; set; }
        public int? BorId { get; set; }
        public string BookDelId { get; set; }
        public int? StatusId { get; set; }

        public virtual TblItiBookdetails BookDel { get; set; }
        public virtual TblItiBorrow Bor { get; set; }
        public virtual TblItiStatus Status { get; set; }
    }
}