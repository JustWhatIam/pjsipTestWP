// pjsipLibRuntimeComponent.cpp
#include "pch.h"
#include "pjsipLibRuntimeComponent.h"

using namespace pjsipLibRuntimeComponent;
using namespace Platform;

extern std::string str_output_temp;
extern void SetLog(const char* src, const char* format, ...);

Platform::String^ pjRC::GetLog() {
	char* temp = (char*)malloc(str_output_temp.length());
	strcpy(temp, str_output_temp.c_str());
	wchar_t * w_temp;
	CharToWChar(temp, &w_temp);
	str_output_temp = "";
	return ref new String(w_temp);
}

int pjRC::CharToWChar(const char *charVar, wchar_t **w_charVar){
	size_t len = strlen(charVar) + 1;
	size_t converted = 0;

	*w_charVar = (wchar_t*)malloc(len*sizeof(wchar_t));
	mbstowcs_s(&converted, *w_charVar, len, charVar, _TRUNCATE);
	return converted;
}

int pjRC::WCharToChar(const wchar_t *w_charVar, char** charVar) {
	size_t len = wcslen(w_charVar) + 1;
	size_t converted = 0;

	*charVar = (char*)malloc(len*sizeof(char));
	wcstombs_s(&converted, *charVar, len, w_charVar, _TRUNCATE);
	return converted;
}
dataHandler^ data_handler;
static void cb(char * data) {
	if (data_handler != nullptr) {
		size_t len = strlen(data) + 1;
		size_t converted = 0;

		wchar_t *w_charVar = (wchar_t*)malloc(len*sizeof(wchar_t));
		mbstowcs_s(&converted, w_charVar, len, data, _TRUNCATE);
		data_handler(ref new String(w_charVar));
	}
}

pjRC::pjRC()
{
	pjRC_init();
	setCB(cb);
}

void pjRC::setDataHandler(dataHandler^ handler) { 
	data_handler = handler;
}



void pjRC::start(){
	icedemo_start_nego();
}

void pjRC::send(Platform::String^ data) {
	char * buffer;
	WCharToChar(data->Data(), &buffer);
	icedemo_send_data(1, buffer);
	
}

void pjRC::inputSDP(Platform::String^ sdp) {
	char * buffer;
	WCharToChar(sdp->Data(), &buffer);

	input_info(buffer);
}

void pjRC::destroy() {
	icedemo_destroy_instance();
}

void pjRC::show() {

	icedemo_show_ice();
}


void pjRC::initSession(int roleNum) {
	setRoleNum(roleNum == 0 ? 'o' : 'a');

	create_instance();
}