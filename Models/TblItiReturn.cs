﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace ITI_Libraly_Api.Models
{
    public partial class TblItiReturn
    {
        public TblItiReturn()
        {
            TblItiReturndeltails = new HashSet<TblItiReturndeltails>();
        }

        public int RetId { get; set; }
        public int? UseDelId { get; set; }
        public DateTime? UseEditDate { get; set; }
        public DateTime? RetDate { get; set; }

        public virtual TblUseLogintime UseDel { get; set; }
        public virtual ICollection<TblItiReturndeltails> TblItiReturndeltails { get; set; }
    }
}