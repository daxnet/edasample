using EdaSample.Common;
using EdaSample.Common.Events.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace EdaSample.Tests.Models
{
    public class Book : AggregateRootWithEventSourcing
    {
        public void ChangeTitle(string newTitle)
        {
            this.Raise(new BookTitleChangedEvent(newTitle));
        }
        
        public string Title { get; private set; }

        [HandlesInline]
        private void OnTitleChanged(BookTitleChangedEvent @event)
        {
            this.Title = @event.NewTitle;
        }

        public override string ToString()
        {
            return Title;
        }
    }
}
