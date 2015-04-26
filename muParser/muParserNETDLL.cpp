#if defined(MUPARSER_DLL)

// copiado do muParserDLL.cpp --------------------------------------------------

#if defined(_WIN32)
#define WIN32_LEAN_AND_MEAN
#include <windows.h>
#endif

#include "muParserNETDLL.h"
#include "muParser.h"
#include "muParserInt.h"
#include "muParserError.h"


#define MU_TRY  \
        try     \
		        {

#define MU_CATCH                                                 \
		        }                                                        \
        catch(muError_t &e)                                      \
		        {                                                        \
          ParserTag *pTag = static_cast<ParserTag*>(a_hParser);  \
          pTag->exc = e;                                         \
          pTag->bError = true;                                   \
          if (pTag->errHandler)                                  \
            (pTag->errHandler)(a_hParser);                       \
		        }                                                        \
        catch(...)                                               \
		        {                                                        \
          ParserTag *pTag = static_cast<ParserTag*>(a_hParser);  \
          pTag->exc = muError_t(mu::ecINTERNAL_ERROR);           \
          pTag->bError = true;                                   \
          if (pTag->errHandler)                                  \
            (pTag->errHandler)(a_hParser);                       \
		        }

// private types
typedef mu::ParserBase::exception_type muError_t;
typedef mu::ParserBase muParser_t;

// declaração da classe que está implementada no muParserDLL
class ParserTag
{
public:
	ParserTag(int nType);

	~ParserTag();

	mu::ParserBase *pParser;
	mu::ParserBase::exception_type exc;
	muErrorHandler_t errHandler;
	bool bError;

private:
	ParserTag(const ParserTag &ref);
	ParserTag& operator=(const ParserTag &ref);

	int m_nParserType;
};

// cria outro buff
muChar_t s_tmpOutBuf[2048];

muParser_t* AsParser(muParserHandle_t a_hParser);
ParserTag* AsParserTag(muParserHandle_t a_hParser);

// -----------------------------------------------------------------------------

API_EXPORT(void) mupClearInfixOprt(muParserHandle_t a_hParser)
{
	MU_TRY
		muParser_t* const p(AsParser(a_hParser));
		p->ClearInfixOprt();
	MU_CATCH
}

API_EXPORT(void) mupClearPostfixOprt(muParserHandle_t a_hParser)
{
	MU_TRY
		muParser_t* const p(AsParser(a_hParser));
		p->ClearPostfixOprt();
	MU_CATCH
}

API_EXPORT(void) mupEnableBuiltInOprt(muParserHandle_t a_hParser, muBool_t a_bOprtEn)
{
	MU_TRY
		muParser_t* const p(AsParser(a_hParser));
		p->EnableBuiltInOprt(a_bOprtEn == 1);
	MU_CATCH
}

API_EXPORT(void) mupEnableOptimizer(muParserHandle_t a_hParser, muBool_t a_bOptmEn)
{
	MU_TRY
		muParser_t* const p(AsParser(a_hParser));
		p->EnableOptimizer(a_bOptmEn == 1);
	MU_CATCH
}

API_EXPORT(const muChar_t*) mupValidNameChars(muParserHandle_t a_hParser)
{
	MU_TRY
		muParser_t* const p(AsParser(a_hParser));
		sprintf(s_tmpOutBuf, "%s", p->ValidNameChars());
		return s_tmpOutBuf;
	MU_CATCH

	return "";
}

API_EXPORT(const muChar_t*) mupValidOprtChars(muParserHandle_t a_hParser)
{
	MU_TRY
		muParser_t* const p(AsParser(a_hParser));
		sprintf(s_tmpOutBuf, "%s", p->ValidOprtChars());
		return s_tmpOutBuf;
	MU_CATCH

	return "";
}

API_EXPORT(const muChar_t*) mupValidInfixOprtChars(muParserHandle_t a_hParser)
{
	MU_TRY
		muParser_t* const p(AsParser(a_hParser));
		sprintf(s_tmpOutBuf, "%s", p->ValidInfixOprtChars());
		return s_tmpOutBuf;
	MU_CATCH

	return "";
}

#endif