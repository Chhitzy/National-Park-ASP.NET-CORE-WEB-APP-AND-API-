using System.ComponentModel.DataAnnotations;
using static National_Park_Project.Model.Trail;

namespace National_Park_Web_App.Models
{
    public class Trail
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Distance { get; set; }
        [Required]
        public string Elevation { get; set; }
        //public DateTime DateCreated { get; set; }
        public enum DifficultyType { Easy, Moderate, Difficult } 
        public DifficultyType Difficulty { get; set; }
        public int NationalParkId { get; set; }

        public NationalPark? NationalPark { get; set; }
    }
}
