using System;
using System.Collections.Generic;
using CaptiveAire.VPL.Interfaces;
using CaptiveAire.VPL.Model;
using CaptiveAire.VPL.Operators;
using CaptiveAire.VPL.Statements;
using CaptiveAire.VPL.ViewModel;

namespace CaptiveAire.VPL.Factory
{
    public class ElementFactoryManager : IElementFactoryManager
    {
        private readonly IDictionary<Guid, IElementFactory> _uniqueFactories = new Dictionary<Guid, IElementFactory>();
        private readonly List<IElementFactory> _factories;

        public ElementFactoryManager(IEnumerable<IElementFactory> extensionFactories = null)
        {
            _factories = new List<IElementFactory>(EnumerateFactories(extensionFactories));

            foreach (var factory in _factories)
            {
                if (!_uniqueFactories.ContainsKey(factory.ElementTypeId))
                {
                    _uniqueFactories.Add(factory.ElementTypeId, factory);
                }
            }
        }

        public IEnumerable<IElementFactory> Factories
        {
            get { return _factories; }
        }

        private IEnumerable<IElementFactory> EnumerateFactories(IEnumerable<IElementFactory> extensionFactories)
        {
            yield return new ElementFactory(SystemElementIds.Wait, FactoryCategoryNames.Control, "Wait", context => new WaitStatement(context.Owner), typeof(WaitStatement));

            yield return new ElementFactory(SystemElementIds.IfElse, FactoryCategoryNames.Control, "If / Else", context => new IfElseStatement(context.Owner), typeof(IfElseStatement));

            yield return new ElementFactory(SystemElementIds.BinaryLogicOperator, FactoryCategoryNames.Logic, "And", context => new BinaryLogicalOperator(context, BinaryLogicalOperator.BinaryLogicalOperatorType.And), typeof(BinaryLogicalOperator), VplTypeId.Boolean);

            yield return new ElementFactory(SystemElementIds.BinaryLogicOperator, FactoryCategoryNames.Logic, "Or", context => new BinaryLogicalOperator(context, BinaryLogicalOperator.BinaryLogicalOperatorType.Or), typeof(BinaryLogicalOperator), VplTypeId.Boolean);

            yield return new ElementFactory(SystemElementIds.NotOperator, FactoryCategoryNames.Logic, "Not", context => new NotOperator(context.Owner), typeof(NotOperator), VplTypeId.Boolean);
            
            yield return new ElementFactory(SystemElementIds.BinaryMathOperator, FactoryCategoryNames.Math, "+", context => new BinaryMathOperator(context, BinaryMathOperator.BinaryMathOperatorType.Addition), typeof (BinaryMathOperator), VplTypeId.Float);

            yield return new ElementFactory(SystemElementIds.BinaryMathOperator, FactoryCategoryNames.Math, "-", context => new BinaryMathOperator(context, BinaryMathOperator.BinaryMathOperatorType.Subtraction), typeof(BinaryMathOperator), VplTypeId.Float);

            yield return new ElementFactory(SystemElementIds.BinaryMathOperator, FactoryCategoryNames.Math, "*", context => new BinaryMathOperator(context, BinaryMathOperator.BinaryMathOperatorType.Multiplication), typeof(BinaryMathOperator), VplTypeId.Float);

            yield return new ElementFactory(SystemElementIds.BinaryMathOperator, FactoryCategoryNames.Math, "/", context => new BinaryMathOperator(context, BinaryMathOperator.BinaryMathOperatorType.Division), typeof(BinaryMathOperator), VplTypeId.Float);

            yield return new ElementFactory(SystemElementIds.VariableGetter, FactoryCategoryNames.Variable, "Get", context => new VariableGetterViewModel(context), typeof(VariableGetterViewModel), showInToolbox:false);

            yield return new ElementFactory(SystemElementIds.VariableSetter, FactoryCategoryNames.Variable, "Set", context => new VariableSetterViewModel(context), typeof(VariableSetterViewModel), showInToolbox: false);

            yield return new ElementFactory(SystemElementIds.Repeat, FactoryCategoryNames.Control, "Repeat", context => new RepeatStatement(context.Owner), typeof(RepeatStatement));

            yield return new ElementFactory(SystemElementIds.While, FactoryCategoryNames.Control, "While", context => new WhileStatement(context.Owner), typeof(WhileStatement));

            yield return new ElementFactory(SystemElementIds.CallFunction, FactoryCategoryNames.Control, "Call", context => new CallFunctionStatement(context), typeof(CallFunctionStatement));

            yield return new ElementFactory(SystemElementIds.EvaluateFunction, FactoryCategoryNames.Control, "Evaluate", context => new EvaluateFunctionOperator(context), typeof(EvaluateFunctionOperator), VplTypeId.Any);

            yield return new ElementFactory(SystemElementIds.ComparisonOperator, FactoryCategoryNames.Comparison, "<", context => new ComparisonOperator(context, ComparisonOperator.ComparisonOperatorType.LessThan), typeof(ComparisonOperator), VplTypeId.Boolean);

            yield return new ElementFactory(SystemElementIds.ComparisonOperator, FactoryCategoryNames.Comparison, "<=", context => new ComparisonOperator(context, ComparisonOperator.ComparisonOperatorType.LessThanOrEqual), typeof(ComparisonOperator), VplTypeId.Boolean);

            yield return new ElementFactory(SystemElementIds.ComparisonOperator, FactoryCategoryNames.Comparison, "=", context => new ComparisonOperator(context, ComparisonOperator.ComparisonOperatorType.Equal), typeof(ComparisonOperator), VplTypeId.Boolean);

            yield return new ElementFactory(SystemElementIds.ComparisonOperator, FactoryCategoryNames.Comparison, "<>", context => new ComparisonOperator(context, ComparisonOperator.ComparisonOperatorType.NotEqual), typeof(ComparisonOperator), VplTypeId.Boolean);

            yield return new ElementFactory(SystemElementIds.ComparisonOperator, FactoryCategoryNames.Comparison, ">", context => new ComparisonOperator(context, ComparisonOperator.ComparisonOperatorType.GreaterThan), typeof(ComparisonOperator), VplTypeId.Boolean);

            yield return new ElementFactory(SystemElementIds.ComparisonOperator, FactoryCategoryNames.Comparison, ">=", context => new ComparisonOperator(context, ComparisonOperator.ComparisonOperatorType.GreaterThanOrEqual), typeof(ComparisonOperator), VplTypeId.Boolean);

            yield return new ElementFactory(SystemElementIds.Comment, FactoryCategoryNames.Annotation, "Comment", context => new CommentStatement(context), typeof(CommentStatement));

            //See if we have any extension factories
            if (extensionFactories != null)
            {
                //Enumerate the custom extensions last.
                foreach (var extensionFactory in extensionFactories)
                {
                    yield return extensionFactory;
                }
            }
        }

        public IElementFactory GetFactory(Guid elementTypeId)
        {
            return _uniqueFactories[elementTypeId];
        }
    }
}