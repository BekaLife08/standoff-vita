using DeadMosquito.AndroidGoodies.Internal;
using MiniJSON;
using System.Collections.Generic;
using System.Linq;

namespace DeadMosquito.AndroidGoodies
{
	public sealed class ContactPickResult
	{
		public string DisplayName
		{
			get;
			private set;
		}

		public string PhotoUri
		{
			get;
			private set;
		}

		public List<string> Phones
		{
			get;
			private set;
		}

		public List<string> Emails
		{
			get;
			private set;
		}

		private ContactPickResult()
		{
			Phones = new List<string>();
			Emails = new List<string>();
		}

		public static ContactPickResult FromJson(string json)
		{
			ContactPickResult contactPickResult = new ContactPickResult();
			Dictionary<string, object> dictionary = Json.Deserialize(json) as Dictionary<string, object>;
			contactPickResult.DisplayName = dictionary.GetStr("displayName");
			contactPickResult.PhotoUri = dictionary.GetStr("photoUri");
			contactPickResult.Phones = ((List<object>)dictionary["phones"]).OfType<string>().ToList();
			contactPickResult.Emails = ((List<object>)dictionary["emails"]).OfType<string>().ToList();
			return contactPickResult;
		}

		public override string ToString()
		{
			return $"[ContactPickResult: DisplayName={DisplayName}]";
		}
	}
}
