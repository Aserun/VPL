using System.Windows;
using CaptiveAire.VPL.Factory;
using CaptiveAire.VPL.Interfaces;
using CaptiveAire.VPL.Model;
using CaptiveAire.VPL.Plugins.Annotations;
using CaptiveAire.VPL.Plugins.Functions;
using CaptiveAire.VPL.Plugins.Operators;
using CaptiveAire.VPL.Plugins.Statements;

namespace CaptiveAire.VPL.Plugins
{
    /// <summary>
    /// Factory for system plugins.
    /// </summary>
    public static class SystemPluginFactory
    {
        /// <summary>
        /// Creates the logical plugin.
        /// </summary>
        /// <returns></returns>
        public static IVplPlugin CreateLogicalPlugin()
        {
            var factories = new IElementFactory[]
            {
                new ElementFactory(PluginElementIds.BinaryLogicOperator, CategoryNames.Logic, "And", context => new BinaryLogicalOperator(context, BinaryLogicalOperator.BinaryLogicalOperatorType.And), typeof(BinaryLogicalOperator), VplTypeId.Boolean),
                new ElementFactory(PluginElementIds.BinaryLogicOperator, CategoryNames.Logic, "Or", context => new BinaryLogicalOperator(context, BinaryLogicalOperator.BinaryLogicalOperatorType.Or), typeof(BinaryLogicalOperator), VplTypeId.Boolean),
                new ElementFactory(PluginElementIds.NotOperator, CategoryNames.Logic, "Not", context => new NotOperator(context.Owner), typeof(NotOperator), VplTypeId.Boolean)
            };

            return new VplPlugin("Logical", factories);
        }

        /// <summary>
        /// Creates the comparison plugin.
        /// </summary>
        /// <returns></returns>
        public static IVplPlugin CreateComparisonPlugin()
        {
            var factories = new IElementFactory[]
            {
                new ElementFactory(PluginElementIds.ComparisonOperator, CategoryNames.Comparison, "<", context => new ComparisonOperator(context, ComparisonOperator.ComparisonOperatorType.LessThan), typeof(ComparisonOperator), VplTypeId.Boolean),
                new ElementFactory(PluginElementIds.ComparisonOperator, CategoryNames.Comparison, "<=", context => new ComparisonOperator(context, ComparisonOperator.ComparisonOperatorType.LessThanOrEqual), typeof(ComparisonOperator), VplTypeId.Boolean),
                new ElementFactory(PluginElementIds.ComparisonOperator, CategoryNames.Comparison, "=", context => new ComparisonOperator(context, ComparisonOperator.ComparisonOperatorType.Equal), typeof(ComparisonOperator), VplTypeId.Boolean),
                new ElementFactory(PluginElementIds.ComparisonOperator, CategoryNames.Comparison, "<>", context => new ComparisonOperator(context, ComparisonOperator.ComparisonOperatorType.NotEqual), typeof(ComparisonOperator), VplTypeId.Boolean),
                new ElementFactory(PluginElementIds.ComparisonOperator, CategoryNames.Comparison, ">", context => new ComparisonOperator(context, ComparisonOperator.ComparisonOperatorType.GreaterThan), typeof(ComparisonOperator), VplTypeId.Boolean),
                new ElementFactory(PluginElementIds.ComparisonOperator, CategoryNames.Comparison, ">=", context => new ComparisonOperator(context, ComparisonOperator.ComparisonOperatorType.GreaterThanOrEqual), typeof(ComparisonOperator), VplTypeId.Boolean),
            };

            return new VplPlugin("Comparison", factories);
        }

