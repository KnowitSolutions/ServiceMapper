using System;
using System.ServiceModel.Channels;

namespace ServiceMapper
{
	public class Map
	{
		public Type Type { get; private set; }
		public bool Ignored { get; private set; }
		public string Location { get; private set; }
		public Binding Binding { get; private set; }
		public Type HostingType { get; private set; }

		public Map(Type type)
		{
			Type = type;
		}
		public void SetLocation(string name)
		{
			Location = name;
		}
		public void SetBinding(Binding binding)
		{
			Binding = binding;
		}
		public void SetHostingType(Type type)
		{
			HostingType = type;
		}
		public void Ignore()
		{
			Ignored = true;
		}
	}
}