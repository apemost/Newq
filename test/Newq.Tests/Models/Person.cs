namespace Newq.Tests.Models
{
    using Attributes;

    [Table("PERSION")]
    public class Person
    {
        protected string email;

        public string Name { get; set; }

        [ColumnIgnore]
        public int Age { get; set; }

        public Country Country { get; set; }

        [Key]
        [Column("ID")]
        public string id { get; set; }

    }
}
