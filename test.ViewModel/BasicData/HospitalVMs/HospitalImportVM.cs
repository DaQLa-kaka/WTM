using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using test.Model;


namespace test.ViewModel.BasicData.HospitalVMs
{
    public partial class HospitalTemplateVM : BaseTemplateVM
    {
        [Display(Name = "医院级别")]
        public ExcelPropety Level_Excel = ExcelPropety.CreateProperty<Hospital>(x => x.Level);
        [Display(Name = "医院地点")]
        public ExcelPropety Location_Excel = ExcelPropety.CreateProperty<Hospital>(x => x.LocationId);
        [Display(Name = "医院名称")]
        public ExcelPropety Name_Excel = ExcelPropety.CreateProperty<Hospital>(x => x.Name);

	    protected override void InitVM()
        {
            Location_Excel.DataType = ColumnDataType.ComboBox;
            Location_Excel.ListItems = DC.Set<City>().GetSelectListItems(Wtm, y => y.CityName);
        }

    }

    public class HospitalImportVM : BaseImportVM<HospitalTemplateVM, Hospital>
    {

    }

}
