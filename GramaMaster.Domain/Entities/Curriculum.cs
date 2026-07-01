using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Domain.Entities
{
    public class Curriculum:BaseEntity
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public ICollection<Topic> Topics { get; set; } = new List<Topic>();
        public ICollection<Contest> Contests { get; set; } = new List<Contest>();
        public ICollection<Student> Students { get; set; } = new List<Student>();


    }
}
