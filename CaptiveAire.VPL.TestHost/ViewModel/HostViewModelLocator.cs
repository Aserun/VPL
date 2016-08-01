using System;
using System.Collections.Generic;
using System.Linq;
using CaptiveAire.VPL.Factory;
using CaptiveAire.VPL.Interfaces;
using CaptiveAire.VPL.Metadata;
using CaptiveAire.VPL.Model;
using CaptiveAire.VPL.Plugins;
using CaptiveAire.VPL.TestHost.Model;
using Cas.Common.WPF;
using Cas.Common.WPF.Interfaces;

namespace CaptiveAire.VPL.TestHost.ViewModel
{
    public static class HostViewModelLocator
    {
        public static readonly IVplService VplService; 

        public static readonly IFileDialogService FileDialogService = new FileDialogService();

        private static readonly Lazy<MainViewModel> _main = new Lazy<MainViewModel>(() => new MainViewModel(FileDialogService));

        static HostViewModelLocator()
        {
            var customResources = new View.CustomResources();

            customResources.InitializeComponent();

            var commentFactory = new ElementFactory(CustomElementTypeIds.Comment, "Custom", "Comment",
                context => new CommentViewModel(context), typeof(CommentViewModel));

            var functionService = new FunctionService(GetFunctions, GetFunction);

            var factories = new IElementFactory[]
            {
                commentFactory,
            };

            //Create the plugin
            var plugin = new VplPlugin(
                "Host",
                factories,
                new[] { customResources },
                services: new[] { functionService });

            var plugins = SystemPluginFactory.CreateAllPlugins()
                .ToList();

            plugins.Add(plugin);
            
            VplService = new VplService(plugins);
        }

        public static IEnumerable<FunctionMetadata> GetFunctions()
        {
            var functions =  Main.Program.Functions.Select(f => f.GetMetadata());

            return functions;
        }

        public static FunctionMetadata GetFunction(Guid id)
        {
            return Main.Program.Functions
                .Where(f => f.Id == id)
                .Select(f => f.GetMetadata())
                .FirstOrDefault();
        }

        public static MainViewModel Main
        {
            get { return _main.Value; }
        }
    }
}