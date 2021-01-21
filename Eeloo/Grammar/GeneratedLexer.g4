lexer grammar GeneratedLexer;
/* Edit this file for changes to the lexer 
 * All the rules with << >> brackets bounding 
 * a keyword will be replaced with many alias
 * possibilities upon build. Aliases are 
 * specified in the Aliases.yml file.
*/

fragment LOWERCASE_LTR  : [a-z] ;
fragment UPPERCASE_LTR  : [A-Z] ;
fragment DIGIT          : [0-9] ;


DBL_EQL    :   '==' ;
GRT_EQL    :   '>=' | (IS WS)? (WS 'greater' WS 'than' WS 'or' WS 'equal' WS 'to' WS) ;
LESS_EQL   :   '<=' | (IS WS)? (WS 'less' WS 'than' WS 'or' WS 'equal' WS 'to' WS) ;
NOT_EQL    :   '!=' ;
LESS       :   '<'  | (IS WS)?  (WS 'less' WS 'than' WS) ;
GRT        :   '>'  | (IS WS)?  (WS 'greater' WS 'than' WS) ;

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
ASSERT     :    WS? 'assert' WS? | WS? 'assert' WS 'that' WS? | WS? 'make' WS 'sure' WS? | WS? 'make' WS 'sure' WS 'that' WS? | WS? 'make' WS 'sure' WS 'of' WS? | WS? 'force' WS 'check' WS?  ;
BETWEEN	   :	WS 'between' WS | WS 'is' WS 'between' WS | WS 'in' WS 'range' WS 'of' WS | WS 'in' WS 'the' WS 'range' WS 'of' WS	;

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

COMMENT	   : ((NL* 'start comment' .*? 'end comment' NL*) | '//' ~[\r\n]* ) -> skip ;
