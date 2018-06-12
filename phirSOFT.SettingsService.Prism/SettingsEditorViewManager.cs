using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Microsoft.Practices.ServiceLocation;
using Prism.Regions;

namespace phirSOFT.SettingsService.Prism
{
    class SettingsEditorViewManager : ISettingsEditorViewManager
    {
        private readonly IRegionManager _regionManager;
        private readonly ISettingsEditorView _editorView;
        private readonly string _targetRegionName;
        private readonly ConcurrentDictionary<Uri, Uri> _editorViews;
        private IRegionManager _scopedRegionManager;
        private readonly SettingsPageNode _rootNode;

        public SettingsEditorViewManager(IRegionManager regionManager, ISettingsEditorView editorView, string targetRegionName)
        {
            _regionManager = regionManager;
            _editorView = editorView;
            _targetRegionName = targetRegionName;
            _editorViews = new ConcurrentDictionary<Uri, Uri>();
            _rootNode = new SettingsPageNode(this, new Uri("settings:///"), null);
        }

        public void RegisterEditorPage(Uri editorUri, Uri editorPageUri)
        {
            _editorViews.TryAdd(editorUri, editorPageUri);

            var segments = ((IEnumerable<string>) editorUri.Segments).GetEnumerator();
            _rootNode.AddChild(editorUri, editorPageUri, segments);
        }

        public void RequestNavigate(Uri editorUri, NavigationParameters navigationParameters)
        {
            if(!_editorViews.TryGetValue(editorUri, out var navUri))
                throw new InvalidOperationException();

            if (!_regionManager.Regions[_targetRegionName].Views.Contains(_editorView))
                _scopedRegionManager = _regionManager.Regions[_targetRegionName].Add(_editorView, "SettingsView", true);

            if (_scopedRegionManager == null)
                _scopedRegionManager = RegionManager.GetRegionManager((DependencyObject) _editorView);

            _regionManager.Regions[_targetRegionName].Activate(_editorView);

            
            _scopedRegionManager.RequestNavigate(_editorView.SettingsPageRegionName, navUri, navigationParameters);
        }

        public void RegisterViewModelResolver(Uri resolveUri, Func<Uri, object> resolver, Func<Uri, bool> canResolve)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyCollection<ISettingsPageNode> RootNodes => _rootNode.Children;

        private object ResolveDataContext(Uri navigationUri)
        {
            throw new NotImplementedException();
        }

        internal class SettingsPageNode : ISettingsPageNode
        {
            private readonly SettingsEditorViewManager _manager;
            private readonly Uri _treeUri;
            private readonly ObservableCollection<SettingsPageNode> _children;
            private readonly Lazy<object> _datacontext;
            private readonly int depth;
            private readonly string key;

            public SettingsPageNode(SettingsEditorViewManager manager, Uri treeUri, Uri navigationUri)
            {
                _manager = manager;
                _treeUri = treeUri;
                NavigationUri = navigationUri;
                _children = new ObservableCollection<SettingsPageNode>();
                Children = new ReadOnlyObservableCollection<SettingsPageNode>(_children);
                _datacontext = new Lazy<object>(() => _manager.ResolveDataContext(NavigationUri));
                depth = treeUri.Segments.Length - 1;
                key = treeUri.Segments[depth];
            }

         

            public void AddChild(Uri childUri, Uri navigationUri, IEnumerator<string> segments)
            {
                var lookup = childUri.Segments[depth + 1];
                var child = _children.FirstOrDefault(c =>
                    string.Equals(c.key, lookup, StringComparison.InvariantCultureIgnoreCase));

                if (child == null)
                {
                    child = new SettingsPageNode(_manager, new UriBuilder(), );
                }
            }

            public Uri NavigationUri { get; }
            public object DataContext => _datacontext.Value;

            public IReadOnlyCollection<ISettingsPageNode> Children { get; }
        }
    }
}