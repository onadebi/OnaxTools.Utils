namespace OnaxTools.Dto
{
    public class AppUser
    {
        public virtual string DisplayName { get; set; }
        public virtual string Email { get; set; }
        public virtual long Id { get; set; }
        public virtual List<string> Roles { get; set; }
    }
}
