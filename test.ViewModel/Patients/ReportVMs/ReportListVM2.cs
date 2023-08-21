using System.Collections.Generic;
using System.Linq;
using test.Model;
using WalkingTec.Mvvm.Core;

namespace test.ViewModel.Patients.ReportVMs
{
    public partial class ReportListVM2 : BasePagedListVM<Report, ReportSearcher>
    {
        protected override List<GridAction> InitGridAction()
        {
            return new List<GridAction>
            {
                this.MakeAction("Report", "Create2", "新建","新建", GridActionParameterTypesEnum.NoId,"Patients").SetOnClickScript("CreateReport"),
                this.MakeStandardAction("Report", GridActionStandardTypesEnum.Edit, "修改","Patients", dialogWidth: 800),
                this.MakeStandardAction("Report", GridActionStandardTypesEnum.Delete, "删除", "Patients",dialogWidth: 800),
                this.MakeStandardAction("Report", GridActionStandardTypesEnum.Details, "详细","Patients", dialogWidth: 800),
                this.MakeStandardAction("Report", GridActionStandardTypesEnum.BatchEdit, "批量修改","Patients", dialogWidth: 800),
                this.MakeStandardAction("Report", GridActionStandardTypesEnum.BatchDelete, "批量删除","Patients", dialogWidth: 800),
            };
        }

        protected override IEnumerable<IGridColumn<Report>> InitGridHeader()
        {
            return new List<GridColumn<Report>>
                {
                    this.MakeGridHeader(x => x.Temprature),
                    this.MakeGridHeader(x => x.Remark),
                    this.MakeGridHeader(x => x.PatientName_view),
                };
        }

        public override IOrderedQueryable<Report> GetSearchQuery()
        {
            var query = DC.Set<Report>()
                .Where(x => x.PatientId == Searcher.PatientId)
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