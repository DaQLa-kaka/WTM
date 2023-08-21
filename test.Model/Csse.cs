using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WalkingTec.Mvvm.Core;

namespace test.Model
{
    public class Csse : TopBasePoco
    {
        [Display(Name = "国家")]
        public string Country { get; set; }

        [Display(Name = "省")]
        public string Province { get; set; }

        [Display(Name = "纬度")]
        public double Lat { get; set; }

        [Display(Name = "经度")]
        public double Long { get; set; }

        [Display(Name = "日期")]
        [Column(TypeName = "DATE")]
        public DateTime Date { get; set; }

        [Display(Name = "确诊")]
        public int ConfirmedCase { get; set; }
    }
}