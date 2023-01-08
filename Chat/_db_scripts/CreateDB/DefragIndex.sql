-- таблица для сохранения результатов дефрагментации индексов
if not exists(select 1 from sysobjects where xtype='U' 
	and [name]='Defrag' and [uid] = SCHEMA_ID('dbo'))
begin
CREATE TABLE [dbo].[Defrag](
	[ID] [bigint] IDENTITY(794,1) NOT NULL,
	[db] [nvarchar](100) NULL,
	[shema] [nvarchar](100) NULL,
	[table] [nvarchar](100) NULL,
	[IndexName] [nvarchar](100) NULL,
	[frag_num] [int] NULL,
	[frag] [decimal](6, 2) NULL,
	[page] [int] NULL,
	[rec] [int] NULL,
    [func] [int] NULL,
	[ts] [datetime] NULL,
	[tf] [datetime] NULL,
	[frag_after] [decimal](6, 2) NULL,
	[object_id] [int] NULL,
	[idx] [int] NULL,
	[InsertUTCDate] [datetime] NOT NULL,
 CONSTRAINT [PK_Defrag] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY];

ALTER TABLE [dbo].[Defrag] ADD  CONSTRAINT [DF_Defrag_InsertUTCDate]  DEFAULT (getutcdate()) FOR [InsertUTCDate];
 PRINT '[dbo].[Defrag]: CREATE TABLE'
end
GO

-- представление, с помощью которого можно получить какие индексы и на сколько в процентах фрагментированы
CREATE OR ALTER view [dbo].[IndexDefragView]
as
with info as 
(SELECT
	[object_id],
	database_id,
	index_id,
	index_type_desc,
	index_level,
	fragment_count,
	avg_fragmentation_in_percent,
	avg_fragment_size_in_pages,
	page_count,
	record_count,
	ghost_record_count
	FROM sys.dm_db_index_physical_stats(DB_ID(N'FLChat'), NULL, NULL, NULL, N'LIMITED')
	where index_level = 0
	)
SELECT
	b.name as db,
	s.name as shema,
	t.name as tb,
	i.index_id as idx,
	i.database_id,
	idx.name as index_name,
	i.index_type_desc,i.index_level as [level],
	i.[object_id],
	i.fragment_count as frag_num,
	round(i.avg_fragmentation_in_percent,2) as frag,
	round(i.avg_fragment_size_in_pages,2) as frag_page,
	i.page_count as [page],
	i.record_count as rec,
	i.ghost_record_count as ghost,
	round(i.avg_fragmentation_in_percent*i.page_count,0) as func
FROM Info as i
inner join [sys].[databases]	as b	on i.database_id = b.database_id
inner join [sys].[all_objects]	as t	on i.object_id = t.object_id
inner join [sys].[schemas]	as s	on t.[schema_id] = s.[schema_id]
inner join [sys].[indexes]	as idx on t.object_id = idx.object_id and idx.index_id = i.index_id
 where i.avg_fragmentation_in_percent >= 25 and i.index_type_desc <> 'HEAP';
GO

-- представление для просмотра статистики по результатам дефрагментации индексов
CREATE OR ALTER view [dbo].[StatisticDefragView] as
SELECT top 1000
	[db]
	,[shema]
	,[table]
	,[IndexName]
	,avg([frag]) as AvgFrag
	,avg([frag_after]) as AvgFragAfter
	,avg(page) as AvgPage
  FROM [dbo].[Defrag]
  group by [db], [shema], [table], [IndexName]
  order by abs(avg([frag])-avg([frag_after])) desc;
GO

-- хранимая процедура для анализа и дефрагментации выбранного индекса

CREATE OR ALTER PROCEDURE [System].[AutoDefragIndex]
AS
BEGIN
	SET NOCOUNT ON;

	--объявляем необходимые переменные
	declare @IndexName nvarchar(100) --название индекса
	,@db nvarchar(100)			 --название базы данных
	,@Shema nvarchar(100)			 --название схемы
	,@Table nvarchar(100)			 --название таблицы
	,@SQL_Str nvarchar (2000)		 --строка для формирования команды
	,@frag decimal(6,2)				 --% фрагментации до процесса дефрагментации
	,@frag_after decimal(6,2)		 --% фрагментации после процесса дефрагментации
    --Количество фрагментов на конечном уровне единицы распределения IN_ROW_DATA	
    ,@frag_num int				 
	,@func int					 --round(i.avg_fragmentation_in_percent*i.page_count,0)
	,@page int					 --кол-во страниц индекса		
	,@rec int						 --общее кол-во записей
	,@ts datetime					 --дата и время начала дефрагментации
	,@tf datetime					 --дата и время окончания дефрагментации
	--идентификатор объекта таблицы или представления, для которых создан индекс
    ,@object_id int					 
	,@idx int;						 --ID индекса

	--получаем текущую дату и время
	set @ts = getdate();
	
	--получаем очередной индекс для дефрагментации
	--здесь именно важный индекс выбирается. При этом никогда не случиться, что один индекс будет
    --постоянно дефрагментироваться, а все остальные не будут выбраны для дефрагментации
	select top 1
		@IndexName = index_name,
		@db=db,
		@Shema = shema,
		@Table = tb,
		@frag = frag,
		@frag_num = frag_num,
		@func=func,
		@page =[page],
		@rec = rec,
		@object_id = [object_id],
		@idx = idx 
	from  [dbo].[IndexDefragView]
	order by func*power((1.0-
	  convert(float,(select count(*) from [dbo].[Defrag] vid where vid.db=db 
			 and vid.shema = shema
			 and vid.[table] = tb
			 and vid.IndexName = index_name))
	 /
	 convert(float,
                  case  when (exists (select top 1 1 from [dbo].[Defrag] vid1 where vid1.db=db))
                            then (select count(*) from [dbo].[Defrag] vid1 where vid1.db=db)
                            else 1.0 end))
                    ,3) desc

	--если такой индекс получен
	if (@db is not null)
	begin
	   --непосредственно реорганизация индекса
	   set @SQL_Str = 'alter index ['+@IndexName+'] on ['+@Shema+'].['+@Table+'] Reorganize';

		execute sp_executesql  @SQL_Str;

		--получаем текущую дату и время
		set @tf = getdate()

		--получаем процент фрагментации после дефрагментации
		SELECT @frag_after = avg_fragmentation_in_percent
		FROM sys.dm_db_index_physical_stats
			(DB_ID(@db), @object_id, @idx, NULL ,
			N'DETAILED')
		where index_level = 0;

		--записываем результат работы
		insert into [dbo].[Defrag](
			[db],
			[shema],
			[table],
			[IndexName],
			[frag_num],
			[frag],
			[page],
			[rec],
			ts,
			tf,
			frag_after,
			object_id,
			idx
			  )
		select
			@db,
			@shema,
			@table,
			@IndexName,
			@frag_num,
			@frag,
			@page,
			@rec,
			@ts,
			@tf,
			@frag_after,
			@object_id,
			@idx;
		
		--обновляем статистику для индекса
		set @SQL_Str = 'UPDATE STATISTICS ['+@Shema+'].['+@Table+'] ['+@IndexName+']';

		execute sp_executesql  @SQL_Str;
	end
END

