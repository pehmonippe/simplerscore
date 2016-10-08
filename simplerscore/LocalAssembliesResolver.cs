namespace SimplerScore
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Web.Http.Dispatcher;
    using JetBrains.Annotations;

    /// <summary>
    /// LocalAssembliesResolver defines the assemblies used for Controller look-up
    /// to be only either the caller (of this class') assembly or defined by explicit
    /// list of types.
    /// </summary>
    public class LocalAssembliesResolver : IAssembliesResolver
    {
        private readonly List<Assembly> assemblies;

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalAssembliesResolver"/> class.
        /// Using only the calling assembly as acceptable.
        /// </summary>
        public LocalAssembliesResolver ()
        {
            assemblies = new List<Assembly> { Assembly.GetCallingAssembly() };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalAssembliesResolver"/> class.
        /// Using only the object's type's assembly
        /// </summary>
        /// <param name="obj">The object.</param>
        public LocalAssembliesResolver ([NotNull] object obj)
        {
            assemblies = new List<Assembly> { obj.GetType().Assembly };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalAssembliesResolver"/> class.
        /// Using only the type's assembly as acceptable.
        /// </summary>
        /// <param name="type">The type.</param>
        public LocalAssembliesResolver ([NotNull] Type type)
        {
            assemblies = new List<Assembly> { type.Assembly };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalAssembliesResolver" /> class.
        /// Using assemblies of provided types.
        /// </summary>
        /// <param name="types">The types.</param>
        public LocalAssembliesResolver ([NotNull] IEnumerable<Type> types)
        {
            assemblies = types
                .Select(t => t.Assembly)
                .Distinct()
                .ToList();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalAssembliesResolver" /> class.
        /// Using assemblies of provided object's types.
        /// </summary>
        /// <param name="items">The items.</param>
        public LocalAssembliesResolver ([NotNull] IEnumerable<object> items)
        {
            assemblies = items
                .Select(t => t.GetType().Assembly)
                .Distinct()
                .ToList();
        }

        /// <summary>
        /// Returns a list of assemblies available for the application.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.Generic.ICollection`1" /> of assemblies.
        /// </returns>
        public ICollection<Assembly> GetAssemblies ()
        {
            var internals = new InternalAssemblyResolver().GetInternalAssemblies();

            var outgoing = (internals as List<Assembly> ?? internals.ToList());
            outgoing.AddRange(assemblies);

            return outgoing;
        }
    }
}