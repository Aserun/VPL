using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using CaptiveAire.VPL.Extensions;
using CaptiveAire.VPL.Interfaces;
using CaptiveAire.VPL.Metadata;
using CaptiveAire.VPL.Model;
using CaptiveAire.VPL.View;
using CaptiveAire.VPL.ViewModel;
using Cas.Common.WPF;
using GalaSoft.MvvmLight;

namespace CaptiveAire.VPL
{
    internal class Function : ViewModelBase, IFunction
    {
        private readonly IVplServiceContext _context;
        private readonly Guid _functionId;
        private string _name;
        private double _width = 1000;
        private double _height = 1000;
        private readonly ObservableCollection<IElement> _elements = new ObservableCollection<IElement>();
        private readonly ObservableCollection<IVariable> _variables = new ObservableCollection<IVariable>();
        private readonly OrderedListViewModel<IArgument> _arguments;
        private Guid? _returnTypeId;
        private bool _isDirty;

        public Function(IVplServiceContext context, Guid functionId)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            _context = context;
            _functionId = functionId;

            _arguments = new OrderedListViewModel<IArgument>(
                CreateArgument,
                deletedAction: DeleteArgument,
                addedAction: ArgumentAdded);
        }

        /// <summary>
        /// Returns a new argument if one was created, false otherwise.
        /// </summary>
        /// <returns></returns>
        private IArgument CreateArgument()
        {
            var metadata = new ArgumentMetadata()
            {
                Id = Guid.NewGuid(),
            };

            //Create the view model
            var viewModel = new ArgumentDialogViewModel(metadata, _context.Types);

            //Create the dialog
            var view = new ArgumentDialogView()
            {
                DataContext = viewModel,
                Owner = WindowUtil.GetActiveWindow()
            };

            //Show the dialog
            if (view.ShowDialog() == true)
            {
                //Create the argument
                return new Argument(this, viewModel.ToMetadata());               
            }

            return null;
        }

        private void ArgumentAdded(IArgument argument)
        {
            if (argument == null)
                return;

            //Create the variable for this argument.
            var variable = new ArgumentVariable(this, this.GetVplTypeOrThrow(argument.TypeId), argument)
            {
                Name = argument.Name
            };

            //Add the variable.
            AddVariable(variable);

            //I feel so dirty now.
            MarkDirty();
        }

        public void AddArgument(IArgument argument)
        {
            if (argument == null) throw new ArgumentNullException(nameof(argument));
            if (argument.Id == Guid.Empty)
                throw new ArgumentException($"The id of argument '{argument.Name}' was empty.");

            //Add the argument
            _arguments.Add(argument);

            //I feel so dirty now.
            MarkDirty();
        }

        private void DeleteArgument(IArgument argument)
        {
            if (argument == null) throw new ArgumentNullException(nameof(argument));

            //Delete the corresponding variable
            var variable = Variables.FirstOrDefault(v => v.Id == argument.Id);

            if (variable != null && variable.CanDelete())
            {
                variable.Delete();
            }

            Arguments.Remove(argument);
            MarkDirty();
        }

        public bool IsDirty
        {
            get { return _isDirty; }
            private set
            {
                if (_isDirty != value)
                {
                    _isDirty = value;
                    RaisePropertyChanged();
                    CommandManager.InvalidateRequerySuggested();
                }
            }
        }

        public IEnumerable<IArgument> GetArguments()
        {
            return _arguments.ToArray();
        }

        public void MarkDirty()
        {
            IsDirty = true;
        }

        public void MarkClean()
        {
            IsDirty = false;
        }

        private IElement CreateElement(IElementFactory factory)
        {
            if (factory == null) throw new ArgumentNullException(nameof(factory));

            var context = new ElementCreationContext(this, null, factory);

            return factory.Create(context);
        }

        public void DropFromToolbox(IElementFactory factory, Point point)
        {
            var element = CreateElement(factory);

            element.Location = point;

            Elements.Add(element);

            MarkDirty();
        }

        public void DropFromToolbox(IElementFactory factory, IElementDropTarget dropTarget)
        {
            var element = CreateElement(factory);

            dropTarget.Drop(element);

            MarkDirty();
        }

        public void Add(IElement element)
        {
            if (element != null && !_elements.Contains(element))
            {
                _elements.Add(element);

                MarkDirty();
            }
        }

        public void Remove(IElement element)
        {
            if (element != null)
            {
                Elements.Remove(element);

                MarkDirty();
            }
        }

        public IEnumerable<IVariable> Variables
        {
            get { return _variables; }
        }

        public void AddVariable(IVariable variable)
        {
            _variables.Add(variable);

            MarkDirty();
        }

        public void RemoveVariable(IVariable variable)
        {
            _variables.Remove(variable);

            MarkDirty();
        }

        public ObservableCollection<IElement> Elements
        {
            get { return _elements; }
        }

