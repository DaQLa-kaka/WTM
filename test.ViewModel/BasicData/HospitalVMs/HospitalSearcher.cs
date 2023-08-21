using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using test.Model;


namespace test.ViewModel.BasicData.HospitalVMs
{
    public partial class HospitalSearcher : BaseSearcher
    {
        [Display(Name = "医院级别")]
        public HospitalLevel? Level { get; set; }
        public List<ComboSelectListItem> AllLocations { get; set; }
        [Display(Name = "医院地点")]
        public Guid? LocationId { get; set; }
        [Display(Name = "医院名称")]
        public String Name { get; set; }

        protected override void InitVM()
        {
            AllLocations = DC.Set<City>().GetSelectListItems(Wtm, y => y.CityName);
        }

    }
}
