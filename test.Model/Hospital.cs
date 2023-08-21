using System;
using System.ComponentModel.DataAnnotations;
using WalkingTec.Mvvm.Core;

namespace test.Model
{
    public enum HospitalLevel
    {
        [Display(Name = "一甲医院")]
        Class1,

        [Display(Name = ("二甲医院"))]
        Class2,

        [Display(Name = "三甲医院")]
        Class3
    }

    public class Hospital : TopBasePoco
    {
        [Display(Name = "医院级别")]
        [Required(ErrorMessage = "医院级别为必填项")]
        public HospitalLevel? Level { get; set; }

        [Display(Name = "医院地点")]
        public City Location { get; set; }

        [Display(Name = "医院地点编号")]
        [Required(ErrorMessage = "医院地点编号为必填项")]
        public Guid? LocationId { get; set; }

        [Display(Name = "医院名称")]
        [Required(ErrorMessage = "医院名称为必填项")]
        public string Name { get; set; }
    }
}