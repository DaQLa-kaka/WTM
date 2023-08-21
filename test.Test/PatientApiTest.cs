using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WalkingTec.Mvvm.Core;
using test.Controllers;
using test.ViewModel.Api.PatientVMs;
using test.Model;
using test.DataAccess;


namespace test.Test
{
    [TestClass]
    public class PatientApiTest
    {
        private PatientApiController _controller;
        private string _seed;

        public PatientApiTest()
        {
            _seed = Guid.NewGuid().ToString();
            _controller = MockController.CreateApi<PatientApiController>(new DataContext(_seed, DBTypeEnum.Memory), "user");
        }

        [TestMethod]
        public void SearchTest()
        {
            ContentResult rv = _controller.Search(new PatientApiSearcher()) as ContentResult;
            Assert.IsTrue(string.IsNullOrEmpty(rv.Content)==false);
        }

        [TestMethod]
        public void CreateTest()
        {
            PatientApiVM vm = _controller.Wtm.CreateVM<PatientApiVM>();
            Patient v = new Patient();
            
            v.ID = 51;
            v.PatientName = "2s2";
            v.IdNumber = "tfNM2rAM7yxPUy69REG";
            v.Gender = test.Model.GenderEnum.Female;
            v.Status = test.Model.PatientStatusEnum.SiWang;
            v.Birthday = DateTime.Parse("2021-10-30 16:45:50");
            v.LocationId = AddCity();
            v.HospitalID = AddHospital();
            v.PhotoId = AddFileAttachment();
            vm.Entity = v;
            var rv = _controller.Add(vm);
            Assert.IsInstanceOfType(rv, typeof(OkObjectResult));

            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                var data = context.Set<Patient>().Find(v.ID);
                
                Assert.AreEqual(data.ID, 51);
                Assert.AreEqual(data.PatientName, "2s2");
                Assert.AreEqual(data.IdNumber, "tfNM2rAM7yxPUy69REG");
                Assert.AreEqual(data.Gender, test.Model.GenderEnum.Female);
                Assert.AreEqual(data.Status, test.Model.PatientStatusEnum.SiWang);
                Assert.AreEqual(data.Birthday, DateTime.Parse("2021-10-30 16:45:50"));
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
       			
                v.ID = 51;
                v.PatientName = "2s2";
                v.IdNumber = "tfNM2rAM7yxPUy69REG";
                v.Gender = test.Model.GenderEnum.Female;
                v.Status = test.Model.PatientStatusEnum.SiWang;
                v.Birthday = DateTime.Parse("2021-10-30 16:45:50");
                v.LocationId = AddCity();
                v.HospitalID = AddHospital();
                v.PhotoId = AddFileAttachment();
                context.Set<Patient>().Add(v);
                context.SaveChanges();
            }

            PatientApiVM vm = _controller.Wtm.CreateVM<PatientApiVM>();
            var oldID = v.ID;
            v = new Patient();
            v.ID = oldID;
       		
            v.PatientName = "3Yk2";
            v.IdNumber = "mXs0bUiKjt99D9ldr";
            v.Gender = test.Model.GenderEnum.Male;
            v.Status = test.Model.PatientStatusEnum.SiWang;
            v.Birthday = DateTime.Parse("2023-04-22 16:45:50");
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
            var rv = _controller.Edit(vm);
            Assert.IsInstanceOfType(rv, typeof(OkObjectResult));

            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                var data = context.Set<Patient>().Find(v.ID);
 				
                Assert.AreEqual(data.PatientName, "3Yk2");
                Assert.AreEqual(data.IdNumber, "mXs0bUiKjt99D9ldr");
                Assert.AreEqual(data.Gender, test.Model.GenderEnum.Male);
                Assert.AreEqual(data.Status, test.Model.PatientStatusEnum.SiWang);
                Assert.AreEqual(data.Birthday, DateTime.Parse("2023-04-22 16:45:50"));
                Assert.AreEqual(data.UpdateBy, "user");
                Assert.IsTrue(DateTime.Now.Subtract(data.UpdateTime.Value).Seconds < 10);
            }

        }

		[TestMethod]
        public void GetTest()
        {
            Patient v = new Patient();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
        		
                v.ID = 51;
                v.PatientName = "2s2";
                v.IdNumber = "tfNM2rAM7yxPUy69REG";
                v.Gender = test.Model.GenderEnum.Female;
                v.Status = test.Model.PatientStatusEnum.SiWang;
                v.Birthday = DateTime.Parse("2021-10-30 16:45:50");
                v.LocationId = AddCity();
                v.HospitalID = AddHospital();
                v.PhotoId = AddFileAttachment();
                context.Set<Patient>().Add(v);
                context.SaveChanges();
            }
            var rv = _controller.Get(v.ID.ToString());
            Assert.IsNotNull(rv);
        }

        [TestMethod]
        public void BatchDeleteTest()
        {
            Patient v1 = new Patient();
            Patient v2 = new Patient();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
				
                v1.ID = 51;
                v1.PatientName = "2s2";
                v1.IdNumber = "tfNM2rAM7yxPUy69REG";
                v1.Gender = test.Model.GenderEnum.Female;
                v1.Status = test.Model.PatientStatusEnum.SiWang;
                v1.Birthday = DateTime.Parse("2021-10-30 16:45:50");
                v1.LocationId = AddCity();
                v1.HospitalID = AddHospital();
                v1.PhotoId = AddFileAttachment();
                v2.ID = 85;
                v2.PatientName = "3Yk2";
                v2.IdNumber = "mXs0bUiKjt99D9ldr";
                v2.Gender = test.Model.GenderEnum.Male;
                v2.Status = test.Model.PatientStatusEnum.SiWang;
                v2.Birthday = DateTime.Parse("2023-04-22 16:45:50");
                v2.LocationId = v1.LocationId; 
                v2.HospitalID = v1.HospitalID; 
                v2.PhotoId = v1.PhotoId; 
                context.Set<Patient>().Add(v1);
                context.Set<Patient>().Add(v2);
                context.SaveChanges();
            }

            var rv = _controller.BatchDelete(new string[] { v1.ID.ToString(), v2.ID.ToString() });
            Assert.IsInstanceOfType(rv, typeof(OkObjectResult));

            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                var data1 = context.Set<Patient>().Find(v1.ID);
                var data2 = context.Set<Patient>().Find(v2.ID);
                Assert.AreEqual(data1, null);
            Assert.AreEqual(data2, null);
            }

            rv = _controller.BatchDelete(new string[] {});
            Assert.IsInstanceOfType(rv, typeof(OkResult));

        }

        private Guid AddCity()
        {
            City v = new City();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                try{

                v.CityName = "PhJfYwe6W";
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
                v.Name = "ncTllS";
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

                v.FileName = "TOOis3SRkraz00U";
                v.FileExt = "jsDfHK";
                v.Path = "eOyvkAjHAyYg";
                v.Length = 52;
                v.UploadTime = DateTime.Parse("2023-11-25 16:45:50");
                v.SaveMode = "KZgyw";
                v.ExtraInfo = "mSc5lXYXYlek";
                v.HandlerInfo = "Dw3fm08y1MK4nxI5";
                context.Set<FileAttachment>().Add(v);
                context.SaveChanges();
                }
                catch{}
            }
            return v.ID;
        }


    }
}
