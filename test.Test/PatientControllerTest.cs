using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WalkingTec.Mvvm.Core;
using test.Controllers;
using test.ViewModel.Patients.PatientVMs;
using test.Model;
using test.DataAccess;


namespace test.Test
{
    [TestClass]
    public class PatientControllerTest
    {
        private PatientController _controller;
        private string _seed;

        public PatientControllerTest()
        {
            _seed = Guid.NewGuid().ToString();
            _controller = MockController.CreateController<PatientController>(new DataContext(_seed, DBTypeEnum.Memory), "user");
        }

        [TestMethod]
        public void SearchTest()
        {
            PartialViewResult rv = (PartialViewResult)_controller.Index();
            Assert.IsInstanceOfType(rv.Model, typeof(IBasePagedListVM<TopBasePoco, BaseSearcher>));
            string rv2 = _controller.Search((rv.Model as PatientListVM).Searcher);
            Assert.IsTrue(rv2.Contains("\"Code\":200"));
        }

        [TestMethod]
        public void CreateTest()
        {
            PartialViewResult rv = (PartialViewResult)_controller.Create();
            Assert.IsInstanceOfType(rv.Model, typeof(PatientVM));

            PatientVM vm = rv.Model as PatientVM;
            Patient v = new Patient();
			
            v.ID = 7;
            v.PatientName = "lmwELbPm";
            v.IdNumber = "sjM5TzFDJM";
            v.Gender = test.Model.GenderEnum.Female;
            v.Status = test.Model.PatientStatusEnum.SiWang;
            v.Birthday = DateTime.Parse("2022-01-07 21:38:43");
            v.LocationId = AddCity();
            v.HospitalID = AddHospital();
            v.PhotoId = AddFileAttachment();
            vm.Entity = v;
            _controller.Create(vm);

            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                var data = context.Set<Patient>().Find(v.ID);
				
                Assert.AreEqual(data.ID, 7);
                Assert.AreEqual(data.PatientName, "lmwELbPm");
                Assert.AreEqual(data.IdNumber, "sjM5TzFDJM");
                Assert.AreEqual(data.Gender, test.Model.GenderEnum.Female);
                Assert.AreEqual(data.Status, test.Model.PatientStatusEnum.SiWang);
                Assert.AreEqual(data.Birthday, DateTime.Parse("2022-01-07 21:38:43"));
                Assert.AreEqual(data.CreateBy, "user");
                Assert.IsTrue(DateTime.Now.Subtract(data.CreateTime.Value).Seconds < 10);
            }

        }

        [TestMethod]
        public void EditTest()
        {
            Patient v = new Patient();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
       			
                v.ID = 7;
                v.PatientName = "lmwELbPm";
                v.IdNumber = "sjM5TzFDJM";
                v.Gender = test.Model.GenderEnum.Female;
                v.Status = test.Model.PatientStatusEnum.SiWang;
                v.Birthday = DateTime.Parse("2022-01-07 21:38:43");
                v.LocationId = AddCity();
                v.HospitalID = AddHospital();
                v.PhotoId = AddFileAttachment();
                context.Set<Patient>().Add(v);
                context.SaveChanges();
            }

            PartialViewResult rv = (PartialViewResult)_controller.Edit(v.ID.ToString());
            Assert.IsInstanceOfType(rv.Model, typeof(PatientVM));

            PatientVM vm = rv.Model as PatientVM;
            vm.Wtm.DC = new DataContext(_seed, DBTypeEnum.Memory);
            v = new Patient();
            v.ID = vm.Entity.ID;
       		
            v.PatientName = "XdZeXEXx50zlOCC";
            v.IdNumber = "JSRzoZONmAo2tP8Hzva";
            v.Gender = test.Model.GenderEnum.Female;
            v.Status = test.Model.PatientStatusEnum.WuZhengZhuang;
            v.Birthday = DateTime.Parse("2023-03-29 21:38:43");
            vm.Entity = v;
            vm.FC = new Dictionary<string, object>();
			
            vm.FC.Add("Entity.ID", "");
            vm.FC.Add("Entity.PatientName", "");
            vm.FC.Add("Entity.IdNumber", "");
            vm.FC.Add("Entity.Gender", "");
            vm.FC.Add("Entity.Status", "");
            vm.FC.Add("Entity.Birthday", "");
            vm.FC.Add("Entity.LocationId", "");
            vm.FC.Add("Entity.HospitalID", "");
            vm.FC.Add("Entity.PhotoId", "");
            _controller.Edit(vm);

            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                var data = context.Set<Patient>().Find(v.ID);
 				
