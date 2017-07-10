parser grammar KryptonParser;

options 
    { tokenVocab=KryptonTokens; }

// Root members

init : imports+=import_statement* groups+=group_definition* libraries+=library_definition* protocols+=protocol_definition*
     ;

import_statement : IMPORT (path=directory)? file=IDENTIFIER '.' KPDL ';'
                 ;

group_definition : GROUP name=IDENTIFIER ';' ;

library_definition : LIBRARY name=IDENTIFIER '{' library_statement* '}' ;

library_statement
    : packet_definition
    | type_declaration
    ;

protocol_definition : PROTOCOL 
                      ns=namespace '[' name=IDENTIFIER ']'
                      '{' message_definitions? packet_definition* '}' ;

message_definitions : name=IDENTIFIER (',' message_definitions)?
                    ;

packet_definition : PACKET name=IDENTIFIER (':' packet_parent)? 
                    '{' operation_statement+ '}' ;

packet_parent : ns=namespace '[' name=IDENTIFIER ']' (',' packet_parent)?
               ;

// Types

type_reference 
    : PRIMITIVE
    | declared_type_reference
    ;
    
declared_type_reference
    : IDENTIFIER
    | declared_generic_type_reference
    ;
    
declared_generic_type_reference
    : IDENTIFIER '<' type_reference (',' type_reference)* '>'
    ;

// Type declaration

type_declaration 
    : DECLARE type_name '{' meta_declaration? operation_statement+ return_statement '}'
    ;
 
type_name 
    : generic_type_name
    | IDENTIFIER
    ;
        
generic_type_name
    : IDENTIFIER '<' type_name (',' type_name)* '>'
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

return_statement
    : RETURN IDENTIFIER ';'
    ;

// Operation statements

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

// Utility

namespace : IDENTIFIER ('.' IDENTIFIER)* 
          ;

directory : IDENTIFIER ('/' IDENTIFIER)* '/'
          ;
