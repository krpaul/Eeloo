lexer grammar GeneratedLexer;


fragment LOWERCASE_LTR  : [a-z] ;
fragment UPPERCASE_LTR  : [A-Z] ;
fragment DIGIT          : [0-9] ;

fragment COMPARISON_PREFIX : WS (IS | ISNT | IS WS NOT) ;
GRT_EQL    :   COMPARISON_PREFIX? (WS 'greater' WS 'or' WS 'equal' WS 'to' WS | WS 'greater' WS 'than' WS 'or' WS 'equal' WS 'to' WS | WS '>=' WS) ;
LESS_EQL   :   COMPARISON_PREFIX? (WS 'less' WS 'or' WS 'equal' WS 'to' WS | WS 'less' WS 'than' WS 'or' WS 'equal' WS 'to' WS | WS '<=' WS) ;
LESS       :   COMPARISON_PREFIX? (WS 'less' WS 'than' WS | WS 'less' WS | WS '<' WS) ;
GRT        :   COMPARISON_PREFIX? (WS 'greater' WS 'than' WS | WS 'greater' WS | WS '>' WS) ;

DBL_EQL    :   '==' ;
NOT_EQL    :   '!=' ;

L_SQ_BRACK :    '['         ;
R_SQ_BRACK :    ']'         ;
COMMA      :    ','         ;
DOT		   :	'.'			;

EXIT       :    'exit'      ;
WHILE      :    'while'     ;
UNTIL	   :    'until'		;
FOR_EACH   :    WS? 'for' WS? | WS? 'for' WS 'each' WS? | WS? 'for' WS 'every' WS? | WS? 'using' WS 'each' WS? | WS? 'using' WS 'every' WS?	;
FROM	   :	'from'		;
REPEAT	   :    'repeat'    ;
TIMES	   :    'times'     ;
DO		   :	'do'		;
IF         :    'if'        ;
THEN       :    'then'      ;
ELSE       :    'else'      ;
IS         :    'is'        ;
ISNT	   :	'isn\'t'	;
NOT		   :	'not'		;
IN		   :	'in'		;
AND		   :	'and'		;
OR		   :    'or'		;
FUNCTION   :	'function'	;
DEFINE     :    'define'    ;
NEW        :    'new'       ;
RETURN	   :	'return'    ;
END        :    'end'       ;
USE		   :	'use'		; 
THAN	   :	'than'		;
TOK_LESS   :    'less'		;
ASSERT     :    WS? 'assert' WS? | WS? 'assert' WS 'that' WS? | WS? 'make' WS 'sure' WS? | WS? 'make' WS 'sure' WS 'that' WS? | WS? 'make' WS 'sure' WS 'of' WS? | WS? 'force' WS 'check' WS?  ;
BETWEEN	   :	WS 'between' WS | WS 'is' WS 'between' WS | WS 'in' WS 'range' WS 'of' WS | WS 'in' WS 'the' WS 'range' WS 'of' WS	;
TRY		   :	WS 'try' WS		;
CATCH	   :    WS 'catch' WS	;
FINALLY	   :	WS 'finally' WS	;

// Types
			   // Empty isn't a real modifier, just treated as one and instantiates an empty list 
LIST_MODIFIER: 'empty' | 'unique' ;
NUMBER_MODIFIER: 'positive' | 'negative' | 'even' | 'odd' ;

STRING_TOK :	'string'	;
LIST_TOK   :	'list'		;
NUMBER_TOK :    'number'	;
BOOL_TOK   :    'bool'      ;

TRUE  :    'true'      ;
FALSE :    'false'     ;

IDENTIFIER :   (LOWERCASE_LTR | UPPERCASE_LTR | '_') (LOWERCASE_LTR | UPPERCASE_LTR | '_' | DIGIT)*  ;    

STR		   : ["].*?["]	;
NUMBER     : DIGIT+     ;

TO		   :   'to'		;
OF		   : WS 'of' WS ;
AS         : WS 'as' WS ;
BY		   :   'by'     ;

ARROW      :   '<-' ;

