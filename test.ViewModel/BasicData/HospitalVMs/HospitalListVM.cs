using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using test.Model;


namespace test.ViewModel.BasicData.HospitalVMs
{
    public partial class HospitalListVM : BasePagedListVM<Hospital_View, HospitalSearcher>
    {
        protected override List<GridAction> InitGridAction()
        {
            return new List<GridAction>
            {
                this.MakeStandardAction("Hospital", GridActionStandardTypesEnum.Create, Localizer["Sys.Create"],"BasicData", dialogWidth: 800),
                this.MakeStandardAction("Hospital", GridActionStandardTypesEnum.Edit, Localizer["Sys.Edit"], "BasicData", dialogWidth: 800),
                this.MakeStandardAction("Hospital", GridActionStandardTypesEnum.Delete, Localizer["Sys.Delete"], "BasicData", dialogWidth: 800),
                this.MakeStandardAction("Hospital", GridActionStandardTypesEnum.Details, Localizer["Sys.Details"], "BasicData", dialogWidth: 800),
                this.MakeStandardAction("Hospital", GridActionStandardTypesEnum.BatchEdit, Localizer["Sys.BatchEdit"], "BasicData", dialogWidth: 800),
                this.MakeStandardAction("Hospital", GridActionStandardTypesEnum.BatchDelete, Localizer["Sys.BatchDelete"], "BasicData", dialogWidth: 800),
                this.MakeStandardAction("Hospital", GridActionStandardTypesEnum.Import, Localizer["Sys.Import"], "BasicData", dialogWidth: 800),
                this.MakeStandardAction("Hospital", GridActionStandardTypesEnum.ExportExcel, Localizer["Sys.Export"], "BasicData"),
            };
        }


        protected override IEnumerable<IGridColumn<Hospital_View>> InitGridHeader()
        {
            return new List<GridColumn<Hospital_View>>{
                this.MakeGridHeader(x => x.Level),
                this.MakeGridHeader(x => x.CityName_view),
                this.MakeGridHeader(x => x.Name),
                this.MakeGridHeaderAction(width: 200)
            };
        }

        public override IOrderedQueryable<Hospital_View> GetSearchQuery()
        {
            var query = DC.Set<Hospital>()
                .CheckEqual(Searcher.Level, x=>x.Level)
                .CheckEqual(Searcher.LocationId, x=>x.LocationId)
                .CheckContain(Searcher.Name, x=>x.Name)
                .Select(x => new Hospital_View
                {
				    ID = x.ID,
                    Level = x.Level,
                    CityName_view = x.Location.CityName,
                    Name = x.Name,
                })
                .OrderBy(x => x.ID);
            return query;
        }

    }

    public class Hospital_View : Hospital{
        [Display(Name = "城市名称")]
        public String CityName_view { get; set; }

    }
}
