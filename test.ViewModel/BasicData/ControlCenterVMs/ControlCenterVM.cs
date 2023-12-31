﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using test.Model;


namespace test.ViewModel.BasicData.ControlCenterVMs
{
    public partial class ControlCenterVM : BaseCRUDVM<ControlCenter>
    {
        public List<ComboSelectListItem> AllLocations { get; set; }

        public ControlCenterVM()
        {
            SetInclude(x => x.Location);
        }

        protected override void InitVM()
        {
            AllLocations = DC.Set<City>().GetSelectListItems(Wtm, y => y.CityName);
        }

        public override void DoAdd()
        {           
            base.DoAdd();
        }

        public override void DoEdit(bool updateAllFields = false)
        {
            base.DoEdit(updateAllFields);
        }

        public override void DoDelete()
        {
            base.DoDelete();
        }
    }
}
