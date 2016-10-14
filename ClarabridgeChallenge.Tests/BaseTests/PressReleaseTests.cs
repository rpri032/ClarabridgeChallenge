using ClarabridgeChallenge.Business.Responses;
using ClarabridgeChallenge.Resources;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace ClarabridgeChallenge.Tests.BaseTests
{
    [TestClass]
    public class PressReleaseTests
    {
        [TestInitialize]
        public void TestInitialize()
        {
            //Start with a clean empty list of press releases
            var busPressRelease = new Business.PressRelease();
            busPressRelease.DeleteAll(); 
            var pressReleaseResponse = busPressRelease.GetAll();
            Assert.AreEqual(pressReleaseResponse.Status, ResponseStatus.Success);
            Assert.IsNull(pressReleaseResponse.Errors);
            Assert.AreEqual(pressReleaseResponse.Items.Count, 0);
        }

        [TestMethod]
        public void TestAdd()
        {
            const int numberOfPressReleasesToAdd = 5;
            for (int i = 0; i < numberOfPressReleasesToAdd; i++)
            {
                string title = string.Format("Press Release #{0}", i);
                string descriptionHtml = string.Format("<p>Press Release Description #{0}</p>", i);
                DateTime datePublished = DateTime.Now.AddDays(i);

                CreateIndividualPressRelease(title, descriptionHtml, datePublished);
            }
        }

        [TestMethod]
        public void TestAddValidation()
        {
            const string titleOverLimit = "Press Release 01234567890123456789012345678901234567890123456789abcd";
            const string descriptionHtml = "<p>Press Release Description</p>";
            DateTime datePublished = DateTime.Now;

            var busPressRelease = new Business.PressRelease();
            
            ResponseInfoDetail pressReleaseResponse = busPressRelease.Add(new Models.PressRelease() { Title = string.Empty, DescriptionHtml = descriptionHtml, DatePublished = datePublished });
            Assert.AreEqual(pressReleaseResponse.Status, ResponseStatus.Failed);
            Assert.AreEqual(pressReleaseResponse.Errors.Count, 1);
            Assert.AreEqual(pressReleaseResponse.Errors[0].Message, String.Format(Errors.CanNotBeEmpty, "title"));
            Assert.IsNull(pressReleaseResponse.Item, null);

            
            pressReleaseResponse = busPressRelease.Add(new Models.PressRelease() { Title = titleOverLimit, DescriptionHtml = descriptionHtml, DatePublished = datePublished });
            Assert.AreEqual(pressReleaseResponse.Status, ResponseStatus.Failed);
            Assert.AreEqual(pressReleaseResponse.Errors.Count, 1);
            Assert.AreEqual(pressReleaseResponse.Errors[0].Message, String.Format(Errors.CanNotBeGreaterThan, "title", 50));
            Assert.IsNull(pressReleaseResponse.Item, null);
            
            pressReleaseResponse = busPressRelease.Add(new Models.PressRelease() { Title = titleOverLimit, DescriptionHtml = descriptionHtml, DatePublished = datePublished.AddDays(-1) });
            Assert.AreEqual(pressReleaseResponse.Status, ResponseStatus.Failed);
            Assert.AreEqual(pressReleaseResponse.Errors.Count, 2);
            Assert.AreEqual(pressReleaseResponse.Errors[0].Message, String.Format(Errors.CanNotBeGreaterThan, "title", 50));
            Assert.AreEqual(pressReleaseResponse.Errors[1].Message, String.Format(Errors.DateMustBeInTheFuture, "publication date"));
            Assert.IsNull(pressReleaseResponse.Item, null);
        }

        [TestMethod]
        public void TestUpdate()
        {
            const int numberOfPressReleasesToAdd = 5;
            const int pressReleaseIndexToUpdate = 2;
            const string appendedUpdateText = "updated!";

            for (int i = 0; i < numberOfPressReleasesToAdd; i++)
            {
                string title = string.Format("Press Release #{0}", i);
                string descriptionHtml = string.Format("<p>Press Release Description #{0}</p>", i);
                DateTime datePublished = DateTime.Now.AddDays(i);

                CreateIndividualPressRelease(title, descriptionHtml, datePublished);
            }

            var busPressRelease = new Business.PressRelease();
            var pressReleaseListReponse = busPressRelease.GetAll();

            var pressReleaseToUpdate =
                (Models.PressRelease)pressReleaseListReponse.Items[pressReleaseIndexToUpdate];
            Assert.IsNotNull(pressReleaseToUpdate);
            
            var pressReleaseToUpdateResponse = busPressRelease.Update(
                new Models.PressRelease() {
                    Id = pressReleaseToUpdate.Id,
                    Title = string.Format("{0}:{1}", pressReleaseToUpdate.Title, appendedUpdateText),
                    DescriptionHtml = string.Format("{0}:{1}", pressReleaseToUpdate.DescriptionHtml, appendedUpdateText),
                    DatePublished = pressReleaseToUpdate.DatePublished.AddDays(numberOfPressReleasesToAdd) });

            Assert.AreEqual(pressReleaseToUpdateResponse.Status, ResponseStatus.Success);
            Assert.IsNull(pressReleaseToUpdateResponse.Errors);
            Assert.IsNotNull(pressReleaseToUpdateResponse.Item);

            pressReleaseListReponse = busPressRelease.GetAll();

            //Ensure no records we're updated except the updated press release
            int countOfRecordsNotUpdated = 0;
            int countOfRecordsUpdated = 0;
            for(int i = 0; i < pressReleaseListReponse.Items.Count; i++)
            {
                var pressReleaseItemFromGet = (Models.PressRelease)pressReleaseListReponse.Items[i];

                if (pressReleaseItemFromGet.Id != pressReleaseToUpdate.Id)
                {
                    Assert.AreNotEqual(pressReleaseItemFromGet.Title, ((Models.PressRelease)pressReleaseToUpdateResponse.Item).Title);
                    Assert.AreNotEqual(pressReleaseItemFromGet.DescriptionHtml, ((Models.PressRelease)pressReleaseToUpdateResponse.Item).DescriptionHtml);
                    Assert.AreNotEqual(pressReleaseItemFromGet.DatePublished, ((Models.PressRelease)pressReleaseToUpdateResponse.Item).DatePublished);
                    countOfRecordsNotUpdated ++;
                }
                else
                {
                    Assert.AreEqual(pressReleaseItemFromGet.Title, ((Models.PressRelease)pressReleaseToUpdateResponse.Item).Title);
                    Assert.AreEqual(pressReleaseItemFromGet.DescriptionHtml, ((Models.PressRelease)pressReleaseToUpdateResponse.Item).DescriptionHtml);
                    Assert.AreEqual(pressReleaseItemFromGet.DatePublished, ((Models.PressRelease)pressReleaseToUpdateResponse.Item).DatePublished);
                    countOfRecordsUpdated++;
                }
            }

            Assert.AreEqual(countOfRecordsNotUpdated, 4);
            Assert.AreEqual(countOfRecordsUpdated, 1);
        }

        [TestMethod]
        public void TestUpdateValidation()
        {
            const string title = "Press Release";
            const string titleOverLimit = "Press Release 01234567890123456789012345678901234567890123456789abcd";
            const string descriptionHtml = "<p>Press Release Description</p>";
            DateTime datePublished = DateTime.Now;

            CreateIndividualPressRelease(title, descriptionHtml, datePublished);

            var busPressRelease = new Business.PressRelease();

            var pressReleaseOriginal = (Models.PressRelease)busPressRelease.GetAll().Items[0];
            Assert.IsNotNull(pressReleaseOriginal);

            var errorGuid = Guid.NewGuid();
            var pressReleaseResponse = busPressRelease.Update(new Models.PressRelease() { Id = errorGuid, Title = title, DescriptionHtml = descriptionHtml, DatePublished = datePublished });
            Assert.AreEqual(pressReleaseResponse.Status, ResponseStatus.Failed);
            Assert.AreEqual(pressReleaseResponse.Errors.Count, 1);
            Assert.AreEqual(pressReleaseResponse.Errors[0].Message, String.Format(Errors.RecordDoesNotExist, errorGuid));
            Assert.IsNull(pressReleaseResponse.Item);

            pressReleaseResponse = busPressRelease.Update(new Models.PressRelease() { Id = pressReleaseOriginal.Id, Title = string.Empty, DescriptionHtml = descriptionHtml, DatePublished = datePublished });
            
            Assert.AreEqual(pressReleaseResponse.Status, ResponseStatus.Failed);
            Assert.AreEqual(pressReleaseResponse.Errors.Count, 1);
            Assert.AreEqual(pressReleaseResponse.Errors[0].Message, String.Format(Errors.CanNotBeEmpty, "title"));
            Assert.IsNull(pressReleaseResponse.Item);


            pressReleaseResponse = busPressRelease.Update(new Models.PressRelease() { Id = pressReleaseOriginal.Id, Title = titleOverLimit, DescriptionHtml = descriptionHtml, DatePublished = datePublished });
            
            Assert.AreEqual(pressReleaseResponse.Status, ResponseStatus.Failed);
            Assert.AreEqual(pressReleaseResponse.Errors.Count, 1);
            Assert.AreEqual(pressReleaseResponse.Errors[0].Message, String.Format(Errors.CanNotBeGreaterThan, "title", 50));
            Assert.IsNull(pressReleaseResponse.Item);
            
            pressReleaseResponse = busPressRelease.Update(new Models.PressRelease() { Id = pressReleaseOriginal.Id, Title = titleOverLimit, DescriptionHtml = descriptionHtml, DatePublished = datePublished.AddDays(-1) });
            Assert.AreEqual(pressReleaseResponse.Status, ResponseStatus.Failed);
            Assert.AreEqual(pressReleaseResponse.Errors.Count, 2);
            Assert.AreEqual(pressReleaseResponse.Errors[0].Message, String.Format(Errors.CanNotBeGreaterThan, "title", 50));
            Assert.AreEqual(pressReleaseResponse.Errors[1].Message, String.Format(Errors.DateMustBeInTheFuture, "publication date"));
            Assert.IsNull(pressReleaseResponse.Item);
        }

        [TestMethod]
        public void TestDeleteAll()
        {
            const int numberOfPressReleasesToAdd = 5;

            var busPressRelease = new Business.PressRelease();

            var pressReleaseResponse = busPressRelease.GetAll();
            Assert.AreEqual(pressReleaseResponse.Status, ResponseStatus.Success);
            Assert.IsNull(pressReleaseResponse.Errors);
            Assert.AreEqual(pressReleaseResponse.Items.Count, 0);

            var pressReleaseDeleteResponse = busPressRelease.DeleteAll();
            Assert.AreEqual(pressReleaseDeleteResponse.Status, ResponseStatus.Success);
            Assert.IsNull(pressReleaseDeleteResponse.Errors);

            pressReleaseResponse = busPressRelease.GetAll();
            Assert.AreEqual(pressReleaseResponse.Status, ResponseStatus.Success);
            Assert.IsNull(pressReleaseResponse.Errors);
            Assert.AreEqual(pressReleaseResponse.Items.Count, 0);

            for (int i = 0; i < numberOfPressReleasesToAdd; i++)
            {
                string title = string.Format("Press Release #{0}", i);
                string descriptionHtml = string.Format("<p>Press Release Description #{0}</p>", i);
                DateTime datePublished = DateTime.Now.AddDays(i);

                CreateIndividualPressRelease(title, descriptionHtml, datePublished);
            }

            pressReleaseResponse = busPressRelease.GetAll();
            Assert.AreEqual(pressReleaseResponse.Status, ResponseStatus.Success);
            Assert.IsNull(pressReleaseResponse.Errors);
            Assert.AreEqual(pressReleaseResponse.Items.Count, numberOfPressReleasesToAdd);

            pressReleaseDeleteResponse = busPressRelease.DeleteAll();
            Assert.AreEqual(pressReleaseDeleteResponse.Status, ResponseStatus.Success);
            Assert.IsNull(pressReleaseDeleteResponse.Errors);

            pressReleaseResponse = busPressRelease.GetAll();
            Assert.AreEqual(pressReleaseResponse.Status, ResponseStatus.Success);
            Assert.IsNull(pressReleaseResponse.Errors);
            Assert.AreEqual(pressReleaseResponse.Items.Count, 0);

        }

        [TestMethod]
        public void TestGetAll()
        {
            const int numberOfPressReleasesToAdd = 5;

            var busPressRelease = new Business.PressRelease();

            var pressReleaseResponse = busPressRelease.GetAll();
            Assert.AreEqual(pressReleaseResponse.Status, ResponseStatus.Success);
            Assert.IsNull(pressReleaseResponse.Errors);
            Assert.AreEqual(pressReleaseResponse.Items.Count, 0);

            for (int i = 0; i < numberOfPressReleasesToAdd; i++)
            {
                string title = string.Format("Press Release #{0}", i);
                string descriptionHtml = string.Format("<p>Press Release Description #{0}</p>", i);
                DateTime datePublished = DateTime.Now.AddDays(i);

                CreateIndividualPressRelease(title, descriptionHtml, datePublished);
            }

            pressReleaseResponse = busPressRelease.GetAll();
            Assert.AreEqual(pressReleaseResponse.Status, ResponseStatus.Success);
            Assert.IsNull(pressReleaseResponse.Errors);
            Assert.AreEqual(pressReleaseResponse.Items.Count, numberOfPressReleasesToAdd);

            //Now check correct DESCENDING DATE ORDER
            int reverseIndex = numberOfPressReleasesToAdd - 1;
            foreach(var pressRelease in pressReleaseResponse.Items.Cast<Models.PressRelease>())
            {
                string title = string.Format("Press Release #{0}", reverseIndex);
                string descriptionHtml = string.Format("<p>Press Release Description #{0}</p>", reverseIndex);
                DateTime datePublished = DateTime.Now.AddDays(reverseIndex);

                Assert.AreEqual(pressRelease.Title, title);
                Assert.AreEqual(pressRelease.DescriptionHtml, descriptionHtml);
                Assert.AreEqual(pressRelease.DatePublished.Date, datePublished.Date);
                reverseIndex--;
            }
        }

        [TestMethod]
        public void TestGet()
        {
            const int numberOfPressReleasesToAdd = 3;

            for (int i = 0; i < numberOfPressReleasesToAdd; i++)
            {
                string title = string.Format("Press Release #{0}", i);
                string descriptionHtml = string.Format("<p>Press Release Description #{0}</p>", i);
                DateTime datePublished = DateTime.Now.AddDays(i);

                CreateIndividualPressRelease(title, descriptionHtml, datePublished);
            }

            var busPressRelease = new Business.PressRelease();
            var pressReleaseListReponse = busPressRelease.GetAll();
            Assert.AreEqual(pressReleaseListReponse.Status, ResponseStatus.Success);
            Assert.IsNull(pressReleaseListReponse.Errors);
            Assert.AreEqual(pressReleaseListReponse.Items.Count, numberOfPressReleasesToAdd);

            //Get the middle item
            var pressRelease =
                (Models.PressRelease)pressReleaseListReponse.Items[1];

            var pressReleaseDetailResponse = busPressRelease.Get(pressRelease.Id);
            Assert.AreEqual(pressReleaseDetailResponse.Status, ResponseStatus.Success);
            Assert.IsNull(pressReleaseDetailResponse.Errors);
            Assert.IsNotNull(pressReleaseDetailResponse.Item);

            var pressReleaseFromGet = (Models.PressRelease)pressReleaseDetailResponse.Item;
            Assert.AreEqual(pressReleaseFromGet.Id, pressRelease.Id);
            Assert.AreEqual(pressReleaseFromGet.Title, pressRelease.Title);
            Assert.AreEqual(pressReleaseFromGet.DescriptionHtml, pressRelease.DescriptionHtml);
            Assert.AreEqual(pressReleaseFromGet.DatePublished, pressRelease.DatePublished);

            //Get the first item
            pressRelease =
                (Models.PressRelease)pressReleaseListReponse.Items[0];

            pressReleaseDetailResponse = busPressRelease.Get(pressRelease.Id);
            Assert.AreEqual(pressReleaseDetailResponse.Status, ResponseStatus.Success);
            Assert.IsNull(pressReleaseDetailResponse.Errors);
            Assert.IsNotNull(pressReleaseDetailResponse.Item);

            pressReleaseFromGet = (Models.PressRelease)pressReleaseDetailResponse.Item;
            Assert.AreEqual(pressReleaseFromGet.Id, pressRelease.Id);
            Assert.AreEqual(pressReleaseFromGet.Title, pressRelease.Title);
            Assert.AreEqual(pressReleaseFromGet.DescriptionHtml, pressRelease.DescriptionHtml);
            Assert.AreEqual(pressReleaseFromGet.DatePublished, pressRelease.DatePublished);

            //Get the last item
            pressRelease =
                (Models.PressRelease)pressReleaseListReponse.Items[2];

            pressReleaseDetailResponse = busPressRelease.Get(pressRelease.Id);
            Assert.AreEqual(pressReleaseDetailResponse.Status, ResponseStatus.Success);
            Assert.IsNull(pressReleaseDetailResponse.Errors);
            Assert.IsNotNull(pressReleaseDetailResponse.Item);

            pressReleaseFromGet = (Models.PressRelease)pressReleaseDetailResponse.Item;
            Assert.AreEqual(pressReleaseFromGet.Id, pressRelease.Id);
            Assert.AreEqual(pressReleaseFromGet.Title, pressRelease.Title);
            Assert.AreEqual(pressReleaseFromGet.DescriptionHtml, pressRelease.DescriptionHtml);
            Assert.AreEqual(pressReleaseFromGet.DatePublished, pressRelease.DatePublished);

        }

        [TestMethod]
        public void TestGetValidation()
        {
            Guid emptyGuid = Guid.Empty;
            Guid randomGuid = Guid.NewGuid();

            const string title = "Press Release";
            const string descriptionHtml = "<p>Press Release Description</p>";
            DateTime datePublished = DateTime.Now;

            var busPressRelease = new Business.PressRelease();

            var pressReleaseListReponse = busPressRelease.GetAll();
            Assert.AreEqual(pressReleaseListReponse.Items.Count, 0);

            var pressReleaseDetailResponse = busPressRelease.Get(Guid.Empty);
            Assert.AreEqual(pressReleaseDetailResponse.Status, ResponseStatus.Failed);
            Assert.AreEqual(pressReleaseDetailResponse.Errors.Count, 1);
            Assert.AreEqual(pressReleaseDetailResponse.Errors[0].Message, String.Format(Errors.RecordDoesNotExist, emptyGuid));

            pressReleaseDetailResponse = busPressRelease.Get(randomGuid);
            Assert.AreEqual(pressReleaseDetailResponse.Status, ResponseStatus.Failed);
            Assert.AreEqual(pressReleaseDetailResponse.Errors.Count, 1);
            Assert.AreEqual(pressReleaseDetailResponse.Errors[0].Message, String.Format(Errors.RecordDoesNotExist, randomGuid));
            
            pressReleaseDetailResponse = busPressRelease.Add(new Models.PressRelease() { Title = title, DescriptionHtml = descriptionHtml, DatePublished = datePublished });
            Assert.AreEqual(pressReleaseDetailResponse.Status, ResponseStatus.Success);
            Assert.IsNull(pressReleaseDetailResponse.Errors);
            Assert.IsNotNull(pressReleaseDetailResponse.Item);

            var pressReleaseItem = (Models.PressRelease)pressReleaseDetailResponse.Item;
            Assert.AreNotEqual(pressReleaseItem.Id, Guid.Empty);
            Assert.AreEqual(pressReleaseItem.Title, title);
            Assert.AreEqual(pressReleaseItem.DescriptionHtml, descriptionHtml);
            Assert.AreEqual(pressReleaseItem.DatePublished, datePublished);

            pressReleaseListReponse = busPressRelease.GetAll();
            Assert.AreEqual(pressReleaseListReponse.Items.Count, 1);

            pressReleaseDetailResponse = busPressRelease.Get(pressReleaseItem.Id);
            Assert.AreEqual(pressReleaseDetailResponse.Status, ResponseStatus.Success);
            Assert.IsNull(pressReleaseDetailResponse.Errors);

            pressReleaseListReponse = busPressRelease.GetAll();
            Assert.AreEqual(pressReleaseListReponse.Items.Count, 1);
        }

        [TestMethod]
        public void TestDeleteValidation()
        {
            Guid emptyGuid = Guid.Empty;
            Guid randomGuid = Guid.NewGuid();

            const string title = "Press Release";
            const string descriptionHtml = "<p>Press Release Description</p>";
            DateTime datePublished = DateTime.Now;

            var busPressRelease = new Business.PressRelease();

            var pressReleaseListReponse = busPressRelease.GetAll();
            Assert.AreEqual(pressReleaseListReponse.Items.Count, 0);

            ResponseInfoDelete pressReleaseDeleteResponse = busPressRelease.Delete(Guid.Empty);
            Assert.AreEqual(pressReleaseDeleteResponse.Status, ResponseStatus.Failed);
            Assert.AreEqual(pressReleaseDeleteResponse.Errors.Count, 1);
            Assert.AreEqual(pressReleaseDeleteResponse.Errors[0].Message, String.Format(Errors.RecordDoesNotExist, emptyGuid));

            pressReleaseDeleteResponse = busPressRelease.Delete(randomGuid);
            Assert.AreEqual(pressReleaseDeleteResponse.Status, ResponseStatus.Failed);
            Assert.AreEqual(pressReleaseDeleteResponse.Errors.Count, 1);
            Assert.AreEqual(pressReleaseDeleteResponse.Errors[0].Message, String.Format(Errors.RecordDoesNotExist, randomGuid));
            
            var pressReleaseDetailResponse = busPressRelease.Add(new Models.PressRelease() { Title = title, DescriptionHtml = descriptionHtml, DatePublished = datePublished });
            Assert.AreEqual(pressReleaseDetailResponse.Status, ResponseStatus.Success);
            Assert.IsNull(pressReleaseDetailResponse.Errors);
            Assert.IsNotNull(pressReleaseDetailResponse.Item);

            var pressReleaseItem = (Models.PressRelease)pressReleaseDetailResponse.Item;
            Assert.AreNotEqual(pressReleaseItem.Id, Guid.Empty);
            Assert.AreEqual(pressReleaseItem.Title, title);
            Assert.AreEqual(pressReleaseItem.DescriptionHtml, descriptionHtml);
            Assert.AreEqual(pressReleaseItem.DatePublished, datePublished);

            pressReleaseListReponse = busPressRelease.GetAll();
            Assert.AreEqual(pressReleaseListReponse.Items.Count, 1);

            pressReleaseDeleteResponse = busPressRelease.Delete(pressReleaseItem.Id);
            Assert.AreEqual(pressReleaseDeleteResponse.Status, ResponseStatus.Success);
            Assert.IsNull(pressReleaseDeleteResponse.Errors);

            pressReleaseListReponse = busPressRelease.GetAll();
            Assert.AreEqual(pressReleaseListReponse.Items.Count, 0);
        }

        private void CreateIndividualPressRelease(string title, string descriptionHtml, DateTime datePublished)
        {
            var busPressRelease = new Business.PressRelease();
            
            var pressReleaseResponse = busPressRelease.Add(new Models.PressRelease() { Title = title, DescriptionHtml = descriptionHtml, DatePublished = datePublished });
            Assert.AreEqual(pressReleaseResponse.Status, ResponseStatus.Success);
            Assert.IsNull(pressReleaseResponse.Errors);
            Assert.IsNotNull(pressReleaseResponse.Item);

            var pressReleaseItem = (Models.PressRelease)pressReleaseResponse.Item;
            Assert.AreNotEqual(pressReleaseItem.Id, Guid.Empty);
            Assert.AreEqual(pressReleaseItem.Title, title);
            Assert.AreEqual(pressReleaseItem.DescriptionHtml, descriptionHtml);
            Assert.AreEqual(pressReleaseItem.DatePublished, datePublished);

            var pressReleaseResponseFromGet = busPressRelease.Get(pressReleaseItem.Id);
            Assert.AreEqual(pressReleaseResponseFromGet.Status, ResponseStatus.Success);
            Assert.IsNull(pressReleaseResponseFromGet.Errors);
            Assert.IsNotNull(pressReleaseResponseFromGet.Item);

            var pressReleaseItemFromGet = (Models.PressRelease)pressReleaseResponseFromGet.Item;
            Assert.AreEqual(pressReleaseItemFromGet.Id, pressReleaseItem.Id);
            Assert.AreEqual(pressReleaseItemFromGet.Title, pressReleaseItem.Title);
            Assert.AreEqual(pressReleaseItemFromGet.DescriptionHtml, pressReleaseItem.DescriptionHtml);
            Assert.AreEqual(pressReleaseItemFromGet.DatePublished.Date, pressReleaseItem.DatePublished.Date);
        }

    }
}
