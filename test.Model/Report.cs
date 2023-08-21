using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WalkingTec.Mvvm.Core;

namespace test.Model
{
    public class Report : BasePoco
    {
        [Display(Name = "体温")]
        [Required(ErrorMessage = "体温为必填项")]
        [Range(35, 45, ErrorMessage = "体温必须在35°到45°之间")]
        public float? Temprature { get; set; }

        [Display(Name = "备注")]
        public string Remark { get; set; }

        public Patient Patient { get; set; }

        [Display(Name = "患者")]
        [Required(ErrorMessage = "患者为必填项")]
        public int? PatientId { get; set; }

        [Display(Name = "患者姓名")]
        [NotMapped]
        public String PatientName_view { get; set; }
    }
}