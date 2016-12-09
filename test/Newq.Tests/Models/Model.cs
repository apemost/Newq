namespace Newq.Tests.Models
{
    using System;

    public abstract class Model
    {
        public string Id { get; set; }
        public string Remark { get; set; }
        public int Status { get; set; }
        public int Flag { get; set; }
        public int Version { get; set; }
        public string AuthorId { get; set; }
        public string EditorId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
