lexer grammar LexerAliases;


/* Lexer */

fragment LOWERCASE_LTR  : [a-z] ;
fragment UPPERCASE_LTR  : [A-Z] ;
fragment DIGIT          : [0-9] ;


DBL_EQL    :   '==' ;
GRT_EQL    :   '>=' | (IS WS)? 'greater' WS 'than' WS 'or' WS 'equal' WS TO;
LESS_EQL   :   '<=' | (IS WS)? 'less' WS 'than' WS 'or' WS 'equal' WS TO;
NOT_EQL    :   '!=' ;
LESS       :   '<'  | (IS WS)? 'less' WS 'than';
GRT        :   '>'  | (IS WS)? 'greater' WS 'than';

L_SQ_BRACK :    '['         ;
R_SQ_BRACK :    ']'         ;
COMMA      :    ','         ;
DOT		   :	'.'			;

EXIT       :    'exit'      ;
WHILE      :    'while'     ;
UNTIL	   :    'until'		;
FOR_EACH   :    'for' WS 'each'	;
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
RETURN	   :	'return'    ;
END        :    'end'       ;
USE		   :	'use'		; 

// Types
			   // Empty isn't a real modifier, just treated as one and instantiates an empty list 
LIST_MODIFIER: 'empty' | 'unique' ;
NUMBER_MODIFIER: 'positive' | 'negative' | 'even' | 'odd' ;

STRING_TOK :	'string'	;
LIST_TOK   :	'list'		;
NUMBER_TOK :    'number'	;

TRUE  :    'true'      ;
FALSE :    'false'     ;

IDENTIFIER :   (LOWERCASE_LTR | UPPERCASE_LTR | '_') (LOWERCASE_LTR | UPPERCASE_LTR | '_' | DIGIT)*  ;    

STR		   : ["].*?["]	;
NUMBER     : DIGIT+     ;

TO		   :   'to'		;
BY		   :   'by'     ;

EQL        :   '='      ;
PLUS       :   '+'      ;
MINUS      :   '-'      ;
DIVIDE     :   '/'      ;
MULTIPLY   :   '*' | 'times' | 'multiplied' WS 'by' ;
POWER      :   '^' | 'to' WS 'the' WS 'power' WS 'of' | 'raised' WS 'to' WS 'the' | 'to' WS 'the' ; 
MOD		   :   '%' | 'mod' | 'modulo' ;

RANGE_1	   :   '...' | WS TO WS  ;
RANGE_2	   :   WS BY WS ;

LBRACK     :   '('      ;
RBRACK     :   ')'      ;

RL         : (DBL_EQL | GRT_EQL | LESS_EQL | NOT_EQL | LESS | GRT) ;

NL		   : [\r]?[\n] ;

WS		   :   (' ' | '\t' )+ -> skip ;

COMMENT	   : NL* 'start comment' .*? 'end comment' NL* -> skip ;