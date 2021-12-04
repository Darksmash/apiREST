using System;

namespace PruebaApiREST.Models
{
    public class Property
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DisabledAt { get; set; }
        public string Status { get; set; }
    }
}
