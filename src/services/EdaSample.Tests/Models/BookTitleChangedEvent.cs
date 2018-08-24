using EdaSample.Common.Events.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace EdaSample.Tests.Models
{
    public class BookTitleChangedEvent : DomainEvent
    {
        public BookTitleChangedEvent(string newTitle)
        {
            this.NewTitle = newTitle;
        }

        public string NewTitle { get; set; }

        public override string ToString()
        {
            return $"{Sequence} - {NewTitle}";
        }
    }
}
