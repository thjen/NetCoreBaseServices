﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace BaseArchitect.Entities
{
    public partial class AccountDocument
    {
        public int AccountDocumentID { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public int AccountID { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public bool Deleted { get; set; }
    }
}