using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using test.Model;


namespace test.ViewModel.Patients.ReportVMs
{
    public partial class ReportBatchVM : BaseBatchVM<Report, Report_BatchEdit>
    {
        public ReportBatchVM()
        {
            ListVM = new ReportListVM();
            LinkedVM = new Report_BatchEdit();
        }

    }

	/// <summary>
    /// Class to define batch edit fields
    /// </summary>
    public class Report_BatchEdit : BaseVM
    {

        protected override void InitVM()
        {
        }

    }

}
