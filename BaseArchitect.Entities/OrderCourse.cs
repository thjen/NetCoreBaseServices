﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace BaseArchitect.Entities
{
    public partial class OrderCourse
    {
        public int OrderCourseID { get; set; }
        public int OrderID { get; set; }
        public int CourseID { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public bool? Deleted { get; set; }
    }
}