EQL        :   '='      ;
ADD_EQL    :   '+='     ;
SUB_EQL    :   '-='     ;
MULT_EQL   :   '*='     ;
DIV_EQL    :   '/='     ; 

PLUS       :   '+'      ;
MINUS      :   '-'      ;
DIVIDE     :   '/' | WS 'over' WS | WS 'divided' WS 'by' WS     ;
MULTIPLY   :   '*' | WS 'times' WS | WS 'multiplied' WS 'by' WS   ;
POWER      :   '^' | WS 'to' WS 'the' WS 'power' WS 'of' WS | WS 'raised' WS 'to' WS 'the' WS | WS 'to' WS 'the' WS	    ; 
MOD		   :   '%' | WS 'mod' WS | WS 'modulo' WS		;
FACTORIAL  :   '!' ;

RANGE_1	   :   '...' | WS TO WS  ;
RANGE_2	   :   WS 'by' WS | WS 'count' WS 'by' WS | WS 'counting' WS 'by' WS | WS 'skip' WS | WS 'skipping' WS ;

EQUALITY   : WS 'is' WS | WS 'is' WS 'equal' WS 'to' WS | WS 'equals' WS | WS '==' WS ;

LBRACK     :   '('      ;
RBRACK     :   ')'      ;

RL         : (DBL_EQL | GRT_EQL | LESS_EQL | NOT_EQL | LESS | GRT) ;

NL		   : [\r]?[\n] ;

WS		   :   (' ' | '\t' )+ -> skip ;

COMMENT	   : (('start comment' NL .*? NL 'end comment') | '//' ~[\r\n]* ) -> skip ;


/* Auto-generated tokens */
AUTOTOKEN_0000 : 'greater' ;
AUTOTOKEN_0001 : 'or' ;
AUTOTOKEN_0002 : 'equal' ;
AUTOTOKEN_0003 : 'to' ;
AUTOTOKEN_0004 : 'than' ;
AUTOTOKEN_0005 : '>=' ;
AUTOTOKEN_0006 : 'less' ;
AUTOTOKEN_0007 : '<=' ;
AUTOTOKEN_0008 : '<' ;
AUTOTOKEN_0009 : '>' ;
AUTOTOKEN_0010 : 'for' ;
AUTOTOKEN_0011 : 'each' ;
AUTOTOKEN_0012 : 'every' ;
AUTOTOKEN_0013 : 'using' ;
AUTOTOKEN_0014 : 'assert' ;
AUTOTOKEN_0015 : 'that' ;
AUTOTOKEN_0016 : 'make' ;
AUTOTOKEN_0017 : 'sure' ;
AUTOTOKEN_0018 : 'of' ;
AUTOTOKEN_0019 : 'force' ;
AUTOTOKEN_0020 : 'check' ;
AUTOTOKEN_0021 : 'between' ;
AUTOTOKEN_0022 : 'is' ;
AUTOTOKEN_0023 : 'in' ;
AUTOTOKEN_0024 : 'range' ;
AUTOTOKEN_0025 : 'the' ;
AUTOTOKEN_0026 : 'try' ;
AUTOTOKEN_0027 : 'catch' ;
AUTOTOKEN_0028 : 'finally' ;
AUTOTOKEN_0029 : 'over' ;
AUTOTOKEN_0030 : 'divided' ;
AUTOTOKEN_0031 : 'by' ;
AUTOTOKEN_0032 : 'times' ;
AUTOTOKEN_0033 : 'multiplied' ;
AUTOTOKEN_0034 : 'power' ;
AUTOTOKEN_0035 : 'raised' ;
AUTOTOKEN_0036 : 'mod' ;
AUTOTOKEN_0037 : 'modulo' ;
AUTOTOKEN_0038 : 'count' ;
AUTOTOKEN_0039 : 'counting' ;
AUTOTOKEN_0040 : 'skip' ;
AUTOTOKEN_0041 : 'skipping' ;
AUTOTOKEN_0042 : 'equals' ;
AUTOTOKEN_0043 : '==' ;
