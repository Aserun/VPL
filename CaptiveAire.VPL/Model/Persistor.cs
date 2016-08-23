using System;
using System.Collections.Generic;
using System.Linq;
using CaptiveAire.VPL.Interfaces;
using CaptiveAire.VPL.Metadata;

namespace CaptiveAire.VPL.Model
{
    internal static class Persistor
    {
        public static ParameterMetadata ToMetadata(this IParameter parameter)
        {
            if (parameter == null) throw new ArgumentNullException(nameof(parameter));

            var parameterMetadata = new ParameterMetadata()
            {
                Value = parameter.GetValue(),
                Operator = parameter.Operator.ToMetadata(),
                Id = parameter.Id
            };

            return parameterMetadata;
        }

        public static ParameterMetadata[] ToMetadata(this IEnumerable<IParameter> parameters)
        {
            if (parameters == null) throw new ArgumentNullException(nameof(parameters));

            return parameters
                .Select(p => p.ToMetadata())
                .ToArray();
        }

        public static ElementMetadata[] ToMetadata(this IEnumerable<IElement> elements)
        {
            if (elements == null) throw new ArgumentNullException(nameof(elements));

            return elements
                .Select(e => e.ToMetadata())
                .ToArray();
        }

        public static BlockMetadata ToMetadata(this IBlock block)
        {
            if (block == null) throw new ArgumentNullException(nameof(block));

            var blockMetadata = new BlockMetadata()
            {
                Id = block.Id,
                Data = block.GetData(),
                Elements = block.Elements.ToMetadata(),
                Parameters = block.Parameters.ToMetadata()
            };

            return blockMetadata;
        }

        public static BlockMetadata[] ToMetadata(this IEnumerable<IBlock> blocks)
        {
            if (blocks == null) throw new ArgumentNullException(nameof(blocks));

            return blocks
                .Select(b => b.ToMetadata())
                .ToArray();
        }

        public static ElementMetadata ToMetadata(this IElement element)
        {
            if (element == null)
                return null;

            var elementMetadata =  new ElementMetadata()
            {
                Data = element.GetData(),
                Parameters = element.Parameters.ToMetadata(),
                Blocks = element.Blocks.ToMetadata(),
                ElementTypeId = element.ElementTypeId
            };

            return elementMetadata;            
        }

        public static ArgumentMetadata ToMetadata(this IArgument argument)
        {
            if (argument == null) throw new ArgumentNullException(nameof(argument));

            return new ArgumentMetadata()
            {
                Id = argument.Id,
                Name = argument.Name,
                TypeId = argument.TypeId
            };
        }

        public static VariableMetadata ToMetadata(this IVariable variable)
        {
            if (variable == null) throw new ArgumentNullException(nameof(variable));

            return new VariableMetadata()
            {
                Name = variable.Name,
                Id = variable.Id,
                TypeId = variable.Type.Id
            };
        }

        public static FunctionMetadata ToMetadata(this Function function)
        {
            if (function == null) throw new ArgumentNullException(nameof(function));

            return new FunctionMetadata()
            {
                Id = function.Id,
                Name = function.Name,
                Variables = function.Variables.Where(v => v.Persist).Select(v => v.ToMetadata()).ToArray(),
                Elements = function.Elements.Select(e => e.ToMetadata()).ToArray(),
                Arguments = function.Arguments.Select(a => a.ToMetadata()).ToArray(),
                ReturnTypeId = function.ReturnTypeId
            };
        }
    }
}