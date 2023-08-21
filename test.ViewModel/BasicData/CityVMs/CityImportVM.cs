using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using test.Model;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;

namespace test.ViewModel.BasicData.CityVMs
{
    public partial class CityTemplateVM : BaseTemplateVM
    {
        [Display(Name = "省")]
        public ExcelPropety Sheng_Excel = ExcelPropety.CreateProperty<City>(x => x.Sheng);

        [Display(Name = "市")]
        public ExcelPropety Shi_Excel = ExcelPropety.CreateProperty<City>(x => x.Shi);

        [Display(Name = "区")]
        public ExcelPropety Qu_Excel = ExcelPropety.CreateProperty<City>(x => x.Qu);

        protected override void InitVM()
        {
        }
    }

    public class CityImportVM : BaseImportVM<CityTemplateVM, City>
    {
        public override bool BatchSaveData()
        {
            this.SetEntityList();
            List<City> newList = new List<City>();
            var shengs = EntityList.Select(x => x.Sheng).Distinct();
            foreach (var sheng in shengs)
            {
                City c = new City();
                {
                    c.CityName = sheng;
                }
                newList.Add(c);
                var shis = EntityList.Where(x => x.Sheng == sheng).Select(x => x.Shi).Distinct();
                foreach (var shi in shis)
                {
                    City c2 = new City();
                    {
                        c2.CityName = shi;
                        c2.Parent = c;
                        c2.ParentId = c.ID;
                    }
                    newList.Add(c2);
                    var qus = EntityList.Where(x => x.Sheng == sheng && x.Shi == shi && x.Qu != "市辖区").Select(x => x.Qu).Distinct();
                    foreach (var qu in qus)
                    {
                        City c3 = new City();
                        {
                            c3.CityName = qu;
                            c3.Parent = c2;
                            c3.ParentId = c2.ID;
                        }
                        newList.Add(c3);
                    }
                }
            }
            this.EntityList = newList;
            return base.BatchSaveData();
        }
    }
}