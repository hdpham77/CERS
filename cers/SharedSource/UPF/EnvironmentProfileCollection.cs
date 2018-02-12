using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using UPF.Configuration;

namespace UPF {

	public class EnvironmentProfileCollection : Collection<EnvironmentProfile> {


		public EnvironmentProfileCollection() {

		}

		public void Add(RuntimeEnvironment environment) {
			this.Add(new EnvironmentProfile(environment));
		}

		public EnvironmentProfileCollection(EnvironmentProfileConfigurationElementCollection configElements) {
			AddRange(configElements);
		}

		public void AddRange(EnvironmentProfileConfigurationElementCollection configElements) {
			if (configElements != null) {
				foreach (EnvironmentProfileConfigurationElement configElement in configElements) {
					this.Add(new EnvironmentProfile(configElement));
				}
			}
		}

		public void AddRange(IEnumerable<EnvironmentProfile> profiles) {
			if (profiles != null) {
				foreach (EnvironmentProfile profile in profiles) {
					this.Add(profile);
				}
			}
		}

		public EnvironmentProfile this[RuntimeEnvironment environment] {
			get {
				return this.SingleOrDefault(e => e.Environment == environment);
			}
		}

		public bool Contains(RuntimeEnvironment environment) {
			return (this.Count(e => e.Environment == environment) > 0);
		}

	}
}
