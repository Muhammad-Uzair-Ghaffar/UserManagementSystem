namespace UserManagementSystem.Models
{
    public class UserRole
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string RoleId { get; set; }
        public User User { get; set; } // Navigation property
        public Role Role { get; set; } // Navigation property

    }
}
