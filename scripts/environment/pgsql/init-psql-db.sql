create schema if not exists edasample;
drop table if exists edasample.eventstore;
create table edasample.EventStore
(
	EventId uuid,
	EventPayload text,
	eventTimestamp time,
	sunny integer
);