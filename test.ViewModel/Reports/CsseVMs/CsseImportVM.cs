using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using test.Model;


namespace test.ViewModel.Reports.CsseVMs
{
    public partial class CsseTemplateVM : BaseTemplateVM
    {
        [Display(Name = "国家")]
        public ExcelPropety Country_Excel = ExcelPropety.CreateProperty<Csse>(x => x.Country);
        [Display(Name = "省")]
        public ExcelPropety Province_Excel = ExcelPropety.CreateProperty<Csse>(x => x.Province);
        [Display(Name = "纬度")]
        public ExcelPropety Lat_Excel = ExcelPropety.CreateProperty<Csse>(x => x.Lat);
        [Display(Name = "经度")]
        public ExcelPropety Long_Excel = ExcelPropety.CreateProperty<Csse>(x => x.Long);
        [Display(Name = "日期")]
        public ExcelPropety Date_Excel = ExcelPropety.CreateProperty<Csse>(x => x.Date);
        [Display(Name = "确诊")]
        public ExcelPropety ConfirmedCase_Excel = ExcelPropety.CreateProperty<Csse>(x => x.ConfirmedCase);

	    protected override void InitVM()
        {
        }

    }

    public class CsseImportVM : BaseImportVM<CsseTemplateVM, Csse>
    {

    }

}
