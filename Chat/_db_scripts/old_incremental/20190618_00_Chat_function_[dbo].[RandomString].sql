use [FLChat]
go

create view [dbo].[vwGetNewID]
as
select newid() as new_id
go

IF OBJECT_ID (N'dbo.RandomString', N'TF') IS NOT NULL  
    DROP FUNCTION [dbo].[RandomString];  
GO  

create function [dbo].[RandomString](@Length int)
returns nvarchar(100)
as
begin
  declare @chars varchar(2048)
  declare @charsLength int;
  declare @result nvarchar(100);

  set @chars = 'abcdefghijkmnopqrstuvwxyz' +  --lower letters
               'ABCDEFGHIJKLMNPQRSTUVWXYZ' +      --upper letters
               '0123456789';       --number characters	
  set @charsLength = LEN(@chars);

  set @result = 
    (select top (@Length)
      substring(@chars, 1 + (number % @charsLength), 1) as [text()]       
     from master..spt_values
     where type = 'P'
     order by (select new_id from vwGetNewID) --instead of using newid()
     for xml path(''));

  return @result;
end
go 
