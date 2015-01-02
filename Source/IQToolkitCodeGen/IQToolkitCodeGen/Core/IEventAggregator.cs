using System;

namespace IQToolkitCodeGen.Core {
    public interface IEventAggregator {
        void Publish<T>(T eventData);
        IDisposable Subscribe<T>(Action<T> action);
    }
}