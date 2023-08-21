using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using test.Model;


namespace test.ViewModel.BasicData.CityVMs
{
    public partial class CitySearcher : BaseSearcher
    {
        [Display(Name = "城市名称")]
        public String CityName { get; set; }
        public List<ComboSelectListItem> AllParents { get; set; }
        [Display(Name = "上级省市")]
        public Guid? ParentId { get; set; }

        protected override void InitVM()
        {
            AllParents = DC.Set<City>().GetSelectListItems(Wtm, y => y.CityName);
        }

    }
}