        public Guid Id
        {
            get { return _functionId; }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value; 
                RaisePropertyChanged();
                MarkDirty();
            }
        }

        public double Width
        {
            get { return _width; }
            set
            {
                _width = value; 
                RaisePropertyChanged();
                MarkDirty();
            }
        }

        public double Height
        {
            get { return _height; }
            set
            {
                _height = value; 
                RaisePropertyChanged();
                MarkDirty();
            }
        }

        public Guid? ReturnTypeId
        {
            get { return _returnTypeId; }
            set
            {
                _returnTypeId = value; 
                RaisePropertyChanged();
                MarkDirty();
            }
        }

        public async Task<object> ExecuteAsync(object[] parameters, CancellationToken cancellationToken)
        {
            if (parameters == null) throw new ArgumentNullException(nameof(parameters));

            cancellationToken.ThrowIfCancellationRequested();

            //Make sure that the # of parameters match.
            if (parameters.Length != Arguments.Count)
            {
                throw new ArgumentMismatchException($"The function '{Name}' contains {Arguments.Count} arguments but {parameters.Length} were specified.");
            }

            int argumentIndex = 0;

            //Copy the parameter values to the variable values
            foreach (var argument in Arguments)
            {
                //Look for the variable with the same id.
                var variable = Variables.FirstOrDefault(v => v.Id == argument.Id);

                //Check to make sure we found it.
                if (variable == null)
                {
                    throw new ArgumentException($"Argument variable missing for argument '{argument.Name}' in function '{Name}'.");                   
                }

                //Copy the value
                variable.Value = parameters[argumentIndex];

                argumentIndex++;
            }

            //Look for the entrance point.
            var entrancePoint = this.GetEntrancePoint();

            //Check to see if if found an entrance point.
            if (entrancePoint.Statement == null)
            {
                throw new EntrancePointNotFoundException($"Unable to find an entrance point in function '{Name}': {entrancePoint.Error}");
            }

            //Clear all of the errors
            entrancePoint.Statement.ForAll(e =>
            {
                var errorSource = e as IErrorSource;

                errorSource?.ClearErrors();
            });

            //Create the executor.
            var executor = new StatementExecutor();

            //Execute the function.
            await executor.ExecuteAsync(entrancePoint.Statement, cancellationToken);

            //Find the return variable
            var returnVariable = Variables.FirstOrDefault(v => v.Id == ReturnValueVariable.ReturnVariableId);

            //return the returnVariable value (or null)
            return returnVariable?.Value;
        }

        public void ClearErrors()
        {
            //Do this for all of the root element chains.
            foreach (var rootElement in Elements)
            {
                //Clear anything that implments IErrorSource.
                rootElement.ForAll(e => (e as IErrorSource)?.ClearErrors());
            }
        }

        public void SetError(string message)
        {
        }

        public IError[] CheckForErrors()
        {
            //Attempt to get the entrance point.
            var entrancePoint = this.GetEntrancePoint();

            //Create a place to put the errors
            var errors = new List<IError>();

            //Check to find out if we found an executable statement.
            if (entrancePoint.Statement == null)
            {
                //Add the error.
                errors.Add(new Error(this, $"Unable to find an entrance point in function '{Name}': {entrancePoint.Error}", ErrorLevel.Error));
            }
            else
            {
                //Now check through the statements for errors:
                entrancePoint.Statement.ForAll(e =>
                {
                    //Check to see if it's an error source.
                    var errorSource = e as IErrorSource;

                    //Check for errors
                    var childErrors = errorSource?.CheckForErrors();

                    //Check to see if there were any errors.
                    if (childErrors != null && childErrors.Length > 0)
                    {
                        //Add the errors to the list.
                        errors.AddRange(childErrors);
                    }
                });
            }

            return errors.ToArray();
        }

        public bool HasError
        {
            get { return false; }
        }

        public IVplServiceContext Context
        {
            get { return _context; }
        }

        public OrderedListViewModel<IArgument> Arguments
        {
            get { return _arguments; }
        }

        private IEnumerable<IElement> EnumerateElements(IElement element)
        {
            var current = element;

            while (current != null)
            {
                foreach (var parameter in current.Parameters)
                {
                    if (parameter.Next != null)
                    {
                        foreach (var parameterElement in EnumerateElements(parameter.Next))
                        {
                            yield return parameterElement;
                        }
                    }
                }

                foreach (var block in current.Blocks)
                {
                    if (block.Next != null)
                    {
                        foreach (var blockElement in EnumerateElements(block.Next))
                        {
                            yield return blockElement;
                        }
                    }
                }

                yield return current;

                current = current.Next;
            }
        }

        public IEnumerable<IElement> GetAllElements()
        {
            return Elements.SelectMany(EnumerateElements);
        }

        public IEnumerable<IElement> GetRootElements()
        {
            return Elements
                .ToArray();
        }

       
    }
}