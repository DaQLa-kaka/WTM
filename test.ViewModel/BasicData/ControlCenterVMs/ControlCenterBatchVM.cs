using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using test.Model;


namespace test.ViewModel.BasicData.ControlCenterVMs
{
    public partial class ControlCenterBatchVM : BaseBatchVM<ControlCenter, ControlCenter_BatchEdit>
    {
        public ControlCenterBatchVM()
        {
            ListVM = new ControlCenterListVM();
            LinkedVM = new ControlCenter_BatchEdit();
        }

    }

	/// <summary>
    /// Class to define batch edit fields
    /// </summary>
    public class ControlCenter_BatchEdit : BaseVM
    {
        public List<ComboSelectListItem> AllLocations { get; set; }
        [Display(Name = "中心地点")]
        public Guid? LocationId { get; set; }

        protected override void InitVM()
        {
            AllLocations = DC.Set<City>().GetSelectListItems(Wtm, y => y.CityName);
        }

    }

}
