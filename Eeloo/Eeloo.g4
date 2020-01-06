grammar Eeloo;

/* Parser */

program      : lines NL* EOF ;

lines        : (stmt NL+)+ ;

stmt: assignment
    | while_stmt
    | if_stmt
	| for_stmt
	| fn_call
	| method_call
	| fn_def
	| return_stmt
    ;

assignment: IDENTIFIER EQL exp ;

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
	| exp IS NOT? exp					 #equalityExp
    | MINUS exp				             #negationExp
	| exp RANGE exp						 #rangeExp
	| exp IN exp						 #inExp
	| exp AND exp						 #andExp
	| exp OR exp						 #orExp
    ;

exps: exp (COMMA exp)* COMMA? ;

while_stmt: WHILE exp NL lines END ;

for_stmt: FOR_EACH var IN exp NL lines END ;

if_stmt: if_partial else_if_partial* else_partial? END ;

if_partial: IF exp THEN NL lines ;
else_if_partial: ELSE IF exp THEN NL lines ;
else_partial: ELSE NL lines;

fn_call: IDENTIFIER LBRACK exps RBRACK ;

fn_def: FUNCTION IDENTIFIER LBRACK fn_args? RBRACK NL lines END ;

fn_args: fn_arg (COMMA fn_arg)* COMMA? ;
fn_arg: IDENTIFIER (EQL exp)? ;

method_call: exp DOT fn_call ;

return_stmt: RETURN exps ;

/* Lexer */

fragment LOWERCASE_LTR  : [a-z] ;
fragment UPPERCASE_LTR  : [A-Z] ;
fragment DIGIT          : [0-9] ;

DBL_EQL    :   '==' ;
GRT_EQL    :   '>=' ;
LESS_EQL   :   '<=' ;
NOT_EQL    :   '!=' ;
LESS       :   '<' | 'less' | 'less than';
GRT        :   '>' | 'greater' | 'greater than';

L_SQ_BRACK :    '['         ;
R_SQ_BRACK :    ']'         ;
COMMA      :    ','         ;
DOT		   :	'.'			;

EXIT       :    'exit'      ;
WHILE      :    'while'     ;
FOR_EACH   :    'for each'	;
IF         :    'if'        ;
THEN       :    'then'      ;
ELSE       :    'else'      ;
IS         :    'is'        ;
NOT		   :	'not'		;
IN		   :	'in'		;
AND		   :	'and'		;
OR		   :    'or'		;
FUNCTION   :	'function'	;
RETURN	   :	'return'    ;
END        :    'end'       ;

// Types
/*
LIST_MODIFIER: "unique"		;
NUMBER_MODIFIER: "negative" | "decimal" | "integer" ;

STRING_TOK :	'string'	;
LIST_TOK   :	'list'		;
NUMBER_TOK :    'number'	;
*/

TRUE  :    'true'      ;
FALSE :    'false'     ;

IDENTIFIER :   (LOWERCASE_LTR | UPPERCASE_LTR | '_') (LOWERCASE_LTR | UPPERCASE_LTR | '_' | DIGIT)*  ;    

STR		   : ["].*?["]	;
NUMBER     : DIGIT+     ;

EQL        :   '='      ;
PLUS       :   '+'      ;
MINUS      :   '-'      ;
DIVIDE     :   '/'      ;
MULTIPLY   :   '*'      ;
POWER      :   '^'      ;
MOD		   :   '%'		;
RANGE	   :   '...'	;

LBRACK     :   '('      ;
RBRACK     :   ')'      ;

RL         : (DBL_EQL | GRT_EQL | LESS_EQL | NOT_EQL | LESS | GRT) ;

NL		   : [\r]?[\n]  ;

COMMENT	   : 'start comment' .*? 'end comment' -> skip ;

WHITESPACE :   ([\t])+  -> skip ;

