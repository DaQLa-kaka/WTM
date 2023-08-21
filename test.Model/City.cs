using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WalkingTec.Mvvm.Core;

namespace test.Model
{
    public class City : TreePoco<City>
    {
        [Display(Name = "城市名称")]
        [Required(ErrorMessage = "不能为空")]
        public string CityName { get; set; }

        public List<City> Children { get; set; }

        [Display(Name = "父级")]
        public City Parent { get; set; }

        [Display(Name = "父级")]
        public Guid? ParentId { get; set; }

        [NotMapped]
        public string Sheng { get; set; }

        [NotMapped]
        public string Shi { get; set; }

        [NotMapped]
        public string Qu { get; set; }
    }
}