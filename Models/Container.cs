using PUCCI.Models.Audit;

namespace PUCCI.Models
{
    public class Container : AuditModel
    {
        public string? ID { get; set; }
        public string? UserId { get; set; }
        public string? Name { get; set; }
        public string? Image { get; set; }
        public string? Status { get; set; }
    }
}
