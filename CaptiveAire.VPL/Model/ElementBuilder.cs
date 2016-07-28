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
    public class ElementBuilder
    {
        private readonly IElementFactoryManager _factoryManager;
        private readonly IElementOwner _owner;
        
        public ElementBuilder(IElementFactoryManager factoryManager, IElementOwner owner)
        {
            if (factoryManager == null) throw new ArgumentNullException(nameof(factoryManager));
            if (owner == null) throw new ArgumentNullException(nameof(owner));

            _factoryManager = factoryManager;
            _owner = owner;
        }

        public IElement CreateElement(ElementMetadata elementMetadata)
        {
            var factory = _factoryManager.GetFactory(elementMetadata.ElementTypeId);

            var context = new ElementCreationContext(_owner, elementMetadata.Data);

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
                            ((IElement)parameter).CommonDrop(CreateElement(parameterMetadata.Operator));
                        }
                    }
                }
            }

            //Handle next
            if (elementMetadata.Next != null)
            {
                var next = CreateElement(elementMetadata.Next);

                element.CommonDrop(next);
            }
        }

        public void AddToOwner(IEnumerable<ElementMetadata> rootElements)
        {
            foreach (var elementMetadata in rootElements)
            {
                var element = CreateElement(elementMetadata);

                element.Location = elementMetadata.Location;

                _owner.Add(element);
            }
        }

        private void AddToOwner(IElementOwner owner,  IEnumerable<VariableMetadata> variableMetadatas)
        {
            foreach (var variableMetadata in variableMetadatas)
            {
                var variableType = owner.GetVplType(variableMetadata.TypeId);

                var variable = new VariableViewModel(owner, variableType, variableMetadata.Id)
                {
                    Name = variableMetadata.Name
                };

                owner.AddVariable(variable);
            }
        }

        private void AddToOwner(IFunction owner, IEnumerable<ArgumentMetadata> argumentMetadatas)
        {
            foreach (var argumentMetadata in argumentMetadatas)
            {
                var argument = new ArgumentViewModel(argumentMetadata);

                owner.AddArgument(argument);
            }
        }

        public void LoadFunction(IFunction function, FunctionMetadata functionMetadata)
        {
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

            //Add elements
            if (functionMetadata.Elements != null)
            {
                AddToOwner(functionMetadata.Elements);
            }

            //Add arguments
            if (functionMetadata.Arguments != null)
            {
                 AddToOwner(function, functionMetadata.Arguments);
            }

            function.MarkClean();
        }
    }
}