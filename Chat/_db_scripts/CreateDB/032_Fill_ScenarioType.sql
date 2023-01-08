use FLChat
go

declare @dt as table (
  [ScenarioId] integer NOT NULL,
  [Step] integer NOT NULL,
  [Description] nvarchar(200) NULL
);


declare @id int
select @id = id FROM [Cfg].[Scenario] where [Name] = 'Invite'

INSERT INTO @dt ([ScenarioId], [Step], [Description])
VALUES
( @id, 0, N'Начало')
,( @id, 1, N'1')
,( @id, 2, N'Ручной ввод номера телефона')
,( @id, 3, N'Проверка подтверждения номера телефона')
,( @id, 4, N'Фиксация подтвержденого номера телефона')

select @id = id FROM [Cfg].[Scenario] where [Name] = 'Common'

INSERT INTO @dt ([ScenarioId], [Step], [Description])
VALUES
( @id, 0, N'Начало')
,( @id, 1, N'1')
,( @id, 2, N'Ручной ввод номера телефона')
,( @id, 3, N'Проверка подтверждения номера телефона')
,( @id, 4, N'Фиксация подтвержденого номера телефона')



--declare @it as table (
--  [Id] integer NULL ,
--  [ScenarioId] integer NOT NULL,
--  [Step] integer NOT NULL,
--  [Description] nvarchar(200) NULL
--);

update cfg
set cfg.[Description] = d.[Description]
from [Cfg].[ScenarioStep] cfg
inner join @dt d on (d.[ScenarioId] = cfg.[ScenarioId] and d.[Step] = cfg.[Step])
where cfg.[Description] <> d.[Description] 



INSERT INTO [Cfg].[ScenarioStep] ([ScenarioId], [Step], [Description])
select sel.[ScenarioId], [Step], [Description] from
(select [Id], dt.[ScenarioId], dt.[Step], dt.[Description] from @dt dt
left join [Cfg].[ScenarioStep] cfg on (dt.[ScenarioId] = cfg.[ScenarioId] and dt.[Step] = cfg.[Step]) ) sel
where sel.[Id] is null

go

select * from [Cfg].[ScenarioStep] 