                Assert.AreEqual(data.PatientName, "XdZeXEXx50zlOCC");
                Assert.AreEqual(data.IdNumber, "JSRzoZONmAo2tP8Hzva");
                Assert.AreEqual(data.Gender, test.Model.GenderEnum.Female);
                Assert.AreEqual(data.Status, test.Model.PatientStatusEnum.WuZhengZhuang);
                Assert.AreEqual(data.Birthday, DateTime.Parse("2023-03-29 21:38:43"));
                Assert.AreEqual(data.UpdateBy, "user");
                Assert.IsTrue(DateTime.Now.Subtract(data.UpdateTime.Value).Seconds < 10);
            }

        }


        [TestMethod]
        public void DeleteTest()
        {
            Patient v = new Patient();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
        		
                v.ID = 7;
                v.PatientName = "lmwELbPm";
                v.IdNumber = "sjM5TzFDJM";
                v.Gender = test.Model.GenderEnum.Female;
                v.Status = test.Model.PatientStatusEnum.SiWang;
                v.Birthday = DateTime.Parse("2022-01-07 21:38:43");
                v.LocationId = AddCity();
                v.HospitalID = AddHospital();
                v.PhotoId = AddFileAttachment();
                context.Set<Patient>().Add(v);
                context.SaveChanges();
            }

            PartialViewResult rv = (PartialViewResult)_controller.Delete(v.ID.ToString());
            Assert.IsInstanceOfType(rv.Model, typeof(PatientVM));

            PatientVM vm = rv.Model as PatientVM;
            v = new Patient();
            v.ID = vm.Entity.ID;
            vm.Entity = v;
            _controller.Delete(v.ID.ToString(),null);

            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                var data = context.Set<Patient>().Find(v.ID);
                Assert.AreEqual(data.IsValid, false);
          }

        }


        [TestMethod]
        public void DetailsTest()
        {
            Patient v = new Patient();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
				
                v.ID = 7;
                v.PatientName = "lmwELbPm";
                v.IdNumber = "sjM5TzFDJM";
                v.Gender = test.Model.GenderEnum.Female;
                v.Status = test.Model.PatientStatusEnum.SiWang;
                v.Birthday = DateTime.Parse("2022-01-07 21:38:43");
                v.LocationId = AddCity();
                v.HospitalID = AddHospital();
                v.PhotoId = AddFileAttachment();
                context.Set<Patient>().Add(v);
                context.SaveChanges();
            }
            PartialViewResult rv = (PartialViewResult)_controller.Details(v.ID.ToString());
            Assert.IsInstanceOfType(rv.Model, typeof(IBaseCRUDVM<TopBasePoco>));
            Assert.AreEqual(v.ID, (rv.Model as IBaseCRUDVM<TopBasePoco>).Entity.GetID());
        }

        [TestMethod]
        public void BatchEditTest()
        {
            Patient v1 = new Patient();
            Patient v2 = new Patient();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
				
                v1.ID = 7;
                v1.PatientName = "lmwELbPm";
                v1.IdNumber = "sjM5TzFDJM";
                v1.Gender = test.Model.GenderEnum.Female;
                v1.Status = test.Model.PatientStatusEnum.SiWang;
                v1.Birthday = DateTime.Parse("2022-01-07 21:38:43");
                v1.LocationId = AddCity();
                v1.HospitalID = AddHospital();
                v1.PhotoId = AddFileAttachment();
                v2.ID = 74;
                v2.PatientName = "XdZeXEXx50zlOCC";
                v2.IdNumber = "JSRzoZONmAo2tP8Hzva";
                v2.Gender = test.Model.GenderEnum.Female;
                v2.Status = test.Model.PatientStatusEnum.WuZhengZhuang;
                v2.Birthday = DateTime.Parse("2023-03-29 21:38:43");
                v2.LocationId = v1.LocationId; 
                v2.HospitalID = v1.HospitalID; 
                v2.PhotoId = v1.PhotoId; 
                context.Set<Patient>().Add(v1);
                context.Set<Patient>().Add(v2);
                context.SaveChanges();
            }

            PartialViewResult rv = (PartialViewResult)_controller.BatchDelete(new string[] { v1.ID.ToString(), v2.ID.ToString() });
            Assert.IsInstanceOfType(rv.Model, typeof(PatientBatchVM));

            PatientBatchVM vm = rv.Model as PatientBatchVM;
            vm.Ids = new string[] { v1.ID.ToString(), v2.ID.ToString() };
            
            vm.FC = new Dictionary<string, object>();
			
            _controller.DoBatchEdit(vm, null);

            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                var data1 = context.Set<Patient>().Find(v1.ID);
                var data2 = context.Set<Patient>().Find(v2.ID);
 				
                Assert.AreEqual(data1.UpdateBy, "user");
                Assert.IsTrue(DateTime.Now.Subtract(data1.UpdateTime.Value).Seconds < 10);
                Assert.AreEqual(data2.UpdateBy, "user");
                Assert.IsTrue(DateTime.Now.Subtract(data2.UpdateTime.Value).Seconds < 10);
            }
        }


        [TestMethod]
        public void BatchDeleteTest()
        {
            Patient v1 = new Patient();
            Patient v2 = new Patient();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
				
                v1.ID = 7;
                v1.PatientName = "lmwELbPm";
                v1.IdNumber = "sjM5TzFDJM";
                v1.Gender = test.Model.GenderEnum.Female;
                v1.Status = test.Model.PatientStatusEnum.SiWang;
                v1.Birthday = DateTime.Parse("2022-01-07 21:38:43");
                v1.LocationId = AddCity();
                v1.HospitalID = AddHospital();
                v1.PhotoId = AddFileAttachment();
                v2.ID = 74;
                v2.PatientName = "XdZeXEXx50zlOCC";
                v2.IdNumber = "JSRzoZONmAo2tP8Hzva";
                v2.Gender = test.Model.GenderEnum.Female;
                v2.Status = test.Model.PatientStatusEnum.WuZhengZhuang;
                v2.Birthday = DateTime.Parse("2023-03-29 21:38:43");
                v2.LocationId = v1.LocationId; 
                v2.HospitalID = v1.HospitalID; 
                v2.PhotoId = v1.PhotoId; 
                context.Set<Patient>().Add(v1);
                context.Set<Patient>().Add(v2);
                context.SaveChanges();
            }

            PartialViewResult rv = (PartialViewResult)_controller.BatchDelete(new string[] { v1.ID.ToString(), v2.ID.ToString() });
            Assert.IsInstanceOfType(rv.Model, typeof(PatientBatchVM));

            PatientBatchVM vm = rv.Model as PatientBatchVM;
            vm.Ids = new string[] { v1.ID.ToString(), v2.ID.ToString() };
            _controller.DoBatchDelete(vm, null);

            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                var data1 = context.Set<Patient>().Find(v1.ID);
                var data2 = context.Set<Patient>().Find(v2.ID);
                Assert.AreEqual(data1.IsValid, false);
            Assert.AreEqual(data2.IsValid, false);
            }
        }

        [TestMethod]
        public void ExportTest()
        {
            PartialViewResult rv = (PartialViewResult)_controller.Index();
            Assert.IsInstanceOfType(rv.Model, typeof(IBasePagedListVM<TopBasePoco, BaseSearcher>));
            IActionResult rv2 = _controller.ExportExcel(rv.Model as PatientListVM);
            Assert.IsTrue((rv2 as FileContentResult).FileContents.Length > 0);
        }

        private Guid AddCity()
        {
            City v = new City();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                try{

                v.CityName = "Xl2";
                context.Set<City>().Add(v);
                context.SaveChanges();
                }
                catch{}
            }
            return v.ID;
        }

        private Guid AddHospital()
        {
            Hospital v = new Hospital();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                try{

                v.Level = test.Model.HospitalLevel.Class2;
                v.LocationId = AddCity();
                v.Name = "nRPMXC";
                context.Set<Hospital>().Add(v);
                context.SaveChanges();
                }
                catch{}
            }
            return v.ID;
        }

        private Guid AddFileAttachment()
        {
            FileAttachment v = new FileAttachment();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                try{

                v.FileName = "nGAJoKlqY0cgn5";
                v.FileExt = "xZ0VrU7fn";
                v.Path = "e8f7ZqYa";
                v.Length = 16;
                v.UploadTime = DateTime.Parse("2022-05-20 21:38:43");
                v.SaveMode = "6Euz";
                v.ExtraInfo = "HdMaw9pzB7XOqiABj";
                v.HandlerInfo = "DddrWt15Ou";
                context.Set<FileAttachment>().Add(v);
                context.SaveChanges();
                }
                catch{}
            }
            return v.ID;
        }


    }
}
