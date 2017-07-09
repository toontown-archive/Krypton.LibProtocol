parser grammar TypeDeclaration;

import OperationStatements;

options 
    { tokenVocab=KryptonTokens; }


type_declaration 
    : DECLARE type_name '{' meta_declaration? operation_statement+ '}'
    ;
 
type_name 
    : generic_type_name
    | IDENTIFIER
    ;
        
generic_type_name
    : IDENTIFIER '<' identifier_list '>'
    ;

enumerable_declaration
    : ENUMERABLE '<' type_reference '>' IDENTIFIER ';'
    ;

meta_statement
    : enumerable_declaration
    ;

meta_declaration
    : META '{' meta_statement+ '}' ';'
    ;

identifier_list 
    : IDENTIFIER (',' identifier_list)? 
    ;
