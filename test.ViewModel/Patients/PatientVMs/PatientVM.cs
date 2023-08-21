using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using test.Model;
using test.ViewModel.Patients.ReportVMs;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;

namespace test.ViewModel.Patients.PatientVMs
{
    public partial class PatientVM : BaseCRUDVM<Patient>
    {
        public ReportListVM Reports { get; set; } = new ReportListVM();
        public List<ComboSelectListItem> AllSheng { get; set; }
        public List<ComboSelectListItem> AllShi { get; set; }
        public List<ComboSelectListItem> AllQu { get; set; }
        public List<ComboSelectListItem> AllHospitals { get; set; }
        public List<ComboSelectListItem> AllVirusess { get; set; }

        [Display(Name = "病毒")]
        public List<string> SelectedVirusesIDs { get; set; }

        public Guid? ShengId { get; set; }

        public Guid? ShiId { get; set; }

        public PatientVM()
        {
            SetInclude(x => x.Location);
            SetInclude(x => x.Hospital);
            SetInclude(x => x.Viruses);
        }

        protected override void InitVM()
        {
            Reports.CopyContext(this);
            Reports.Searcher.PatientId = Entity.ID;
            AllSheng = DC.Set<City>().Where(x => x.ParentId == null).GetSelectListItems(Wtm, y => y.CityName);

            if (Entity.LocationId != null)
            {
                ShiId = DC.Set<City>().Where(a => a.ID == Entity.LocationId).Select(a => a.ParentId).SingleOrDefault();
                ShengId = DC.Set<City>().Where(a => a.ID == ShiId).Select(a => a.ParentId).SingleOrDefault();
                AllQu = DC.Set<City>().Where(x => x.ParentId == ShiId).GetSelectListItems(Wtm, y => y.CityName);
                AllShi = DC.Set<City>().Where(x => x.ParentId == ShengId).GetSelectListItems(Wtm, y => y.CityName);
            }

            AllHospitals = DC.Set<Hospital>().GetSelectListItems(Wtm, y => y.Name);
            AllVirusess = DC.Set<Virus>().GetSelectListItems(Wtm, y => y.VirusName);
            SelectedVirusesIDs = Entity.Viruses?.Select(x => x.VirusId.ToString()).ToList();
        }

        public override void DoAdd()
        {
            Entity.Viruses = new List<PatientVirus>();
            if (SelectedVirusesIDs != null)
            {
                foreach (var id in SelectedVirusesIDs)
                {
                    PatientVirus middle = new PatientVirus();
                    middle.SetPropertyValue("VirusId", id);
                    Entity.Viruses.Add(middle);
                }
            }

            base.DoAdd();
        }

        public override void DoEdit(bool updateAllFields = false)
        {
            Entity.Viruses = new List<PatientVirus>();
            if (SelectedVirusesIDs != null)
            {
                foreach (var item in SelectedVirusesIDs)
                {
                    PatientVirus middle = new PatientVirus();
                    middle.SetPropertyValue("VirusId", item);
                    Entity.Viruses.Add(middle);
                }
            }

            base.DoEdit(updateAllFields);
        }

        public override void DoDelete()
        {
            base.DoDelete();
        }

        public override DuplicatedInfo<Patient> SetDuplicatedCheck()
        {
            var rv = CreateFieldsInfo(SimpleField(x => x.IdNumber));
            return rv;
        }
    }
}