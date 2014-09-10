#pragma once
#include <ice.h>


namespace pjsipLibRuntimeComponent
{
	public enum  class pjsipStatus
	{
		Success,
		Init_Failure,
		CreateInstance_Failure,
	};

	public delegate void dataHandler(Platform::String^ data);

    public ref class pjRC sealed
    {
    public:
		pjRC();
		//0 for offerer, 1 for answerer
		void initSession(int roleNum);
		void show();
		Platform::String^ GetLog();
		void inputSDP(Platform::String^ sdp);
		void start();
		void destroy();
		void send(Platform::String^ data);
		void setDataHandler(dataHandler^ handler);
		property pjsipStatus pStatus;
	private:
		int CharToWChar(const char *charVar, wchar_t **w_charVar);
		int WCharToChar(const wchar_t *, char**);
    };
}



