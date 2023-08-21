using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Mvc;
using WalkingTec.Mvvm.Core.Extensions;
using test.ViewModel.BasicData.CityVMs;
using test.Model;
using System.Linq;

namespace test.Controllers
{
    [Area("BasicData")]
    [ActionDescription("城市管理")]
    public partial class CityController : BaseController
    {
        #region Search

        [ActionDescription("Sys.Search")]
        public ActionResult Index()
        {
            var vm = Wtm.CreateVM<CityListVM>();
            return PartialView(vm);
        }

        [ActionDescription("Sys.Search")]
        [HttpPost]
        public string Search(CitySearcher searcher)
        {
            var vm = Wtm.CreateVM<CityListVM>(passInit: true);
            if (ModelState.IsValid)
            {
                vm.Searcher = searcher;
                return vm.GetJson(false);
            }
            else
            {
                return vm.GetError();
            }
        }

        #endregion Search

        #region Create

        [ActionDescription("Sys.Create")]
        public ActionResult Create()
        {
            var vm = Wtm.CreateVM<CityVM>();
            return PartialView(vm);
        }

        [HttpPost]
        [ActionDescription("Sys.Create")]
        public ActionResult Create(CityVM vm)
        {
            if (!ModelState.IsValid)
            {
                return PartialView(vm);
            }
            else
            {
                vm.DoAdd();
                if (!ModelState.IsValid)
                {
                    vm.DoReInit();
                    return PartialView(vm);
                }
                else
                {
                    return FFResult().CloseDialog().RefreshGrid();
                }
            }
        }

        #endregion Create

        #region Edit

        [ActionDescription("Sys.Edit")]
        public ActionResult Edit(string id)
        {
            var vm = Wtm.CreateVM<CityVM>(id);
            return PartialView(vm);
        }

        [ActionDescription("Sys.Edit")]
        [HttpPost]
        [ValidateFormItemOnly]
        public ActionResult Edit(CityVM vm)
        {
            if (!ModelState.IsValid)
            {
                return PartialView(vm);
            }
            else
            {
                vm.DoEdit();
                if (!ModelState.IsValid)
                {
                    vm.DoReInit();
                    return PartialView(vm);
                }
                else
                {
                    return FFResult().CloseDialog().RefreshGridRow(vm.Entity.ID);
                }
            }
        }

        #endregion Edit

        #region Delete

        [ActionDescription("Sys.Delete")]
        public ActionResult Delete(string id)
        {
            var vm = Wtm.CreateVM<CityVM>(id);
            return PartialView(vm);
        }

        [ActionDescription("Sys.Delete")]
        [HttpPost]
        public ActionResult Delete(string id, IFormCollection nouse)
        {
            var vm = Wtm.CreateVM<CityVM>(id);
            vm.DoDelete();
            if (!ModelState.IsValid)
            {
                return PartialView(vm);
            }
            else
            {
                return FFResult().CloseDialog().RefreshGrid();
            }
        }

        #endregion Delete

        #region Details

        [ActionDescription("Sys.Details")]
        public ActionResult Details(string id)
        {
            var vm = Wtm.CreateVM<CityVM>(id);
            return PartialView(vm);
        }

        #endregion Details

        #region BatchEdit

        [HttpPost]
        [ActionDescription("Sys.BatchEdit")]
        public ActionResult BatchEdit(string[] IDs)
        {
            var vm = Wtm.CreateVM<CityBatchVM>(Ids: IDs);
            return PartialView(vm);
        }

        [HttpPost]
        [ActionDescription("Sys.BatchEdit")]
        public ActionResult DoBatchEdit(CityBatchVM vm, IFormCollection nouse)
        {
            if (!ModelState.IsValid || !vm.DoBatchEdit())
            {
                return PartialView("BatchEdit", vm);
            }
            else
            {
                return FFResult().CloseDialog().RefreshGrid().Alert(Localizer["Sys.BatchEditSuccess", vm.Ids.Length]);
            }
        }

        #endregion BatchEdit

        #region BatchDelete

        [HttpPost]
        [ActionDescription("Sys.BatchDelete")]
        public ActionResult BatchDelete(string[] IDs)
        {
            var vm = Wtm.CreateVM<CityBatchVM>(Ids: IDs);
            return PartialView(vm);
        }

        [HttpPost]
        [ActionDescription("Sys.BatchDelete")]
        public ActionResult DoBatchDelete(CityBatchVM vm, IFormCollection nouse)
        {
            if (!ModelState.IsValid || !vm.DoBatchDelete())
            {
                return PartialView("BatchDelete", vm);
            }
            else
            {
                return FFResult().CloseDialog().RefreshGrid().Alert(Localizer["Sys.BatchDeleteSuccess", vm.Ids.Length]);
            }
        }

        #endregion BatchDelete

        #region Import

        [ActionDescription("Sys.Import")]
        public ActionResult Import()
        {
            var vm = Wtm.CreateVM<CityImportVM>();
            return PartialView(vm);
        }

        [HttpPost]
        [ActionDescription("Sys.Import")]
        public ActionResult Import(CityImportVM vm, IFormCollection nouse)
        {
            if (vm.ErrorListVM.EntityList.Count > 0 || !vm.BatchSaveData())
            {
                return PartialView(vm);
            }
            else
            {
                return FFResult().CloseDialog().RefreshGrid().Alert(Localizer["Sys.ImportSuccess", vm.EntityList.Count.ToString()]);
            }
        }

        #endregion Import

        [ActionDescription("Sys.Export")]
        [HttpPost]
        public IActionResult ExportExcel(CityListVM vm)
        {
            return vm.GetExportData();
        }

        public IActionResult GetChildrenCity(Guid id)
        {
            var rv = DC.Set<City>().Where(x => x.ParentId == id).GetSelectListItems(Wtm, x => x.CityName);
            return JsonMore(rv);
        }
    }
}