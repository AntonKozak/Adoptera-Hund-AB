using System.ComponentModel.DataAnnotations;

namespace adoptera_hund.api.ViewModels;

public class AddDogViewModel
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; } = "";
        [Required(ErrorMessage = "Age is required")]
        public int Age { get; set; }
        [Required(ErrorMessage = "Race is required")]
        public string Race { get; set; } = "";
        [Required(ErrorMessage = "Gender is required")]
        public string Gender { get; set; } = "";
        public string City { get; set; } = "";
        public string Description { get; set; } = "";
    }
