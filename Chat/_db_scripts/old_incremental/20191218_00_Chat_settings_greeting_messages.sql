use [FLChat]
go

delete from [Cfg].[Settings] where [Name] in (
	N'TEXT_LITELINK_GREETING_MSG',
	N'TEXT_LITELINK_ROUTED',
	N'TEXT_LITELINK_UNROUTED',
	N'TEXT_DEEPLINK_GREETING_MSG_REJECTED',
	N'TEXT_LITELINK_GREETING_MSG_VIBER');

insert into [Cfg].[Settings]([Name], [Value], [Descr])
values
(N'TEXT_LITELINK_GREETING_MSG', N'������ ����, #���.', N'����������� ��� ���������� ������������, ����������� �� ����� ������.'),
(N'TEXT_LITELINK_ROUTED', N'��� ������ ����������� %addressee%. �� ������ ������ ��� ������ ����� ������.', N'������ ����� ����������� ��� ���������� ������������, ����������� �� ����� ������.'),
(N'TEXT_LITELINK_UNROUTED', N'����������� �� ����� � ��������� ���������� ���������� � ����� ������ � ������.', '����������� ��� ���������� ������������, ����������� �� ����� ������, ��� �������� ��� ���������� � �������'),
(N'TEXT_DEEPLINK_GREETING_MSG_REJECTED', N'������ ����!
����������� �� ����� � ��������� ���������� ���������� �� ������ � ��������.', '����������� ��� ������������ ������������, ����������� �� ������'),
(N'TEXT_LITELINK_GREETING_MSG_VIBER', N'������ ����, #���. ��������� � ������ ������������� FABERLIC � ��������� ��������� ������ �� ��� �������.', '������ ����� ���������� ��� ������������ ����������� �� ����� ������ � ������');

