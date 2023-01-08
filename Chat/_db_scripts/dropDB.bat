SET PWD=%1
sqlcmd -S . -U sa -P %PWD% -i ".\CreateDB\-1_DropDB.sql"