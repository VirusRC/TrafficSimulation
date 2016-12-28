#pragma once

#include <string>
#include <Windows.h>
#include <locale>
#include <codecvt>

using namespace std;

class Helper
{
public:
	//Function used from: http://stackoverflow.com/questions/4804298/how-to-convert-wstring-into-string
	static wstring s2ws(const std::string& str)
	{
		using convert_typeX = std::codecvt_utf8<wchar_t>;
		std::wstring_convert<convert_typeX, wchar_t> converterX;

		return converterX.from_bytes(str);
	}



private:


};