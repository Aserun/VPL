using System;
using CaptiveAire.VPL.Interfaces;

namespace CaptiveAire.VPL.Extensions
{
    public class GetEntrancePointResult
    {
        private readonly string _error;
        private readonly IStatement _statement;

        public GetEntrancePointResult(IStatement statement)
        {
            if (statement == null) throw new ArgumentNullException(nameof(statement));
            _statement = statement;
        }

        public GetEntrancePointResult(string error)
        {
            if (error == null) throw new ArgumentNullException(nameof(error));
            _error = error;
        }

        public IStatement Statement
        {
            get { return _statement; }
        }

        public string Error
        {
            get { return _error; }
        }
    }
}