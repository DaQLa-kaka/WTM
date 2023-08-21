using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WalkingTec.Mvvm.Core;
using test.Controllers;
using test.ViewModel.Patients.ReportVMs;
using test.Model;
using test.DataAccess;

namespace test.Test
{
    [TestClass]
    public class ReportControllerTest
    {
        private ReportController _controller;
        private string _seed;

        public ReportControllerTest()
        {
            _seed = Guid.NewGuid().ToString();
            _controller = MockController.CreateController<ReportController>(new DataContext(_seed, DBTypeEnum.Memory), "user");
        }

        [TestMethod]
        public void SearchTest()
        {
            PartialViewResult rv = (PartialViewResult)_controller.Index();
            Assert.IsInstanceOfType(rv.Model, typeof(IBasePagedListVM<TopBasePoco, BaseSearcher>));
            string rv2 = _controller.Search((rv.Model as ReportListVM).Searcher);
            Assert.IsTrue(rv2.Contains("\"Code\":200"));
        }

        [TestMethod]
        public void CreateTest()
        {
            PartialViewResult rv = (PartialViewResult)_controller.Create();
            Assert.IsInstanceOfType(rv.Model, typeof(ReportVM));

            ReportVM vm = rv.Model as ReportVM;
            Report v = new Report();

            v.Temprature = 37;
            v.Remark = "Mw214ez";
            v.PatientId = AddPatient();
            vm.Entity = v;
            _controller.Create(vm);

            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                var data = context.Set<Report>().Find(v.ID);

                Assert.AreEqual(data.Temprature, 37);
                Assert.AreEqual(data.Remark, "Mw214ez");
                Assert.AreEqual(data.CreateBy, "user");
                Assert.IsTrue(DateTime.Now.Subtract(data.CreateTime.Value).Seconds < 10);
            }
        }

        [TestMethod]
        public void EditTest()
        {
            Report v = new Report();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                v.Temprature = 37;
                v.Remark = "Mw214ez";
                v.PatientId = AddPatient();
                context.Set<Report>().Add(v);
                context.SaveChanges();
            }

            PartialViewResult rv = (PartialViewResult)_controller.Edit(v.ID.ToString());
            Assert.IsInstanceOfType(rv.Model, typeof(ReportVM));

            ReportVM vm = rv.Model as ReportVM;
            vm.Wtm.DC = new DataContext(_seed, DBTypeEnum.Memory);
            v = new Report();
            v.ID = vm.Entity.ID;

            v.Temprature = 39;
            v.Remark = "B7m0apzFMn5Nl5T";
            vm.Entity = v;
            vm.FC = new Dictionary<string, object>();

            vm.FC.Add("Entity.Temprature", "");
            vm.FC.Add("Entity.Remark", "");
            vm.FC.Add("Entity.PatientId", "");
            _controller.Edit(vm);

            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                var data = context.Set<Report>().Find(v.ID);

