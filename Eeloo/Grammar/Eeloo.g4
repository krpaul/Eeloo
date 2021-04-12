grammar Eeloo;
import GeneratedLexer;

/* Parser */

program      : lines NL* EOF ;

lines        : (stmt NL+)+ ;

stmt: assignment
    | loop
    | if_stmt
	| return_stmt
	| fn_def
	| assert_stmt
	| continue_stmt
	| exp
    ;

loop: while_stmt
	| for_stmt
	| from_loop
	| repeat_loop
	;

assignment: var EQL exp #regAssign
		  | var opr=(
				ADD_EQL
				| SUB_EQL 
				| MULT_EQL
				| DIV_EQL)
		  exp			#augAssign
		  ;

assert_stmt: ASSERT WS? exp ;

creator: LIST_MODIFIER? LIST_TOK       #listCreator
		| NUMBER_MODIFIER? NUMBER_TOK  #numberCreator
		| STRING_TOK                   #stringCreator 
		;

bool_stmt: TRUE | FALSE ;

list: L_SQ_BRACK NL* exps? NL* R_SQ_BRACK ;

string: STR ;

var: IDENTIFIER							 #variable
   | var (L_SQ_BRACK exp R_SQ_BRACK)	 #arrayIndex
   ;

num: NUMBER				#int
   | NUMBER DOT NUMBER  #dec
   ;

exp:  MINUS? num     			         #numExp
	| MINUS? var						 #varExp
    | string							 #strExp
    | bool_stmt							 #boolExp
    | list								 #listExp
	| exp DOT IDENTIFIER			     #attributeRefExp
	| IDENTIFIER OF exp				     #verboseAttributeExp
	| <assoc=right> exp POWER exp		 #pwrExp
	| exp opr=(MULTIPLY | DIVIDE 
			   | MOD) exp				 #multiplicativeOprExp
	| exp opr=(PLUS | MINUS) exp		 #additiveOprExp
	| exp FACTORIAL    					 #factorialExp
	| exp opr=(LESS_EQL | GRT_EQL | 
			   LESS     | GRT     ) exp  #comparisonExp
	| exp (IS NOT | ISNT) exp			 #inequalityExp
	| <assoc=right> exp 
		opr=(SQUARED | CUBED)			 #singlePwrExp
	| exp AS (STRING_TOK | LIST_TOK | 
	          NUMBER_TOK | BOOL_TOK)     #typecastExpression
	| exp RANGE_1 exp (RANGE_2 exp)?	 #rangeExp
	| exp IN exp						 #inExp
	| exp BETWEEN exp AND exp			 #betweenExp					
	| exp AND exp						 #andExp
	| exp OR exp						 #orExp
	| NOT exp							 #notExp

	/* Methods */
	| exp DOT fn_call                  #method_standardSyntax
	| fn_call KEYWORD exp	           #method_expandedSyntax
	| IDENTIFIER KEYWORD exp		   #method_expandedSyntaxNoBrackets
	| IDENTIFIER exps KEYWORD exp      #method_expandedSyntaxLooseArgs

	| exp L_SQ_BRACK exp RANGE_1 exp 
		(RANGE_2 exp)? R_SQ_BRACK	     #arraySlice
	| MINUS? fn_call					 #functionCallExp
	| exp EQUALITY exp					 #equalityExp
	| NEW? creator						 #creatorExpression
	| IF exp						     #prefixedInlineBool /* must be 2nd last */
    | LBRACK exp RBRACK		             #bracketedExp       /* must be last */
    ;

exps: exp NL* (COMMA NL* exp)* NL* COMMA?    #plainExps
	| LBRACK NL* exps NL* RBRACK	         #brackExps
	;

/* Loops */

while_stmt: ( WHILE | UNTIL ) exp NL lines END ;

for_stmt: FOR_EACH var (IN | FROM) exp NL lines END ;

from_loop: FROM exp RANGE_1 exp (RANGE_2 exp)? USE IDENTIFIER NL lines END ;

repeat_loop: REPEAT exp TIMES NL lines END
			| exp TIMES (DO)? NL lines END
			| (DO)? exp TIMES NL lines END
			;

continue_stmt: CONTINUE ;

/* End loops */

/* If statement construction */
if_stmt: if_partial else_if_partial* else_partial? END ;

if_partial: IF exp THEN? NL lines ;
else_if_partial: ELSE IF exp THEN? NL lines ;
else_partial: ELSE THEN? NL lines;

/* Try-catch statement construction */
try_stmt: try_partial catch_partial* finally_partial?;

try_partial: TRY NL lines ;
catch_partial: CATCH exp NL lines ;
finally_partial: FINALLY lines ;

fn_call: IDENTIFIER NL* LBRACK NL* exps? NL* RBRACK |
		 IDENTIFIER NL* ARROW NL* exps				;

fn_def_keyword : DEFINE | (DEFINE? NEW? FUNCTION) ;
fn_def: fn_def_keyword IDENTIFIER (LBRACK fn_args? RBRACK)? NL lines END 
	  | fn_def_keyword IDENTIFIER (ARROW fn_args)? NL lines END
	  ;

fn_args: fn_arg (COMMA fn_arg)* COMMA? ;
fn_arg: IDENTIFIER (EQL exp)? ;

return_stmt: RETURN exp   #expReturn
		   | RETURN exps  #multiExpReturn
		   ;
