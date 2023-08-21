using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using test.Model;


namespace test.ViewModel.BasicData.ControlCenterVMs
{
    public partial class ControlCenterListVM : BasePagedListVM<ControlCenter_View, ControlCenterSearcher>
    {
        protected override List<GridAction> InitGridAction()
        {
            return new List<GridAction>
            {
                this.MakeStandardAction("ControlCenter", GridActionStandardTypesEnum.Create, Localizer["Sys.Create"],"BasicData", dialogWidth: 800),
                this.MakeStandardAction("ControlCenter", GridActionStandardTypesEnum.Edit, Localizer["Sys.Edit"], "BasicData", dialogWidth: 800),
                this.MakeStandardAction("ControlCenter", GridActionStandardTypesEnum.Delete, Localizer["Sys.Delete"], "BasicData", dialogWidth: 800),
                this.MakeStandardAction("ControlCenter", GridActionStandardTypesEnum.Details, Localizer["Sys.Details"], "BasicData", dialogWidth: 800),
                this.MakeStandardAction("ControlCenter", GridActionStandardTypesEnum.BatchEdit, Localizer["Sys.BatchEdit"], "BasicData", dialogWidth: 800),
                this.MakeStandardAction("ControlCenter", GridActionStandardTypesEnum.BatchDelete, Localizer["Sys.BatchDelete"], "BasicData", dialogWidth: 800),
                this.MakeStandardAction("ControlCenter", GridActionStandardTypesEnum.Import, Localizer["Sys.Import"], "BasicData", dialogWidth: 800),
                this.MakeStandardAction("ControlCenter", GridActionStandardTypesEnum.ExportExcel, Localizer["Sys.Export"], "BasicData"),
            };
        }


        protected override IEnumerable<IGridColumn<ControlCenter_View>> InitGridHeader()
        {
            return new List<GridColumn<ControlCenter_View>>{
                this.MakeGridHeader(x => x.Name),
                this.MakeGridHeader(x => x.CityName_view),
                this.MakeGridHeaderAction(width: 200)
            };
        }

        public override IOrderedQueryable<ControlCenter_View> GetSearchQuery()
        {
            var query = DC.Set<ControlCenter>()
                .CheckContain(Searcher.Name, x=>x.Name)
                .CheckEqual(Searcher.LocationId, x=>x.LocationId)
                .Select(x => new ControlCenter_View
                {
				    ID = x.ID,
                    Name = x.Name,
                    CityName_view = x.Location.CityName,
                })
                .OrderBy(x => x.ID);
            return query;
        }

    }

    public class ControlCenter_View : ControlCenter{
        [Display(Name = "城市名称")]
        public String CityName_view { get; set; }

    }
}
