using System.ComponentModel.DataAnnotations;

namespace HowProgrammingWorksOnDotNet.WebMvc.CrudProject.Data
{
    public class Entry
    {
        public int Id { get; set; }

        [Required, StringLength(300)]
        [Display(Name = "Content name")]
        public string Content { get; set; }

        [Required, EmailAddress]
        [Display(Name = "Entry email")]
        public string Email { get; set; }

        public int Likes { get; set; }

        [Display(Name = "Best color")]
        public string Color { get; set; } = "green";
    }
}
