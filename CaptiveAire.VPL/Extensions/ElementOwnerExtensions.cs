using System;
using System.Collections.Generic;
using System.Linq;
using CaptiveAire.VPL.Interfaces;
using CaptiveAire.VPL.Model;

namespace CaptiveAire.VPL.Extensions
{
    public static class ElementOwnerExtensions
    {
        public static IVplType GetVplType(this IElementOwner owner, Guid vplTypeId)
        {
            return owner.Context.Types.First(t => t.Id == vplTypeId);
        }

        public static IVplType GetBooleanType(this IElementOwner owner)
        {
            return owner.GetVplType(VplTypeId.Boolean);
        }

        public static IVplType GetFloatType(this IElementOwner owner)
        {
            return owner.GetVplType(VplTypeId.Float);
        }

        public static IVplType GetAnyType(this IElementOwner owner)
        {
            return owner.GetVplType(VplTypeId.Any);
        }

        public static IVplType GetStringType(this IElementOwner owner)
        {
            return owner.GetVplType(VplTypeId.String);
        }

        public static IVariable GetVariable(this IElementOwner owner, Guid variableId)
        {
            return owner.Variables.FirstOrDefault(v => v.Id == variableId);
        }

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

        public static GetEntrancePointResult GetEntrancePoint(this IElementOwner owner)
        {
            var rootStatements = owner.GetRootElements()
                .OfType<IStatement>()
                .ToArray();

            if (rootStatements.Length == 0)
            {
                return new GetEntrancePointResult("Unable to find an executable statement.");
            }

            if (rootStatements.Length > 1)
            {
                return new GetEntrancePointResult("There is more than one entrance point.");
            }

            //We have found the entrance point.
            return new GetEntrancePointResult(rootStatements[0]);           
        }
    }
}