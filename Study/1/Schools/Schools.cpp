#include "stdafx.h"
#include "Schools.h"


Schools::Schools()
{
}


Schools::~Schools()
{
}

int Schools::countAll(string code, int year)
{
	int sum = 0;
	
	int num = schools.size();
	bool find = false;
	int index = 0;
	// Перебор всех учебных заведений
	for (int i = 0; i < num; ++i)
	{
		find = schools[i].code == code;
		if (find)
		{
			index = i;  // Найдена
			break;
		}
	}
	if (find)
	{
		// Найдена - считаем
		sum = schools[index].countAll(year);		
	}
	return sum;
}

int Schools::countHigher(string code, int year)
{
	int sum = 0;

	int num = schools.size();
	bool find = false;
	int index = 0;
	// Перебор всех учебных заведений
	for (int i = 0; i < num; ++i)
	{
		find = schools[i].code == code;
		if (find)
		{
			index = i;  // Найдена
			break;
		}
	}
	if (find)
	{
		// Найдена - считаем
		sum = schools[index].countHigher(year);
	}
	return sum;
}

int Schools::countGoldMedals(int year)
{
	int num = schools.size();
	int sum = 0;
	// Перебор всех учебных заведений
	for (int i = 0; i < num; ++i)
	{
		sum += schools[i].countGoldMedals(year);  // Из каждого добываем некоторое количество строк
	}
	return sum;
}

bool Schools::ReadFromFile(const string FName)
{
	ifstream FRead(FName);
	bool ret = true;
	int bufsize = 256;
	char *buff = new char[bufsize]; // буфер промежуточного хранения считываемого из файла текста

	
	//int num = 0;

	try
	{
		if (!FRead.is_open()) // если файл не открыт
		{
			return false;
		}
		else
		{			
			while (FRead.getline(buff, bufsize))  // Читаем строку из файла
			{				
				string str = buff;  // Перевод в string
				std::string::size_type found0 = -1;
				std::string::size_type found1 = str.find_first_of(";");
				//К какой школе относится строка
				string code = str.substr(found0 + 1, found1 - found0 - 1);

				//Поиск школы с таким кодом. Если нет - добавить
				int num = schools.size();
				bool find = false;
				School school;
				int index = 0;
				for (int i = 0; i < num; ++i)
				{
					find = schools[i].code == code;
					if (find)
					{
						index = i;  // Найдена
						break;						
					}
				}
				if (!find)  
				{
					// Не нацдена - добавляем
				
					school.code = code;
					schools.push_back(school);
					index = schools.size()-1;  // Номер - последний
				}

				schools[index].GetData(str);  // Запись данных
			}
			FRead.close(); // закрываем файл
		}
	}
	catch (...)
	{
		ret = false;
	}
	delete buff;
}
bool Schools::WriteToFile(const string FName)
{
	ofstream FWrite(FName);
	bool ret = true;
	int bufsize = 256;
	char *buff = new char[bufsize]; // буфер промежуточного хранения считываемого из файла текста

	School school;
	//int num = 0;

	try
	{
		if (!FWrite.is_open()) // если файл не открыт
		{
			return false;
		}
		else
		{
			int num = schools.size();
			vector<string> text;
			// Перебор всех учебных заведений
			for (int i = 0; i < num; ++i)
			{
				schools[i].ToCSV(text);  // Из каждого добываем некоторое количество строк
			}
			num = text.size();
			// Все полученные строки по всем школам
			for (int i = 0; i < num; ++i)
			{
				FWrite << text[i] << endl;
			}
			FWrite.close(); // закрываем файл
		}
	}
	catch (...)
	{
		ret = false;
	}
	return ret;
}