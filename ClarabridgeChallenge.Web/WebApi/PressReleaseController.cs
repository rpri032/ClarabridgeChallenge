using ClarabridgeChallenge.Business.Responses;
using ClarabridgeChallenge.Models;
using Newtonsoft.Json.Linq;
using Ngp.Oberon.Web;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ClarabridgeChallenge.Web.WebApi
{
    public class PressReleaseController : ApiController
    {
        // GET api/<controller>
        [HttpGet]
        public HttpResponseMessage Get()
        {
            var response = new Business.PressRelease().GetAll();
            if (response.Errors != null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "<br/>" + String.Join("<br/>", response.Errors.Select(e => e.Message)));
            }

            return Request.CreateResponse(HttpStatusCode.OK,
                response.Items.Cast<PressRelease>().Select(r => new JObject(
                    new JProperty("Id", r.Id),
                    new JProperty("Title", r.Title),
                    new JProperty("DescriptionHtml", Business.Common.Formatters.TrimText(
                        Business.Common.CommonFunctions.RemoveHtml(r.DescriptionHtml))),
                    new JProperty("DatePublished", Business.Common.Formatters.DateTimeFormats.Format_DatePicker(r.DatePublished)),
                    new JProperty("DatePublishedFormatted", Business.Common.Formatters.DateTimeFormats.FormatShortDate_WithTime(r.DatePublished))
                )));
        }

        // GET api/<controller>/9366314D-D1E2-47F5-9B4A-07C622CC929C
        [HttpGet]
        public HttpResponseMessage Get(Guid id)
        {
            var response = new Business.PressRelease().Get(id);
            if (response.Errors != null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "<br/>" + String.Join("<br/>", response.Errors.Select(e => e.Message)));
            }
            var result = response.Item as PressRelease;
            return Request.CreateResponse(HttpStatusCode.OK,
                new JObject(
                    new JProperty("Id", result.Id),
                    new JProperty("Title", result.Title),
                    new JProperty("DescriptionHtml", result.DescriptionHtml),
                    new JProperty("DatePublished", Business.Common.Formatters.DateTimeFormats.Format_DatePicker(result.DatePublished)),
                    new JProperty("DatePublishedFormatted", Business.Common.Formatters.DateTimeFormats.FormatShortDate_WithTime(result.DatePublished))
            ));
        }

        // POST api/<controller>
        [HttpPost]
        public HttpResponseMessage Post(PressRelease pressRelease)
        {
            ResponseInfoDetail response;
            if (pressRelease.Id == null || pressRelease.Id == Guid.Empty)
            {
                response = new Business.PressRelease().Add(pressRelease);
            }
            else
            {
                response = new Business.PressRelease().Update(pressRelease);
            }
            if (response.Errors != null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "<br/>" + String.Join("<br/>", response.Errors.Select(e => e.Message)));
            }
            //Send SignalR Notification to all clients as Press Releases have been added/updated
            System.Threading.Tasks.Task.Factory.StartNew(() => this.BroadcastPressReleaseNotification());
            var result = response.Item as PressRelease;
            return Request.CreateResponse(HttpStatusCode.OK,
                new JObject(
                    new JProperty("Id", result.Id),
                    new JProperty("Title", result.Title),
                    new JProperty("DescriptionHtml", result.DescriptionHtml),
                    new JProperty("DatePublished", Business.Common.Formatters.DateTimeFormats.Format_DatePicker(result.DatePublished)),
                    new JProperty("DatePublishedFormatted", Business.Common.Formatters.DateTimeFormats.FormatShortDate_WithTime(result.DatePublished))
            ));
        }

        // DELETE api/<controller>/5
        [HttpDelete]
        public HttpResponseMessage Delete(Guid id)
        {
            var response = new Business.PressRelease().Delete(id);
            if (response.Errors != null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "<br/>" + String.Join("<br/>", response.Errors.Select(e => e.Message)));
            }

            //Send SignalR Notification to all clients as Press Releases have deleted
            System.Threading.Tasks.Task.Factory.StartNew(() => this.BroadcastPressReleaseNotification());
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        // DELETE api/<controller>/5
        [HttpDelete]
        public HttpResponseMessage DeleteAll()
        {
            var response = new Business.PressRelease().DeleteAll();
            if (response.Errors != null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "<br/>" + String.Join("<br/>", response.Errors.Select(e => e.Message)));
            }
            //Send SignalR Notification to all clients as Press Releases have deleted
            System.Threading.Tasks.Task.Factory.StartNew(() => this.BroadcastPressReleaseNotification());
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        private void BroadcastPressReleaseNotification()
        {
            PressReleaseNotificationHub.PressReleaseListRefresh();
        }
    }
}