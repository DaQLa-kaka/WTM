using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using test.Model;


namespace test.ViewModel.Api.PatientVMs
{
    public partial class PatientApiSearcher : BaseSearcher
    {
        [Display(Name = "患者姓名")]
        public String PatientName { get; set; }
        [Display(Name = "状态")]
        public PatientStatusEnum? Status { get; set; }
        [Display(Name = "所属医院")]
        public Guid? HospitalID { get; set; }

        protected override void InitVM()
        {
        }

    }
}
