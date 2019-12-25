using EdaSample.Common.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EdaSample.Common.Sagas.States
{
    public class SagaState<TSagaData>
        where TSagaData : ISagaData
    {
        private readonly List<SagaStateBinding<TSagaData>> bindings = new List<SagaStateBinding<TSagaData>>();
        private readonly Func<SagaReplyEvent, TSagaData, CancellationToken, Task<bool>> executor;

        public SagaState(string name, Func<SagaReplyEvent, TSagaData, CancellationToken, Task<bool>> executor)
        {
            this.executor = executor;
            Name = name;
        }

        public async Task<bool> ExecuteAsync(SagaReplyEvent replyMessage, TSagaData sagaData, CancellationToken cancellationToken = default)
        {
            return await this.executor(replyMessage, sagaData, cancellationToken);
        }

        public void AddBinding(Type replyMessageType, string nextState)
        {
            if (!BindingExists(replyMessageType, nextState))
            {
                bindings.Add(new SagaStateBinding<TSagaData>(nextState, replyMessageType));
            }
        }

        public void ClearBindings() => bindings.Clear();

        public string Name { get; }

        public IEnumerable<SagaStateBinding<TSagaData>> Bindings => bindings;

        public override string ToString() => Name;

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return obj is SagaState<TSagaData> ss && string.Equals(ss.Name, Name);
        }

        public override int GetHashCode() => Name?.GetHashCode() ?? base.GetHashCode();

        private bool BindingExists(Type eventType, string stateName) => bindings.Any(b => b.ReplyMessageType == eventType && b.NextState == stateName);
    }
}
