using System;
using System.Collections.Generic;

namespace UIAutomation.Client.Internal
{
	internal class SessionStore
	{
		private static SessionStore instance;
		private static Dictionary<Guid, Session> Sessions;

		static SessionStore()
		{
			Sessions = new Dictionary<Guid, Session>();
		}

		internal static SessionStore Instance => instance ?? (instance = new SessionStore());

		internal Session this[Guid guid] => Sessions[guid];

		internal void Add(Session session)
		{
			Sessions.Add(session.SessionId, session);
		}

		internal void Remove(Session session)
		{
			Sessions.Remove(session.SessionId);
		}

		internal void Remove(Guid guid)
		{
			Sessions.Remove(guid);
		}
	}
}
