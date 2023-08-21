using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using test.Model;


namespace test.ViewModel.BasicData.CityVMs
{
    public partial class CityListVM : BasePagedListVM<City_View, CitySearcher>
    {
        protected override List<GridAction> InitGridAction()
        {
            return new List<GridAction>
            {
                this.MakeStandardAction("City", GridActionStandardTypesEnum.Create, Localizer["Sys.Create"],"BasicData", dialogWidth: 800),
                this.MakeStandardAction("City", GridActionStandardTypesEnum.Edit, Localizer["Sys.Edit"], "BasicData", dialogWidth: 800),
                this.MakeStandardAction("City", GridActionStandardTypesEnum.Delete, Localizer["Sys.Delete"], "BasicData", dialogWidth: 800),
                this.MakeStandardAction("City", GridActionStandardTypesEnum.Details, Localizer["Sys.Details"], "BasicData", dialogWidth: 800),
                this.MakeStandardAction("City", GridActionStandardTypesEnum.BatchEdit, Localizer["Sys.BatchEdit"], "BasicData", dialogWidth: 800),
                this.MakeStandardAction("City", GridActionStandardTypesEnum.BatchDelete, Localizer["Sys.BatchDelete"], "BasicData", dialogWidth: 800),
                this.MakeStandardAction("City", GridActionStandardTypesEnum.Import, Localizer["Sys.Import"], "BasicData", dialogWidth: 800),
                this.MakeStandardAction("City", GridActionStandardTypesEnum.ExportExcel, Localizer["Sys.Export"], "BasicData"),
            };
        }


        protected override IEnumerable<IGridColumn<City_View>> InitGridHeader()
        {
            return new List<GridColumn<City_View>>{
                this.MakeGridHeader(x => x.CityName),
                this.MakeGridHeader(x => x.CityName_view),
                this.MakeGridHeaderAction(width: 200)
            };
        }

        public override IOrderedQueryable<City_View> GetSearchQuery()
        {
            var query = DC.Set<City>()
                .CheckContain(Searcher.CityName, x=>x.CityName)
                .CheckEqual(Searcher.ParentId, x=>x.ParentId)
                .Select(x => new City_View
                {
				    ID = x.ID,
                    CityName = x.CityName,
                    CityName_view = x.Parent.CityName,
                })
                .OrderBy(x => x.ID);
            return query;
        }

    }

    public class City_View : City{
        [Display(Name = "城市名称")]
        public String CityName_view { get; set; }

    }
}
