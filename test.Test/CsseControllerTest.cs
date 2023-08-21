using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WalkingTec.Mvvm.Core;
using test.Controllers;
using test.ViewModel.Reports.CsseVMs;
using test.Model;
using test.DataAccess;


namespace test.Test
{
    [TestClass]
    public class CsseControllerTest
    {
        private CsseController _controller;
        private string _seed;

        public CsseControllerTest()
        {
            _seed = Guid.NewGuid().ToString();
            _controller = MockController.CreateController<CsseController>(new DataContext(_seed, DBTypeEnum.Memory), "user");
        }

        [TestMethod]
        public void SearchTest()
        {
            PartialViewResult rv = (PartialViewResult)_controller.Index();
            Assert.IsInstanceOfType(rv.Model, typeof(IBasePagedListVM<TopBasePoco, BaseSearcher>));
            string rv2 = _controller.Search((rv.Model as CsseListVM).Searcher);
            Assert.IsTrue(rv2.Contains("\"Code\":200"));
        }

        [TestMethod]
        public void CreateTest()
        {
            PartialViewResult rv = (PartialViewResult)_controller.Create();
            Assert.IsInstanceOfType(rv.Model, typeof(CsseVM));

            CsseVM vm = rv.Model as CsseVM;
            Csse v = new Csse();
			
            v.Country = "6NNxV6";
            v.Province = "bkXBlk9VN9";
            v.Lat = 95;
            v.Long = 19;
            v.Date = DateTime.Parse("2023-03-19 20:06:28");
            v.ConfirmedCase = 26;
            vm.Entity = v;
            _controller.Create(vm);

            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                var data = context.Set<Csse>().Find(v.ID);
				
                Assert.AreEqual(data.Country, "6NNxV6");
                Assert.AreEqual(data.Province, "bkXBlk9VN9");
                Assert.AreEqual(data.Lat, 95);
                Assert.AreEqual(data.Long, 19);
                Assert.AreEqual(data.Date, DateTime.Parse("2023-03-19 20:06:28"));
                Assert.AreEqual(data.ConfirmedCase, 26);
            }

        }

        [TestMethod]
        public void EditTest()
        {
            Csse v = new Csse();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
       			
                v.Country = "6NNxV6";
                v.Province = "bkXBlk9VN9";
                v.Lat = 95;
                v.Long = 19;
                v.Date = DateTime.Parse("2023-03-19 20:06:28");
                v.ConfirmedCase = 26;
                context.Set<Csse>().Add(v);
                context.SaveChanges();
            }

            PartialViewResult rv = (PartialViewResult)_controller.Edit(v.ID.ToString());
            Assert.IsInstanceOfType(rv.Model, typeof(CsseVM));

            CsseVM vm = rv.Model as CsseVM;
            vm.Wtm.DC = new DataContext(_seed, DBTypeEnum.Memory);
            v = new Csse();
            v.ID = vm.Entity.ID;
       		
            v.Country = "hhWJeIcBFxvmcph24SZ";
            v.Province = "GVrHjKjU7KT67Xqku";
            v.Lat = 53;
            v.Long = 29;
            v.Date = DateTime.Parse("2023-07-25 20:06:28");
            v.ConfirmedCase = 40;
            vm.Entity = v;
            vm.FC = new Dictionary<string, object>();
			
            vm.FC.Add("Entity.Country", "");
            vm.FC.Add("Entity.Province", "");
            vm.FC.Add("Entity.Lat", "");
            vm.FC.Add("Entity.Long", "");
            vm.FC.Add("Entity.Date", "");
            vm.FC.Add("Entity.ConfirmedCase", "");
            _controller.Edit(vm);

            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                var data = context.Set<Csse>().Find(v.ID);
 				
