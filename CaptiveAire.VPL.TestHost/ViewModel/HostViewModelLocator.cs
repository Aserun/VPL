using System;
using System.Collections.Generic;
using System.Linq;
using CaptiveAire.VPL.Interfaces;
using CaptiveAire.VPL.Metadata;
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

            var functionService = new FunctionService(GetFunctions, GetFunction, EditFunction);

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

            var plugin1 = new VplPlugin("Alert",
                new IElementFactory[]
                {
                    new ElementFactory(new Guid("FBB6804C-B90C-4A88-B28B-8B733C1A9F0D"), "Interaction", "Alert", context => new Alert(context), typeof(Alert)), 
                });

            var plugins = SystemPluginFactory.CreateAllPlugins()
                .ToList();

            plugins.Add(plugin);
            plugins.Add(plugin1);

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

        private static void EditFunction(Guid id)
        {
            var functionViewModel = _main.Value.Program.Functions.FirstOrDefault(f => f.Id == id);

            if (functionViewModel != null)
            {
                functionViewModel.Edit();
            }
        }
    }
}