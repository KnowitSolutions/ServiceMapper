using System;
using System.Linq;
using System.Reflection;

namespace ServiceMapper
{
	public class AssemblyLoader
	{
		public static void LoadAllAssemblies()
		{
			foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
			{
				LoadReferencedAssembly(assembly);
			}
		}

		public static void LoadReferencedAssembly(Assembly assembly)
		{
			foreach (AssemblyName name in assembly.GetReferencedAssemblies())
			{
				if (!AppDomain.CurrentDomain.GetAssemblies().Any(a => a.FullName == name.FullName))
				{
					LoadReferencedAssembly(Assembly.Load(name));
				}
			}
		}
	}
}
