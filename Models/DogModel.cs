namespace adoptera_hund.api.Models;

    public class DogModel
    {
        //A unique identifier for each dog.
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public int Age { get; set; }
        public string Race { get; set; } = "";
        public string Gender { get; set; } = "";
        public string City { get; set; } = "";
        public string Description { get; set; } = "";

        // A field to track whether the dog is available for adoption or has already been adopted.
        public DogStatusEnum Status { get; set; }
    }
