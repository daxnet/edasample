using EdaSample.Common.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EdaSample.Common.Sagas
{
    internal sealed class SagaManagerInlineEventHandler<TSagaState> : BaseEventHandler<IEvent>
        where TSagaState : ISagaData, new()
    {
        #region Private Fields

        private readonly ISagaManager<TSagaState> sagaManager;

        #endregion Private Fields

        #region Public Constructors

        public SagaManagerInlineEventHandler(ISagaManager<TSagaState> sagaManager)
        {
            this.sagaManager = sagaManager;
        }

        #endregion Public Constructors

        #region Public Methods

        public override async Task<bool> HandleAsync(IEvent message, CancellationToken cancellationToken = default)
        {
            if (message is SagaReplyEvent srm)
            {
                return await sagaManager.HandleReplyEventAsync(srm, cancellationToken);
            }

            return false;
        }

        #endregion Public Methods
    }
}
