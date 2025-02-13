namespace OnaxTools.Dto.Identity
{
    public class AppUserIdentity
    {
        public virtual string DisplayName { get; set; }
        public virtual string Email { get; set; }
        public virtual long Id { get; set; }
        public virtual string GuestId { get; set; }
        public virtual string Guid { get; set; }
        public virtual List<string> Roles { get; set; }
        public virtual DateTime ExpiresAt { get; set; }
        public virtual DateTime CreatedAt { get; set; }
    }
}
