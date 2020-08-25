using System;
using System.Web.Http;
using System.Web.Http.Results;
using UIAutomation.Client.Internal;
using UIAutomation.Core;

namespace UIAutomation.Client
{
	[RoutePrefix("session")]
	public class SessionController : ApiController
	{
		private SessionStore SessionStore = SessionStore.Instance;

		[HttpGet]
		[Route("")]
		public string GetInfo(LaunchSession launchSession)
		{
			return "Server running";
		}

		[HttpPost]
		[Route("")]
		public Guid CreateNewSession(LaunchSession launchSession)
		{
			var session = new Session(launchSession);

			SessionStore.Add(session);

			session.StartApplication();
			return session.SessionId;
		}

		[HttpDelete]
		[Route("{sessionId}")]
		public IHttpActionResult RemoveSession(string sessionId)
		{
			var session = GetSession(sessionId);

			session.KillApplication();

			SessionStore.Remove(new Guid(sessionId));
			return Ok($"Session {session.SessionId} successfully removed");
		}

		[HttpPost]
		[Route("{sessionId}/element")]
		public Element FindElement(string sessionId, FindElementStrategy strategy)
		{
			var session = GetSession(sessionId);
			var element = new Element { Id = session.FindElement(strategy) };
			return element;
		}

		[HttpPost]
		[Route("{sessionId}/element/{elementId}/element")]
		public Element FindElementInElement(string sessionId, string elementId, FindElementStrategy strategy)
		{
			var session = GetSession(sessionId);
			var element = new Element { Id = session.FindElement(new Guid(elementId), strategy) };
			return element;
		}

		[HttpPost]
		[Route("{sessionId}/element/{elementId}/action")]
		public JsonResult<object> PerformAction(string sessionId, string elementId, ElementAction elementAction)
		{
			var session = GetSession(sessionId);
			var result = session.PerformAction(new Guid(elementId), elementAction);
			return Json(result);
		}

		private Session GetSession(string id)
		{
			return SessionStore[new Guid(id)];
		}
	}
}