        /// <summary>
        /// Create the control plugin (has elements such as "Wait", "If /Else"
        /// </summary>
        /// <returns></returns>
        public static IVplPlugin CreateControlPlugin()
        {
            var factories = new IElementFactory[]
            {
                new ElementFactory(PluginElementIds.Wait, CategoryNames.Control, "Wait", context => new WaitStatement(context.Owner), typeof(WaitStatement)),
                new ElementFactory(PluginElementIds.IfElse, CategoryNames.Control, "If / Else", context => new IfElseStatement(context), typeof(IfElseStatement)),
                new ElementFactory(PluginElementIds.Repeat, CategoryNames.Control, "Repeat", context => new RepeatStatement(context.Owner), typeof(RepeatStatement)),
                new ElementFactory(PluginElementIds.While, CategoryNames.Control, "While", context => new WhileStatement(context.Owner), typeof(WhileStatement)),
            };

            return new VplPlugin("Comparison", factories);
        }

        /// <summary>
        /// Create the math plugin.
        /// </summary>
        /// <returns></returns>
        public static IVplPlugin CreateMathPlugin()
        {
            var factories = new IElementFactory[]
            {
                new ElementFactory(PluginElementIds.BinaryMathOperator, CategoryNames.Math, "+", context => new BinaryMathOperator(context, BinaryMathOperator.BinaryMathOperatorType.Addition), typeof(BinaryMathOperator), VplTypeId.Float),
                new ElementFactory(PluginElementIds.BinaryMathOperator, CategoryNames.Math, "-", context => new BinaryMathOperator(context, BinaryMathOperator.BinaryMathOperatorType.Subtraction), typeof(BinaryMathOperator), VplTypeId.Float),
                new ElementFactory(PluginElementIds.BinaryMathOperator, CategoryNames.Math, "*", context => new BinaryMathOperator(context, BinaryMathOperator.BinaryMathOperatorType.Multiplication), typeof(BinaryMathOperator), VplTypeId.Float),
                new ElementFactory(PluginElementIds.BinaryMathOperator, CategoryNames.Math, "/", context => new BinaryMathOperator(context, BinaryMathOperator.BinaryMathOperatorType.Division), typeof(BinaryMathOperator), VplTypeId.Float)
            };

            return new VplPlugin("Math", factories);
        }

        /// <summary>
        /// Provides support for calling functions.
        /// </summary>
        /// <returns></returns>
        public static IVplPlugin CreateFunctionsPlugin()
        {
            var factories = new IElementFactory[]
            {
                new ElementFactory(PluginElementIds.CallFunction, CategoryNames.Control, "Call", context => new CallFunctionStatement(context), typeof(CallFunctionStatement), description: "Calls a function and ignores any return value."),
                new ElementFactory(PluginElementIds.EvaluateFunction, CategoryNames.Control, "Evaluate", context => new EvaluateFunctionOperator(context), typeof(EvaluateFunctionOperator), VplTypeId.Any, description: "Calls a function and returns its value."),
            };

            return new VplPlugin("Functions", factories);
        }

        /// <summary>
        /// Creates the annotation plugin.
        /// </summary>
        /// <returns></returns>
        public static IVplPlugin CreateAnnotationPlugin()
        {
            var resources = new ResourceDictionary[]
            {
                new AnnotationResources()
            };

            var factories = new IElementFactory[]
            {
                new ElementFactory(PluginElementIds.Comment, CategoryNames.Annotation, "Comment", context => new CommentStatement(context), typeof(CommentStatement), description: "An inline comment."),
                new ElementFactory(PluginElementIds.Annotation, CategoryNames.Annotation, "Annotation", context => new Annotation(context), typeof(Annotation), description: "A free floating comment."), 
            };

            return new VplPlugin("Annotations", factories, resources);
        }

        /// <summary>
        /// Creates all System Plugins.
        /// </summary>
        /// <returns></returns>
        public static IVplPlugin[] CreateAllPlugins()
        {
            return new []
            {
                CreateLogicalPlugin(),
                CreateComparisonPlugin(),
                CreateControlPlugin(),
                CreateMathPlugin(),
                CreateFunctionsPlugin(),
                CreateAnnotationPlugin()
            };
        }

    }
}