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
    public partial class ReportTemplateVM : BaseTemplateVM
    {

	    protected override void InitVM()
        {
        }

    }

    public class ReportImportVM : BaseImportVM<ReportTemplateVM, Report>
    {

    }

}
