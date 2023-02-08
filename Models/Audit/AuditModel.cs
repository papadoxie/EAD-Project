namespace PUCCI.Models.Audit
{
    public interface AuditModel
    {
        public string CreatedBy { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime ModifiedTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}
