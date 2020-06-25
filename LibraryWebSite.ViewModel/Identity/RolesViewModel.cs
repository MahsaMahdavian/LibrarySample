﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Newtonsoft.Json;

namespace LibraryWebSite.ViewModel.Identity
{
    public class RolesViewModel
    {
        [JsonProperty("Id")]
        public int? Id { get; set; }

        [JsonProperty("ردیف")]
        public int Row { get; set; }

        [Display(Name = "عنوان نقش"), JsonProperty("عنوان نقش")]
        [Required(ErrorMessage = "وارد نمودن {0} الزامی است.")]
        public string Name { get; set; }

        [Display(Name = "توضیحات"), JsonProperty("توضیحات")]
        [Required(ErrorMessage = "وارد نمودن {0} الزامی است.")]
        public string Description { get; set; }

        [JsonProperty("تعداد کاربران")]
        public int UsersCount { get; set; }

    }
}