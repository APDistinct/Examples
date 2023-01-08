use [FLChat]
go

select count(*) from [Msg].[MessageToSegment]

drop table if exists #mtstmp

select *
into #mtstmp
from  [Msg].[MessageToSegment]

drop table [Msg].[MessageToSegment]

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON

CREATE TABLE [Msg].[MessageToSegment](
    [Idx] bigint NOT NULL IDENTITY(1,1),
	[MsgId] uniqueidentifier NOT NULL,
	[SegmentId] uniqueidentifier NOT NULL,
	[MaxDeep] int NULL,
    CONSTRAINT [PK__MsgMessageToSegment] PRIMARY KEY NONCLUSTERED (
	    [MsgId], [SegmentId]),
	CONSTRAINT [UNQ__MsgMessageToSegment__Idx] UNIQUE CLUSTERED ([Idx]),
	CONSTRAINT [FK__MsgMessageToSegment__MsgMessage] 
		FOREIGN KEY([MsgId])
		REFERENCES [Msg].[Message] ([Id])
		ON DELETE CASCADE,
	CONSTRAINT [FK__MsgMessageToSegment__UsrSegment] 
		FOREIGN KEY([SegmentId])
		REFERENCES [Usr].[Segment] ([Id])
) ON [PRIMARY]
GO

insert into [Msg].[MessageToSegment] ([MsgId], [SegmentId], [MaxDeep])
select t.[MsgId], t.[SegmentId], t.[MaxDeep] 
from #mtstmp t
inner join [Msg].[Message] m on t.[MsgId] = m.[Id]
order by m.[PostTm]

select count(*) from [Msg].[MessageToSegment]

drop table if exists #mtstmp