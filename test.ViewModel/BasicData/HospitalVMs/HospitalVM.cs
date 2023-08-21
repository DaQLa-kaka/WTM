using Microsoft.EntityFrameworkCore;
using System.Linq;
using test.Model;
using test.ViewModel.BasicData.CityVMs;
using WalkingTec.Mvvm.Core;

namespace test.ViewModel.BasicData.HospitalVMs
{
    public partial class HospitalVM : BaseCRUDVM<Hospital>
    {
        //public List<ComboSelectListItem> AllLocations { get; set; }
        public CityListVM Cities { get; set; }

        public HospitalVM()
        {
            SetInclude(x => x.Location);
        }

        public override void Validate()
        {
            var patientCount = DC.Set<Patient>().AsNoTracking().Where(x => x.LocationId == Entity.LocationId).Count();
            if (patientCount == 0)
            {
                MSD.AddModelError("Entity.LocationId", "所选地区没有病人");
            }
            base.Validate();
        }

        protected override void InitVM()
        {
            //AllLocations = DC.Set<City>().GetSelectListItems(Wtm, y => y.CityName);
            Cities = new CityListVM();
            Cities.CopyContext(this);
        }

        protected override Hospital GetById(object Id)
        {
            return base.GetById(Id);
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

        public override DuplicatedInfo<Hospital> SetDuplicatedCheck()
        {
            var rv = CreateFieldsInfo(SimpleField(x => x.Name));
            return rv;
        }
    }
}