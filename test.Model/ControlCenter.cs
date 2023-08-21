﻿using System;
using System.ComponentModel.DataAnnotations;
using WalkingTec.Mvvm.Core;

namespace test.Model
{
    public class ControlCenter : TopBasePoco
    {
        [Display(Name = "中心名称")]
        [Required(ErrorMessage = "中心名称为必填项")]
        public string Name { get; set; }

        [Display(Name = "中心地点")]
        public City Location { get; set; }

        [Display(Name = "中心地点")]
        [Required(ErrorMessage = "中心地点为必填项")]
        public Guid? LocationId { get; set; }
    }
}