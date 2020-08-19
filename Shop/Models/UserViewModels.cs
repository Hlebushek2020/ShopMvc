using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Shop.Models
{
    public class UserCreateViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Адрес электронной почты")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Значение {0} должно содержать не менее {2} символов.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Подтверждение пароля")]
        [Compare("Password", ErrorMessage = "Пароль и его подтверждение не совпадают.")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Администратор")]
        public bool IsAdmin { get; set; }

        [Display(Name = "Телефон")]
        [Phone]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Не указан заказчик")]
        [Display(Name = "Заказчик")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Не указан код заказчика")]
        [Display(Name = "Код заказчика")]
        [RegularExpression(@"[0-9]{4}-[0-9]{4}", ErrorMessage = "Код заказчика не соответствует шаблону: ХХХХ-ГГГГ, где Х – число, ГГГГ – год в котором зарегистрирован заказчик")]
        public string Code { get; set; }

        [Display(Name = "Адрес заказчика")]
        public string Address { get; set; }

        [Display(Name = "Скидка (%)")]
        public double? Discount { get; set; }
    }

    public class UserEditViewModel
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Значение {0} должно содержать не менее {2} символов.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Новый пароль")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Подтверждение пароля")]
        [Compare("Password", ErrorMessage = "Пароль и его подтверждение не совпадают.")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Администратор")]
        public bool IsAdmin { get; set; }

        [Display(Name = "Телефон")]
        [Phone]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Не указан заказчик")]
        [Display(Name = "Заказчик")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Не указан код заказчика")]
        [Display(Name = "Код заказчика")]
        [RegularExpression(@"[0-9]{4}-[0-9]{4}", ErrorMessage = "Код заказчика не соответствует шаблону: ХХХХ-ГГГГ, где Х – число, ГГГГ – год в котором зарегистрирован заказчик")]
        public string Code { get; set; }

        [Display(Name = "Адрес заказчика")]
        public string Address { get; set; }

        [Display(Name = "Скидка (%)")]
        public double? Discount { get; set; }
    }
}