                Assert.AreEqual(data.Temprature, 39);
                Assert.AreEqual(data.Remark, "B7m0apzFMn5Nl5T");
                Assert.AreEqual(data.UpdateBy, "user");
                Assert.IsTrue(DateTime.Now.Subtract(data.UpdateTime.Value).Seconds < 10);
            }
        }

        [TestMethod]
        public void DeleteTest()
        {
            Report v = new Report();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                v.Temprature = 37;
                v.Remark = "Mw214ez";
                v.PatientId = AddPatient();
                context.Set<Report>().Add(v);
                context.SaveChanges();
            }

            PartialViewResult rv = (PartialViewResult)_controller.Delete(v.ID.ToString());
            Assert.IsInstanceOfType(rv.Model, typeof(ReportVM));

            ReportVM vm = rv.Model as ReportVM;
            v = new Report();
            v.ID = vm.Entity.ID;
            vm.Entity = v;
            _controller.Delete(v.ID.ToString(), null);

            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                var data = context.Set<Report>().Find(v.ID);
                Assert.AreEqual(data, null);
            }
        }

        [TestMethod]
        public void DetailsTest()
        {
            Report v = new Report();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                v.Temprature = 37;
                v.Remark = "Mw214ez";
                v.PatientId = AddPatient();
                context.Set<Report>().Add(v);
                context.SaveChanges();
            }
            PartialViewResult rv = (PartialViewResult)_controller.Details(v.ID.ToString());
            Assert.IsInstanceOfType(rv.Model, typeof(IBaseCRUDVM<TopBasePoco>));
            Assert.AreEqual(v.ID, (rv.Model as IBaseCRUDVM<TopBasePoco>).Entity.GetID());
        }

        [TestMethod]
        public void BatchEditTest()
        {
            Report v1 = new Report();
            Report v2 = new Report();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                v1.Temprature = 37;
                v1.Remark = "Mw214ez";
                v1.PatientId = AddPatient();
                v2.Temprature = 39;
                v2.Remark = "B7m0apzFMn5Nl5T";
                v2.PatientId = v1.PatientId;
                context.Set<Report>().Add(v1);
                context.Set<Report>().Add(v2);
                context.SaveChanges();
            }

            PartialViewResult rv = (PartialViewResult)_controller.BatchDelete(new string[] { v1.ID.ToString(), v2.ID.ToString() });
            Assert.IsInstanceOfType(rv.Model, typeof(ReportBatchVM));

            ReportBatchVM vm = rv.Model as ReportBatchVM;
            vm.Ids = new string[] { v1.ID.ToString(), v2.ID.ToString() };

            vm.FC = new Dictionary<string, object>();

            _controller.DoBatchEdit(vm, null);

            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                var data1 = context.Set<Report>().Find(v1.ID);
                var data2 = context.Set<Report>().Find(v2.ID);

                Assert.AreEqual(data1.UpdateBy, "user");
                Assert.IsTrue(DateTime.Now.Subtract(data1.UpdateTime.Value).Seconds < 10);
                Assert.AreEqual(data2.UpdateBy, "user");
                Assert.IsTrue(DateTime.Now.Subtract(data2.UpdateTime.Value).Seconds < 10);
            }
        }

        [TestMethod]
        public void BatchDeleteTest()
        {
            Report v1 = new Report();
            Report v2 = new Report();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                v1.Temprature = 37;
                v1.Remark = "Mw214ez";
                v1.PatientId = AddPatient();
                v2.Temprature = 39;
                v2.Remark = "B7m0apzFMn5Nl5T";
                v2.PatientId = v1.PatientId;
                context.Set<Report>().Add(v1);
                context.Set<Report>().Add(v2);
                context.SaveChanges();
            }

            PartialViewResult rv = (PartialViewResult)_controller.BatchDelete(new string[] { v1.ID.ToString(), v2.ID.ToString() });
            Assert.IsInstanceOfType(rv.Model, typeof(ReportBatchVM));

            ReportBatchVM vm = rv.Model as ReportBatchVM;
            vm.Ids = new string[] { v1.ID.ToString(), v2.ID.ToString() };
            _controller.DoBatchDelete(vm, null);

            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                var data1 = context.Set<Report>().Find(v1.ID);
                var data2 = context.Set<Report>().Find(v2.ID);
                Assert.AreEqual(data1, null);
                Assert.AreEqual(data2, null);
            }
        }

        [TestMethod]
        public void ExportTest()
        {
            PartialViewResult rv = (PartialViewResult)_controller.Index();
            Assert.IsInstanceOfType(rv.Model, typeof(IBasePagedListVM<TopBasePoco, BaseSearcher>));
            IActionResult rv2 = _controller.ExportExcel(rv.Model as ReportListVM);
            Assert.IsTrue((rv2 as FileContentResult).FileContents.Length > 0);
        }

        private Guid AddCity()
        {
            City v = new City();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                try
                {
                    v.CityName = "EO0B8oUL";
                    context.Set<City>().Add(v);
                    context.SaveChanges();
                }
                catch { }
            }
            return v.ID;
        }

        private Guid AddHospital()
        {
            Hospital v = new Hospital();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                try
                {
                    v.Level = test.Model.HospitalLevel.Class3;
                    v.LocationId = AddCity();
                    v.Name = "cumd5MC848BSrau";
                    context.Set<Hospital>().Add(v);
                    context.SaveChanges();
                }
                catch { }
            }
            return v.ID;
        }

        private Guid AddFileAttachment()
        {
            FileAttachment v = new FileAttachment();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                try
                {
                    v.FileName = "qzv";
                    v.FileExt = "3FokWp";
                    v.Path = "oAe14j";
                    v.Length = 36;
                    v.UploadTime = DateTime.Parse("2023-12-05 21:09:12");
                    v.SaveMode = "hVYNlnx0eh";
                    v.ExtraInfo = "4BIaXi5YUv";
                    v.HandlerInfo = "6e";
                    context.Set<FileAttachment>().Add(v);
                    context.SaveChanges();
                }
                catch { }
            }
            return v.ID;
        }

        private Int32 AddPatient()
        {
            Patient v = new Patient();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                try
                {
                    v.ID = 15;
                    v.PatientName = "0PGaCJq24XeZyr0";
                    v.IdNumber = "60";
                    v.Gender = test.Model.GenderEnum.Female;
                    v.Status = test.Model.PatientStatusEnum.YiSi;
                    v.Birthday = DateTime.Parse("2021-10-29 21:09:12");
                    v.LocationId = AddCity();
                    v.HospitalID = AddHospital();
                    v.PhotoId = AddFileAttachment();
                    context.Set<Patient>().Add(v);
                    context.SaveChanges();
                }
                catch { }
            }
            return v.ID;
        }
    }
}