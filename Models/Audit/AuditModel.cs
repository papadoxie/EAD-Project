namespace PUCCI.Models.Audit
{
    public abstract class AuditModel
    {
        public string? CreatedBy { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime ModifiedTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}
