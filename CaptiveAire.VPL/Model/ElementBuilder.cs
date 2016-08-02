using System;
using System.Collections.Generic;
using System.Linq;
using CaptiveAire.VPL.Extensions;
using CaptiveAire.VPL.Interfaces;
using CaptiveAire.VPL.Metadata;
using CaptiveAire.VPL.ViewModel;

namespace CaptiveAire.VPL.Model
{
    /// <summary>
    /// Handles the creation of elements from metadata.
    /// </summary>
    internal class ElementBuilder : IElementBuilder
    {
        private readonly IElementFactoryManager _factoryManager;
        private readonly IVplServiceContext _context;

        public ElementBuilder(IElementFactoryManager factoryManager, IVplServiceContext context)
        {
            if (factoryManager == null) throw new ArgumentNullException(nameof(factoryManager));
            if (context == null) throw new ArgumentNullException(nameof(context));

            _factoryManager = factoryManager;
            _context = context;
        }

        public IElement CreateElement(IElementOwner owner, ElementMetadata elementMetadata)
        {
            if (owner == null) throw new ArgumentNullException(nameof(owner));
            if (elementMetadata == null) throw new ArgumentNullException(nameof(elementMetadata));

            //Find the factory
            var factory = _factoryManager.GetFactory(elementMetadata.ElementTypeId);

            //Create the context
            var context = new ElementCreationContext(owner, elementMetadata.Data);

            //Create the element
            var element = factory.Create(context);

            SetElementProperties(element, elementMetadata);

            //Blocks
            if (elementMetadata.Blocks != null)
            {
                foreach (var blockMetadata in elementMetadata.Blocks)
                {
                    var block = element.Blocks.FirstOrDefault(b => b.Id == blockMetadata.Id);

                    if (block == null)
                    {
                        Console.WriteLine("Whoa - didn't find a block");
                    }
                    else
                    {
                        SetElementProperties(block, blockMetadata);
                    }
                }
            }

            return element;
        }

        private void SetElementProperties(IElement element, ElementMetadataBase elementMetadata)
        {
            if (element == null) throw new ArgumentNullException(nameof(element));
            if (elementMetadata == null) throw new ArgumentNullException(nameof(elementMetadata));

            //Parameters
            if (elementMetadata.Parameters != null)
            {
                //Deal with each parameter
                foreach (var parameterMetadata in elementMetadata.Parameters)
                {
                    //Find the parameter in question
                    var parameter = element.Parameters.FirstOrDefault(p => p.Id == parameterMetadata.Id);

                    if (parameter == null)
                    {
                        Console.WriteLine($"Unable to find parameter {parameterMetadata.Id}");
                    }
                    else
                    {
                        //Set the value of the printer
                        parameter.SetValue(parameterMetadata.Value);

                        if (parameterMetadata.Operator != null)
                        {
                            ((IElement)parameter).CommonDrop(CreateElement(element.Owner, parameterMetadata.Operator));
                        }
                    }
                }
            }

            //Handle next
            if (elementMetadata.Next != null)
            {
                var next = CreateElement(element.Owner, elementMetadata.Next);

                element.CommonDrop(next);
            }
        }

        public void AddToOwner(IElementOwner owner, IEnumerable<ElementMetadata> rootElements)
        {
            if (rootElements == null) throw new ArgumentNullException(nameof(rootElements));
            if (owner == null) throw new ArgumentNullException(nameof(owner));

            foreach (var elementMetadata in rootElements)
            {
                var element = CreateElement(owner, elementMetadata);

                element.Location = elementMetadata.Location;

                owner.Add(element);
            }
        }       

        private void AddToOwner(IElementOwner owner,  IEnumerable<VariableMetadata> variableMetadatas)
        {
            if (owner == null) throw new ArgumentNullException(nameof(owner));
            if (variableMetadatas == null) throw new ArgumentNullException(nameof(variableMetadatas));

            foreach (var variableMetadata in variableMetadatas)
            {
                var variableType = owner.GetVplType(variableMetadata.TypeId);

                var variable = new Variable(owner, variableType, variableMetadata.Id)
                {
                    Name = variableMetadata.Name
                };

                owner.AddVariable(variable);
            }
        }

        private void AddToOwner(IFunction owner, IEnumerable<ArgumentMetadata> argumentMetadatas)
        {
            if (owner == null) throw new ArgumentNullException(nameof(owner));
            if (argumentMetadatas == null) throw new ArgumentNullException(nameof(argumentMetadatas));

            foreach (var argumentMetadata in argumentMetadatas)
            {
                if (argumentMetadata.Id == Guid.Empty)
                {
                    argumentMetadata.Id = Guid.NewGuid();
                }

                var argument = new Argument(owner, argumentMetadata);

                owner.AddArgument(argument);
            }
        }

        public void LoadFunction(IFunction function, FunctionMetadata functionMetadata)
        {
            if (function == null) throw new ArgumentNullException(nameof(function));
            if (functionMetadata == null)
                return;

            function.Width = functionMetadata.Width;
            function.Height = functionMetadata.Height;
            function.Name = functionMetadata.Name;
            function.ReturnTypeId = functionMetadata.ReturnTypeId;

            //Add variables
            if (functionMetadata.Variables != null)
            {
                AddToOwner(function, functionMetadata.Variables);
            }

            //Add arguments
            if (functionMetadata.Arguments != null)
            {
                AddToOwner(function, functionMetadata.Arguments);
            }

            //Add return variable
            if (functionMetadata.ReturnTypeId != null)
            {
                //Get the type for the return variable.
                var type = function.GetVplType(functionMetadata.ReturnTypeId.Value);

                //Add the return variable.
                function.AddVariable(new ReturnValueVariable(function, type));
            }

            //Add elements
            if (functionMetadata.Elements != null)
            {
                AddToOwner(function, functionMetadata.Elements);
            }           

            function.MarkClean();
        }

        public IFunction LoadFunction(FunctionMetadata functionMetadata)
        {
            if (functionMetadata == null) throw new ArgumentNullException(nameof(functionMetadata));

            var function = new Function(_context, functionMetadata.Id);

            LoadFunction(function, functionMetadata);

            return function;
        }
    }
}