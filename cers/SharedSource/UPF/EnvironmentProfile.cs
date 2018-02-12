using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UPF.Configuration;

namespace UPF {
	public class EnvironmentProfile {

		public RuntimeEnvironment Environment { get; set; }
		public bool ShowUIIndicator { get; set; }
		public string CustomMessage { get; set; }
		public string FriendlyName { get; set; }
		
		public EnvironmentProfile(RuntimeEnvironment environment) : this(environment, false, string.Empty, environment.ToString()) {

		}

		public EnvironmentProfile(RuntimeEnvironment environment, bool showUIIndicator, string customMessage, string friendlyName) {
			Environment = environment;
			ShowUIIndicator = showUIIndicator;
			CustomMessage = customMessage;
			FriendlyName = friendlyName;
		}

		public EnvironmentProfile(EnvironmentProfileConfigurationElement configElement) {
			Environment = configElement.Key;
			ShowUIIndicator = configElement.ShowUIIndicator;
			CustomMessage = configElement.GetCustomMessage();
			FriendlyName = configElement.GetFriendlyName();
		}

	}
}
