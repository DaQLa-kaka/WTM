﻿using System.Collections.Generic;
using test.Model;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;

namespace test.ViewModel.Patients.ReportVMs
{
    public partial class ReportVM : BaseCRUDVM<Report>
    {
        public List<ComboSelectListItem> AllPatients { get; set; }

        public ReportVM()
        {
            SetInclude(x => x.Patient);
        }

        protected override void InitVM()
        {
            AllPatients = DC.Set<Patient>().GetSelectListItems(Wtm, y => y.PatientName);
        }

        public override void DoAdd()
        {
            base.DoAdd();
        }

        public override void DoEdit(bool updateAllFields = false)
        {
            base.DoEdit(updateAllFields);
        }

        public override void DoDelete()
        {
            base.DoDelete();
        }
    }
}