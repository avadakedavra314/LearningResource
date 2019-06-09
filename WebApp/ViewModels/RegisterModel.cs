using System.ComponentModel.DataAnnotations;
using WebApp.Models;

namespace WebApp.ViewModels
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Необходимо заполнить \"Логин\"")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Необходимо заполнить \"Пароль\"")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Необходимо заполнить \"Имя\"")]
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Patronymic { get; set; }

        [Required(ErrorMessage = "Необходимо заполнить \"Номер группы\"")]
        public int GroupId { get; set; }


    }
}