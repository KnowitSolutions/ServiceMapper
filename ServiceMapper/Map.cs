using System;

namespace ServiceMapper
{
	public class Map
	{
		public Type Type { get; private set; }
		public bool Ignored { get; set; }
		private string _location;
		private string _binding;

		public Map(Type type)
		{
			Type = type;
		}
		public Map Location(string name) {
			_location = name;
			return this;
		}
		public Map Binding(string binding) {
			_binding = binding;
			return this;
		}
		public Map Ignore() {
			Ignored = true;
			return this;
		}
	}
}