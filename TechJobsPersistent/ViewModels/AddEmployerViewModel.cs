using System;
using System.ComponentModel.DataAnnotations;

namespace TechJobsPersistent.ViewModels
{
    public class AddEmployerViewModel
    {
        [Required(ErrorMessage = "Employer Name is a required entry.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Employer Location is a required entry.")]
        public string Location { get; set; }

        public AddEmployerViewModel()
        {

        }
    }
}
