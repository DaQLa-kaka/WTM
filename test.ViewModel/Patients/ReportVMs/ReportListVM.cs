using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using test.Model;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;

namespace test.ViewModel.Patients.ReportVMs
{
    public partial class ReportListVM : BasePagedListVM<Report_View, ReportSearcher>
    {
        public ReportListVM()
        { DetailGridPrix = "Entity.Reports"; }

        protected override List<GridAction> InitGridAction()
        {
            if (SearcherMode == ListVMSearchModeEnum.MasterDetail)
            {
                return new List<GridAction>();
            }
            else
            {
                return new List<GridAction>
            {
                this.MakeStandardAction("Report", GridActionStandardTypesEnum.Create, Localizer["Sys.Create"],"Patients", dialogWidth: 800),
                this.MakeStandardAction("Report", GridActionStandardTypesEnum.Delete, Localizer["Sys.Delete"], "Patients", dialogWidth: 800),
            };
            }
        }

        protected override IEnumerable<IGridColumn<Report_View>> InitGridHeader()
        {
            if (SearcherMode == ListVMSearchModeEnum.MasterDetail)
            {
                return new List<GridColumn<Report_View>>{
                this.MakeGridHeader(x => x.Temprature),
                this.MakeGridHeader(x => x.Remark),
                this.MakeGridHeader(x => x.PatientName_view)
                };
            }
            else
            {
                return new List<GridColumn<Report_View>>{
                this.MakeGridHeader(x => x.Temprature).SetEditType(EditTypeEnum.TextBox),
                this.MakeGridHeader(x => x.Remark).SetEditType(EditTypeEnum.TextBox),
                this.MakeGridHeaderAction(width: 200)
            };
            }
        }

        public override IOrderedQueryable<Report_View> GetSearchQuery()
        {
            var query = DC.Set<Report>()
                .CheckEqual(Searcher.PatientId, x => x.PatientId)
                .Select(x => new Report_View
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

    public class Report_View : Report
    {
        [Display(Name = "患者姓名")]
        public String PatientName_view { get; set; }
    }
}