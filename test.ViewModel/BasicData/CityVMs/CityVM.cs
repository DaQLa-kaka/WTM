using System.Collections.Generic;
using System.Linq;
using test.Model;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;

namespace test.ViewModel.BasicData.CityVMs
{
    public partial class CityVM : BaseCRUDVM<City>
    {
        public List<ComboSelectListItem> AllParents { get; set; }

        public CityVM()
        {
            SetInclude(x => x.Parent);
        }

        protected override void InitVM()
        {
            AllParents = DC.Set<City>().GetSelectListItems(Wtm, y => y.CityName);
        }

        public override void DoAdd()
        {
            using (var transation = DC.BeginTransaction())
            {
                try
                {
                    base.DoAdd();
                    HospitalVMs.HospitalVM vm = new HospitalVMs.HospitalVM();
                    vm.CopyContext(this);
                    vm.SetEntity(new Hospital
                    {
                        Name = Entity.CityName + "默认医院",
                        LocationId = Entity.ID
                    });
                    vm.Validate();
                    vm.DoAdd();
                    transation.Commit();
                }
                catch
                {
                    transation.Rollback();
                    MSD.AddModelError("no123", "添加失败");
                }
            }
        }

        public override void DoEdit(bool updateAllFields = false)
        {
            base.DoEdit(updateAllFields);
        }

        public override void DoDelete()
        {
            var patients = DC.Set<Patient>().Where(x => x.Hospital.LocationId == Entity.ID).Select(x => x.ID).ToList();
            foreach (var item in patients)
            {
                var patient = new Patient { ID = item, HospitalID = null };
                DC.UpdateProperty(patient, x => x.HospitalID);
            }
            base.DoDelete();
        }
    }
}