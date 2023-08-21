using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using test.ViewModel.Patients.PatientVMs;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using WalkingTec.Mvvm.Mvc;

namespace test.Controllers
{
    [Area("Patients")]
    [ActionDescription("病例管理2")]
    [AuthorizeJwtWithCookie]
    public partial class Patient2Controller : BaseController
    {
        #region 搜索

        [ActionDescription("搜索")]
        public ActionResult Index()
        {
            var vm = Wtm.CreateVM<PatientListVM21>();
            return PartialView(vm);
        }

        [ActionDescription("搜索")]
        [HttpPost]
        public string Search(PatientListVM21 vm)
        {
            return vm.GetJson(false);
        }

        #endregion 搜索

        #region 新建

        [ActionDescription("新建")]
        public ActionResult Create()
        {
            var vm = Wtm.CreateVM<PatientVM>();
            return PartialView(vm);
        }

        [HttpPost]
        [ActionDescription("新建")]
        public ActionResult Create(PatientVM vm)
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

        #endregion 新建

        #region 修改

        [ActionDescription("修改")]
        public ActionResult Edit(int id)
        {
            var vm = Wtm.CreateVM<PatientVM>(id);
            vm.Reports.Searcher.PatientId = id;
            return PartialView(vm);
        }

        [ActionDescription("修改")]
        [HttpPost]
        [ValidateFormItemOnly]
        public ActionResult Edit(PatientVM vm)
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

        #endregion 修改

        #region 删除

        [ActionDescription("删除")]
        public ActionResult Delete(string id)
        {
            var vm = Wtm.CreateVM<PatientVM>(id);
            vm.DoDelete();
            if (!ModelState.IsValid)
            {
                return FFResult().Alert("删除失败");
            }
            else
            {
                return FFResult().RefreshGrid().Alert("删除成功");
            }
        }

        [ActionDescription("删除")]
        [HttpPost]
        public ActionResult Delete(string id, IFormCollection nouse)
        {
            var vm = Wtm.CreateVM<PatientVM>(id);
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

        #endregion 删除

        #region 详细

        [ActionDescription("详细")]
        public ActionResult Details(string id)
        {
            var vm = Wtm.CreateVM<PatientVM>(id);
            vm.Reports.SearcherMode = ListVMSearchModeEnum.MasterDetail;
            return PartialView(vm);
        }

        #endregion 详细

        #region 批量修改

        [HttpPost]
        [ActionDescription("批量修改")]
        public ActionResult BatchEdit(string[] IDs)
        {
            var vm = Wtm.CreateVM<PatientBatchVM>(Ids: IDs);
            return PartialView(vm);
        }

        [HttpPost]
        [ActionDescription("批量修改")]
        public ActionResult DoBatchEdit(PatientBatchVM vm, IFormCollection nouse)
        {
            if (!ModelState.IsValid || !vm.DoBatchEdit())
            {
                return PartialView("BatchEdit", vm);
            }
            else
            {
                return FFResult().CloseDialog().RefreshGrid().Alert("操作成功，共有" + vm.Ids.Length + "条数据被修改");
            }
        }

        #endregion 批量修改

        #region 批量删除

        [HttpPost]
        [ActionDescription("批量删除")]
        public ActionResult BatchDelete(string[] IDs)
        {
            var vm = Wtm.CreateVM<PatientBatchVM>(Ids: IDs);
            return PartialView(vm);
        }

        [HttpPost]
        [ActionDescription("批量删除")]
        public ActionResult DoBatchDelete(PatientBatchVM vm, IFormCollection nouse)
        {
            if (!ModelState.IsValid || !vm.DoBatchDelete())
            {
                return PartialView("BatchDelete", vm);
            }
            else
            {
                return FFResult().CloseDialog().RefreshGrid().Alert("操作成功，共有" + vm.Ids.Length + "条数据被删除");
            }
        }

        #endregion 批量删除

        #region 导入

        [ActionDescription("导入")]
        public ActionResult Import()
        {
            var vm = Wtm.CreateVM<PatientImportVM>();
            return PartialView(vm);
        }

        [HttpPost]
        [ActionDescription("导入")]
        public ActionResult Import(PatientImportVM vm, IFormCollection nouse)
        {
            if (vm.ErrorListVM.EntityList.Count > 0 || !vm.BatchSaveData())
            {
                return PartialView(vm);
            }
            else
            {
                return FFResult().CloseDialog().RefreshGrid().Alert("成功导入 " + vm.EntityList.Count.ToString() + " 行数据");
            }
        }

        #endregion 导入

        [ActionDescription("导出")]
        [HttpPost]
        public IActionResult ExportExcel(PatientListVM vm)
        {
            vm.SearcherMode = vm.Ids != null && vm.Ids.Count > 0 ? ListVMSearchModeEnum.CheckExport : ListVMSearchModeEnum.Export;
            var data = vm.GenerateExcel();
            return File(data, "application/vnd.ms-excel", $"Export_Patient_{DateTime.Now.ToString("yyyy-MM-dd")}.xls");
        }
    }
}