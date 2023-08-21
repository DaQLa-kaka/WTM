using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WalkingTec.Mvvm.Core;

namespace test.Model
{
    public enum VirusTypeEnum
    {
        RNA, DNA
    }

    public class Virus : TopBasePoco
    {
        [Display(Name = "病毒代码")]
        [Required(ErrorMessage = "病毒代码为必填项")]
        [StringLength(10, ErrorMessage = "病毒代码最多10个字符")]
        public string VirusCode { get; set; }

        [Display(Name = "病毒名称")]
        [Required(ErrorMessage = "病毒名称为必填项")]
        public string VirusName { get; set; }

        [Display(Name = "病毒种类")]
        [Required(ErrorMessage = "病毒种类为必填项")]
        public VirusTypeEnum? VirusType { get; set; }

        [Display(Name = "病毒描述")]
        public string Remark { get; set; }

        [Display(Name = "患者")]
        public List<PatientVirus> Patients { get; set; }
    }
}