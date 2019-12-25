using EdaSample.Common.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EdaSample.Common.Sagas.States
{
    public class SagaStateMachine<TSagaData>
        where TSagaData : ISagaData
    {

        #region Private Fields

        private readonly List<SagaState<TSagaData>> states = new List<SagaState<TSagaData>>();
        private SagaState<TSagaData> initialState;

        #endregion Private Fields

        #region Internal Constructors

        internal SagaStateMachine()
        {

        }

        #endregion Internal Constructors

        #region Public Properties

        public SagaState<TSagaData> CurrentState { get; private set; }

        public IEnumerable<Type> TriggeringEventTypes
            => states.SelectMany(state => state.Bindings.Select(b => b.ReplyMessageType));

        #endregion Public Properties

        #region Public Methods

        public async Task<bool> RaiseEventAsync(SagaReplyEvent @event, TSagaData sagaData, CancellationToken cancellationToken = default)
        {
            var eventType = @event.GetType();
            var seekingState = CurrentState ?? initialState;
            var handled = false;
            foreach (var binding in seekingState.Bindings)
            {
                if (binding.ReplyMessageType == eventType)
                {
                    var nextState = FindState(binding.NextState);
                    handled = await nextState.ExecuteAsync(@event, sagaData, cancellationToken);
                    CurrentState = nextState;
                    break;
                }
            }

            return handled;
        }

        public async Task StartAsync(TSagaData data, CancellationToken cancellationToken = default)
        {
            await initialState.ExecuteAsync(null, data, cancellationToken);
            this.CurrentState = initialState;
        }

        #endregion Public Methods

        #region Internal Methods

        internal void BindState(Type replyMessageType, SagaState<TSagaData> state)
        {
            if (!states.Contains(state))
            {
                states.Add(state);
            }

            initialState.AddBinding(replyMessageType, state.Name);
        }

        internal void BindState<TReplyEvent>(SagaState<TSagaData> state) where TReplyEvent : SagaReplyEvent
            => BindState(typeof(TReplyEvent), state);

        internal void BindState(string precedingStateName, Type replyMessageType, SagaState<TSagaData> nextState)
        {
            if (!states.Contains(nextState))
            {
                states.Add(nextState);
            }

            var precedingState = FindState(precedingStateName);
            if (precedingState != null)
            {
                precedingState.AddBinding(replyMessageType, nextState.Name);
            }
        }

        internal void BindState<TReplyEvent>(string precedingStateName, SagaState<TSagaData> nextState) where TReplyEvent : SagaReplyEvent
            => BindState(precedingStateName, typeof(TReplyEvent), nextState);

        internal void SetInitialState(SagaState<TSagaData> initialState)
        {
            this.initialState = initialState;
            this.states.Clear();
            this.states.Add(initialState);
        }

        internal void SetInitialState(Func<TSagaData, CancellationToken, Task<bool>> initiateCallback) => SetInitialState(new SagaState<TSagaData>("INITIAL", (_, data, token) => initiateCallback(data, token)));

        #endregion Internal Methods

        #region Private Methods

        private SagaState<TSagaData> FindState(string name) => states.Find(s => string.Equals(s.Name, name));

        #endregion Private Methods

    }
}
