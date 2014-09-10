#pragma once
#include <pjlib-util.h>
#include <pjlib.h>
#include <pjnath.h>
#include <windows.h>
#include <string>
#include <global.h>

#define THIS_FILE "demo"
#define DemoOutput(var) OutputDebugString(L##var)

#define PRINT(fmt, arg0, arg1, arg2, arg3, arg4, arg5)	    \
	printed = pj_ansi_snprintf(p, maxlen - (p-buffer),  \
				   fmt, arg0, arg1, arg2, arg3, arg4, arg5); \
	if (printed <= 0) return -PJ_ETOOSMALL; \
	p += printed

#define CHECK(expr)	status=expr; \
			if (status!=PJ_SUCCESS) { \
			    err_exit(#expr, status); \
						}
#define LOG_OUTPUT(arg) SetLog (NULL, arg)

//#define CHECK(expr) expr

#define KA_INTERVAL 300


pj_status_t pjRC_init();
void init_stun();
void create_instance();
void icedemo_init_session(/*unsigned rolechar*/);

//pj_ice_strans_cfg ice_config;
//pj_ice_strans_cb ice_cb;
//pj_ice_strans *ice_strans;

void input_info(char * dataArr);
void icedemo_start_nego(void);
void icedemo_send_data(unsigned comp_id, const char *data);

int print_cand(char buffer[], unsigned maxlen, const pj_ice_sess_cand *cand);
void icedemo_perror(const char *title, pj_status_t status);
void err_exit(const char *title, pj_status_t status);
void reset_rem_info(void);
int icedemo_worker_thread(void *unused);
pj_status_t handle_events(unsigned max_msec, unsigned *p_count);
void icedemo_usage(); 
void cb_on_ice_complete(pj_ice_strans *ice_st, pj_ice_strans_op op, pj_status_t status);
void cb_on_rx_data(pj_ice_strans *ice_st, unsigned comp_id, void *pkt, pj_size_t size, const pj_sockaddr_t *src_addr, unsigned src_addr_len);
int encode_session(char buffer[], unsigned maxlen);
void icedemo_destroy_instance(void);
void icedemo_show_ice(void);
void setRoleNum(unsigned rolechar);
void setCB(void(*cb)(char *));



