using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace phirSOFT.SettingsService.Prism
{
    /// <summary>
    /// Represents a settings page in the settings page tree
    /// </summary>
    public interface ISettingsPageNode
    {
        /// <summary>
        /// The uri to navigate to.
        /// </summary>
        Uri NavigationUri { get; }

        /// <summary>
        /// The Datacontext of this settings page node 
        /// </summary>
        object DataContext { get; }

        /// <summary>
        /// The children of this node.
        /// </summary>
        IReadOnlyCollection<ISettingsPageNode> Children { get; }
    }

    
}