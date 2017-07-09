parser grammar OperationStatements;

import Types;

options 
    { tokenVocab=KryptonTokens; }


conditional_value
    : TRUE 
    | FALSE 
    | INTEGER 
    | IDENTIFIER 
    ;

if_statement
    : conditional_value OPERATOR conditional_value
    ;
    
loop_statement
    : conditional_value LOOPOPERATOR conditional_value
    ;
 
 conditional
    : if_statement
    | loop_statement
    ;
 
 conditional_statement
    : '(' conditional ')' '=>' '{' operation_statement+ '}' ';'
    ;
 
 meta_operation
    : IDENTIFIER METAOPERATOR ';'
    ;
 
 data_statement
    : type_reference IDENTIFIER ';'
    ;
 
 operation_statement
    : conditional_statement
    | meta_operation
    | data_statement
    ;
 