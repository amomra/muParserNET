#ifndef MU_PARSER_NET_DLL_H
#define MU_PARSER_NET_DLL_H

/*
 * This header contains the functions that are available in the parser class
 * but not in the DLL interface that are used by the muParserNET.
 * 
 * The separation of the new functions from the older ones was made to allow
 * an easier version change.
 */

#include "muParserDLL.h"

#ifdef __cplusplus
extern "C"
{
#endif

API_EXPORT(void) mupClearInfixOprt(muParserHandle_t a_hParser);
API_EXPORT(void) mupClearPostfixOprt(muParserHandle_t a_hParser);
API_EXPORT(void) mupEnableBuiltInOprt(muParserHandle_t a_hParser, muBool_t a_bOprtEn);
API_EXPORT(void) mupEnableOptimizer(muParserHandle_t a_hParser, muBool_t a_bOptmEn);
API_EXPORT(const muChar_t*) mupValidNameChars(muParserHandle_t a_hParser);
API_EXPORT(const muChar_t*) mupValidOprtChars(muParserHandle_t a_hParser);
API_EXPORT(const muChar_t*) mupValidInfixOprtChars(muParserHandle_t a_hParser);

#ifdef __cplusplus
}
#endif

#endif