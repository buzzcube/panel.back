using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace panel.back.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string InvitationId { get; set; } = Guid.NewGuid().ToString();
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = false;
        public string Status { get; set; } = string.Empty;
    }
}
