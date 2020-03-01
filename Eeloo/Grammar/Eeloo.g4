grammar Eeloo;
import GeneratedLexer;

/* Parser */

program      : lines NL* EOF ;

lines        : (stmt NL+)+ ;

stmt: assignment
    | loop
    | if_stmt
	| fn_call
	| method_call
	| return_stmt
	| fn_def
	| assert_stmt
    ;

loop: while_stmt
	| for_stmt
	| from_loop
	| repeat_loop
	;

assignment: var EQL exp	;

assert_stmt: ASSERT WS? exp ;

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

exps: exp (COMMA exp)* COMMA?     #plainExps
	| LBRACK exps RBRACK	      #brackExps
	;

/* Loops */

while_stmt: ( WHILE | UNTIL ) exp NL lines END ;

for_stmt: FOR_EACH var (IN | FROM) exp NL lines END ;

from_loop: FROM exp RANGE_1 exp (RANGE_2 exp)? USE IDENTIFIER NL lines END ;

repeat_loop: REPEAT exp TIMES NL lines END 
			| exp TIMES (DO)? NL lines END;

if_stmt: if_partial else_if_partial* else_partial? END ;

if_partial: IF exp THEN? NL lines ;
else_if_partial: ELSE IF exp THEN? NL lines ;
else_partial: ELSE THEN? NL lines;

fn_call: IDENTIFIER NL* LBRACK NL* exps? NL* RBRACK ;

fn_def: FUNCTION IDENTIFIER (LBRACK fn_args RBRACK)? NL lines END 
	  | FUNCTION IDENTIFIER ((LESS | GRT) fn_args)? NL lines END
	  ;

fn_args: fn_arg (COMMA fn_arg)* COMMA? ;
fn_arg: IDENTIFIER (EQL exp)? ;

method_call: exp DOT fn_call ;

return_stmt: RETURN exp   #expReturn
		   | RETURN exps  #multiExpReturn
		   ;