using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Data;

namespace ebyteLearner.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; init; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string? PhoneNumber { get; set; }
        public string? ProfilePhoto { get; set; }
        public string? ZipCode { get; set; }
        public string? NIF { get; set; } 
        public string? Docn { get; set; }
        public string? Gender { get; set; }
        public string? Nationality { get; set; }

        [StringLength(50)]
        public UserRole UserRole { get; set; } = UserRole.Student;
        public bool Active { get; set; } = true;
        public ICollection<UserSession> UserSessions { get; set; }
        public DateTimeOffset? Birthday { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTimeOffset CreatedDate { get; init; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTimeOffset UpdatedDate { get; init; }
    }

    public enum UserRole
    {
        Teacher,
        Student,
        Admin
    }
}