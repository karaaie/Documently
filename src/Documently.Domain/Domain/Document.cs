using System;
using CommonDomain.Core;
using Documently.Domain.Events;
using Magnum;

namespace Documently.Domain.Domain
{
	public class Document : AggregateBase<DomainEvent>
	{
		private Guid _DocumentBlobId;

		public Document()
		{
		}

		public Document(string title, DateTime utcCreated)
		{
			var @event = new DocumentMetaDataCreated(
				 CombGuid.Generate(), title, DocumentState.Created, utcCreated);

			RaiseEvent(@event);
		}

		public void Apply(DocumentMetaDataCreated evt)
		{
			Id = evt.AggregateId;
		}

		public void AssociateWithDocumentBlob(Guid blobId)
		{
			var evt = new AssociatedIndexingPending(DocumentState.AssociatedIndexingPending, blobId, Id, (uint)Version + 1);
			RaiseEvent(evt);
		}

		public void Apply(AssociatedIndexingPending evt)
		{
			_DocumentBlobId = evt.BlobId;
		}
	}
}