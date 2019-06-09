using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class User
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string Patronymic { get; set; }

        public string LastName { get; set; }

        public string About { get; set; }

        public string Photo { get; set; }

        [Required(ErrorMessage = "Необходимо заполнить \"Логин\"")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Необходимо заполнить \"Пароль\"")]
        public string Password { get; set; }

        public int? RoleId { get; set; }
        public Role Role { get; set; }

        public int? GroupId { get; set; }
        public Group Group { get; set; }
    }
}
