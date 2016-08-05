using System;
using System.Windows.Media;
using CaptiveAire.VPL.Interfaces;

namespace CaptiveAire.VPL
{
    /// <summary>
    /// Default implementation of the IVplType interface.
    /// </summary>
    public class VplType : IVplType
    {
        private readonly Guid _id;
        private readonly string _name;
        private readonly Func<Visual> _visualFactory;
        private readonly object _defaultValue;
        private readonly Type _netType;

        public VplType(Guid id, string name, Func<Visual> visualFactory, object defaultValue, Type netType)
        {
            if (visualFactory == null) throw new ArgumentNullException(nameof(visualFactory));
            if (netType == null) throw new ArgumentNullException(nameof(netType));

            _id = id;
            _name = name;
            _visualFactory = visualFactory;
            _defaultValue = defaultValue;
            _netType = netType;
        }

        public Guid Id
        {
            get { return _id; }
        }

        public string Name
        {
            get { return _name; }
        }

        public object DefaultValue
        {
            get { return _defaultValue; }
        }

        public Type NetType
        {
            get { return _netType; }
        }

        public Visual CreateVisual()
        {
            return _visualFactory();
        }

        public override bool Equals(object obj)
        {
            var type = obj as IVplType;

            if (type == null)
                return false;

            return type.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}