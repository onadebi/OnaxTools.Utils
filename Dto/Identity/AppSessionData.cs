using System;

namespace OnaxTools.Dto.Identity
{
    public class AppSessionData<T> where T : class
    {

        public virtual string MachineName { get; set; } = string.Empty;
        public virtual string IpAddress { get; set; }
        public virtual string SessionId { get; set; }
        public virtual string Email { get; set; } = string.Empty;
        /// <summary>
        /// This property serves as an identifiable or anonymous identifier for an object/user.
        /// </summary>
        public virtual string Identifier { get; set; } = string.Empty;
        public virtual DateTime LastUpdated { get; set; } = DateTime.UtcNow;
        public T Data { get; set; } = null;
    }
}
