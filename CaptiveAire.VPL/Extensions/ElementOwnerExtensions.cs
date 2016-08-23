using System;
using System.Collections.Generic;
using System.Linq;
using CaptiveAire.VPL.Interfaces;
using CaptiveAire.VPL.Metadata;
using CaptiveAire.VPL.Model;

namespace CaptiveAire.VPL.Extensions
{
    /// <summary>
    /// Extensions for the IElementOwner type.
    /// </summary>
    public static class ElementOwnerExtensions
    {
        public static IElement[] CreateElements(this IElementOwner owner, IElementClipboardData data)
        {
            if (owner == null) throw new ArgumentNullException(nameof(owner));
            if (data == null) throw new ArgumentNullException(nameof(data));

            //Create the elements
            return data.Items
                .Select(item =>
                {
                    if (item.ElementMetadata != null)
                    {
                        return owner.Context.ElementBuilder.CreateElement(owner, item.ElementMetadata);
                    }

                    if (item.Factory != null)
                    {
                        return owner.CreateElement(item.Factory);
                    }
                    
                    //This shouldn't happen
                    return null;                   
                })
                .ToArray();
        }
        
        public static IElement CreateElement(this IElementOwner owner, IElementFactory factory)
        {
            if (factory == null) throw new ArgumentNullException(nameof(factory));

            var context = new ElementCreationContext(owner, null, factory);

            return factory.Create(context);
        }

        public static bool AreAllItemsStatements(this IElementOwner owner, IElementClipboardData data)
        {
            if (owner == null) throw new ArgumentNullException(nameof(owner));
            if (data == null) throw new ArgumentNullException(nameof(data));

            return data.Items.Any()
                   && data.Items.All(i =>
                   {
                       IElementFactory factory;

                       if (i.Factory != null)
                       {
                           //First try to use the factory
                           factory = i.Factory;
                       }
                       else if (i.ElementMetadata != null)
                       {
                           //Get the factory for this element type
                           factory = owner.Context.ElementFactoryManager.GetFactory(i.ElementMetadata.ElementTypeId);
                       }
                       else
                       {
                           //This shouldn't happen
                           return false;
                       }

                       //Determine if this is a statement
                       return factory.ElementType.IsStatement();
                   });
        }

        /// <summary>
        /// Gets the specified VplType or throws an exception.
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="vplTypeId"></param>
        /// <returns></returns>
        public static IVplType GetVplTypeOrAny(this IElementOwner owner, Guid vplTypeId)
        {
            return owner.Context.Types.FirstOrDefault(t => t.Id == vplTypeId) ?? owner.GetAnyType();
        }

        /// <summary>
        /// Gets the specified VplType or throws an exception.
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="vplTypeId"></param>
        /// <returns></returns>
        public static IVplType GetVplType(this IElementOwner owner, Guid vplTypeId)
        {
            return owner.Context.Types.FirstOrDefault(t => t.Id == vplTypeId);
        }

        /// <summary>
        /// Gets the specified VplType or throws an exception.
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="vplTypeId"></param>
        /// <returns></returns>
        public static IVplType GetVplTypeOrThrow(this IElementOwner owner, Guid vplTypeId)
        {
            var type = owner.GetVplType(vplTypeId);

            if (type == null)
            {
                throw new TypeAccessException($"Unable to find the VplType with id {vplTypeId}.");
            }

            return type;
        }

        /// <summary>
        /// Gets the Boolean VplType.
        /// </summary>
        /// <param name="owner"></param>
        /// <returns></returns>
        public static IVplType GetBooleanType(this IElementOwner owner)
        {
            return owner.GetVplTypeOrThrow(VplTypeId.Boolean);
        }

        /// <summary>
        /// Gets the Float VplType (.net type is double).
        /// </summary>
        /// <param name="owner"></param>
        /// <returns></returns>
        public static IVplType GetFloatType(this IElementOwner owner)
        {
            return owner.GetVplTypeOrThrow(VplTypeId.Float);
        }

        /// <summary>
        /// Gets the 'Any' type.
        /// </summary>
        /// <param name="owner"></param>
        /// <returns></returns>
        public static IVplType GetAnyType(this IElementOwner owner)
        {
            return owner.GetVplTypeOrThrow(VplTypeId.Any);
        }

