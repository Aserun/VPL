using CaptiveAire.VPL.Factory;
using CaptiveAire.VPL.Interfaces;
using CaptiveAire.VPL.Model;
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
                new ElementFactory(SystemElementIds.BinaryLogicOperator, CategoryNames.Logic, "And", context => new BinaryLogicalOperator(context, BinaryLogicalOperator.BinaryLogicalOperatorType.And), typeof(BinaryLogicalOperator), VplTypeId.Boolean),
                new ElementFactory(SystemElementIds.BinaryLogicOperator, CategoryNames.Logic, "Or", context => new BinaryLogicalOperator(context, BinaryLogicalOperator.BinaryLogicalOperatorType.Or), typeof(BinaryLogicalOperator), VplTypeId.Boolean),
                new ElementFactory(SystemElementIds.NotOperator, CategoryNames.Logic, "Not", context => new NotOperator(context.Owner), typeof(NotOperator), VplTypeId.Boolean)
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
                new ElementFactory(SystemElementIds.ComparisonOperator, CategoryNames.Comparison, "<", context => new ComparisonOperator(context, ComparisonOperator.ComparisonOperatorType.LessThan), typeof(ComparisonOperator), VplTypeId.Boolean),
                new ElementFactory(SystemElementIds.ComparisonOperator, CategoryNames.Comparison, "<=", context => new ComparisonOperator(context, ComparisonOperator.ComparisonOperatorType.LessThanOrEqual), typeof(ComparisonOperator), VplTypeId.Boolean),
                new ElementFactory(SystemElementIds.ComparisonOperator, CategoryNames.Comparison, "=", context => new ComparisonOperator(context, ComparisonOperator.ComparisonOperatorType.Equal), typeof(ComparisonOperator), VplTypeId.Boolean),
                new ElementFactory(SystemElementIds.ComparisonOperator, CategoryNames.Comparison, "<>", context => new ComparisonOperator(context, ComparisonOperator.ComparisonOperatorType.NotEqual), typeof(ComparisonOperator), VplTypeId.Boolean),
                new ElementFactory(SystemElementIds.ComparisonOperator, CategoryNames.Comparison, ">", context => new ComparisonOperator(context, ComparisonOperator.ComparisonOperatorType.GreaterThan), typeof(ComparisonOperator), VplTypeId.Boolean),
                new ElementFactory(SystemElementIds.ComparisonOperator, CategoryNames.Comparison, ">=", context => new ComparisonOperator(context, ComparisonOperator.ComparisonOperatorType.GreaterThanOrEqual), typeof(ComparisonOperator), VplTypeId.Boolean),
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
                new ElementFactory(SystemElementIds.Wait, CategoryNames.Control, "Wait", context => new WaitStatement(context.Owner), typeof(WaitStatement)),
                new ElementFactory(SystemElementIds.IfElse, CategoryNames.Control, "If / Else", context => new IfElseStatement(context), typeof(IfElseStatement)),
                new ElementFactory(SystemElementIds.Repeat, CategoryNames.Control, "Repeat", context => new RepeatStatement(context.Owner), typeof(RepeatStatement)),
                new ElementFactory(SystemElementIds.While, CategoryNames.Control, "While", context => new WhileStatement(context.Owner), typeof(WhileStatement)),
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
                new ElementFactory(SystemElementIds.BinaryMathOperator, CategoryNames.Math, "+", context => new BinaryMathOperator(context, BinaryMathOperator.BinaryMathOperatorType.Addition), typeof(BinaryMathOperator), VplTypeId.Float),
                new ElementFactory(SystemElementIds.BinaryMathOperator, CategoryNames.Math, "-", context => new BinaryMathOperator(context, BinaryMathOperator.BinaryMathOperatorType.Subtraction), typeof(BinaryMathOperator), VplTypeId.Float),
                new ElementFactory(SystemElementIds.BinaryMathOperator, CategoryNames.Math, "*", context => new BinaryMathOperator(context, BinaryMathOperator.BinaryMathOperatorType.Multiplication), typeof(BinaryMathOperator), VplTypeId.Float),
                new ElementFactory(SystemElementIds.BinaryMathOperator, CategoryNames.Math, "/", context => new BinaryMathOperator(context, BinaryMathOperator.BinaryMathOperatorType.Division), typeof(BinaryMathOperator), VplTypeId.Float)
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
                new ElementFactory(SystemElementIds.CallFunction, CategoryNames.Control, "Call", context => new CallFunctionStatement(context), typeof(CallFunctionStatement)),
                new ElementFactory(SystemElementIds.EvaluateFunction, CategoryNames.Control, "Evaluate", context => new EvaluateFunctionOperator(context), typeof(EvaluateFunctionOperator), VplTypeId.Any),
            };

            return new VplPlugin("Functions", factories);
        }

        /// <summary>
        /// Creates the annotation plugin.
        /// </summary>
        /// <returns></returns>
        public static IVplPlugin CreateAnnotationPlugin()
        {
            var factories = new IElementFactory[]
            {
                new ElementFactory(SystemElementIds.Comment, CategoryNames.Annotation, "Comment", context => new CommentStatement(context), typeof(CommentStatement)),
            };

            return new VplPlugin("Annotations", factories);
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