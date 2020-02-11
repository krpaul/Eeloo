grammar Eeloo;

/* Parser */

program      : lines NL* EOF ;

lines        : (stmt NL+)+ ;

stmt: assignment
    | while_stmt
    | if_stmt
	| for_stmt
	| from_loop
	| fn_call
	| method_call
	| fn_def
	| return_stmt
    ;

assignment: var EQL exp	;

creator: LIST_MODIFIER? LIST_TOK       #listCreator
		| NUMBER_MODIFIER? NUMBER_TOK  #numberCreator
		| STRING_TOK                   #stringCreator 
		;

bool_stmt: TRUE | FALSE ;

list: L_SQ_BRACK exps? R_SQ_BRACK ;

string: STR ;

var: IDENTIFIER								#variable
   | IDENTIFIER L_SQ_BRACK exp R_SQ_BRACK	#arrayIndex
   ;

exp:  NUMBER						     #numExp
	| var						         #varExp
    | string							 #strExp
    | bool_stmt							 #boolExp
    | list								 #listExp
	| fn_call							 #functionCallExp
    | LBRACK exp RBRACK		             #bracketedExp
	| <assoc=right> exp POWER exp		 #pwrExp
	| exp opr=(MULTIPLY | DIVIDE | MOD) exp	 #multiplicativeOprExp
	| exp opr=(PLUS | MINUS) exp		 #additiveOprExp
	| exp opr=(LESS_EQL | GRT_EQL | 
			   LESS     | GRT     ) exp  #comparisonExp
	| exp (IS NOT | ISNT) exp			 #inequalityExp
	| exp (IS | DBL_EQL) exp			 #equalityExp
    | MINUS exp				             #negationExp
	| exp RANGE_1 exp         		     #rangeExp
	| exp RANGE_1 exp RANGE_2 exp		 #rangeExtendedExp 
	| exp IN exp						 #inExp
	| exp AND exp						 #andExp
	| exp OR exp						 #orExp
	| creator							 #creatorExpression
	| IF exp						     #prefixedInlineBool /* must be last */
    ;

exps: exp (COMMA exp)* COMMA? #plainExps
	| LBRACK exps RBRACK	  #brackExps
	;

/* Loops */

while_stmt: ( WHILE | UNTIL ) exp NL lines END ;

for_stmt: FOR_EACH var IN exp NL lines END ;

from_loop: FROM exp RANGE_1 exp (RANGE_2 exp)? USE IDENTIFIER NL lines END ;

if_stmt: if_partial else_if_partial* else_partial? END ;

if_partial: IF exp THEN? NL lines ;
else_if_partial: ELSE IF exp THEN? NL lines ;
else_partial: ELSE THEN? NL lines;

fn_call: IDENTIFIER NL* LBRACK NL* exps NL* RBRACK ;

fn_def: FUNCTION IDENTIFIER LBRACK fn_args? RBRACK NL lines END ;

fn_args: fn_arg (COMMA fn_arg)* COMMA? ;
fn_arg: IDENTIFIER (EQL exp)? ;

method_call: exp DOT fn_call ;

return_stmt: RETURN exp   #expReturn
		   | RETURN exps  #multiExpReturn
		   ;

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
MULTIPLY   :   '*'      ;
POWER      :   '^'      ;
MOD		   :   '%' | ' mod ' ;

RANGE_1	   :   '...' | WS TO WS  ;
RANGE_2	   :   WS BY WS ;

LBRACK     :   '('      ;
RBRACK     :   ')'      ;

RL         : (DBL_EQL | GRT_EQL | LESS_EQL | NOT_EQL | LESS | GRT) ;

NL		   : [\r]?[\n] ;

WS		   :   (' ' | '\t' )+ -> skip ;

COMMENT	   : NL* 'start comment' .*? 'end comment' NL* -> skip ;