                Assert.AreEqual(data.Country, "hhWJeIcBFxvmcph24SZ");
                Assert.AreEqual(data.Province, "GVrHjKjU7KT67Xqku");
                Assert.AreEqual(data.Lat, 53);
                Assert.AreEqual(data.Long, 29);
                Assert.AreEqual(data.Date, DateTime.Parse("2023-07-25 20:06:28"));
                Assert.AreEqual(data.ConfirmedCase, 40);
            }

        }


        [TestMethod]
        public void DeleteTest()
        {
            Csse v = new Csse();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
        		
                v.Country = "6NNxV6";
                v.Province = "bkXBlk9VN9";
                v.Lat = 95;
                v.Long = 19;
                v.Date = DateTime.Parse("2023-03-19 20:06:28");
                v.ConfirmedCase = 26;
                context.Set<Csse>().Add(v);
                context.SaveChanges();
            }

            PartialViewResult rv = (PartialViewResult)_controller.Delete(v.ID.ToString());
            Assert.IsInstanceOfType(rv.Model, typeof(CsseVM));

            CsseVM vm = rv.Model as CsseVM;
            v = new Csse();
            v.ID = vm.Entity.ID;
            vm.Entity = v;
            _controller.Delete(v.ID.ToString(),null);

            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                var data = context.Set<Csse>().Find(v.ID);
                Assert.AreEqual(data, null);
            }

        }


        [TestMethod]
        public void DetailsTest()
        {
            Csse v = new Csse();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
				
                v.Country = "6NNxV6";
                v.Province = "bkXBlk9VN9";
                v.Lat = 95;
                v.Long = 19;
                v.Date = DateTime.Parse("2023-03-19 20:06:28");
                v.ConfirmedCase = 26;
                context.Set<Csse>().Add(v);
                context.SaveChanges();
            }
            PartialViewResult rv = (PartialViewResult)_controller.Details(v.ID.ToString());
            Assert.IsInstanceOfType(rv.Model, typeof(IBaseCRUDVM<TopBasePoco>));
            Assert.AreEqual(v.ID, (rv.Model as IBaseCRUDVM<TopBasePoco>).Entity.GetID());
        }

        [TestMethod]
        public void BatchEditTest()
        {
            Csse v1 = new Csse();
            Csse v2 = new Csse();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
				
                v1.Country = "6NNxV6";
                v1.Province = "bkXBlk9VN9";
                v1.Lat = 95;
                v1.Long = 19;
                v1.Date = DateTime.Parse("2023-03-19 20:06:28");
                v1.ConfirmedCase = 26;
                v2.Country = "hhWJeIcBFxvmcph24SZ";
                v2.Province = "GVrHjKjU7KT67Xqku";
                v2.Lat = 53;
                v2.Long = 29;
                v2.Date = DateTime.Parse("2023-07-25 20:06:28");
                v2.ConfirmedCase = 40;
                context.Set<Csse>().Add(v1);
                context.Set<Csse>().Add(v2);
                context.SaveChanges();
            }

            PartialViewResult rv = (PartialViewResult)_controller.BatchDelete(new string[] { v1.ID.ToString(), v2.ID.ToString() });
            Assert.IsInstanceOfType(rv.Model, typeof(CsseBatchVM));

            CsseBatchVM vm = rv.Model as CsseBatchVM;
            vm.Ids = new string[] { v1.ID.ToString(), v2.ID.ToString() };
            
            vm.LinkedVM.ConfirmedCase = 49;
            vm.FC = new Dictionary<string, object>();
			
            vm.FC.Add("LinkedVM.ConfirmedCase", "");
            _controller.DoBatchEdit(vm, null);

            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                var data1 = context.Set<Csse>().Find(v1.ID);
                var data2 = context.Set<Csse>().Find(v2.ID);
 				
                Assert.AreEqual(data1.ConfirmedCase, 49);
                Assert.AreEqual(data2.ConfirmedCase, 49);
            }
        }


        [TestMethod]
        public void BatchDeleteTest()
        {
            Csse v1 = new Csse();
            Csse v2 = new Csse();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
				
                v1.Country = "6NNxV6";
                v1.Province = "bkXBlk9VN9";
                v1.Lat = 95;
                v1.Long = 19;
                v1.Date = DateTime.Parse("2023-03-19 20:06:28");
                v1.ConfirmedCase = 26;
                v2.Country = "hhWJeIcBFxvmcph24SZ";
                v2.Province = "GVrHjKjU7KT67Xqku";
                v2.Lat = 53;
                v2.Long = 29;
                v2.Date = DateTime.Parse("2023-07-25 20:06:28");
                v2.ConfirmedCase = 40;
                context.Set<Csse>().Add(v1);
                context.Set<Csse>().Add(v2);
                context.SaveChanges();
            }

            PartialViewResult rv = (PartialViewResult)_controller.BatchDelete(new string[] { v1.ID.ToString(), v2.ID.ToString() });
            Assert.IsInstanceOfType(rv.Model, typeof(CsseBatchVM));

            CsseBatchVM vm = rv.Model as CsseBatchVM;
            vm.Ids = new string[] { v1.ID.ToString(), v2.ID.ToString() };
            _controller.DoBatchDelete(vm, null);

            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                var data1 = context.Set<Csse>().Find(v1.ID);
                var data2 = context.Set<Csse>().Find(v2.ID);
                Assert.AreEqual(data1, null);
            Assert.AreEqual(data2, null);
            }
        }


    }
}
