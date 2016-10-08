namespace SimplerScore
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    internal class InternalAssemblyResolver
    {
        private const string InternalResourcePrefix = "SimplerScore.packages";

        public Assembly ResolveAssembly (AssemblyName assemblyName)
        {
            var runningAssembly = Assembly.GetExecutingAssembly();
            var fullResourceName = DetermineEmbeddedName(assemblyName, runningAssembly);

            return LoadInternalAssembly(runningAssembly, fullResourceName);
        }

        private static Assembly LoadInternalAssembly (Assembly runningAssembly, string fullResourceName)
        {
            using (var stream = runningAssembly.GetManifestResourceStream(fullResourceName))
            {
                if (null == stream)
                    return null;

                var data = new byte[stream.Length];
                stream.Read(data, 0, data.Length);

                var assembly = Assembly.Load(data);
                return assembly;
            }
        }

        internal IEnumerable<Assembly> GetInternalAssemblies ()
        {
            var runningAssembly = Assembly.GetExecutingAssembly();

            var internalAssemblies = new List<Assembly>();

            runningAssembly.GetManifestResourceNames()
                .Where(n => n.StartsWith(InternalResourcePrefix)
                            && n.EndsWith(".dll"))
                .ToList()
                .ForEach(name =>
                {
                    var assy = LoadInternalAssembly(runningAssembly, name);
                    internalAssemblies.Add(assy);
                });

            return internalAssemblies;
        }

        private static string DetermineEmbeddedName (AssemblyName assemblyName, Assembly runningAssembly)
        {
            var nameForResolving = assemblyName.Name;

            if (nameForResolving.Contains(","))
            {
                var elements = nameForResolving.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                if (elements.Length > 0)
                    nameForResolving = elements[0];
            }

            // from available resources match the one that ends with nameForResolving.
            var fullResourceName = runningAssembly.GetManifestResourceNames()
                .FirstOrDefault(n => n.StartsWith(InternalResourcePrefix)
                                     && n.EndsWith(nameForResolving + ".dll"));
            return fullResourceName;
        }
    }

    public class WebApiSupportAssemblyResolver
    {
        private static readonly ConcurrentDictionary<AssemblyName, Assembly> Cache = new ConcurrentDictionary<AssemblyName, Assembly>();
        private static readonly InternalAssemblyResolver AssemblyResolver = new InternalAssemblyResolver();

        public static void RegisterAssemblyResolver ()
        {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
        }

        public static void UnregisterAssemblyResolver ()
        {
            AppDomain.CurrentDomain.AssemblyResolve -= CurrentDomain_AssemblyResolve;
        }

        private static Assembly CurrentDomain_AssemblyResolve (object sender, ResolveEventArgs args)
        {
            var targetName = new AssemblyName(args.Name);

            if (!Cache.ContainsKey(targetName))
            {
                var assembly = AssemblyResolver.ResolveAssembly(new AssemblyName(args.Name));

                if (null == assembly)
                    return null;

                Cache.TryAdd(targetName, assembly);
            }

            var assy = Cache[targetName];
            return assy;
        }
    }
}
