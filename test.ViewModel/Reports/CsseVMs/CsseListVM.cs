using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using test.Model;


namespace test.ViewModel.Reports.CsseVMs
{
    public partial class CsseListVM : BasePagedListVM<Csse_View, CsseSearcher>
    {
        protected override List<GridAction> InitGridAction()
        {
            return new List<GridAction>
            {
                this.MakeStandardAction("Csse", GridActionStandardTypesEnum.Create, Localizer["Sys.Create"],"Reports", dialogWidth: 800),
                this.MakeStandardAction("Csse", GridActionStandardTypesEnum.Edit, Localizer["Sys.Edit"], "Reports", dialogWidth: 800),
                this.MakeStandardAction("Csse", GridActionStandardTypesEnum.Delete, Localizer["Sys.Delete"], "Reports", dialogWidth: 800),
                this.MakeStandardAction("Csse", GridActionStandardTypesEnum.Details, Localizer["Sys.Details"], "Reports", dialogWidth: 800),
                this.MakeStandardAction("Csse", GridActionStandardTypesEnum.BatchEdit, Localizer["Sys.BatchEdit"], "Reports", dialogWidth: 800),
                this.MakeStandardAction("Csse", GridActionStandardTypesEnum.BatchDelete, Localizer["Sys.BatchDelete"], "Reports", dialogWidth: 800),
                this.MakeStandardAction("Csse", GridActionStandardTypesEnum.Import, Localizer["Sys.Import"], "Reports", dialogWidth: 800),
                this.MakeStandardAction("Csse", GridActionStandardTypesEnum.ExportExcel, Localizer["Sys.Export"], "Reports"),
            };
        }


        protected override IEnumerable<IGridColumn<Csse_View>> InitGridHeader()
        {
            return new List<GridColumn<Csse_View>>{
                this.MakeGridHeader(x => x.Country),
                this.MakeGridHeader(x => x.Province),
                this.MakeGridHeader(x => x.Lat),
                this.MakeGridHeader(x => x.Long),
                this.MakeGridHeader(x => x.Date),
                this.MakeGridHeader(x => x.ConfirmedCase),
                this.MakeGridHeaderAction(width: 200)
            };
        }

        public override IOrderedQueryable<Csse_View> GetSearchQuery()
        {
            var query = DC.Set<Csse>()
                .CheckContain(Searcher.Country, x=>x.Country)
                .CheckContain(Searcher.Province, x=>x.Province)
                .Select(x => new Csse_View
                {
				    ID = x.ID,
                    Country = x.Country,
                    Province = x.Province,
                    Lat = x.Lat,
                    Long = x.Long,
                    Date = x.Date,
                    ConfirmedCase = x.ConfirmedCase,
                })
                .OrderBy(x => x.ID);
            return query;
        }

    }

    public class Csse_View : Csse{

    }
}
