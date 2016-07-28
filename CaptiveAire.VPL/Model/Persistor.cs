using System.Collections.Generic;
using System.Linq;
using CaptiveAire.VPL.Interfaces;
using CaptiveAire.VPL.Metadata;
using CaptiveAire.VPL.ViewModel;

namespace CaptiveAire.VPL.Model
{
    internal static class Persistor
    {
        public static ParameterMetadata ToMetadata(this IParameter parameter)
        {
            var parameterMetadata = new ParameterMetadata()
            {
                Value = parameter.GetValue(),
                Operator = parameter.GetNext().ToMetadata(),
                Id = parameter.Id
            };

            return parameterMetadata;
        }

        public static ParameterMetadata[] ToMetadata(this IEnumerable<IParameter> parameters)
        {
            return parameters
                .Select(p => p.ToMetadata())
                .ToArray();
        }

        public static BlockMetadata ToMetadata(this IBlock block)
        {
            var blockMetadata = new BlockMetadata()
            {
                Id = block.Id,
                Data = block.GetData(),
                Next = block.GetNext().ToMetadata(),
                Parameters = block.Parameters.ToMetadata()
            };

            return blockMetadata;
        }

        public static BlockMetadata[] ToMetadata(this IEnumerable<IBlock> blocks)
        {
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
                Location = element.Location,
                Data = element.GetData(),
                Parameters = element.Parameters.ToMetadata(),
                Blocks = element.Blocks.ToMetadata(),
                ElementTypeId = element.ElementTypeId
            };

            var next = element.GetNext();

            if (next != null)
            {
                elementMetadata.Next = next.ToMetadata();
            }

            return elementMetadata;            
        }

        public static ArgumentMetadata ToMetadata(this IArgument argument)
        {
            return new ArgumentMetadata()
            {
                Name = argument.Name,
                TypeId = argument.TypeId
            };
        }

        public static VariableMetadata ToMetadata(this IVariable variable)
        {
            return new VariableMetadata()
            {
                Name = variable.Name,
                Id = variable.Id,
                TypeId = variable.Type.Id
            };
        }

        public static FunctionMetadata ToMetadata(this FunctionViewModel function)
        {
            return new FunctionMetadata()
            {
                Id = function.Id,
                Name = function.Name,
                Width = function.Width,
                Height = function.Height,
                Variables = function.Variables.Select(v => v.ToMetadata()).ToArray(),
                Elements = function.Elements.Select(e => e.ToMetadata()).ToArray(),
                Arguments = function.Arguments.Select(a => a.ToMetadata()).ToArray(),
                ReturnTypeId = function.ReturnTypeId
            };
        }

      
    }
}