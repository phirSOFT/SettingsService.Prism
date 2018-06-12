using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Prism.Regions;

namespace phirSOFT.SettingsService.Prism
{
    /// <summary>
    ///     Manages different editor pages to modify settings.
    /// </summary>
    public interface ISettingsEditorViewManager
    {
        /// <summary>
        /// Registers an editor view to this manager.
        /// </summary>
        /// <param name="editorViewType">The type of the editor view.</param>
        /// <param name="editorUri">The uri to this editor View. This Uri is used to place the edtitor in the editor pages tree.</param>
        void RegisterEditorView(Type editorViewType, Uri editorUri);

        /// <summary>
        /// Requests the editor manager to navigate to the specific editor page.
        /// </summary>
        /// <param name="editorUri">The uri of the editor page, that was used when registring the editor view.</param>
        /// <param name="navigationParameters">Navigation parameters to for the navigation</param>
        void RequestNavigate(Uri editorUri, NavigationParameters navigationParameters);

        /// <summary>
        ///     Adds an resolver to the editor view manager. This is used to resolve the labels in the tree
        /// </summary>
        /// <param name="resolveUri">The uri that can be resolved using this resolver. The uri can end with a '*' to signal, that the resolver will handle all children.</param>
        /// <param name="resolver">This function resolves the view model for the editor view in the tree.</param>
        /// <param name="canResolve">This function determines, wheter the resolver can resolve a model or not.</param>
        void RegisterViewModelResolver(Uri resolveUri, Func<Uri, object> resolver, Func<Uri, bool> canResolve);
    }
}
