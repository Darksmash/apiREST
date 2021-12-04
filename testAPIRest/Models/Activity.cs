using System;
using System.Collections.Generic;

namespace PruebaApiREST.Models
{
    public class Activity
    {
        public int Id { get; set; }
        public int PropertyId { get; set; }
        public DateTime Schedule { get; set; }
        public string Title { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string Status { get; set; }
        public string Condition { get; set; }
    }
}
