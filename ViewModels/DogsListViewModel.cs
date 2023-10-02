using adoptera_hund.api.Models;

namespace adoptera_hund.api.ViewModels;

    public class DogsListViewModel
    {
         //A unique identifier for each dog.
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public int Age { get; set; }
        public string Race { get; set; } = "";
        public string Gender { get; set; } = "";
        public string City { get; set; } = "";
        public string Description { get; set; } = "";

    }
