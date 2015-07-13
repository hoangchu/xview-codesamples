using Tridion.ContentManager;
using Tridion.ContentManager.Templating;

namespace Chimote.Tridion.Templating.Intranet.Common
{
    public static partial class TomExtensions
    {
        /// <summary>
        /// Gets an <see cref="IdentifiableObject"/> of a given type for the given TcmUri string or WebDavURL.
        /// </summary>
        /// <typeparam name="T">Type of <see cref="IdentifiableObject"/>.</typeparam>
        /// <param name="engine"><see cref="Engine"/> object.</param>
        /// <param name="itemUriOrWebDavUrl">TcmUri string or WebDavURL.</param>
        /// <returns><see cref="IdentifiableObject"/> object of the given type.</returns>
        public static T GetObject<T>(this Engine engine, string itemUriOrWebDavUrl) where T : IdentifiableObject
        {
            return engine.GetObject(itemUriOrWebDavUrl) as T;
        }

        /// <summary>
        /// Gets an <see cref="IdentifiableObject"/> of a given type for the given <see cref="TcmUri"/>.
        /// </summary>
        /// <typeparam name="T">Type of <see cref="IdentifiableObject"/>.</typeparam>
        /// <param name="engine"><see cref="Engine"/> object.</param>
        /// <param name="tcmUri"><see cref="TcmUri"/> object.</param>
        /// <returns><see cref="IdentifiableObject"/> object of the given type.</returns>
        public static T GetObject<T>(this Engine engine, TcmUri tcmUri) where T : IdentifiableObject
        {
            return engine.GetObject(tcmUri) as T;
        }

        /// <summary>
        /// Gets an <see cref="IdentifiableObject"/> of a given type from the given <see cref="Item"/>.
        /// </summary>
        /// <typeparam name="T">Type of <see cref="IdentifiableObject"/>.</typeparam>
        /// <param name="engine"><see cref="Engine"/> object.</param>
        /// <param name="packageItem"><see cref="Item"/> object.</param>
        /// <returns><see cref="IdentifiableObject"/> object of the given type.</returns>
        public static T GetObject<T>(this Engine engine, Item packageItem) where T : IdentifiableObject
        {
            return engine.GetObject(packageItem) as T;
        }
    }
}