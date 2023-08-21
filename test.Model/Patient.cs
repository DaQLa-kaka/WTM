using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WalkingTec.Mvvm.Core;

namespace test.Model
{
    public enum GenderEnum
    {
        [Display(Name = "男")]
        Male,

        [Display(Name = "女")]
        Female
    }

    public enum PatientStatusEnum
    {
        [Display(Name = "无症状")]
        WuZhengZhuang,

        [Display(Name = "疑似")]
        YiSi,

        [Display(Name = "确诊")]
        QueZhen,

        [Display(Name = "治愈")]
        ZhiYu,

        [Display(Name = "死亡")]
        SiWang
    }

    [Table("MyPatient")]
    public class Patient : PersistPoco
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public new int ID { get; set; }

        [Display(Name = "患者姓名")]
        public string PatientName { get; set; }

        [Display(Name = "身份证")]
        [Required(ErrorMessage = "身份证号为必填项")]
        [RegularExpression("/^(\\d{18,18}|\\d{15,15}|\\d{17,17}x)/", ErrorMessage = "身份证号格式错误")]
        public string IdNumber { get; set; }

        [Display(Name = "性别")]
        public GenderEnum? Gender { get; set; }

        [Display(Name = "状态")]
        public PatientStatusEnum? Status { get; set; }

        [Display(Name = "生日")]
        public DateTime? Birthday { get; set; }

        [Display(Name = "籍贯")]
        public City Location { get; set; }

        [Display(Name = "籍贯")]
        public Guid? LocationId { get; set; }

        [Display(Name = "所属医院")]
        public Hospital Hospital { get; set; }

        [Display(Name = "所属医院")]
        public Guid? HospitalID { get; set; }

        public FileAttachment Photo { get; set; }

        [Display(Name = "照片")]
        public Guid? PhotoId { get; set; }

        [Display(Name = "病毒")]
        public List<PatientVirus> Viruses { get; set; }

        public List<Report> Reports { get; set; }

        [Display(Name = "年龄")]
        [NotMapped]
        public int Age
        {
            get
            {
                return DateTime.Now.Year - Birthday.Value.Year;
            }
        }
    }
}