using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using LibraryWebSite.Entities.Identity;
using Newtonsoft.Json;

namespace LibraryWebSite.ViewModel.Identity
{

    public class UsersViewModel
    {
        [JsonProperty("Id")]
        public int? Id { get; set; }

        [JsonProperty("ردیف")]
        public int Row { get; set; }

        [Display(Name = "تصویر پروفایل"), JsonProperty("تصویر")]
        public string Image { get; set; }

        [Required(ErrorMessage = "وارد نمودن {0} الزامی است.")]
        [Display(Name = "نام کاربری"), JsonProperty("نام کاربری")]
        public string UserName { get; set; }

        [Display(Name = "ایمیل"), JsonProperty("ایمیل")]
        [Required(ErrorMessage = "وارد نمودن {0} الزامی است.")]
        [EmailAddress(ErrorMessage = "ایمیل وارد شده صحیح نمی باشد.")]
        public string Email { get; set; }

      


        [Display(Name = "شماره موبایل"), JsonProperty("شماره تماس")]
        [Required(ErrorMessage = "وارد نمودن {0} الزامی است.")]
        public string PhoneNumber { get; set; }

        [Display(Name = "نام"), JsonProperty("نام")]
        [Required(ErrorMessage = "وارد نمودن {0} الزامی است.")]
        public string FirstName { get; set; }

        [Display(Name = "نام خانوادگی"), JsonProperty("نام خانوادگی")]
        [Required(ErrorMessage = "وارد نمودن {0} الزامی است.")]
        public string LastName { get; set; }

        [Display(Name = "تاریخ تولد"), JsonIgnore()]
        public DateTime? BirthDate { get; set; }

        [Display(Name = "آخرین بازدید")]
        public string PersianLastVisitDateTime { get; set; }

        [Display(Name = "تاریخ تولد"), JsonProperty("تاریخ تولد")]
        [Required(ErrorMessage = "وارد نمودن {0} الزامی است.")]
        public string PersianBirthDate { get; set; }

        [Display(Name = "تاریخ عضویت"), JsonIgnore]
        public DateTime? CreateDateTime { get; set; }

        [Display(Name = "تاریخ عضویت"), JsonProperty("تاریخ عضویت")]
        public string PersianRegisterDateTime { get; set; }

        [Display(Name = "فعال / غیرفعال"), JsonProperty("IsActive")]
        public bool IsActive { get; set; }

   

        [JsonProperty("جنسیت")]
        public string GenderName { get; set; }

        [JsonIgnore]
        public ICollection<UserRole> Roles { get; set; }


        public int RoleId { get; set; }
        [JsonProperty("نقش")]
        public string RoleName { get; set; }


        public IEnumerable<string> RolesName { get; set; }

        [JsonIgnore]
        public bool PhoneNumberConfirmed { get; set; }

        [JsonIgnore]
        public bool TwoFactorEnabled { get; set; }

        [JsonIgnore]
        public bool LockoutEnabled { get; set; }

        [JsonIgnore]
        public bool EmailConfirmed { get; set; }

        [JsonIgnore]
        public int AccessFailedCount { get; set; }

        [JsonIgnore]
        public DateTimeOffset? LockoutEnd { get; set; }
    }
}

