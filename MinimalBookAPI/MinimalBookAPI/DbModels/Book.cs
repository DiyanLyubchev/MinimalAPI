using System.ComponentModel.DataAnnotations;

namespace MinimalBookAPI.DbModels
{
    public class Book
    {
        [Key]
        public int Id { get;  set; }

        public string Name { get; set; }
        public int Page { get; set; }
    }
}
