﻿using SMS.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SMS.Models
{
    public class Course
    {
        [Key]
        [DisplayName("Course Id")]
        public int CourseId { get; set; }

        [Required]
        [DisplayName("Course Name")]
        public string CourseName { get; set; }

        [Required]
        [DisplayName("Course Type")]
        public CourseTypes CourseType { get; set; }

        [DisplayName("Description")]
        public string? CourseDescription { get; set; }

        [DisplayName("Offered")]
        public bool IsOffered { get; set; }

        public Fees? Fees { get; set; }

        public ICollection<Enrollment>? Enrollments { get; set; }
    }
}
