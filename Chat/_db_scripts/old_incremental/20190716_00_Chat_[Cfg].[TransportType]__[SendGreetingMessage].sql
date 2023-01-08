use [FLChat]
go

alter table [Cfg].[TransportType]
add [SendGreetingMessage] bit not null default 0
go

update [Cfg].[TransportType]
set [SendGreetingMessage] = 1
where [Id] in(/**Telegram**/1, /**Test**/-1)
go

insert into [Cfg].[Settings] ([Name], [Value], [Descr])
values (
	N'TEXT_GREETING_MSG', 
	N'����� ���������� � Faberlic Chat. ��������� � ������������ �� Faberlic, ��������� ������ � ������ ���������.', 
	N'�������������� ��������� ��� ����������� ������ ������')
go

insert into [Cfg].[Settings] ([Name], [Value], [Descr])
values (
	N'TEXT_CHANGE_MESSAGE_ADDRESSEE', 
	N'��������� ����� ���������� ������������: %FullName%', 
	N'��������� ��� ����� ')
go