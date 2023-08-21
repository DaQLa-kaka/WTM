using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using test.Model;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;

namespace test.ViewModel.Patients.PatientVMs
{
    public partial class PatientSearcher : BaseSearcher
    {
        [Display(Name = "患者姓名")]
        public String PatientName { get; set; }

        [Display(Name = "身份证")]
        public String IdNumber { get; set; }

        [Display(Name = "性别")]
        public Model.GenderEnum? Gender { get; set; }

        [Display(Name = "状态")]
        public PatientStatusEnum? Status { get; set; }

        public List<ComboSelectListItem> AllHospitals { get; set; }

        [Display(Name = "所属医院")]
        public Guid? HospitalID { get; set; }

        public List<ComboSelectListItem> AllVirusess { get; set; }

        [Display(Name = "病毒")]
        public List<Guid> SelectedVirusesIDs { get; set; }

        public Guid? LocationId { get; set; }

        protected override void InitVM()
        {
            AllHospitals = DC.Set<Hospital>().GetSelectListItems(Wtm, y => y.Name);
            AllVirusess = DC.Set<Virus>().GetSelectListItems(Wtm, y => y.VirusName);
        }
    }
}