using System;
using System.Linq;
using System.Windows;
using CaptiveAire.VPL.Extensions;
using CaptiveAire.VPL.Factory;
using CaptiveAire.VPL.Interfaces;
using CaptiveAire.VPL.Model;
using CaptiveAire.VPL.Operators;
using CaptiveAire.VPL.Statements;

namespace CaptiveAire.VPL.ViewModel
{
    internal static class ViewModelLocator
    {
        

        private static FunctionViewModel CreateEmptyFunction()
        {
            var context = new VplServiceContext();

            return new FunctionViewModel(context, Guid.NewGuid());
        }

        private static FunctionViewModel CreateFunction(string name)
        {
            var function = CreateEmptyFunction();

            function.Name = name;

            var waitStatement = new WaitStatement(function)
            {
                Location = new Point(100, 100)
            };

            function.Elements.Add(waitStatement);

            AddWait(function, new Point(20, 20));
            AddWait(function, new Point(75, 75));

            AddIfElse(function, new Point(400, 100));

            AddAnd(function, new Point(50, 50));

            AddAdditionOperator(function, new Point(300, 300));

            function.AddVariable(new VariableViewModel(function, function.GetBooleanType(), Guid.NewGuid())
            {
                Name = "Variable 1"
            });

            return function;
        }

        private static IfElseStatement AddIfElse(FunctionViewModel function, Point point)
        {
            var compoundStatement = new IfElseStatement(function)
            {
                Location = new Point(400, 100)
            };

           function.Add(compoundStatement);

            return compoundStatement;
        }

        private static WaitStatement AddWait(FunctionViewModel function, Point point)
        {
            var wait = new WaitStatement(function)
            {
                Location = point
            };

            function.Elements.Add(wait);

            return wait;
        }

        private static OperatorViewModel AddAdditionOperator(FunctionViewModel function, Point location)
        {
            var context = new ElementCreationContext(function, null);

            var op = new BinaryMathOperator(context, BinaryMathOperator.BinaryMathOperatorType.Addition)
            {
                Location = location
            };

            function.Add(op);

            return op;
        }

        public static StatementViewModel Statement
        {
            get
            {
                var function = CreateEmptyFunction();

                return new WaitStatement(function);
            }
        }

        public static IfElseStatement CompoundStatement
        {
            get
            {
                var function = CreateEmptyFunction();

                return AddIfElse(function, new Point());
            }
        }

        public static ParameterViewModel NumericParameter
        {
            get
            {
                var function = CreateEmptyFunction();

                return new ParameterViewModel(function, "seconds", function.GetVplType(VplTypeId.Float))
                {
                    Prefix = "Wait",
                    Postfix = "Seconds"
                };
            }
        }

        public static ParameterViewModel BooleanParameter
        {
            get
            {
                var function = CreateEmptyFunction();

                return new ParameterViewModel(function, "param", function.GetVplType(VplTypeId.Boolean))
                {
                    Prefix = "Prefix",
                    Postfix = "Postfix"
                };
            }
        }

        public static ParametersViewModel Parameters
        {
            get
            {
                var function = CreateEmptyFunction();

                return new ParametersViewModel()
                {
                    new ParameterViewModel(function, "a", function.GetVplType(VplTypeId.Float))
                    {
                        Prefix = "Prefix",
                        Value = 42,
                        Postfix = "Postfix"
                    },
                    new ParameterViewModel(function, "b", function.GetVplType(VplTypeId.Float))
                    {
                        Prefix = "Prefix",
                        Value = 12,
                        Postfix = "Postfix"
                    }
                };
            }
        }

        private static OperatorViewModel AddAnd(FunctionViewModel function, Point location)
        {
            var context = new ElementCreationContext(function, null);

            var andOperator = new BinaryLogicalOperator(context, BinaryLogicalOperator.BinaryLogicalOperatorType.And)
            {
                Location = location
            };

            function.Elements.Add(andOperator);

            return andOperator;
        }

        public static OperatorViewModel NumericOperator
        {
            get
            {
                var function = CreateEmptyFunction();

                return AddAdditionOperator(function, new Point(0, 0));
            }
        }

        public static OperatorViewModel BooleanOperator
        {
            get
            {
                var function = CreateEmptyFunction();

                return AddAnd(function, new Point(0, 0));
            }
        }

        public static ToolsViewModel<IElementFactory> Tools
        {
            get
            {
                var factoryManager = new ElementFactoryManager();

                return new ToolsViewModel<IElementFactory>(factoryManager.Factories);
            }
        }
    }
}