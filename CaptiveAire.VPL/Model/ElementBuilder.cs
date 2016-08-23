using System;
using System.Collections.Generic;
using System.Linq;
using CaptiveAire.VPL.Extensions;
using CaptiveAire.VPL.Interfaces;
using CaptiveAire.VPL.Metadata;

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
            var context = new ElementCreationContext(owner, elementMetadata.Data, factory);

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

                        if (blockMetadata.Elements != null)
                        {
                            //Add the block children
                            block.Elements.AddRange(blockMetadata.Elements.Select(e => CreateElement(owner, e)).ToArray());
                        }
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
                        //Set the value of the parameter
                        parameter.SetValue(parameterMetadata.Value);

                        if (parameterMetadata.Operator != null)
                        {
                            parameter.Operator = (IOperator)(CreateElement(element.Owner, parameterMetadata.Operator)) ;
                        }
                    }
                }
            }
        }

        public void AddToOwner(IElementOwner owner, IEnumerable<ElementMetadata> rootElements)
        {
            if (rootElements == null) throw new ArgumentNullException(nameof(rootElements));
            if (owner == null) throw new ArgumentNullException(nameof(owner));

            foreach (var elementMetadata in rootElements)
            {
                var element = CreateElement(owner, elementMetadata);

                owner.Add(element);
            }
        }       

        private void AddToOwner(IElementOwner owner,  IEnumerable<VariableMetadata> variableMetadatas)
        {
            if (owner == null) throw new ArgumentNullException(nameof(owner));
            if (variableMetadatas == null) throw new ArgumentNullException(nameof(variableMetadatas));

            foreach (var variableMetadata in variableMetadatas)
            {
                var variableType = owner.GetVplTypeOrThrow(variableMetadata.TypeId);

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

#pragma warning disable CS0618
        private static IEnumerable<ElementMetadata> EnumerateElementsWhileFixingBlocks(ElementMetadata first)
        {
            var current = first;

            while (current != null)
            {
                if (current.Blocks != null)
                {
                    foreach (var block in current.Blocks)
                    {
                        FixBlock(block);
                    }
                }

                yield return current;

                current = current.Next;
            }
        }

        private static void FixBlock(BlockMetadata blockMetadata)
        {
            if (blockMetadata == null) throw new ArgumentNullException(nameof(blockMetadata));

            if (blockMetadata.Next != null)
            {
                //Convert to a list
                blockMetadata.Elements = EnumerateElementsWhileFixingBlocks(blockMetadata.Next)
                    .ToArray();

                //Ditch the next pointer
                blockMetadata.Next = null;
            }
        }

        private bool RequiresConversion(ElementMetadata elementMetadata)
        {
            if (elementMetadata == null) throw new ArgumentNullException(nameof(elementMetadata));

            if (elementMetadata.Next != null)
                return true;

            if (elementMetadata.Blocks != null)
            {
                if (elementMetadata.Blocks.Any(b => b.Next != null))
                {
                    return true;
                }
            }

            return false;
        }

        private void FixFunctionMetadata(FunctionMetadata functionMetadata)
        {
            if (functionMetadata == null) throw new ArgumentNullException(nameof(functionMetadata));

            if (functionMetadata.Elements != null)
            {
                //Check to see if need to convert
                if (functionMetadata.Elements.Any(RequiresConversion))
                {
                    //This is where we'll put all of the elements.
                    var allElements = new List<ElementMetadata>();

                    //Consider each of the root elements
                    foreach (var rootElement in functionMetadata.Elements)
                    {
                        //Flatten the element chain
                        var elements = EnumerateElementsWhileFixingBlocks(rootElement);

                        //Add this to the list of all elements
                        allElements.AddRange(elements);                        
                    }

                    //Set the next element to null
                    allElements.ForEach(e => e.Next = null);

                    functionMetadata.Elements = allElements
                        .ToArray();
                }               
            }
        }
#pragma warning restore CS0618

        public void LoadFunction(IFunction function, FunctionMetadata functionMetadata)
        {
            if (function == null) throw new ArgumentNullException(nameof(function));
            if (functionMetadata == null)
                return;

            //Fix the bloody metadata
            FixFunctionMetadata(functionMetadata);

            //Now get the simple stuff
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
                var type = function.GetVplTypeOrThrow(functionMetadata.ReturnTypeId.Value);

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