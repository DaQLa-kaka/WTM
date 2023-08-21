using System.Collections.Generic;
using System.Linq;
using test.Model;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;

namespace test.ViewModel.Patients.ReportVMs
{
    public partial class ReportListVM21 : BasePagedListVM<Report, ReportSearcher>
    {
        protected override List<GridAction> InitGridAction()
        {
            return new List<GridAction>
            {
                this.MakeAction("Report","Create2","新建","新建",GridActionParameterTypesEnum.NoId,"Patients").SetOnClickScript("CreateReport"),
                this.MakeStandardAction("Report", GridActionStandardTypesEnum.Edit, Localizer["Sys.Edit"], "Patients", dialogWidth: 800),
                this.MakeStandardAction("Report", GridActionStandardTypesEnum.Delete, Localizer["Sys.Delete"], "Patients", dialogWidth: 800),
                this.MakeStandardAction("Report", GridActionStandardTypesEnum.Details, Localizer["Sys.Details"], "Patients", dialogWidth: 800),
                this.MakeStandardAction("Report", GridActionStandardTypesEnum.BatchEdit, Localizer["Sys.BatchEdit"], "Patients", dialogWidth: 800),
                this.MakeStandardAction("Report", GridActionStandardTypesEnum.BatchDelete, Localizer["Sys.BatchDelete"], "Patients", dialogWidth: 800),
                this.MakeStandardAction("Report", GridActionStandardTypesEnum.Import, Localizer["Sys.Import"], "Patients", dialogWidth: 800),
                this.MakeStandardAction("Report", GridActionStandardTypesEnum.ExportExcel, Localizer["Sys.Export"], "Patients"),
            };
        }

        protected override IEnumerable<IGridColumn<Report>> InitGridHeader()
        {
            return new List<GridColumn<Report>>{
                this.MakeGridHeader(x => x.Temprature),
                this.MakeGridHeader(x => x.Remark),
                this.MakeGridHeader(x => x.Patient.PatientName),
                this.MakeGridHeaderAction(width: 200)
            };
        }

        public override IOrderedQueryable<Report> GetSearchQuery()
        {
            var query = DC.Set<Report>()
                .CheckEqual(Searcher.PatientId, x => x.PatientId)
                .Select(x => new Report
                {
                    ID = x.ID,
                    Temprature = x.Temprature,
                    Remark = x.Remark,
                    PatientName_view = x.Patient.PatientName,
                })
                .OrderBy(x => x.ID);
            return query;
        }
    }
}