        /// <summary>
        /// Gets the string VplType.
        /// </summary>
        /// <param name="owner"></param>
        /// <returns></returns>
        public static IVplType GetStringType(this IElementOwner owner)
        {
            return owner.GetVplTypeOrThrow(VplTypeId.String);
        }

        /// <summary>
        /// Gets a variable. If it is not found, null is returned.
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="variableId">The id of the variable.</param>
        /// <returns></returns>
        public static IVariable GetVariable(this IElementOwner owner, Guid variableId)
        {
            return owner.Variables.FirstOrDefault(v => v.Id == variableId);
        }

        /// <summary>
        /// Attempts to get the variable with the specified id. If the variable isn't found, an exception is thrown.
        /// </summary>
        /// <param name="owner">Owner.</param>
        /// <param name="variableId">The id of the variable.</param>
        /// <returns></returns>
        public static IVariable GetVariableOrThrow(this IElementOwner owner, Guid variableId)
        {
            var variable = owner.GetVariable(variableId);

            if (variable == null)
                throw new KeyNotFoundException($"Unable to find variable with id '{variableId}'");

            return variable;
        }

        /// <summary>
        /// Gets the service of the specified type.
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="owner"></param>
        /// <returns></returns>
        public static TService GetService<TService>(this IElementOwner owner)
        {
            return owner.Context.Services
                .OfType<TService>()
                .FirstOrDefault();
        }

        ///// <summary>
        ///// Gets the entrance point for a function.
        ///// </summary>
        ///// <param name="owner"></param>
        ///// <returns></returns>
        //internal static GetEntrancePointResult GetEntrancePoint(this IElementOwner owner)
        //{
        //    //Get the root statements.
        //    var rootStatements = owner.GetRootElements()
        //        .OfType<IStatement>()
        //        .ToArray();

        //    //Check to see if we have any
        //    if (rootStatements.Length == 0)
        //    {
        //        return new GetEntrancePointResult("Unable to find an executable statement.");
        //    }

        //    //Check to see if we have too many
        //    if (rootStatements.Length > 1)
        //    {
        //        //Set errors so it's more obvious to the user what is happening.
        //        foreach (var rootStatement in rootStatements)
        //        {
        //            rootStatement.SetError("Multiple entrance points.");
        //        }

        //        return new GetEntrancePointResult("There is more than one entrance point.");
        //    }

        //    //We have found the entrance point.
        //    return new GetEntrancePointResult(rootStatements[0]);           
        //}

        /// <summary>
        /// Creates an instance of IElementCreationContext
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <param name="elementTypeId">The type of element.</param>
        /// <param name="data">Optional data.</param>
        /// <returns></returns>
        public static IElementCreationContext CreateElementCreationContext(this IElementOwner owner, Guid elementTypeId, string data = null)
        {
            var factory = owner.Context.ElementFactoryManager.GetFactory(elementTypeId);

            var context = new ElementCreationContext(owner, data, factory);

            return context;
        }

        /// <summary>
        /// Creates a parameter.
        /// </summary>
        /// <param name="owner">Owner.</param>
        /// <param name="id">The id of the parameter.</param>
        /// <param name="type">The type of parameter.</param>
        /// <param name="prefix">Optional prefix</param>
        /// <param name="postfix">Optionl postfix</param>
        /// <returns></returns>
        public static IParameter CreateParameter(this IElementOwner owner, string id, IVplType type, string prefix = null, string postfix = null)
        {
            var context = CreateElementCreationContext(owner, SystemElementIds.Parameter);

            var parameter = new Parameter(context, id, type)
            {
                Prefix = prefix,
                Postfix = postfix
            };

            return parameter;
        }

        /// <summary>
        /// Creates a block.
        /// </summary>
        /// <param name="owner">Owner.</param>
        /// <param name="id">The id of the block.</param>
        /// <param name="label">An optional label.</param>
        /// <returns></returns>
        public static IBlock CreateBlock(this IElementOwner owner, string id, object label = null)
        {
            //Create the context
            var context = CreateElementCreationContext(owner, SystemElementIds.Block);

            //Create the block
            var block = new Block(context, id)
            {
                Label = label
            };

            return block;
        }
    }
}