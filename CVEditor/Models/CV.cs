using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CVEditor.Models
{
    public class CV
    {
        [Required(ErrorMessage = "'First name' is required")]
        [Display(Name = "First name: ")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "'Last name' is required")]
        [Display(Name = "Last name: ")]
        public string LastName { get; set; }

        [Display(Name = "Address: ")]
        public string Address { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone number: ")]
        [Required(ErrorMessage = "'Phone number' is required")]
        public string PhoneNumber { get; set; }

        public List<Job> Jobs { get; set; }
    }

    public class Job
    {
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "'From' is required")]
        [Display(Name = "From :")]
        public DateTime From { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = "'To' is required")]
        [Display(Name = "To :")]
        public DateTime To { get; set; }
        public string Company { get; set; }

        [Required(ErrorMessage = "'Job Title' is required")]
        [Display(Name = "Job Title: ")]
        public string JobTitle { get; set; }

        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "'Detailed information' is required")]
        [Display(Name = "Detailed information: ")]
        public string DetailedInformation { get; set; }
    }
}