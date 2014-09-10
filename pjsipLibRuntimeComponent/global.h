#pragma once
#include <pjlib-util.h>
#include <pjlib.h>
#include <pjnath.h>


#define STUN_SERVER_ADDR "stun.ekiga.net:3478"

/* This is our global variables */
struct app_t
{
	/* Command line options are stored here */
	struct options
	{
		unsigned    comp_cnt;
		pj_str_t    ns;
		int	    max_host;
		pj_bool_t   regular;
		pj_str_t    stun_srv;
		pj_str_t    turn_srv;
		pj_bool_t   turn_tcp;
		pj_str_t    turn_username;
		pj_str_t    turn_password;
		pj_bool_t   turn_fingerprint;
		const char *log_file;
	} opt;

	/* Our global variables */
	pj_caching_pool	 cp;
	pj_pool_t		*pool;
	pj_thread_t		*thread;
	pj_bool_t		 thread_quit_flag;
	pj_ice_strans_cfg	 ice_cfg;
	pj_ice_strans	*icest;
	FILE		*log_fhnd;
	pj_ice_sess_role role;
	void(* wp_data_cb)(char *);
	/* Variables to store parsed remote ICE info */
	struct rem_info
	{
		char		 ufrag[80];
		char		 pwd[80];
		unsigned	 comp_cnt;
		pj_sockaddr	 def_addr[PJ_ICE_MAX_COMP];
		unsigned	 cand_cnt;
		pj_ice_sess_cand cand[PJ_ICE_ST_MAX_CAND];
	} rem;


};

pj_status_t global_init();