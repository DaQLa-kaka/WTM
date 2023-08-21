using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using test.Model;


namespace test.ViewModel.BasicData.VirusVMs
{
    public partial class VirusListVM : BasePagedListVM<Virus_View, VirusSearcher>
    {
        protected override List<GridAction> InitGridAction()
        {
            return new List<GridAction>
            {
                this.MakeStandardAction("Virus", GridActionStandardTypesEnum.Create, Localizer["Sys.Create"],"BasicData", dialogWidth: 800),
                this.MakeStandardAction("Virus", GridActionStandardTypesEnum.Edit, Localizer["Sys.Edit"], "BasicData", dialogWidth: 800),
                this.MakeStandardAction("Virus", GridActionStandardTypesEnum.Delete, Localizer["Sys.Delete"], "BasicData", dialogWidth: 800),
                this.MakeStandardAction("Virus", GridActionStandardTypesEnum.Details, Localizer["Sys.Details"], "BasicData", dialogWidth: 800),
                this.MakeStandardAction("Virus", GridActionStandardTypesEnum.BatchEdit, Localizer["Sys.BatchEdit"], "BasicData", dialogWidth: 800),
                this.MakeStandardAction("Virus", GridActionStandardTypesEnum.BatchDelete, Localizer["Sys.BatchDelete"], "BasicData", dialogWidth: 800),
                this.MakeStandardAction("Virus", GridActionStandardTypesEnum.Import, Localizer["Sys.Import"], "BasicData", dialogWidth: 800),
                this.MakeStandardAction("Virus", GridActionStandardTypesEnum.ExportExcel, Localizer["Sys.Export"], "BasicData"),
            };
        }


        protected override IEnumerable<IGridColumn<Virus_View>> InitGridHeader()
        {
            return new List<GridColumn<Virus_View>>{
                this.MakeGridHeader(x => x.VirusCode),
                this.MakeGridHeader(x => x.VirusName),
                this.MakeGridHeader(x => x.VirusType),
                this.MakeGridHeader(x => x.Remark),
                this.MakeGridHeaderAction(width: 200)
            };
        }

        public override IOrderedQueryable<Virus_View> GetSearchQuery()
        {
            var query = DC.Set<Virus>()
                .CheckContain(Searcher.VirusCode, x=>x.VirusCode)
                .CheckContain(Searcher.VirusName, x=>x.VirusName)
                .CheckEqual(Searcher.VirusType, x=>x.VirusType)
                .Select(x => new Virus_View
                {
				    ID = x.ID,
                    VirusCode = x.VirusCode,
                    VirusName = x.VirusName,
                    VirusType = x.VirusType,
                    Remark = x.Remark,
                })
                .OrderBy(x => x.ID);
            return query;
        }

    }

    public class Virus_View : Virus{

    }
}
